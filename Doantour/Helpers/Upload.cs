using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Doantour.Helpers
{
    public class Upload
    {
        static public Account account = new Account("dyjeuo2dv", "159276139847775", "dcGIEi7hJWieISP-77Z-EiJ5Fno");
        static public Cloudinary _cloudinary = new Cloudinary(account);
        public static async Task<string> Upfile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return "";
            }
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    PublicId = "Image" + "_" + DateTime.Now.Ticks,
                };
                var uploadResult = await Upload._cloudinary.UploadAsync(uploadParams);
                string url = uploadResult.SecureUrl.ToString();
                return url;
            }
        }
    }
}
