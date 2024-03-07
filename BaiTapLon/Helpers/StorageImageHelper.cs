using System;
using System.IO;
using System.Web;

namespace BaiTapLon.Helpers
{
    public class StorageImageHelper
    {
        public static string StorageImageUpload(HttpPostedFileBase file, string folderName, string fileNameHash)
        {
            if (file != null && file.ContentLength > 0)
            {
                var folderPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Uploads/" + folderName));
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var filePath = Path.Combine(folderPath, fileNameHash);

                file.SaveAs(filePath);

                var relativePath = "~/Content/Uploads/" + folderName + "/" + fileNameHash;
                return relativePath;
            }

            return null;
        }

    }

}
