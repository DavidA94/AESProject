using System;
using System.IO;
using System.Linq;

namespace AES.Shared
{
    public class DBFileManager
    {
        private static bool hasSet = false;

        /// <summary>
        /// Sets the DataDirectory so all services will use the same file
        /// </summary>
        /// <param name="isTest">Indicates if this is setting the directory for a *.Tests project</param>
        public static void SetDataDirectory(bool isTest = false)
        {
            if (hasSet) { return; }

            // If this doesn't work, we're probably on AppHabor, and it is running tests which don't require this
            try
            {
                // Get the directory we're starting in
                DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

                // Loop until we find the folder that holds AES.Web
                while (dir.GetDirectories().FirstOrDefault(d => d.Name == "AES.Web") == null)
                {
                    dir = dir.Parent;
                }

                // Create a sub-directory so the file doesn't mess with the main DB, IFF this is a test
                if (isTest)
                {
                    dir = dir.CreateSubdirectory("TestDB");
                }

                // Set the DataDirectory
                AppDomain.CurrentDomain.SetData("DataDirectory", dir.FullName);

                // We only want to use this flag if we are running tests, outside of the test environment, things will break with this,
                // plus, outside the test environment, there's no chance of two SDF files.
                hasSet = isTest;
            }
            catch { }
        }
    }
}
