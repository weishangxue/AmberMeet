using System.Collections.Generic;
using AmberMeet.Domain.Data;
using AmberMeet.Infrastructure.Search;
using AmberMeet.Infrastructure.Search.Paging;

namespace AmberMeet.Domain.Meets
{
    public interface IMeetRepository
    {
        Meet Find(string id);
        Meet First(string id);
        int FindCount(MeetState state, string ownerId);
        PagedResult<Meet> FindPaged(SearchCriteria<Meet> searchCriteria);
        IList<MeetSignfor> FindMeetSignfors(string[] meetIds);
        IList<OrgUser> FindMeetSignors(string[] signorIds);
        void Add(Meet dto);
    }
}