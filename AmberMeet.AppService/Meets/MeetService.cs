using System;
using System.Data.Linq;
using System.Linq;
using AmberMeet.Domain.Data;
using AmberMeet.Domain.Meets;
using AmberMeet.Domain.MeetSignfors;
using AmberMeet.Dto.Meets;
using AmberMeet.Infrastructure.Exceptions;
using AmberMeet.Infrastructure.Search.Paging;
using AmberMeet.Infrastructure.Utilities;

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
            var meet = new Meet();
            meet.OwnerId = dto.OwnerId;
            meet.Subject = dto.Subject;
            meet.Body = dto.Body;
            meet.StartTime = dto.StartTime;
            meet.EndTime = dto.EndTime;
            meet.Place = dto.Place;
            meet.NeedFeedback = dto.NeedFeedback;
            meet.Status = (int) MeetState.WaitActivate;
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
            meet.StartTime = dto.StartTime;
            meet.EndTime = dto.EndTime;
            meet.Place = dto.Place;
            meet.NeedFeedback = dto.NeedFeedback;
            meet.ModifiedTime = DateTime.Now;

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

        public PagedResult<MeetPagedDto> GetMyWaitSignfors(int pageIndex, int pageSize, string keywords, string ownerId,
            DateTime? activateDate)
        {
            //todo
            return _meetQueryService.GetPaged(
                pageIndex, pageSize, keywords, ownerId, MeetState.WaitActivate, activateDate);
        }
    }
}