using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AmberMeet.Infrastructure.Search.Paging;
using AmberMeet.Infrastructure.Search.Sort;

namespace AmberMeet.Infrastructure.Search
{
    /// <summary>
    ///     搜索参数标准
    /// </summary>
    /// <typeparam name="TEntity">TEntity</typeparam>
    public class SearchCriteria<TEntity>
    {
        public SearchCriteria()
        {
            FilterCriterias = new List<Expression<Func<TEntity, bool>>>();
            SortCriterias = new List<ISortCriteria<TEntity>>();
        }

        public IList<Expression<Func<TEntity, bool>>> FilterCriterias { get; }

        public IList<ISortCriteria<TEntity>> SortCriterias { get; }

        public PagingCriteria PagingCriteria { get; set; }

        public void AddFilterCriteria(Expression<Func<TEntity, bool>> filter)
        {
            FilterCriterias.Add(filter);
        }

        public void AddSortCriteria(ISortCriteria<TEntity> sortCriteria)
        {
            SortCriterias.Add(sortCriteria);
        }
    }
}