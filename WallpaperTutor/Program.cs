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
