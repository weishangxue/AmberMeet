using System;
using System.Collections.Generic;
using System.Linq;
using AmberMeet.Domain.Base;
using AmberMeet.Domain.Data;
using AmberMeet.Infrastructure.Exceptions;
using AmberMeet.Infrastructure.Utilities;

namespace AmberMeet.AppService.FileMaps
{
    internal class FileMapService : IFileMapService
    {
        private readonly IFileMapRepository _repository;

        public FileMapService(IFileMapRepository repository)
        {
            _repository = repository;
        }

        public FileMap Get(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(ExMessage.MustNotBeNullOrEmpty(nameof(id)));
            }
            return _repository.Find(id);
        }

        public IList<FileMap> Get(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                throw new ArgumentNullException(ExMessage.MustNotBeNull(nameof(ids)));
            }
            return _repository.Find(ids).ToList();
        }

        public FileMap Add(FileMap fileMap)
        {
            if (string.IsNullOrEmpty(fileMap.FileName))
            {
                throw new ArgumentNullException(ExMessage.MustNotBeNullOrEmpty(nameof(fileMap.FileName)));
            }
            if (fileMap == null)
            {
                throw new ArgumentNullException(ExMessage.MustNotBeNull(nameof(fileMap)));
            }
            fileMap.Id = ConfigHelper.NewGuid;
            fileMap.FileExtensionName =
                fileMap.FileName.Substring(fileMap.FileName.LastIndexOf(".", StringComparison.Ordinal));
            _repository.Add(fileMap);
            return fileMap;
        }
    }
}