using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using AmberMeet.Domain.Data;
using AmberMeet.Domain.Meets;
using AmberMeet.Domain.MeetSignfors;
using AmberMeet.Domain.Organizations;
using AmberMeet.Dto;
using AmberMeet.Infrastructure.Exceptions;
using AmberMeet.Infrastructure.Search;
using AmberMeet.Infrastructure.Search.Paging;
using AmberMeet.Infrastructure.Search.Sort;
using AmberMeet.Infrastructure.Utilities;

namespace AmberMeet.AppService.Meets
{
    internal class MeetService : IMeetService
    {
        private readonly IOrgUserRepository _orgUserRepository;
        private readonly IMeetRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public MeetService(
            IMeetRepository repository,
            IOrgUserRepository orgUserRepository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _orgUserRepository = orgUserRepository;
            _unitOfWork = unitOfWork;
        }

        public MeetDetailDto Get(string meetId)
        {
            var meet = _repository.Find(meetId);
            var orgUsers = _orgUserRepository.FindAll();
            var meetDetail = new MeetDetailDto
            {
                Id = meet.Id,
                OwnerId = meet.OwnerId,
                Subject = meet.Subject,
                Body = meet.Body,
                StartTime = meet.StartTime,
                EndTime = meet.EndTime,
                Place = meet.Place,
                NeedFeedback = meet.NeedFeedback,
                State = meet.Status
            };
            //meetOwner设置
            var meetOwner = orgUsers.FirstOrDefault(i => i.Id == meet.OwnerId);
            if (meetOwner != null)
            {
                meetDetail.OwnerName = meetOwner.Name;
            }
            //MeetSignfors设置
            IList<KeyValueDto> signors = new List<KeyValueDto>();
            foreach (var signor in meet.MeetSignfors)
            {
                var signorUser = orgUsers.FirstOrDefault(i => i.Id == signor.SignorId);
                if (signorUser == null)
                {
                    continue;
                }
                signors.Add(new KeyValueDto
                {
                    Key = signorUser.Id,
                    Value = signorUser.Name
                });
            }
            meetDetail.Signors = signors;

            return meetDetail;
        }

        public int GetMyDistributeCount(string ownerId)
        {
            return _repository.FindCount(MeetState.WaitActivate, ownerId);
        }

        public PagedResult<MeetPagedDto> GetMyDistributeList(
            int pageIndex, int pageSize, string keywords, string ownerId, MeetState? state, DateTime? activateDate)
        {
            var searchCriteria = new SearchCriteria<Meet>();
            searchCriteria.AddFilterCriteria(t => t.OwnerId == ownerId);
            if (state != null)
            {
                searchCriteria.AddFilterCriteria(t => t.Status == (int) state);
            }
            if (activateDate != null)
            {
                var activateDateVal = activateDate.Value.Date;
                searchCriteria.AddFilterCriteria(t =>
                    (t.MeetActivate == null && t.StartTime.Date == activateDateVal)
                    || (t.MeetActivate != null && t.MeetActivate.StartTime.Date == activateDateVal));
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                searchCriteria.AddFilterCriteria(
                    t => t.Subject.Contains(keywords) ||
                         t.Body.Contains(keywords) ||
                         (t.MeetActivate == null && t.Place.Contains(keywords)) ||
                         (t.MeetActivate != null && t.MeetActivate.Place.Contains(keywords)));
            }
            searchCriteria.AddSortCriteria(
                new ExpressionSortCriteria<Meet, DateTime>(s => s.StartTime, SortDirection.Descending));
            searchCriteria.PagingCriteria = new PagingCriteria(pageIndex, pageSize);
            var pagedResult = _repository.FindPaged(searchCriteria);
            var resultList = pagedResult.Entities.Select(i => new MeetPagedDto
            {
                Id = i.Id,
                Subject = i.Subject,
                Place = i.Place,
                NeedFeedback = i.NeedFeedback,
                StartTime = i.StartTime,
                EndTime = i.EndTime
            }).ToList();
            return new PagedResult<MeetPagedDto>(pagedResult.Count, resultList);
        }

        public string AddMeet(MeetDetailDto dto)
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

        public void ChangeMeet(MeetDetailDto dto)
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
    }
}