using CitizenManagement.Models;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System;
using System.Web.Mvc;
using static CitizenManagement.FilterConfig;

namespace CitizenManagement.Controllers
{
    [RefreshAction]
    public class UploadController : Controller
    {
        string[] ImageExtensions = { ".JPG", ".JPEG", ".GIF", ".PNG" };
        [HttpPost]
        public ActionResult Image(HttpPostedFileBase imageFile)
        {

            try
            {
                var extension = Path.GetExtension(imageFile.FileName).ToUpperInvariant();
                var isValidExtension = ImageExtensions.Contains(extension);

                if (!isValidExtension)
                {
                    throw new InvalidOperationException();
                }

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(imageFile.FileName, imageFile.InputStream),
                };

                var uploadResult = CloudinaryBase.UploadImage(uploadParams);

                if (uploadResult.StatusCode == HttpStatusCode.OK)
                {
                    var imageUrl = uploadResult.SecureUrl.AbsoluteUri;
                    return Json(new { url = imageUrl }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw new Exception("Error uploading image to Cloudinary");
                }

            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
