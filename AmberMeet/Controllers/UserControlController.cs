using System;
using System.IO;
using System.Web.Mvc;
using AmberMeet.AppService.FileMaps;
using AmberMeet.Domain.Base;
using AmberMeet.Domain.Data;
using AmberMeet.Infrastructure.Exceptions;
using AmberMeet.Infrastructure.Utilities;

namespace AmberMeet.Controllers
{
    public class UserControlController : ControllerBase
    {
        private readonly IFileMapService _fileMapService;

        public UserControlController(IFileMapService fileMapService)
        {
            _fileMapService = fileMapService;
        }

        public PartialViewResult FileUpload()
        {
            return PartialView();
        }

        public PartialViewResult FileUploadList()
        {
            return PartialView();
        }

        public PartialViewResult CanvasUpload()
        {
            return PartialView();
        }

        public PartialViewResult IdentityCardReader()
        {
            return PartialView();
        }

        /// <summary>
        ///     上传文件(jquery-fileupload)
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadFile()
        {
            try
            {
                if (Request.Files.Count != 1)
                {
                    throw new PreValidationException("Files count not equal to 1");
                }
                var file = Request.Files[0];
                if (file == null)
                {
                    throw new PreValidationException("The first file is null");
                }
                var physicalFileName = $"{ConfigHelper.NewTimeGuid}-{file.FileName}";
                file.SaveAs($"{ConfigHelper.CustomFilesDir}{physicalFileName}");

                var fileLength = file.ContentLength;
                var newFileMap = _fileMapService.Add(new FileMap
                {
                    FileName = file.FileName,
                    PhysicalFileName = physicalFileName,
                    Length = fileLength
                });
                newFileMap.PhysicalFileName = FileMapEntityService.GetFileSrc(newFileMap);
                return Ok(newFileMap);
            }
            catch (PreValidationException ex)
            {
                return Ok(false, ex.Message);
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                return Ok(false, ex.Message);
            }
        }

        public ActionResult UploadImage(string imageData)
        {
            try
            {
                var data = Convert.FromBase64String(imageData);
                var newFileMap = new FileMap
                {
                    FileName = $"{ConfigHelper.NewTimeGuid}.jpg",
                    Length = data.Length
                };
                var fileName = ConfigHelper.CustomFilesDir + newFileMap.FileName;
                using (var fileStream = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    fileStream.Write(data, 0, data.Length);
                    //fileStream.Close();
                }
                newFileMap = _fileMapService.Add(newFileMap);
                newFileMap.PhysicalFileName = FileMapEntityService.GetFileSrc(newFileMap);
                return Ok(newFileMap);
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                return Ok(false, ex.Message);
            }
        }
    }
}