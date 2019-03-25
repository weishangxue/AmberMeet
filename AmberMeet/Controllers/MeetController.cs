using System;
using System.Web.Mvc;
using AmberMeet.AppService.Meets;
using AmberMeet.AppService.MeetSignfors;
using AmberMeet.Dto.Meets;
using AmberMeet.Infrastructure.Exceptions;
using AmberMeet.Infrastructure.Serialization;
using AmberMeet.Infrastructure.Utilities;
using AmberMeet.Models;
using HtmlHelper = AmberMeet.Infrastructure.Utilities.HtmlHelper;

namespace AmberMeet.Controllers
{
    public class MeetController : ControllerBase
    {
        private readonly MeetJsonService _meetJsonService;
        private readonly IMeetService _meetService;
        private readonly IMeetSignforService _meetSignforService;

        public MeetController(IMeetService meetService, IMeetSignforService meetSignforService)
        {
            _meetService = meetService;
            _meetSignforService = meetSignforService;
            _meetJsonService = new MeetJsonService();
        }

        public ActionResult Index()
        {
            if (!IsValidAccount())
            {
                return ErrorLoginView();
            }
            ViewBag.myDistributeCount = _meetService.GetMyDistributeCount(SessionUserId);
            ViewBag.myActivateCount = _meetService.GetMyActivateCount(SessionUserId);
            ViewBag.myWaitSignforCount = _meetSignforService.GetMyWaitSignforCount(SessionUserId);
            ViewBag.myAlreadySignedCount = _meetSignforService.GetMyAlreadySignedCount(SessionUserId);
            return View();
        }

        public ActionResult MyDistributeList()
        {
            if (!IsValidAccount())
            {
                return ErrorLoginView();
            }
            return View();
        }

        public ActionResult MeetDetail(string meetId)
        {
            try
            {
                if (!IsValidAccount())
                {
                    return ErrorLoginView();
                }
                if (string.IsNullOrEmpty(meetId))
                {
                    throw new PreValidationException("会议ID不允许为空");
                }
                var meet = _meetService.GetDetail(meetId);
                return View(meet);
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                return ErrorView(ex.Message);
            }
        }

        [HttpGet]
        public string GetMyDistributeList(
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
                var list = _meetService.GetMyDistributes(page, rows, keywords, SessionUserId, activateDate);
                return _meetJsonService.GetJqGridJson(list, page, rows);
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                var result = HtmlHelper.Encode(ex.Message);
                return Ok(false, result);
            }
        }

        [HttpGet]
        public string GetMeetDetail(string id)
        {
            try
            {
                if (!IsValidAccount())
                {
                    return OkLoginError();
                }
                return _meetService.GetDetail(id).ToJson();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                var result = HtmlHelper.Encode(ex.Message);
                return Ok(false, result);
            }
        }

        [HttpPost]
        public string PostMeet(MeetDto dto)
        {
            try
            {
                if (!IsValidAccount())
                {
                    return OkLoginError();
                }
                if (string.IsNullOrEmpty(dto.OwnerId))
                {
                    dto.OwnerId = SessionUserId;
                }
                if (string.IsNullOrEmpty(dto.Id))
                {
                    _meetService.AddMeet(dto);
                }
                else
                {
                    _meetService.ChangeMeet(dto);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                var result = HtmlHelper.Encode(ex.Message);
                return Ok(false, result);
            }
        }

        [HttpPost]
        public string PutMeetActivate(MeetDto dto)
        {
            try
            {
                if (!IsValidAccount())
                {
                    return OkLoginError();
                }
                if (string.IsNullOrEmpty(dto.Id))
                {
                    throw new PreValidationException("会议ID不允许为空");
                }
                var meet = _meetService.GetDetail(dto.Id);
                if (meet.OwnerId != SessionUserId)
                {
                    throw new PreValidationException("不允许会非议拥有者激活会议");
                }

                var startTime = dto.StartTime.Date.AddHours(dto.StartHour).AddMinutes(dto.StartMinute);
                DateTime? endTime = null;
                if (dto.EndTime != null && dto.EndHour != null && dto.EndMinute != null)
                {
                    endTime = dto.EndTime.Value.Date.AddHours(dto.EndHour.Value).AddMinutes(dto.EndMinute.Value);
                }
                _meetService.ActivateMeet(dto.Id, startTime, endTime, dto.Place);
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