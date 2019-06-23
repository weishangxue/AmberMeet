using System;
using System.Web.Mvc;
using AmberMeet.AppService.Meets;
using AmberMeet.Domain.Meets;
using AmberMeet.Infrastructure.Utilities;
using AmberMeet.Models;

namespace AmberMeet.Controllers
{
    public class MeetQueryController : ControllerBase
    {
        private readonly MeetJsonService _meetJsonService;
        private readonly IMeetQueryService _meetQueryService;

        public MeetQueryController(IMeetQueryService meetQueryService)
        {
            _meetQueryService = meetQueryService;
            _meetJsonService = new MeetJsonService();
        }

        [HttpGet]
        public string GetMyWaitActivateList(
            int page, int rows, string keywords, string activateIsoDate)
        {
            try
            {
                ValidationLogin();
                DateTime? activateDate = null;
                if (!string.IsNullOrEmpty(activateIsoDate))
                {
                    activateDate = DateTimeHelper.GetIsoDateValue(activateIsoDate);
                }
                var list = _meetQueryService.GetWaitActivates(page, rows, keywords, SessionUserId, activateDate);
                return OkJqGrid(_meetJsonService.GetMyDistributeJqGridJson(list, page, rows));
            }
            catch (Exception ex)
            {
                return OkExceptionStr(ex);
            }
        }

        [HttpGet]
        public string GetMyActivateList(
            int page, int rows, string keywords, string activateIsoDate)
        {
            try
            {
                DateTime? activateDate = null;
                if (!string.IsNullOrEmpty(activateIsoDate))
                {
                    activateDate = DateTimeHelper.GetIsoDateValue(activateIsoDate);
                }
                var list = _meetQueryService.GetActivates(page, rows, keywords, SessionUserId, activateDate);
                return OkJqGrid(_meetJsonService.GetMyActivateJqGridJson(list, page, rows));
            }
            catch (Exception ex)
            {
                return OkExceptionStr(ex);
            }
        }

        [HttpGet]
        public string GetMyAllDistributeList(
            int page, int rows, string keywords, int?state, string activateIsoDate)
        {
            try
            {
                DateTime? activateDate = null;
                if (!string.IsNullOrEmpty(activateIsoDate))
                {
                    activateDate = DateTimeHelper.GetIsoDateValue(activateIsoDate);
                }
                MeetState? meetState = null;
                if (state != null)
                {
                    meetState = (MeetState) state;
                }
                var list = _meetQueryService.GetAllDistributes(
                    page, rows, keywords, SessionUserId, activateDate, meetState);
                return OkJqGrid(_meetJsonService.GetMyAllDistributeJqGridJson(list, page, rows));
            }
            catch (Exception ex)
            {
                return OkExceptionStr(ex);
            }
        }

        [HttpGet]
        public ActionResult GetMeetDetail(string id)
        {
            try
            {
                ValidationLogin();
                return OkJson(_meetQueryService.GetDetail(id));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}