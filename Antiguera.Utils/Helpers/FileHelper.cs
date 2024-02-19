using System;
using System.IO;
using System.Net.Http;
using System.Web;

namespace Antiguera.Utils.Helpers
{
    public class FileHelper
    {
        public static string IncludeFileFromBase64(string path, string fileName, string base64)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(base64));

            using (var fileStream = File.Create($"{path}\\{fileName}"))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);

                return fileStream.Name;
            }
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static MemoryStream ConvertArrayBytesToStream(byte[] bytes)
        {
            string base64String = Convert.ToBase64String(bytes);
            return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(base64String));
        }

        public static MemoryStream ConvertBase64StringToStream(string base64String)
        {
            return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(base64String));
        }

        public static byte[] ConvertStreamToArrayBytes(HttpPostedFileBase file)
        {
            var br = new BinaryReader(file.InputStream);
            return br.ReadBytes(file.ContentLength);
        }

        public static string ConvertArrayBytesToBase64String(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static string ConvertStreamToBase64String(HttpPostedFileBase file)
        {
            var br = new BinaryReader(file.InputStream);
            byte[] bytes = br.ReadBytes(file.ContentLength);
            return Convert.ToBase64String(bytes);
        }
    }
}
