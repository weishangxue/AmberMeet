using System;
using AmberMeet.Domain.Organizations;
using AmberMeet.Infrastructure.Serialization;
using AmberMeet.Infrastructure.Utilities;

namespace AmberMeet.Dto.Organizations
{
    /// <summary>
    ///     用户详细
    /// </summary>
    public class OrgUserDto
    {
        public string Id { get; set; }
        public int IdentityId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public string Mobile { get; set; }
        public int Sex { get; set; }
        public DateTime? Birthday { get; set; }
        public int Role { get; set; }
        public int State { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifiedTime { get; set; }

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
            get { return ((UserRole) Role).ToEnumText(); }
        }
    }
}