using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace PeopleTrackingApi.Web.Controllers.Api
{
    public class UploadFileController : BaseApiController
    {
        [HttpPost]
        public IHttpActionResult Upload()
        {
            var Message = string.Empty;
            string fileId = null;
            var httpRequest = HttpContext.Current.Request;
            var model = httpRequest.Params["title"];
            var postedFile = httpRequest.Files["BlobName"];
            var fileExtension = Path.GetExtension(postedFile.FileName);
            fileId = new string(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
            fileId = fileId + DateTime.Now.ToString("yymmssfff") + fileExtension;
            var picPath = HttpContext.Current.Server.MapPath(@"~\UploadFiles\");
            if (!Directory.Exists(picPath))
            {
                Directory.CreateDirectory(picPath);
            }
            if (!IsValidFile(fileExtension))
                Message = "File Formate is not valid";
            var filePath = Path.Combine(picPath, fileId);
            Stream strm = postedFile.InputStream;

            Compressimage(strm, filePath, postedFile.FileName);
            //postedFile.SaveAs(filePath);
            return Ok(new { ImagePath = fileId, Success = true });
        }
        public static void Compressimage(Stream sourcePath, string targetPath, String filename)
        {
            try
            {
                using (var image = Image.FromStream(sourcePath))
                {
                    float maxHeight = 1800.0f;
                    float maxWidth = 1800.0f;
                    int newWidth;
                    int newHeight;
                    string extension;
                    Bitmap originalBMP = new Bitmap(sourcePath);
                    int originalWidth = originalBMP.Width;
                    int originalHeight = originalBMP.Height;
                    if (originalWidth > maxWidth || originalHeight > maxHeight)
                    {
                        // To preserve the aspect ratio  
                        float ratioX = (float)maxWidth / (float)originalWidth;
                        float ratioY = (float)maxHeight / (float)originalHeight;
                        float ratio = Math.Min(ratioX, ratioY);
                        newWidth = (int)(originalWidth * ratio);
                        newHeight = (int)(originalHeight * ratio);
                    }
                    else
                    {
                        newWidth = (int)originalWidth;
                        newHeight = (int)originalHeight;

                    }
                    Bitmap bitMAP1 = new Bitmap(originalBMP, newWidth, newHeight);
                    Graphics imgGraph = Graphics.FromImage(bitMAP1);
                    extension = Path.GetExtension(targetPath);
                    if (extension == ".png" || extension == ".gif")
                    {
                        imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
                        imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);


                        bitMAP1.Save(targetPath, image.RawFormat);

                        bitMAP1.Dispose();
                        imgGraph.Dispose();
                        originalBMP.Dispose();
                    }
                    else if (extension == ".jpg")
                    {

                        imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
                        imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);
                        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                        Encoder myEncoder = Encoder.Quality;
                        EncoderParameters myEncoderParameters = new EncoderParameters(1);
                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                        myEncoderParameters.Param[0] = myEncoderParameter;
                        bitMAP1.Save(targetPath, jpgEncoder, myEncoderParameters);

                        bitMAP1.Dispose();
                        imgGraph.Dispose();
                        originalBMP.Dispose();

                    }


                }

            }
            catch (Exception)
            {
                throw;

            }
        }


        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private bool IsValidFile(string ext)
        {
            string[] validFileFormate = new string[] { "jpg", "png", "gif", "webp" };
            for (int i = 0; i < validFileFormate.Length; i++)
            {
                string vF = "." + validFileFormate[i];
                if (vF == ext)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
