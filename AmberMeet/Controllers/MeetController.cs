using System;
using System.Web.Mvc;
using AmberMeet.AppService.Meets;
using AmberMeet.Domain.Meets;
using AmberMeet.Dto;
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

        public MeetController(IMeetService meetService)
        {
            _meetService = meetService;
            _meetJsonService = new MeetJsonService();
        }

        public ActionResult Index()
        {
            if (!IsValidAccount())
            {
                return ErrorLoginView();
            }
            ViewBag.myDistributeCount = _meetService.GetMyDistributeCount(SessionUserId);
            ViewBag.meetStates = MeetState.Activate.GetDescriptions();
            return View();
        }

        public ActionResult MyDistributeList()
        {
            if (!IsValidAccount())
            {
                return ErrorLoginView();
            }

            ViewBag.meetStates = MeetState.Activate.GetDescriptions();
            return View();
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
                var list = _meetService.GetMyDistributeList(
                    page, rows, keywords, SessionUserId, MeetState.WaitActivate, activateDate);
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
                return _meetService.Get(id).ToJson();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                var result = HtmlHelper.Encode(ex.Message);
                return Ok(false, result);
            }
        }

        [HttpPost]
        public string PostMeet(MeetDetailDto dto)
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
    }
}