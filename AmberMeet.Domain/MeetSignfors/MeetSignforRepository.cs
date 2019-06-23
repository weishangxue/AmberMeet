using System.Linq;
using AmberMeet.Domain.Data;
using AmberMeet.Infrastructure.Search;
using AmberMeet.Infrastructure.Search.Extensions;
using AmberMeet.Infrastructure.Search.Paging;

namespace AmberMeet.Domain.MeetSignfors
{
    internal class MeetSignforRepository : AbstractRepository, IMeetSignforRepository
    {
        public MeetSignfor Find(string id)
        {
            return DataContext.MeetSignfors.First(t => t.Id == id);
        }

        public MeetSignfor First(string id)
        {
            return FactoryContext.MeetSignfors.First(t => t.Id == id);
        }

        public int FindCount(MeetSignforState state, string signorId)
        {
            using (var dataContext = DataContext)
            {
                if (string.IsNullOrEmpty(signorId))
                {
                    return dataContext.MeetSignfors.Count(t => t.State == (int) state);
                }
                return dataContext.MeetSignfors.Count(t => t.State == (int) state && t.SignorId == signorId);
            }
        }

        public PagedResult<MeetSignfor> FindPaged(SearchCriteria<MeetSignfor> searchCriteria)
        {
            var count = DataContext.MeetSignfors.FilterBy(searchCriteria.FilterCriterias).Count();
            var entities = DataContext.MeetSignfors.SearchBy(searchCriteria);
            return new PagedResult<MeetSignfor>(count, entities);
        }
    }
}