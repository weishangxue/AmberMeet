using System;
using AmberMeet.Domain.Meets;
using AmberMeet.Dto.Meets;
using AmberMeet.Infrastructure.Search.Paging;

namespace AmberMeet.AppService.Meets
{
    public interface IMeetQueryService
    {
        MeetDto GetDetail(string meetId);

        int GetMyDistributeCount(string ownerId);

        int GetMyActivateCount(string ownerId);

        PagedResult<MeetWaitActivatePagedDto> GetWaitActivates(
            int pageIndex, int pageSize, string keywords, string ownerId, DateTime? activateDate);

        PagedResult<MeetPagedDto> GetActivates(
            int pageIndex, int pageSize, string keywords, string ownerId, DateTime? activateDate);

        PagedResult<MeetPagedDto> GetAllDistributes(
            int pageIndex, int pageSize, string keywords, string ownerId, DateTime? activateDate, MeetState? state);
    }
}