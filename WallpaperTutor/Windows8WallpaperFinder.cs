//-----------------------------------------------------------------------
// <copyright file="Windows8WallpaperFinder.cs" company="marshl">
//
// Copyright 2016, Liam Marshall, marshl.
//
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
// along with this program.  If not, see http://www.gnu.org/licenses/
//
// </copyright>
//-----------------------------------------------------------------------

namespace WallpaperTutor
{
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Win32;

    /// <summary>
    /// Class for getting the paths of wallpapers on Windows 10
    /// </summary>
    public class Windows8WallpaperFinder : WallpaperFinder
    {
        /// <summary>
        /// Gets the wallpaper paths for Windows 10
        /// </summary>
        /// <param name="imagePaths">The list of paths that were found (if any)</param>
        /// <returns>True if the paths were found, otherwise false.</returns>
        public override bool GetWallpapers(ref List<string> imagePaths)
        {
            RegistryKey desktopKey = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop");
            object imageCountKey = desktopKey.GetValue("TranscodedImageCount");
            if (imageCountKey == null || !(imageCountKey is int))
            {
                return false;
            }

            int imageCount = (int)imageCountKey;
            for (int imageIndex = 0; imageIndex < imageCount; ++imageIndex)
            {
                string valueName = "TranscodedImageCache_" + imageIndex.ToString("D3");
                byte[] imageCache = desktopKey.GetValue(valueName) as byte[];

                if (imageCache != null)
                {
                    imagePaths.Add(this.TranscodedImageCacheToPath(imageCache));
                }
            }

            // If no images were found, the user might only have one wallpaper
            // In that case use the unnumbered image cache
            if (imagePaths.Count == 0)
            {
                byte[] transcodedBytes = desktopKey.GetValue("TranscodedImageCache") as byte[];
                imagePaths.Add(this.TranscodedImageCacheToPath(transcodedBytes));
            }

            return imagePaths.Count > 0;
        }

        /// <summary>
        /// Converts an array of bytes to a string
        /// </summary>
        /// <param name="bytes">An array of bytes in UTF16 format to convert.</param>
        /// <returns>The string representation of the bytes.</returns>
        private string TranscodedImageCacheToPath(byte[] bytes)
        {
            // The path to the image starts at the 24th bit
            const int PathOffset = 24;
            string str = Encoding.Unicode.GetString(bytes, PathOffset, bytes.Length - PathOffset);
            str = str.Substring(0, str.IndexOf((char)0));
            return str;
        }
    }
}
