using System;
using System.Linq;
using AmberMeet.Domain.Data;
using AmberMeet.Domain.MeetSignfors;
using AmberMeet.Dto.MeetSignfors;
using AmberMeet.Infrastructure.Search;
using AmberMeet.Infrastructure.Search.Paging;
using AmberMeet.Infrastructure.Search.Sort;

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

        public int GetMyWaitSignforCount(string signorId)
        {
            return _repository.FindCount(MeetSignorState.WaitSign, signorId);
        }

        public int GetMyAlreadySignedCount(string signorId)
        {
            return _repository.FindCount(MeetSignorState.AlreadySigned, signorId);
        }

        public PagedResult<MeetSignforPagedDto> GetMyWaitSignfors(
            int pageIndex, int pageSize, string keywords, string signorId, DateTime? activateDate)
        {
            var searchCriteria = new SearchCriteria<MeetSignfor>();
            searchCriteria.AddFilterCriteria(t => t.SignorId == signorId);
            searchCriteria.AddFilterCriteria(t => t.Status == (int) MeetSignorState.WaitSign);

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