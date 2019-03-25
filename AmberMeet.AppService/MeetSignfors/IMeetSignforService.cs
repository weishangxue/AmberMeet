using System;
using AmberMeet.Dto.MeetSignfors;
using AmberMeet.Infrastructure.Search.Paging;

namespace AmberMeet.AppService.MeetSignfors
{
    public interface IMeetSignforService
    {
        MeetSignforDto GetDetail(string signforId);

        int GetMyWaitSignforCount(string signorId);

        int GetMyAlreadySignedCount(string signorId);

        PagedResult<MeetSignforPagedDto> GetMyWaitSignfors(
            int pageIndex, int pageSize, string keywords, string signorId, DateTime? activateDate);

        void Signfor(string signforId, string feedback);
    }
}