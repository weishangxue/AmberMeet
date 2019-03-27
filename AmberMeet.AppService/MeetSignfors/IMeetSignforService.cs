using System;
using System.Collections.Generic;
using AmberMeet.Domain.MeetSignfors;
using AmberMeet.Dto.MeetSignfors;
using AmberMeet.Infrastructure.Search.Paging;

namespace AmberMeet.AppService.MeetSignfors
{
    public interface IMeetSignforService
    {
        MeetSignforDto GetDetail(string signforId);

        int GetMyWaitSignforCount(string signorId);

        int GetMyAlreadySignedCount(string signorId);

        PagedResult<MeetSignforPagedDto> GetWaitSignfors(
            int pageIndex, int pageSize, string keywords, string signorId, DateTime? activateDate);

        PagedResult<MeetSignforPagedDto> GetAlreadySigneds(
            int pageIndex, int pageSize, string keywords, string signorId, DateTime? activateDate);

        PagedResult<MeetSignforPagedDto> GetAllSignfors(
            int pageIndex, int pageSize, string keywords,
            string signorId, DateTime? activateDate, MeetSignforState? state);

        IList<MeetSignforDto> GetMeetSubSignfors(string meetId, string keywords);

        void Signfor(string signforId, string feedback);
    }
}