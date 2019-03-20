using System;
using AmberMeet.Domain.Organizations;
using AmberMeet.Infrastructure.Serialization;
using AmberMeet.Infrastructure.Utilities;

namespace AmberMeet.Dto
{
    public class OrgUserPagedDto
    {
        public string Id { get; set; }
        public string LoginName { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Mobile { get; set; }
        public int Sex { get; set; }
        public DateTime? Birthday { get; set; }
        public int Role { get; set; }

        /***Extension*****************************************************************************/
        public string SexText
        {
            get { return ((UserSex) Sex).ToEnumText(); }
        }

        public string BirthdayText
        {
            get { return FormatHelper.GetIsoDateString(Birthday); }
        }

        public string RoleText
        {
            get { return ((UserRole) Sex).ToEnumText(); }
        }
    }
}