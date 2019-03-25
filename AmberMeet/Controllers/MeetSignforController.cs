using System;
using System.Web.Mvc;
using AmberMeet.AppService.MeetSignfors;
using AmberMeet.Infrastructure.Exceptions;
using AmberMeet.Infrastructure.Serialization;
using AmberMeet.Infrastructure.Utilities;
using AmberMeet.Models;
using HtmlHelper = AmberMeet.Infrastructure.Utilities.HtmlHelper;

namespace AmberMeet.Controllers
{
    public class MeetSignforController : ControllerBase
    {
        private readonly MeetSignforJsonService _jsonService;
        private readonly IMeetSignforService _meetSignforService;

        public MeetSignforController(IMeetSignforService meetSignforService)
        {
            _meetSignforService = meetSignforService;
            _jsonService = new MeetSignforJsonService();
        }

        public ActionResult MyWaitSignforList()
        {
            return View();
        }

        [HttpGet]
        public string GetWaitSignforList(
            int page, int rows, string keywords, string activateIsoDate)
        {
            try
            {
                if (!IsValidAccount())
                {
                    return OkLoginError();
                }
                DateTime? activateDate = null;
                if (!string.IsNullOrEmpty(activateIsoDate))
                {
                    activateDate = DateTimeHelper.GetIsoDateValue(activateIsoDate);
                }
                var list = _meetSignforService.GetMyWaitSignfors(page, rows, keywords, SessionUserId, activateDate);
                return _jsonService.GetJqGridJson(list, page, rows);
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                var result = HtmlHelper.Encode(ex.Message);
                return Ok(false, result);
            }
        }

        [HttpGet]
        public string GetSignfor(string signforId)
        {
            try
            {
                if (!IsValidAccount())
                {
                    return OkLoginError();
                }
                return _meetSignforService.GetDetail(signforId).ToJson();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                var result = HtmlHelper.Encode(ex.Message);
                return Ok(false, result);
            }
        }

        [HttpPost]
        public string PutSignfor(string signforId, string feedback)
        {
            try
            {
                if (!IsValidAccount())
                {
                    return OkLoginError();
                }
                if (string.IsNullOrEmpty(signforId))
                {
                    throw new PreValidationException("ID不允许为空");
                }
                _meetSignforService.Signfor(signforId, feedback);
                return Ok();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                var result = HtmlHelper.Encode(ex.Message);
                return Ok(false, result);
            }
        }
    }
}