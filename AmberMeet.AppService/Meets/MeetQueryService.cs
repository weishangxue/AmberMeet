using System;
using System.Collections.Generic;
using System.Linq;
using AmberMeet.Domain.Data;
using AmberMeet.Domain.Meets;
using AmberMeet.Domain.Organizations;
using AmberMeet.Dto;
using AmberMeet.Dto.Meets;
using AmberMeet.Infrastructure.Exceptions;
using AmberMeet.Infrastructure.Search;
using AmberMeet.Infrastructure.Search.Paging;
using AmberMeet.Infrastructure.Search.Sort;

namespace AmberMeet.AppService.Meets
{
    internal class MeetQueryService
    {
        private readonly IOrgUserRepository _orgUserRepository;
        private readonly IMeetRepository _repository;

        public MeetQueryService(
            IMeetRepository repository,
            IOrgUserRepository orgUserRepository)
        {
            _repository = repository;
            _orgUserRepository = orgUserRepository;
        }

        public MeetDto GetDetail(string meetId)
        {
            if (string.IsNullOrEmpty(meetId))
            {
                throw new ArgumentNullException(ExMessage.MustNotBeNullOrEmpty(nameof(meetId)));
            }
            var meet = _repository.Find(meetId);
            var orgUsers = _orgUserRepository.FindAll();
            var meetDetail = new MeetDto
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

        public PagedResult<MeetPagedDto> GetPaged(
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
    }
}