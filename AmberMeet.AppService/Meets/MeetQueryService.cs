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
using AmberMeet.Infrastructure.Serialization;
using AutoMapper;

namespace AmberMeet.AppService.Meets
{
    internal class MeetQueryService : IMeetQueryService
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
            //MeetActivate
            if (meet.MeetActivate != null)
            {
                meetDto.StartTime = meet.MeetActivate.StartTime;
                meetDto.EndTime = meet.MeetActivate.EndTime;
                meetDto.Place = meet.MeetActivate.Place;
            }
            return meetDto;
        }

        public int GetMyDistributeCount(string ownerId)
        {
            return _repository.FindCount(MeetState.WaitActivate, ownerId);
        }

        public int GetMyActivateCount(string ownerId)
        {
            return _repository.FindCount(MeetState.Activate, ownerId);
        }

        public PagedResult<MeetWaitActivatePagedDto> GetWaitActivates(
            int pageIndex, int pageSize, string keywords, string ownerId, DateTime? activateDate)
        {
            var pagedResult = GetPaged(pageIndex, pageSize, keywords, ownerId, activateDate, MeetState.WaitActivate);
            /*******************MeetWaitActivatePagedDto*********************************************/
            var resultList = pagedResult.Entities.Select(i => Mapper.Map<MeetWaitActivatePagedDto>(i)).ToList();
            var signfors = _repository.FindMeetSignfors(resultList.Select(i => i.Id).ToArray());
            var signors = _repository.FindMeetSignors(signfors.Select(i => i.SignorId).ToArray());
            foreach (var resultDto in resultList)
            {
                var resultSignfors = signfors.Where(i => i.MeetId == resultDto.Id).ToList();
                var resultWaitSigns = resultSignfors.Where(i => i.Status == (int) MeetSignforState.WaitSign).ToList();
                var resultAlreadySigneds =
                    resultSignfors.Where(i => i.Status == (int) MeetSignforState.AlreadySigned).ToList();

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
            return new PagedResult<MeetWaitActivatePagedDto>(pagedResult.Count, resultList);
        }

        public PagedResult<MeetPagedDto> GetActivates(
            int pageIndex, int pageSize, string keywords, string ownerId, DateTime? activateDate)
        {
            var pagedResult = GetPaged(pageIndex, pageSize, keywords, ownerId, activateDate, MeetState.Activate);
            //MeetPagedDto-Activates
            return GetMeetPagedDtoPagedResult(pagedResult);
        }

        public PagedResult<MeetPagedDto> GetAllDistributes(
            int pageIndex, int pageSize, string keywords, string ownerId, DateTime? activateDate, MeetState? state)
        {
            var pagedResult = GetPaged(pageIndex, pageSize, keywords, ownerId, activateDate, state);
            //MeetPagedDto-AllDistributes
            return GetMeetPagedDtoPagedResult(pagedResult);
        }

        /// <summary>
        ///     PagedResult from repository
        /// </summary>
        private PagedResult<Meet> GetPaged(
            int pageIndex, int pageSize, string keywords, string ownerId, DateTime? activateDate, MeetState? state)
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
            return pagedResult;
        }

        /// <summary>
        ///     return map to MeetPagedDto
        /// </summary>
        private PagedResult<MeetPagedDto> GetMeetPagedDtoPagedResult(PagedResult<Meet> pagedResult)
        {
            var resultList = pagedResult.Entities.Select(i => Mapper.Map<MeetPagedDto>(i)).ToList();
            var signfors = _repository.FindMeetSignfors(resultList.Select(i => i.Id).ToArray());
            var signors = _repository.FindMeetSignors(signfors.Select(i => i.SignorId).ToArray());
            foreach (var resultDto in resultList)
            {
                var resultSignfors = signfors.Where(i => i.MeetId == resultDto.Id).ToList();
                IList<string> resultSignorNames = new List<string>();
                foreach (var resultSignfor in resultSignfors)
                {
                    var signStateStr = ((MeetSignforState) resultSignfor.Status).ToEnumText();
                    var signor = signors.FirstOrDefault(i => i.Id == resultSignfor.SignorId);
                    if (signor == null)
                    {
                        continue;
                    }
                    var resultSignorName = $"{signor.Name}-{signStateStr}";
                    resultSignorNames.Add(resultSignorName);
                }
                resultDto.SignorCount = resultSignfors.Count;
                resultDto.SignorNames = resultSignorNames.ToArray();
            }
            return new PagedResult<MeetPagedDto>(pagedResult.Count, resultList);
        }
    }
}