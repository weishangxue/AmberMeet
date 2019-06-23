using System.Collections.Generic;
using AmberMeet.Domain.Data;
using AmberMeet.Infrastructure.Search;

namespace AmberMeet.Domain.Base
{
    public interface IFileMapRepository
    {
        FileMap Find(string id);
        FileMap FindOrDefault(string id);
        IEnumerable<FileMap> Find(SearchCriteria<FileMap> searchCriteria);
        IEnumerable<FileMap> Find(string[] ids);
        string Add(FileMap fileMap);
    }
}