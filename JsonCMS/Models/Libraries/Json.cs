using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.Libraries
{
    public class Json<T>
    {
        private readonly string rootPath;

        public Json(string rootPath)
        {
            this.rootPath = rootPath;
        }

        public string ReadFile(string relativePath, string fileName)
        {
            string path = rootPath + "/" + relativePath + "/" + fileName;
            if (FileExists(relativePath, fileName))
            {
                return Files.ReadTextFile(path);
            }
            else
            {
                return string.Empty;
            }
        }

        public DateTime FileDate(string relativePath, string fileName)
        {
            string path = rootPath + "/" + relativePath + "/" + fileName;
            if (FileExists(relativePath, fileName))
            {
                return Files.FileDate(path);
            }
            else
            {
                return DateTime.Now;
            }
        }

        public bool FileExists(string relativePath, string fileName)
        {
            string path = rootPath + "/" + relativePath + "/" + fileName;
            return Files.FileExists(path);
        }

        public T ReadJsonObject(string json)
        {
            var obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }

        public string FormattedJson(T obj)
        {
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return json;
        }

        public bool WriteJson(string json, string relativePath, string fileName, bool overWrite)
        {
            string path = rootPath + "/" + relativePath + "/" + fileName;
            bool exists = Files.FileExists(path);
            if (!exists || (exists && overWrite))
            {
                return Files.WriteTextFile(path, json);
            }
            else
            {
                return false;
            }
        }

    }

}


