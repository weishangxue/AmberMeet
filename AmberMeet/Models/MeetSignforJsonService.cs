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
                    GetSelectItem(item.Id, item.Subject),
                    item.Place,
                    item.StartTimeText,
                    item.EndTimeText,
                    item.NeedFeedbackText
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
    }
}