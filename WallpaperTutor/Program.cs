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
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
//-----------------------------------------------------------------------
namespace WallpaperTutor
{
    using System;
    using System.Diagnostics;
    using Microsoft.Win32;
    using System.Collections.Generic;
    using System.Linq;

    public class Program
    {
        static string RegistryValueToPath(byte[] bytes)
        {
            string transcodedPath = String.Empty;
            string temp = System.Text.Encoding.Unicode.GetString(bytes, 24, bytes.Length - 24);
            temp = temp.TrimEnd((char)0);
            return temp;
        }

        static bool GetNewPath(ref List<string> imagePaths)
        {
            RegistryKey desktopKey = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop");
            int imageCount = (int)desktopKey.GetValue("TranscodedImageCount");

            byte[] transcodedBytes = desktopKey.GetValue("TranscodedImageCache") as byte[];
            imagePaths.Add(RegistryValueToPath(transcodedBytes));

            for (int imageIndex = 0; imageIndex < imageCount; ++imageIndex)
            {
                string valueName = "TranscodedImageCache_" + imageIndex.ToString("D3");
                byte[] imageCache = desktopKey.GetValue(valueName) as byte[];

                if (imageCache != null)
                {
                    imagePaths.Add(RegistryValueToPath(imageCache));
                }
            }

            return true;
        }

        static bool GetOldPath(ref List<string> imagePaths)
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

        static void Main(string[] args)
        {
            List<string> imagePaths = new List<string>();
            if(GetNewPath(ref imagePaths) || GetOldPath(ref imagePaths))
            {
                foreach (string path in imagePaths.Distinct())
                {
                    // Windows Explorer command line arguments: https://support.microsoft.com/en-us/kb/152457
                    Process.Start("explorer", $"/select,{path}");
                }
            }
        }
    }
}
