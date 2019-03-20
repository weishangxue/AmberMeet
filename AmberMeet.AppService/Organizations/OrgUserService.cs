using System;
using System.Collections.Generic;
using System.Linq;
using AmberMeet.Domain.Data;
using AmberMeet.Domain.Organizations;
using AmberMeet.Dto;
using AmberMeet.Infrastructure.Exceptions;
using AmberMeet.Infrastructure.Search;
using AmberMeet.Infrastructure.Search.Paging;
using AmberMeet.Infrastructure.Search.Sort;
using AmberMeet.Infrastructure.Utilities;

namespace AmberMeet.AppService.Organizations
{
    internal class OrgUserService : IOrgUserService
    {
        private readonly IOrgUserRepository _repository;

        public OrgUserService(IOrgUserRepository repository)
        {
            _repository = repository;
        }

        public OrgUser Get(string userId)
        {
            return _repository.Find(userId);
        }

        public OrgUser GetByAccount(string loginName)
        {
            return _repository.FindByAccount(loginName);
        }

        public bool AnyCode(string code, string excludedId)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(ExMessage.MustNotBeNullOrEmpty(nameof(code)));
            }
            return _repository.AnyCode(code, excludedId);
        }

        public bool AnyLoginName(string loginName, string excludedId)
        {
            if (string.IsNullOrEmpty(loginName))
            {
                throw new ArgumentNullException(ExMessage.MustNotBeNullOrEmpty(nameof(loginName)));
            }
            return _repository.AnyLoginName(loginName, excludedId);
        }

        public PagedResult<OrgUserPagedDto> GetPaged(int pageIndex, int pageSize, int? state, string keywords)
        {
            var searchCriteria = new SearchCriteria<OrgUser>();
            if (state != null)
            {
                searchCriteria.AddFilterCriteria(t => t.Status == state);
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                searchCriteria.AddFilterCriteria(t => t.Name.Contains(keywords) ||
                                                      t.Account.Contains(keywords) ||
                                                      t.Code.Contains(keywords) ||
                                                      t.Mobile.Contains(keywords));
            }
            //排除admin
            searchCriteria.AddFilterCriteria(t => t.Code != "adminitrator");
            searchCriteria.AddSortCriteria(
                new ExpressionSortCriteria<OrgUser, string>(s => s.Name, SortDirection.Ascending));
            searchCriteria.AddSortCriteria(
                new ExpressionSortCriteria<OrgUser, DateTime>(s => s.CreateTime, SortDirection.Descending));
            searchCriteria.PagingCriteria = new PagingCriteria(pageIndex, pageSize);
            var pagedResult = _repository.FindPaged(searchCriteria);
            var resultList = pagedResult.Entities.Select(i => new OrgUserPagedDto
            {
                Id = i.Id,
                LoginName = i.Account,
                Name = i.Name,
                Mail = i.Mail,
                Mobile = i.Mobile,
                Sex = i.Sex,
                Birthday = i.Birthday,
                Role = i.Role
            }).ToList();
            return new PagedResult<OrgUserPagedDto>(pagedResult.Count, resultList);
        }

        public IList<OrgUserPagedDto> GetAll()
        {
            return _repository.FindAll().Select(i => new OrgUserPagedDto
            {
                Id = i.Id,
                LoginName = i.Account,
                Name = i.Name,
                Mail = i.Mail,
                Mobile = i.Mobile,
                Sex = i.Sex,
                Birthday = i.Birthday,
                Role = i.Role
            }).ToList();
        }

        public string AddUser(OrgUser dto)
        {
            var user = _repository.FindByAccount(dto.Account);
            if (user != null)
            {
                throw new NonUniqueException($"user account must be unique,account={dto.Account}");
            }
            user = _repository.FindByCode(dto.Code);
            if (user != null)
            {
                throw new NonUniqueException($"user code must be unique,code={dto.Code}");
            }
            dto.Id = ConfigHelper.NewGuid;
            dto.Password = CryptographicHelper.Hash(ConfigHelper.DefaultUserPwd);
            dto.Status = (int) UserState.Normal;
            if (string.IsNullOrEmpty(dto.Code))
            {
                dto.Code = dto.Id;
            }
            _repository.Add(dto);
            return dto.Id;
        }

        public void ChangeUser(OrgUser dto)
        {
            if (string.IsNullOrEmpty(dto.Id))
            {
                throw new ArgumentNullException(ExMessage.MustNotBeNullOrEmpty(nameof(dto.Id)));
            }
            var user = _repository.FindByAccount(dto.Account);
            if (user != null && user.Id != dto.Id)
            {
                throw new NonUniqueException($"user account must be unique,account={dto.Account}");
            }
            user = _repository.FindByCode(dto.Code);
            if (user != null && user.Code != dto.Code)
            {
                throw new NonUniqueException($"user code must be unique,code={dto.Code}");
            }
            _repository.Modify(dto, dto.Id);
        }

        public void ChangeUserPassword(string id, string password)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(ExMessage.MustNotBeNullOrEmpty(nameof(id)));
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(ExMessage.MustNotBeNullOrEmpty(nameof(password)));
            }
            password = CryptographicHelper.Hash(password);
            _repository.ModifyPassword(id, password);
        }

        public void CancleUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(ExMessage.MustNotBeNullOrEmpty(id));
            }
            _repository.ModifyState(id, UserState.Cancle);
        }

        public void ReactivationUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new Exception(ExMessage.MustNotBeNullOrEmpty(nameof(id)));
            }
            _repository.ModifyState(id, UserState.Normal);
        }
    }
}