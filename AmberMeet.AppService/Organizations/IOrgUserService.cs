using System.Collections.Generic;
using AmberMeet.Domain.Data;
using AmberMeet.Dto;
using AmberMeet.Infrastructure.Search.Paging;

namespace AmberMeet.AppService.Organizations
{
    public interface IOrgUserService
    {
        OrgUser Get(string userId);

        OrgUser GetByAccount(string loginName);

        bool AnyCode(string code, string excludedId);

        bool AnyLoginName(string loginName, string excludedId);

        PagedResult<OrgUserPagedDto> GetPaged(int pageIndex, int pageSize, int? state, string keywords);

        IList<OrgUserPagedDto> GetAll();

        string AddUser(OrgUser dto);

        void ChangeUser(OrgUser dto);

        /// <summary>
        ///     修改用户密码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="password">明文密码</param>
        void ChangeUserPassword(string id, string password);

        void CancleUser(string id);

        void ReactivationUser(string id);
    }
}