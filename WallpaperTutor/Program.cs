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

    public class Program
    {
        static void Main(string[] args)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\Desktop\General");
            if (key == null)
            {
                return;
            }

            object result = key.GetValue("WallpaperSource");
            if (result == null)
            {
                return;
            }

            string source;
            try
            {
                source = (string)result;
            }
            catch (InvalidCastException)
            {
                return;
            }

            if (string.IsNullOrEmpty(source))
            {
                return;
            }

            // Windows Explorer command line arguments: https://support.microsoft.com/en-us/kb/152457
            Process.Start("explorer", "/select," + source);
        }
    }
}
