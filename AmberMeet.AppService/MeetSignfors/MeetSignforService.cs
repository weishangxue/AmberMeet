using System;
using System.Collections.Generic;
using System.Linq;
using AmberMeet.Domain.Data;
using AmberMeet.Domain.MeetSignfors;
using AmberMeet.Dto.MeetSignfors;
using AmberMeet.Infrastructure.Exceptions;
using AmberMeet.Infrastructure.Search;
using AmberMeet.Infrastructure.Search.Paging;
using AmberMeet.Infrastructure.Search.Sort;
using AutoMapper;

namespace AmberMeet.AppService.MeetSignfors
{
    internal class MeetSignforService : IMeetSignforService
    {
        private readonly IMeetSignforRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public MeetSignforService(IMeetSignforRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public MeetSignforDto GetDetail(string signforId)
        {
            if (string.IsNullOrEmpty(signforId))
            {
                throw new ArgumentNullException(ExMessage.MustNotBeNullOrEmpty(nameof(signforId)));
            }
            //map
            var signfor = _repository.Find(signforId);
            return Mapper.Map<MeetSignforDto>(signfor);
        }

        public int GetMyWaitSignforCount(string signorId)
        {
            return _repository.FindCount(MeetSignforState.WaitSign, signorId);
        }

        public int GetMyAlreadySignedCount(string signorId)
        {
            return _repository.FindCount(MeetSignforState.AlreadySigned, signorId);
        }

        public PagedResult<MeetSignforPagedDto> GetWaitSignfors(
            int pageIndex, int pageSize, string keywords, string signorId, DateTime? activateDate)
        {
            return GetPaged(
                pageIndex, pageSize, keywords, signorId, activateDate, MeetSignforState.WaitSign);
        }

        public PagedResult<MeetSignforPagedDto> GetAlreadySigneds(
            int pageIndex, int pageSize, string keywords, string signorId, DateTime? activateDate)
        {
            return GetPaged(
                pageIndex, pageSize, keywords, signorId, activateDate, MeetSignforState.AlreadySigned);
        }

        public PagedResult<MeetSignforPagedDto> GetAllSignfors(
            int pageIndex, int pageSize, string keywords,
            string signorId, DateTime? activateDate, MeetSignforState? state)
        {
            return GetPaged(pageIndex, pageSize, keywords, signorId, activateDate, state);
        }

        public IList<MeetSignforDto> GetMeetSubSignfors(string meetId, string keywords)
        {
            var searchCriteria = new SearchCriteria<MeetSignfor>();
            searchCriteria.AddFilterCriteria(t => t.MeetId == meetId);
            if (!string.IsNullOrEmpty(keywords))
            {
                searchCriteria.AddFilterCriteria(
                    t => t.Feedback.Contains(keywords) ||
                         t.Meet.Subject.Contains(keywords) ||
                         t.Meet.Body.Contains(keywords) ||
                         (t.OrgUser != null && t.OrgUser.Name.Contains(keywords)) ||
                         (t.Meet.MeetActivate == null && t.Meet.Place.Contains(keywords)) ||
                         (t.Meet.MeetActivate != null && t.Meet.MeetActivate.Place.Contains(keywords)));
            }
            var pagedResult = _repository.FindPaged(searchCriteria);
            return pagedResult.Entities.Select(i => Mapper.Map<MeetSignforDto>(i)).ToList();
        }

        public void Signfor(string signforId, string feedback)
        {
            if (string.IsNullOrEmpty(signforId))
            {
                throw new ArgumentNullException(ExMessage.MustNotBeNullOrEmpty(nameof(signforId)));
            }
            var meetSignfor = _repository.First(signforId);
            if (string.IsNullOrEmpty(feedback) && meetSignfor.Meet.NeedFeedback)
            {
                throw new ArgumentNullException(ExMessage.MustNotBeNullOrEmpty(nameof(feedback)));
            }
            meetSignfor.Feedback = feedback;
            meetSignfor.SignTime = DateTime.Now;
            meetSignfor.State = (int) MeetSignforState.AlreadySigned;
            _unitOfWork.Commit();
        }

        /// <summary>
        ///     GetPaged from repository
        /// </summary>
        private PagedResult<MeetSignforPagedDto> GetPaged(
            int pageIndex, int pageSize, string keywords,
            string signorId, DateTime? activateDate, MeetSignforState? state)
        {
            var searchCriteria = new SearchCriteria<MeetSignfor>();

            if (!string.IsNullOrEmpty(signorId))
            {
                searchCriteria.AddFilterCriteria(t => t.SignorId == signorId);
            }

            if (state != null)
            {
                searchCriteria.AddFilterCriteria(t => t.State == (int) state);
            }

            if (activateDate != null)
            {
                var activateDateVal = activateDate.Value.Date;
                searchCriteria.AddFilterCriteria(t =>
                    (t.Meet.MeetActivate == null && t.Meet.StartTime.Date == activateDateVal)
                    || (t.Meet.MeetActivate != null && t.Meet.MeetActivate.StartTime.Date == activateDateVal));
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                searchCriteria.AddFilterCriteria(
                    t => t.Meet.Subject.Contains(keywords) ||
                         t.Meet.Body.Contains(keywords) ||
                         (t.Meet.MeetActivate == null && t.Meet.Place.Contains(keywords)) ||
                         (t.Meet.MeetActivate != null && t.Meet.MeetActivate.Place.Contains(keywords)));
            }
            searchCriteria.AddSortCriteria(
                new ExpressionSortCriteria<MeetSignfor, DateTime>(s => s.Meet.StartTime, SortDirection.Descending));
            searchCriteria.PagingCriteria = new PagingCriteria(pageIndex, pageSize);
            var pagedResult = _repository.FindPaged(searchCriteria);
            return PagedResult(pagedResult);
        }

        /// <summary>
        ///     map to MeetSignforPagedDto PagedResult
        /// </summary>
        private PagedResult<MeetSignforPagedDto> PagedResult(PagedResult<MeetSignfor> pagedResult)
        {
            var resultList = pagedResult.Entities.Select(i => new MeetSignforPagedDto
            {
                Id = i.Id,
                MeetId = i.MeetId,
                Subject = i.Meet.Subject,
                Place = i.Meet.Place,
                NeedFeedback = i.Meet.NeedFeedback,
                StartTime = i.Meet.StartTime,
                EndTime = i.Meet.EndTime
            }).ToList();
            return new PagedResult<MeetSignforPagedDto>(pagedResult.Count, resultList);
        }
    }
}