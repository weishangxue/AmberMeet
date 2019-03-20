using System.Collections.Generic;
using AmberMeet.Domain.Data;

namespace AmberMeet.AppService.FileMaps
{
    public interface IFileMapService
    {
        FileMap Get(string id);
        IList<FileMap> Get(string[] ids);
        FileMap Add(FileMap fileMap);
    }
}