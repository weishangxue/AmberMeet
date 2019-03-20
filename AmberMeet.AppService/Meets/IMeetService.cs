using System;
using AmberMeet.Domain.Meets;
using AmberMeet.Dto;
using AmberMeet.Infrastructure.Search.Paging;

namespace AmberMeet.AppService.Meets
{
    public interface IMeetService
    {
        MeetDetailDto Get(string meetId);

        int GetMyDistributeCount(string ownerId);

        PagedResult<MeetPagedDto> GetMyDistributeList(
            int pageIndex, int pageSize, string keywords, string ownerId, MeetState? state, DateTime? activateDate);

        string AddMeet(MeetDetailDto dto);

        void ChangeMeet(MeetDetailDto dto);
    }
}