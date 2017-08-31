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
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    /// The entry point class
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The program entry point
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            var factory = new WallpaperFinderFactory();
            WallpaperFinder finder = factory.CreateBackgroundFinder(Environment.OSVersion);

            if (finder == null)
            {
                MessageBox.Show(
                    "This operating system is not supported. This script only supports Windows NT 6.0, 6.1, 6.2, 6.3 or 10.x. " +
                    "(i.e. Windows Vista, Windows 7, Windows Server 2008, Windows 8, Windows Server 2012, Windows 8.1, Windows Server 2012 R2 or Windows 10).\n" +
                    $"You seem to be running: {Environment.OSVersion.VersionString}",
                    "WallpaperTutor", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                return;
            }

            List<string> imagePaths = new List<string>();
            if (finder.GetWallpapers(ref imagePaths))
            {
                foreach (string path in imagePaths.Distinct())
                {
                    // Windows Explorer command line arguments: https://support.microsoft.com/en-us/kb/152457
                    Process.Start("explorer", $"/select,{path}");
                }
            }
            else
            {
                MessageBox.Show("No wallpapers could be found.", "WallpaperTutor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
