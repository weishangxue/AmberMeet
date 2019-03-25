using System;
using System.IO;
using System.Web.Mvc;
using AmberMeet.Infrastructure.Utilities;

namespace AmberMeet.Controllers
{
    public class TestController : ControllerBase
    {
        public ActionResult CanvasTest()
        {
            return View();
        }

        public ActionResult FileUploadTest()
        {
            return View();
        }

        public ActionResult SynCardReaderTest()
        {
            return View();
        }

        public string UploadImage(string imageData)
        {
            try
            {
                var fileName = ConfigHelper.CustomFilesDir + ConfigHelper.NewGuid + ".jpg";
                var data = Convert.FromBase64String(imageData);
                using (var fileStream = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    fileStream.Write(data, 0, data.Length);
                    //fileStream.Close();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                return Ok(false, ex.Message);
            }
        }
    }
}