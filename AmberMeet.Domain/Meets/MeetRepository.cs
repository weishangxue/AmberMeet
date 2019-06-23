using System;
using System.Collections.Generic;
using System.Linq;
using AmberMeet.Domain.Data;
using AmberMeet.Infrastructure.Search;
using AmberMeet.Infrastructure.Search.Extensions;
using AmberMeet.Infrastructure.Search.Paging;
using AmberMeet.Infrastructure.Utilities;

namespace AmberMeet.Domain.Meets
{
    internal class MeetRepository : AbstractRepository, IMeetRepository
    {
        public Meet Find(string id)
        {
            return DataContext.Meets.First(t => t.Id == id);
        }

        public Meet First(string id)
        {
            return FactoryContext.Meets.First(t => t.Id == id);
        }

        public int FindCount(MeetState state, string ownerId)
        {
            using (var dataContext = DataContext)
            {
                if (string.IsNullOrEmpty(ownerId))
                {
                    return dataContext.Meets.Count(t => t.State == (int) state);
                }
                return dataContext.Meets.Count(t => t.State == (int) state && t.OwnerId == ownerId);
            }
        }

        public PagedResult<Meet> FindPaged(SearchCriteria<Meet> searchCriteria)
        {
            var count = DataContext.Meets.FilterBy(searchCriteria.FilterCriterias).Count();
            var entities = DataContext.Meets.SearchBy(searchCriteria);
            return new PagedResult<Meet>(count, entities);
        }

        public IList<MeetSignfor> FindMeetSignfors(string[] meetIds)
        {
            using (var dataContext = DataContext)
            {
                return dataContext.MeetSignfors.Where(t => meetIds.Contains(t.MeetId)).ToList();
            }
        }

        public IList<OrgUser> FindMeetSignors(string[] signorIds)
        {
            using (var dataContext = DataContext)
            {
                return dataContext.OrgUsers.Where(t => signorIds.Contains(t.Id)).ToList();
            }
        }

        public void Add(Meet dto)
        {
            if (string.IsNullOrEmpty(dto.Id))
            {
                dto.Id = ConfigHelper.NewGuid;
            }
            dto.CreateTime = dto.ModifiedTime = DateTime.Now;
            using (var dataContext = DataContext)
            {
                dataContext.Meets.InsertOnSubmit(dto);
                dataContext.SubmitChanges();
            }
            //FactoryContext.Meets.InsertOnSubmit(dto);
        }
    }
}