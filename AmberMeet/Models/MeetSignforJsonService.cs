using System.Collections.Generic;
using System.Linq;
using AmberMeet.Dto.MeetSignfors;
using AmberMeet.Infrastructure.Search.Paging;
using AmberMeet.Infrastructure.Serialization;
using AmberMeet.Models.JsonJqGrids;

namespace AmberMeet.Models
{
    internal class MeetSignforJsonService
    {
        public string GetJqGridJson(PagedResult<MeetSignforPagedDto> pagedResult, int pageIndex, int pageSize)
        {
            IList<JqGridRowObject> rows =
                pagedResult.Entities.Select(item => new JqGridRowObject(item.Id, new[]
                {
                    GetSelectItem(item.Id, item.Subject, item.MeetId),
                    item.Place,
                    item.StartTimeText,
                    item.EndTimeText,
                    item.NeedFeedbackText,
                    GetSelectBtn(item.Id)
                })).ToList();
            var jsonJqGridObject = new JqGridObject(rows, pagedResult.Count, pageIndex, pageSize);
            return jsonJqGridObject.ToJson(true);
        }

        private string GetSelectItem(string itemId, string itemName, string meetId)
        {
            const string selectItem =
                "<a name='selectItem' itemId='{itemId}' meetId='{meetId}' style='cursor: pointer'>{itemName}</a>";
            return selectItem.Replace("{itemId}", itemId).Replace("{itemName}", itemName).Replace("{meetId}", meetId);
        }

        private string GetSelectBtn(string itemId)
        {
            const string selectBtn =
                "<a name='signforBtn' itemId='{itemId}' style='cursor: pointer'>签收</a>";
            return selectBtn.Replace("{itemId}", itemId);
        }
    }
}