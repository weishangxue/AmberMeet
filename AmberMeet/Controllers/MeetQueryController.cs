using System;
using System.Web.Mvc;
using AmberMeet.AppService.Meets;
using AmberMeet.Domain.Meets;
using AmberMeet.Infrastructure.Serialization;
using AmberMeet.Infrastructure.Utilities;
using AmberMeet.Models;
using HtmlHelper = AmberMeet.Infrastructure.Utilities.HtmlHelper;

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
                if (!IsValidAccount())
                {
                    return OkLoginError();
                }
                DateTime? activateDate = null;
                if (!string.IsNullOrEmpty(activateIsoDate))
                {
                    activateDate = DateTimeHelper.GetIsoDateValue(activateIsoDate);
                }
                var list = _meetQueryService.GetWaitActivates(page, rows, keywords, SessionUserId, activateDate);
                return _meetJsonService.GetMyDistributeJqGridJson(list, page, rows);
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                var result = HtmlHelper.Encode(ex.Message);
                return Ok(false, result);
            }
        }

        [HttpGet]
        public string GetMyActivateList(
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
                var list = _meetQueryService.GetActivates(page, rows, keywords, SessionUserId, activateDate);
                return _meetJsonService.GetMyActivateJqGridJson(list, page, rows);
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                var result = HtmlHelper.Encode(ex.Message);
                return Ok(false, result);
            }
        }

        [HttpGet]
        public string GetMyAllDistributeList(
            int page, int rows, string keywords, int?state, string activateIsoDate)
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
                MeetState? meetState = null;
                if (state != null)
                {
                    meetState = (MeetState) state;
                }
                var list = _meetQueryService.GetAllDistributes(
                    page, rows, keywords, SessionUserId, activateDate, meetState);
                return _meetJsonService.GetMyAllDistributeJqGridJson(list, page, rows);
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
                return _meetQueryService.GetDetail(id).ToJson();
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