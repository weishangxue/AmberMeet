using System.Linq;
using AmberMeet.Domain.Data;

namespace AmberMeet.Domain.Organizations
{
    public class OrgUserDataService
    {
        public static OrgUser Find(string id)
        {
            return UnitOfWork.DataContext.OrgUsers.First(t => t.Id == id);
        }

        public static OrgUser FindAdmin()
        {
            return UnitOfWork.DataContext.OrgUsers.First(t => t.Account == "admin");
        }
    }
}