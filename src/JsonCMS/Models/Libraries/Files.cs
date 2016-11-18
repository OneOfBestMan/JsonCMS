using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.Libraries
{
    public class Files
    {
        public static string ReadTextFile(string path)
        {
            return File.ReadAllText(path);
        }

        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public static DateTime FileDate(string path)
        {
            return File.GetCreationTimeUtc(path);
        }

        public static bool FolderExists(string path)
        {
            return Directory.Exists(path);
        }

        public static bool WriteTextFile(string path, string text)
        {
            try
            {
                File.WriteAllText(path, text);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static List<string> GetFiles(string path, string extension) // e.g. extension .jpg
        {
            var files = GetFilesWithPaths(path);
            List<string> fileNames = new List<string>();
            foreach (string fullPath in files)
            {
                string fileName = fullPath.Split('\\').Last();
                var addFile = (string.IsNullOrEmpty(extension) || fileName.ToLower().IndexOf(extension) > 0);
                if (addFile)
                {
                    try
                    {
                        fileNames.Add(fileName);
                    }
                    catch (Exception)
                    {   
                        // probably duplicate 
                    }
                }
            }
            return fileNames;
        }

        public static List<string> GetFilesWithPaths(string path)
        {
            return Directory.GetFiles(path).ToList();
        }

        public static bool DeleteFiles(string directoryPath, string fileNames)
        {
            // fileNames can include *
            try
            {
                var dir = new DirectoryInfo(directoryPath);
                foreach (var file in dir.EnumerateFiles(fileNames))
                {
                    file.Delete();
                }
                return true;
            }
            catch (Exception ex)
            {
                var error = "Couldn't delete blog page files : " + ex.ToString();
                return false;
            }

        } 

        public static List<KeyValuePair<int, string>> GetSubFolders(string path)
        {
            List<KeyValuePair<int, string>> subfolders = new List<KeyValuePair<int, string>>();
            if (FolderExists(path))
            {
                var directories = Directory.GetDirectories(path);

                int c = 0;
                foreach (string fullPath in directories)
                {
                    c++;
                    string subFolder = fullPath.Split('\\').Last();
                    KeyValuePair<int, string> kp = new KeyValuePair<int, string>(c, subFolder);
                    subfolders.Add(kp);
                }
            }
            return subfolders;
        }
    }
}
