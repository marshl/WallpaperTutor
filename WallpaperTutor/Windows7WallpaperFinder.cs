//-----------------------------------------------------------------------
// <copyright file="Windows7WallpaperFinder.cs" company="marshl">
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
    using Microsoft.Win32;

    /// <summary>
    /// Class for getting the paths of wallpapers on Windows 7 and Windows Server 2008
    /// </summary>
    public class Windows7WallpaperFinder : WallpaperFinder
    {
        /// <summary>
        /// Gets the wallpapers for the target OS
        /// </summary>
        /// <param name="imagePaths">The list of paths that were found (if any)</param>
        /// <returns>True if any paths were found, otherwise false.</returns>
        public override bool GetWallpapers(ref List<string> imagePaths)
        {
            RegistryKey generalKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\Desktop\General");

            if (generalKey == null)
            {
                return false;
            }

            object wallpaperSource = generalKey.GetValue("WallpaperSource");
            if (wallpaperSource == null || !(wallpaperSource is string))
            {
                return false;
            }

            string wallpaperPath = (string)wallpaperSource;

            if (string.IsNullOrEmpty(wallpaperPath))
            {
                return false;
            }

            imagePaths.Add(wallpaperPath);
            return true;
        }
    }
}
