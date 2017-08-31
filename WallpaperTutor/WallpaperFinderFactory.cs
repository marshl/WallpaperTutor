//-----------------------------------------------------------------------
// <copyright file="WallpaperFinderFactory.cs" company="marshl">
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
    using System;

    /// <summary>
    /// Used to generate <see cref="WallpaperFinder"/> objects.
    /// </summary>
    public class WallpaperFinderFactory
    {
        /// <summary>
        /// Generates a <see cref="WallpaperFinder"/> subclass depending on the given operating system version.
        /// </summary>
        /// <param name="version">The users operating system version.</param>
        /// <returns>The wallpaper finder object if the OS is supported, otherwise null.</returns>
        public WallpaperFinder CreateBackgroundFinder(OperatingSystem version)
        {
            switch (version.Version.Major)
            {
                case 6:
                    switch (version.Version.Minor)
                    {
                        case 0: // Windows Vista
                        case 1: // Windows 7
                            return new Windows7WallpaperFinder();
                        case 2: // Windows 8
                        case 3: // Windows 8.1
                            return new Windows8WallpaperFinder();
                    }

                    return null;
                case 10: // Windows 10
                    return new Windows8WallpaperFinder();
                default:
                    return null;
            }
        }
    }
}
