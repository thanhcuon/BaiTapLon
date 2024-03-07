using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
namespace BaiTapLon.Helpers
{
    public class FileUploadHelper
    {
        public static string UploadFile(HttpPostedFileBase filename, string folderName)
        {
            if (filename != null && filename.ContentLength > 0)
            {
                try
                {
                    string originalFileName = Path.GetFileNameWithoutExtension(filename.FileName);
                    string fileExtension = Path.GetExtension(filename.FileName);
                    string encryptedFileName = EncryptionHelper.HashString(originalFileName + DateTime.Now.ToString());
                    string fileNameHash = encryptedFileName + fileExtension;

                    string folderPath = HttpContext.Current.Server.MapPath("~/Content/Uploads/" + folderName);
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string filePath = Path.Combine(folderPath, fileNameHash);

                    filename.SaveAs(filePath);

                    // Trả về đường dẫn đầy đủ của tài liệu sau khi lưu
                    return "~/Content/Uploads/" + folderName + "/" + fileNameHash;
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi nếu cần
                    // Log.Error("Error uploading file: " + ex.Message);
                    return null;
                }
            }

            return null;
        }
    }
}