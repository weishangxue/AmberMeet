using System.Collections.Generic;
using System.Linq;
using AmberMeet.Dto;
using AmberMeet.Infrastructure.Search.Paging;
using AmberMeet.Infrastructure.Serialization;
using AmberMeet.Models.JsonJqGrids;

namespace AmberMeet.Models
{
    internal class OrganizationJsonService
    {
        public string GetJqGridJson(PagedResult<OrgUserPagedDto> pagedResult, int pageIndex, int pageSize)
        {
            IList<JqGridRowObject> rows =
                pagedResult.Entities.Select(item => new JqGridRowObject(item.Id, new[]
                {
                    GetSelectItem(item.Id, item.Name),
                    item.LoginName,
                    item.Mail,
                    item.Mobile,
                    item.SexText,
                    item.BirthdayText,
                    item.RoleText,
                    GetSelectBtn(item.Id)
                })).ToList();
            var jsonJqGridObject = new JqGridObject(rows, pagedResult.Count, pageIndex, pageSize);
            return jsonJqGridObject.ToJson(true);
        }

        private string GetSelectItem(string itemId, string itemName)
        {
            const string selectItem =
                "<a name='selectItem' itemId='{itemId}' style='cursor: pointer'>{itemName}</a>";
            return selectItem.Replace("{itemId}", itemId).Replace("{itemName}", itemName);
        }

        private string GetSelectBtn(string itemId)
        {
            const string selectBtn =
                "<a name='editBtn' itemId='{itemId}' style='cursor: pointer'>编辑</a>" +
                "&nbsp;|&nbsp;<a name='changeRoleBtn' itemId='{itemId}' style='cursor: pointer'>改角色</a>";
            return selectBtn.Replace("{itemId}", itemId);
        }
    }
}