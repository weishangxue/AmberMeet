using AmberMeet.Domain.Data;
using AmberMeet.Infrastructure.Search;
using AmberMeet.Infrastructure.Search.Paging;

namespace AmberMeet.Domain.MeetSignfors
{
    public interface IMeetSignforRepository
    {
        MeetSignfor Find(string id);
        MeetSignfor First(string id);
        int FindCount(MeetSignforState state, string signorId);
        PagedResult<MeetSignfor> FindPaged(SearchCriteria<MeetSignfor> searchCriteria);
    }
}