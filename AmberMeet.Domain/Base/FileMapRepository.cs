using System;
using System.Collections.Generic;
using System.Linq;
using AmberMeet.Domain.Data;
using AmberMeet.Infrastructure.Search;
using AmberMeet.Infrastructure.Search.Extensions;
using AmberMeet.Infrastructure.Utilities;

namespace AmberMeet.Domain.Base
{
    internal class FileMapRepository : AbstractRepository, IFileMapRepository
    {
        public FileMap Find(string id)
        {
            return DataContext.FileMaps.First(t => t.Id == id);
        }

        public FileMap FindOrDefault(string id)
        {
            return DataContext.FileMaps.FirstOrDefault(t => t.Id == id);
        }

        public IEnumerable<FileMap> Find(SearchCriteria<FileMap> searchCriteria)
        {
            return DataContext.FileMaps.SearchBy(searchCriteria);
        }

        public IEnumerable<FileMap> Find(string[] ids)
        {
            ids = ids.Where(i => i != null && i != string.Empty).ToArray();
            return DataContext.FileMaps.Where(t => ids.Contains(t.Id));
        }

        public string Add(FileMap fileMap)
        {
            if (string.IsNullOrEmpty(fileMap.Id))
            {
                fileMap.Id = ConfigHelper.NewGuid;
            }
            if (string.IsNullOrEmpty(fileMap.Code))
            {
                fileMap.Code = fileMap.Id;
            }
            if (string.IsNullOrEmpty(fileMap.PhysicalFileName))
            {
                fileMap.PhysicalFileName = fileMap.FileName;
            }
            if (string.IsNullOrEmpty(fileMap.PhysicalFullName))
            {
                fileMap.PhysicalFullName = $"{ConfigHelper.CustomFilesDir}{fileMap.PhysicalFileName}";
            }
            var now = DateTime.Now;
            fileMap.CreateTime = now;
            fileMap.ModifiedTime = now;
            using (var dataContext = DataContext)
            {
                dataContext.FileMaps.InsertOnSubmit(fileMap);
                dataContext.SubmitChanges();
                return fileMap.Id;
            }
        }
    }
}