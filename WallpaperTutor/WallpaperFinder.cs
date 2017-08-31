//-----------------------------------------------------------------------
// <copyright file="WallpaperFinder.cs" company="marshl">
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

    /// <summary>
    /// Abstract class for getting the paths of wallpapers on a Windows platform.
    /// </summary>
    public abstract class WallpaperFinder
    {
        /// <summary>
        /// Gets the current wallpaper paths.
        /// </summary>
        /// <param name="imagePaths">The image paths that were found, if any.</param>
        /// <returns>True if any wallpapers were found, otherwise false.</returns>
        public abstract bool GetWallpapers(ref List<string> imagePaths);
    }
}
