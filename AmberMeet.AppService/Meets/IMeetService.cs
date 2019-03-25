using System;
using AmberMeet.Dto.Meets;
using AmberMeet.Infrastructure.Search.Paging;

namespace AmberMeet.AppService.Meets
{
    public interface IMeetService
    {
        MeetDto GetDetail(string meetId);

        int GetMyDistributeCount(string ownerId);

        int GetMyActivateCount(string ownerId);

        PagedResult<MeetPagedDto> GetMyDistributes(
            int pageIndex, int pageSize, string keywords, string ownerId, DateTime? activateDate);

        string AddMeet(MeetDto dto);

        void ChangeMeet(MeetDto dto);

        void ActivateMeet(string meetId, DateTime startTime, DateTime? endTime, string place);
    }
}