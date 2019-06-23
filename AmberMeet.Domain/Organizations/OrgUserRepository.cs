using System;
using System.Linq;
using AmberMeet.Domain.Data;
using AmberMeet.Infrastructure.Search;
using AmberMeet.Infrastructure.Search.Extensions;
using AmberMeet.Infrastructure.Search.Paging;
using AmberMeet.Infrastructure.Utilities;

namespace AmberMeet.Domain.Organizations
{
    internal class OrgUserRepository : AbstractRepository, IOrgUserRepository
    {
        public OrgUser Find(string id)
        {
            return DataContext.OrgUsers.First(t => t.Id == id);
        }

        public OrgUser FindByAccount(string loginName)
        {
            return DataContext.OrgUsers.FirstOrDefault(t => t.Account == loginName);
        }

        public OrgUser FindByCode(string code)
        {
            return DataContext.OrgUsers.FirstOrDefault(t => t.Code == code);
        }

        public bool AnyLoginName(string loginName, string excludedId)
        {
            if (!string.IsNullOrEmpty(excludedId))
                return DataContext.OrgUsers.Any(t => t.Account == loginName && t.Id != excludedId);
            return DataContext.OrgUsers.Any(t => t.Account == loginName);
        }

        public bool AnyCode(string code, string excludedId)
        {
            if (!string.IsNullOrEmpty(excludedId))
                return DataContext.OrgUsers.Any(t => t.Code == code && t.Id != excludedId);
            return DataContext.OrgUsers.Any(t => t.Code == code);
        }

        public IQueryable<OrgUser> FindAll()
        {
            return DataContext.OrgUsers.Where(t => t.State == (int) UserState.Normal && t.Code != "adminitrator");
        }

        public PagedResult<OrgUser> FindPaged(SearchCriteria<OrgUser> searchCriteria)
        {
            //searchCriteria.AddFilterCriteria(t => t.Status != (int) UserState.Cancle);
            var count = DataContext.OrgUsers.FilterBy(searchCriteria.FilterCriterias).Count();
            var entities = DataContext.OrgUsers.SearchBy(searchCriteria);
            return new PagedResult<OrgUser>(count, entities);
        }

        public void Add(OrgUser dto)
        {
            if (string.IsNullOrEmpty(dto.Id))
            {
                dto.Id = ConfigHelper.NewGuid;
            }
            dto.CreateTime = dto.ModifiedTime = DateTime.Now;
            using (var dataContext = DataContext)
            {
                dataContext.OrgUsers.InsertOnSubmit(dto);
                dataContext.SubmitChanges();
            }
        }

        public void Modify(OrgUser dto, string id)
        {
            using (var dataContext = DataContext)
            {
                var user = dataContext.OrgUsers.First(t => t.Id == id);
                user.Code = dto.Code;
                user.Name = dto.Name;
                user.Account = dto.Account;
                user.Mobile = dto.Mobile;
                user.Mail = dto.Mail;
                user.Sex = dto.Sex;
                user.Birthday = dto.Birthday;
                user.State = dto.State;
                user.ModifiedTime = DateTime.Now;
                dataContext.SubmitChanges();
            }
        }

        public void ModifyPassword(string userId, string password)
        {
            using (var dataContext = DataContext)
            {
                var user = dataContext.OrgUsers.First(t => t.Id == userId);
                user.Password = password;
                user.ModifiedTime = DateTime.Now;
                dataContext.SubmitChanges();
            }
        }

        public void ModifyRole(string userId, UserRole role)
        {
            using (var dataContext = DataContext)
            {
                var user = dataContext.OrgUsers.First(t => t.Id == userId);
                user.Role = (int) role;
                user.ModifiedTime = DateTime.Now;
                dataContext.SubmitChanges();
            }
        }

        public void ModifyState(string userId, UserState state)
        {
            using (var dataContext = DataContext)
            {
                var user = dataContext.OrgUsers.First(t => t.Id == userId);
                user.State = (int) state;
                user.ModifiedTime = DateTime.Now;
                dataContext.SubmitChanges();
            }
        }
    }
}