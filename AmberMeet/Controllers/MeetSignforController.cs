using System;
using System.Web.Mvc;
using AmberMeet.AppService.MeetSignfors;
using AmberMeet.Domain.MeetSignfors;
using AmberMeet.Infrastructure.Exceptions;
using AmberMeet.Infrastructure.Serialization;
using AmberMeet.Infrastructure.Utilities;
using AmberMeet.Models;

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
            ValidationLoginV();
            return View();
        }

        public ActionResult MyAlreadySignedList()
        {
            ValidationLoginV();
            return View();
        }

        public ActionResult MyAllSignforList()
        {
            ValidationLoginV();
            ViewBag.signforStates = MeetSignforState.WaitSign.GetDescriptions();
            return View();
        }

        public ActionResult MeetSubSignforList(string meetId)
        {
            ValidationLoginV();
            ViewBag.meetId = meetId;
            return View();
        }

        public string GetWaitSignforList(
            int page, int rows, string keywords, string activateIsoDate)
        {
            try
            {
                DateTime? activateDate = null;
                if (!string.IsNullOrEmpty(activateIsoDate))
                {
                    activateDate = DateTimeHelper.GetIsoDateValue(activateIsoDate);
                }
                var list = _meetSignforService.GetWaitSignfors(page, rows, keywords, SessionUserId, activateDate);
                var grid = _jsonService.GetWaitSignforJqGridJson(list, page, rows);
                return OkJqGrid(grid);
            }
            catch (Exception ex)
            {
                return OkExceptionStr(ex);
            }
        }

        [HttpGet]
        public string GetAlreadySignedList(
            int page, int rows, string keywords, string activateIsoDate)
        {
            try
            {
                DateTime? activateDate = null;
                if (!string.IsNullOrEmpty(activateIsoDate))
                {
                    activateDate = DateTimeHelper.GetIsoDateValue(activateIsoDate);
                }
                var list = _meetSignforService.GetAlreadySigneds(page, rows, keywords, SessionUserId, activateDate);
                return OkJqGrid(_jsonService.GetAlreadySignedJqGridJson(list, page, rows));
            }
            catch (Exception ex)
            {
                return OkExceptionStr(ex);
            }
        }

        [HttpGet]
        public string GetAllSignforList(
            int page, int rows, string keywords, string activateIsoDate, int? state)
        {
            try
            {
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
                return OkJqGrid(_jsonService.GetAllSignforJqGridJson(list, page, rows));
            }
            catch (Exception ex)
            {
                return OkExceptionStr(ex);
            }
        }

        [HttpGet]
        public ActionResult GetMeetSubSignforList(string meetId, string keywords)
        {
            try
            {
                ValidationLogin();
                //DateTime? activateDate = null;
                if (string.IsNullOrEmpty(meetId))
                {
                    throw new PreValidationException("所属会议不允许为空");
                }
                var list = _meetSignforService.GetMeetSubSignfors(meetId, keywords);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet]
        public ActionResult GetSignfor(string signforId)
        {
            try
            {
                return OkJson(_meetSignforService.GetDetail(signforId));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost]
        public ActionResult PutSignfor(string signforId, string feedback)
        {
            try
            {
                if (string.IsNullOrEmpty(signforId))
                {
                    throw new PreValidationException("ID不允许为空");
                }
                _meetSignforService.Signfor(signforId, feedback);
                return Ok();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}