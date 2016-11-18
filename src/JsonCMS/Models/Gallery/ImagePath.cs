using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonCMS.Models.Galleries
{
    public class ImagePath
    {
        public string diskRootPath { get; set; }
        public string relativeFolderPath { get; set; }
        public string fileName { get; set; }
        public ImageVersion originalImage { get; set; }

        public ImagePath()
        {
        }

        public ImagePath(string diskRootPath, string relativeFolderPath, string fileName)
        {
            this.diskRootPath = diskRootPath;
            this.relativeFolderPath = "/" + relativeFolderPath;
            this.fileName = fileName;
        }

        public ImagePath(string diskRootPath, string relativeFolderPath, string fileName, ImageVersion originalImage)
        {
            this.diskRootPath = diskRootPath;
            this.relativeFolderPath = "/" + relativeFolderPath;
            this.fileName = fileName;
            this.originalImage = originalImage;
        }

        [JsonIgnore]
        public string diskFullPath
        {
            get
            {
                return diskRootPath + "/" + relativeFolderPath + "/" + fileName;
            }
        }

        [JsonIgnore]
        public string wwwFullPath
        {
            get
            {
                return relativeFolderPath + "/" + fileName;
            }
        }

        [JsonIgnore]
        public string diskFullPathBase64
        {
            get
            {
                return diskRootPath + "/" + relativeFolderPath + "/" + PrepareBase64(this.Base64Encode(fileName));
            }
        }

        [JsonIgnore]
        public string wwwFullPathBase64
        {
            get
            {
                return relativeFolderPath + "/" + PrepareBase64(this.Base64Encode(fileName));
            }
        }

        [JsonIgnore]
        public string fileNameBase64
        {
            get
            {
                return PrepareBase64(this.Base64Encode(fileName));
            }
        }

        private string PrepareBase64(string filename)
        {
            return filename.Replace("=", "") + ".jpg";
        }

        private string Base64Encode(string plainText)
        {
            if (plainText.IndexOf(".") > 0)
            {
                plainText = plainText.Split('.')[0];
            }
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }

}
