//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="marshl">
// Copyright 2016, Liam Marshall, marshl.
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses
// </copyright>
//-----------------------------------------------------------------------
namespace WallpaperTutor
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using Microsoft.Win32;

    /// <summary>
    /// The entry point class
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The entry point
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            List<string> imagePaths = new List<string>();

            // Prioritise the new locations for wallpaper paths over the old locations
            if (GetNewPath(ref imagePaths) || GetOldPath(ref imagePaths))
            {
                foreach (string path in imagePaths.Distinct())
                {
                    // Windows Explorer command line arguments: https://support.microsoft.com/en-us/kb/152457
                    Process.Start("explorer", $"/select,{path}");
                }
            }
        }

        /// <summary>
        /// Converts an array of bytes to a string
        /// </summary>
        /// <param name="bytes">An array of bytes in UTF16 format to convert.</param>
        /// <returns>The string representation of the bytes.</returns>
        private static string ByteArrayToString(byte[] bytes)
        {
            // The path to the image starts at the 24th bit
            const int PathOffset = 24;
            string str = Encoding.Unicode.GetString(bytes, PathOffset, bytes.Length - PathOffset);
            str = str.TrimEnd((char)0);
            return str;
        }

        /// <summary>
        /// Gets the wallpaper paths for Windows 8 and 10
        /// </summary>
        /// <param name="imagePaths">The list of paths that were found (if any)</param>
        /// <returns>True if the paths were found, otherwise false.</returns>
        private static bool GetNewPath(ref List<string> imagePaths)
        {
            RegistryKey desktopKey = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop");
            object imageCountKey = desktopKey.GetValue("TranscodedImageCount");
            if (imageCountKey == null || !(imageCountKey is int))
            {
                return false;
            }

            int imageCount = (int)imageCountKey;

            byte[] transcodedBytes = desktopKey.GetValue("TranscodedImageCache") as byte[];
            imagePaths.Add(ByteArrayToString(transcodedBytes));

            for (int imageIndex = 0; imageIndex < imageCount; ++imageIndex)
            {
                string valueName = "TranscodedImageCache_" + imageIndex.ToString("D3");
                byte[] imageCache = desktopKey.GetValue(valueName) as byte[];

                if (imageCache != null)
                {
                    imagePaths.Add(ByteArrayToString(imageCache));
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the wallpaper paths for Windows 7
        /// </summary>
        /// <param name="imagePaths">The list of paths that were found (if any)</param>
        /// <returns>True if any paths were found, otherwise false.</returns>
        private static bool GetOldPath(ref List<string> imagePaths)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\Desktop\General");

            if (key == null)
            {
                return false;
            }

            object result = key.GetValue("WallpaperSource");
            if (result == null || !(result is string))
            {
                return false;
            }

            string source = (string)result;

            if (string.IsNullOrEmpty(source))
            {
                return false;
            }

            imagePaths.Add(source);
            return true;
        }
    }
}
