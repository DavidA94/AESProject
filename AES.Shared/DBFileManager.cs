using System;
using System.IO;
using System.Linq;

namespace AES.Shared
{
    public class DBFileManager
    {
        private static bool m_hasBeenCalled = false;

        public static void SetDataDirectory(bool isTest = false)
        {
            if (m_hasBeenCalled)
            {
                return;
            }
            m_hasBeenCalled = true;

            // Get the directory we're starting in
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            // Loop until we find the folder that holds AES.Web
            while (dir.GetDirectories().FirstOrDefault(d => d.Name == "AES.Web") == null)
            {
                dir = dir.Parent;
            }

            // Go into AES.Web
            dir = dir.GetDirectories().FirstOrDefault(d => d.Name == "AES.Web");

            // Create a sub-directory is possible for App_Data
            try
            {
                dir = dir.CreateSubdirectory("App_Data");

                // If we're on a test, make another directory
                if (isTest)
                {
                    dir = dir.CreateSubdirectory("TestDB");
                }
            }
            // If we can't make the directories, too bad
            catch { }

            // Set the DataDirectory
            AppDomain.CurrentDomain.SetData("DataDirectory", dir.FullName);
        }
    }
}
