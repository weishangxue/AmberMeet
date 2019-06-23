using System.Linq;
using AmberMeet.Domain.Data;
using AmberMeet.Infrastructure.Search;
using AmberMeet.Infrastructure.Search.Paging;

namespace AmberMeet.Domain.Organizations
{
    public interface IOrgUserRepository
    {
        OrgUser Find(string id);
        OrgUser FindByAccount(string loginName);
        OrgUser FindByCode(string code);
        bool AnyLoginName(string loginName, string excludedId);
        bool AnyCode(string code, string excludedId);
        IQueryable<OrgUser> FindAll();
        PagedResult<OrgUser> FindPaged(SearchCriteria<OrgUser> searchCriteria);
        void Add(OrgUser dto);
        void Modify(OrgUser dto, string id);
        void ModifyPassword(string userId, string password);
        void ModifyRole(string userId, UserRole role);
        void ModifyState(string userId, UserState state);
    }
}