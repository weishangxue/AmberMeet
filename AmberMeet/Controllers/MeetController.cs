using System;
using System.Web.Mvc;
using AmberMeet.AppService.Meets;
using AmberMeet.AppService.MeetSignfors;
using AmberMeet.Domain.Meets;
using AmberMeet.Dto.Meets;
using AmberMeet.Infrastructure.Exceptions;
using AmberMeet.Infrastructure.Serialization;
using AmberMeet.Infrastructure.Utilities;

namespace AmberMeet.Controllers
{
    public class MeetController : ControllerBase
    {
        private readonly IMeetCommandService _meetCommandService;
        private readonly IMeetQueryService _meetQueryService;
        private readonly IMeetSignforService _meetSignforService;

        public MeetController(
            IMeetCommandService meetCommandService,
            IMeetQueryService meetQueryService,
            IMeetSignforService meetSignforService)
        {
            _meetCommandService = meetCommandService;
            _meetQueryService = meetQueryService;
            _meetSignforService = meetSignforService;
        }

        public ActionResult Index()
        {
            ValidationLoginV();
            ViewBag.myDistributeCount = _meetQueryService.GetMyDistributeCount(SessionUserId);
            ViewBag.myActivateCount = _meetQueryService.GetMyActivateCount(SessionUserId);
            ViewBag.myWaitSignforCount = _meetSignforService.GetMyWaitSignforCount(SessionUserId);
            ViewBag.myAlreadySignedCount = _meetSignforService.GetMyAlreadySignedCount(SessionUserId);
            return View();
        }

        public ActionResult MyDistributeList()
        {
            ValidationLoginV();
            return View();
        }

        public ActionResult MyActivateList()
        {
            ValidationLoginV();
            return View();
        }

        public ActionResult MyAllDistributeList()
        {
            ValidationLoginV();
            ViewBag.meetStates = MeetState.WaitActivate.GetDescriptions();
            return View();
        }

        public ActionResult MeetDetail(string meetId, string subSignforId)
        {
            try
            {
                ValidationLoginV();
                if (string.IsNullOrEmpty(meetId))
                {
                    throw new PreValidationException("会议ID不允许为空");
                }
                var meet = _meetQueryService.GetDetail(meetId);
                if (subSignforId != null)
                {
                    var signfor = _meetSignforService.GetDetail(subSignforId);

                    ViewBag.signorName = signfor.SignorName;
                    ViewBag.stateStr = signfor.StateStr;
                    ViewBag.signTimeStr = signfor.SignTimeStr;
                    ViewBag.feedback = signfor.Feedback;
                }
                return View(meet);
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                return ErrorView(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult PostMeet(MeetDto dto)
        {
            try
            {
                ValidationLogin();
                if (string.IsNullOrEmpty(dto.OwnerId))
                {
                    dto.OwnerId = SessionUserId;
                }
                if (string.IsNullOrEmpty(dto.Id))
                {
                    _meetCommandService.AddMeet(dto);
                }
                else
                {
                    _meetCommandService.ChangeMeet(dto);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost]
        public ActionResult PutMeetActivate(MeetDto dto)
        {
            try
            {
                ValidationLogin();
                if (string.IsNullOrEmpty(dto.Id))
                {
                    throw new PreValidationException("会议ID不允许为空");
                }
                var meet = _meetQueryService.GetDetail(dto.Id);
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
                _meetCommandService.ActivateMeet(dto.Id, startTime, endTime, dto.Place);
                return Ok();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}