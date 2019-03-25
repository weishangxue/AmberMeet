using System;
using System.Data.Linq;
using System.Linq;
using AmberMeet.Domain.Data;
using AmberMeet.Domain.Meets;
using AmberMeet.Domain.MeetSignfors;
using AmberMeet.Dto.Meets;
using AmberMeet.Infrastructure.Exceptions;
using AmberMeet.Infrastructure.Search.Paging;
using AmberMeet.Infrastructure.Serialization;
using AmberMeet.Infrastructure.Utilities;
using AutoMapper;

namespace AmberMeet.AppService.Meets
{
    internal class MeetService : IMeetService
    {
        private readonly MeetQueryService _meetQueryService;
        private readonly IMeetRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public MeetService(
            IMeetRepository repository,
            IUnitOfWork unitOfWork,
            MeetQueryService meetQueryService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _meetQueryService = meetQueryService;
        }

        public MeetDto GetDetail(string meetId)
        {
            return _meetQueryService.GetDetail(meetId);
        }

        public int GetMyDistributeCount(string ownerId)
        {
            return _repository.FindCount(MeetState.WaitActivate, ownerId);
        }

        public int GetMyActivateCount(string ownerId)
        {
            return _repository.FindCount(MeetState.Activate, ownerId);
        }

        public PagedResult<MeetPagedDto> GetMyDistributes(
            int pageIndex, int pageSize, string keywords, string ownerId, DateTime? activateDate)
        {
            return _meetQueryService.GetPaged(
                pageIndex, pageSize, keywords, ownerId, MeetState.WaitActivate, activateDate);
        }

        public string AddMeet(MeetDto dto)
        {
            //add meet
            var meet = Mapper.Map<Meet>(dto);
            meet.Status = (int) MeetState.WaitActivate;
            meet.StartTime = dto.StartTime.Date.AddHours(dto.StartHour).AddMinutes(dto.StartMinute);
            if (dto.EndTime != null && dto.EndHour != null && dto.EndMinute != null)
            {
                meet.EndTime = dto.EndTime.Value.Date.AddHours(dto.EndHour.Value).AddMinutes(dto.EndMinute.Value);
            }
            //add signfors
            var signors = new EntitySet<MeetSignfor>();
            foreach (var signor in dto.Signors)
            {
                if (signors.Any(i => i.SignorId == signor.Key))
                {
                    continue;
                }

                var newMeetSignfor = new MeetSignfor
                {
                    Id = ConfigHelper.NewGuid,
                    SignorId = signor.Key,
                    SignorType = (int) MeetSignorType.Org, //暂时默认内部
                    IsRemind = false,
                    Status = (int) MeetSignorState.WaitSign,
                    ModifiedTime = DateTime.Now
                };

                signors.Add(newMeetSignfor);
            }
            meet.MeetSignfors = signors;
            _repository.Add(meet);

            return dto.Id;
        }

        public void ChangeMeet(MeetDto dto)
        {
            if (string.IsNullOrEmpty(dto.Id))
            {
                throw new ArgumentNullException(ExMessage.MustNotBeNullOrEmpty(nameof(dto.Id)));
            }
            var meet = _repository.First(dto.Id);
            meet.Subject = dto.Subject;
            meet.Body = dto.Body;
            //meet.StartTime = dto.StartTime;
            //meet.EndTime = dto.EndTime;
            meet.Place = dto.Place;
            meet.NeedFeedback = dto.NeedFeedback;
            meet.ModifiedTime = DateTime.Now;

            meet.StartTime = dto.StartTime.Date.AddHours(dto.StartHour).AddMinutes(dto.StartMinute);
            if (dto.EndTime != null && dto.EndHour != null && dto.EndMinute != null)
            {
                meet.EndTime = dto.EndTime.Value.Date.AddHours(dto.EndHour.Value).AddMinutes(dto.EndMinute.Value);
            }

            //更新签收信息
            meet.MeetSignfors.Clear();
            var signors = new EntitySet<MeetSignfor>();
            foreach (var signor in dto.Signors)
            {
                if (signors.Any(i => i.SignorId == signor.Key))
                {
                    continue;
                }

                var newMeetSignfor = new MeetSignfor
                {
                    Id = ConfigHelper.NewGuid,
                    SignorId = signor.Key,
                    SignorType = (int) MeetSignorType.Org, //暂时默认内部
                    IsRemind = false,
                    Status = (int) MeetSignorState.WaitSign,
                    ModifiedTime = DateTime.Now
                };

                signors.Add(newMeetSignfor);
            }
            meet.MeetSignfors.AddRange(signors);

            _unitOfWork.Commit();
        }

        public void ActivateMeet(string meetId, DateTime startTime, DateTime? endTime, string place)
        {
            if (string.IsNullOrEmpty(meetId))
            {
                throw new ArgumentNullException(ExMessage.MustNotBeNullOrEmpty(nameof(meetId)));
            }
            var meet = _repository.First(meetId);
            if (meet.Status != (int) MeetState.WaitActivate)
            {
                throw new ArgumentOutOfRangeException(nameof(meet.Status));
            }
            meet.MeetActivate = new MeetActivate
            {
                MeetId = meet.Id,
                StartTime = startTime,
                EndTime = endTime,
                Place = place,
                CreateTime = DateTime.Now,
                ModifiedTime = DateTime.Now
            };
            meet.Status = (int) MeetState.Activate;
            meet.ModifiedTime = DateTime.Now;

            var signforCount = meet.MeetSignfors.Count;
            for (var i = 0; i < signforCount; i++)
            {
                if (meet.MeetSignfors[i].Status == (int) MeetSignorState.WaitSign)
                {
                    meet.MeetSignfors[i].Status = (int) MeetSignorState.AutoSigned;
                    if (meet.NeedFeedback)
                    {
                        meet.MeetSignfors[i].Feedback = MeetSignorState.AutoSigned.ToEnumText();
                    }
                }
            }
            _unitOfWork.Commit();
        }
    }
}