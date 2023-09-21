using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace CitizenManagement.Models
{
    public class CloudinaryBase
    {
        public static CloudinaryDotNet.Account account = new CloudinaryDotNet.Account(
                ConfigurationManager.AppSettings["CloudinaryAccount"],
                ConfigurationManager.AppSettings["CloudinaryAPIKey"],
                ConfigurationManager.AppSettings["CloudinarySecretKey"]
            );
        public static Cloudinary cloudinary = new Cloudinary(account);
        public static ImageUploadResult UploadImage(ImageUploadParams image)
        {
            image.UploadPreset = "citizen";


            return cloudinary.Upload(image);
        }

        public static DelResResult DeleteImage(string url)
        {
            Uri uri = new Uri(url);
            string fileName = Path.GetFileNameWithoutExtension(uri.LocalPath);

            DelResParams delResParams = new DelResParams();
            delResParams.PublicIds.Add("citizen/" + fileName);

            return cloudinary.DeleteResources(delResParams);
        }

    }
}