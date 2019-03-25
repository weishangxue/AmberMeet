using System;
using System.Collections.Generic;
using System.Linq;
using AmberMeet.Domain.Data;
using AmberMeet.Domain.Meets;
using AmberMeet.Domain.MeetSignfors;
using AmberMeet.Domain.Organizations;
using AmberMeet.Dto;
using AmberMeet.Dto.Meets;
using AmberMeet.Infrastructure.Exceptions;
using AmberMeet.Infrastructure.Search;
using AmberMeet.Infrastructure.Search.Paging;
using AmberMeet.Infrastructure.Search.Sort;
using AutoMapper;

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
            //map
            var meet = _repository.Find(meetId);
            var meetDto = Mapper.Map<MeetDto>(meet);
            //meetOwner设置
            var meetOwner = _orgUserRepository.Find(meet.OwnerId);
            if (meetOwner != null)
            {
                meetDto.OwnerName = meetOwner.Name;
            }
            //MeetSignfors设置
            IList<KeyValueDto> signors = new List<KeyValueDto>();
            foreach (var signor in meet.MeetSignfors)
            {
                signors.Add(new KeyValueDto
                {
                    Key = signor.OrgUser.Id,
                    Value = signor.OrgUser.Name
                });
            }
            meetDto.Signors = signors;

            return meetDto;
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
            var resultList = pagedResult.Entities.Select(i => Mapper.Map<MeetPagedDto>(i)).ToList();
            //Signfor Extensions
            var signfors = _repository.FindMeetSignfors(resultList.Select(i => i.Id).ToArray());
            var signors = _repository.FindMeetSignors(signfors.Select(i => i.SignorId).ToArray());
            foreach (var resultDto in resultList)
            {
                var resultSignfors = signfors.Where(i => i.MeetId == resultDto.Id).ToList();
                var resultWaitSigns = resultSignfors.Where(i => i.Status == (int) MeetSignorState.WaitSign).ToList();
                var resultAlreadySigneds = resultSignfors.Where(i => i.Status == (int) MeetSignorState.AlreadySigned)
                    .ToList();

                var resultWaitSignUsers =
                    signors.Where(t => resultWaitSigns.Select(i => i.SignorId).ToArray().Contains(t.Id)).ToList();
                var resultAlreadySignUsers =
                    signors.Where(t => resultAlreadySigneds.Select(i => i.SignorId).ToArray().Contains(t.Id)).ToList();
                //WaitSignfor
                resultDto.WaitSignforCount = resultWaitSignUsers.Count;
                var waitSignorNamesStr = string.Empty;
                foreach (var resultWaitSignor in resultWaitSignUsers)
                {
                    waitSignorNamesStr = $"{waitSignorNamesStr}{resultWaitSignor.Name};";
                }
                resultDto.WaitSignorNamesStr = waitSignorNamesStr;
                //AlreadySigned
                resultDto.AlreadySignedCount = resultAlreadySignUsers.Count;
                var alreadySignorNamesStr = string.Empty;
                foreach (var resultAlreadySignor in resultAlreadySignUsers)
                {
                    alreadySignorNamesStr = $"{alreadySignorNamesStr}{resultAlreadySignor.Name};";
                }
                resultDto.AlreadySignorNamesStr = alreadySignorNamesStr;
            }
            return new PagedResult<MeetPagedDto>(pagedResult.Count, resultList);
        }
    }
}