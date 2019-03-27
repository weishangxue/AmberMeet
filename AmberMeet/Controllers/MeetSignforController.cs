using System;
using System.Web.Mvc;
using AmberMeet.AppService.MeetSignfors;
using AmberMeet.Domain.MeetSignfors;
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
            if (!IsValidAccount())
            {
                return ErrorLoginView();
            }
            return View();
        }

        public ActionResult MyAlreadySignedList()
        {
            if (!IsValidAccount())
            {
                return ErrorLoginView();
            }
            return View();
        }

        public ActionResult MyAllSignforList()
        {
            if (!IsValidAccount())
            {
                return ErrorLoginView();
            }
            ViewBag.signforStates = MeetSignforState.WaitSign.GetDescriptions();
            return View();
        }

        public ActionResult MeetSubSignforList(string meetId)
        {
            if (!IsValidAccount())
            {
                return ErrorLoginView();
            }
            ViewBag.meetId = meetId;
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
                var list = _meetSignforService.GetWaitSignfors(page, rows, keywords, SessionUserId, activateDate);
                return _jsonService.GetWaitSignforJqGridJson(list, page, rows);
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                var result = HtmlHelper.Encode(ex.Message);
                return Ok(false, result);
            }
        }

        [HttpGet]
        public string GetAlreadySignedList(
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
                var list = _meetSignforService.GetAlreadySigneds(page, rows, keywords, SessionUserId, activateDate);
                return _jsonService.GetAlreadySignedJqGridJson(list, page, rows);
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                var result = HtmlHelper.Encode(ex.Message);
                return Ok(false, result);
            }
        }

        [HttpGet]
        public string GetAllSignforList(
            int page, int rows, string keywords, string activateIsoDate, int? state)
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
                MeetSignforState? signorState = null;
                if (state != null)
                {
                    signorState = (MeetSignforState) state;
                }
                var list = _meetSignforService.GetAllSignfors(page, rows, keywords, SessionUserId, activateDate,
                    signorState);
                return _jsonService.GetAllSignforJqGridJson(list, page, rows);
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                var result = HtmlHelper.Encode(ex.Message);
                return Ok(false, result);
            }
        }

        [HttpGet]
        public string GetMeetSubSignforList(string meetId, string keywords)
        {
            try
            {
                if (!IsValidAccount())
                {
                    return OkLoginError();
                }
                DateTime? activateDate = null;
                if (string.IsNullOrEmpty(meetId))
                {
                    throw new PreValidationException("所属会议不允许为空");
                }
                var list = _meetSignforService.GetMeetSubSignfors(meetId, keywords);
                return Ok(list);
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