using System.Collections.Generic;
using System.Linq;
using AmberMeet.Dto.Meets;
using AmberMeet.Infrastructure.Search.Paging;
using AmberMeet.Infrastructure.Serialization;
using AmberMeet.Infrastructure.Utilities;
using AmberMeet.Models.JsonJqGrids;

namespace AmberMeet.Models
{
    internal class MeetJsonService
    {
        public string GetJqGridJson(PagedResult<MeetPagedDto> pagedResult, int pageIndex, int pageSize)
        {
            IList<JqGridRowObject> rows =
                pagedResult.Entities.Select(item => new JqGridRowObject(item.Id, new[]
                {
                    GetSelectItem(item.Id, item.Subject),
                    item.Place,
                    item.StartTimeText,
                    item.EndTimeText,
                    item.NeedFeedbackText,
                    GetWaitSignforCount(item),
                    AlreadySignedCount(item),
                    GetSelectBtn(item.Id, item.Subject)
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

        private string GetSelectBtn(string itemId, string itemName)
        {
            const string selectBtn =
                "<a name='editBtn' itemId='{itemId}' style='cursor: pointer'>修改</a>" +
                "&nbsp;|&nbsp;<a name='activateBtn' itemId='{itemId}' style='cursor: pointer'>激活</a>";
            return selectBtn.Replace("{itemId}", itemId).Replace("{itemName}", itemName);
        }

        private string GetWaitSignforCount(MeetPagedDto item)
        {
            const string selectItem =
                "<a name='waitSignforCountLabel' namesStr='{namesStr}' style='cursor: pointer'>{count}</a>";
            return selectItem.Replace("{namesStr}", item.WaitSignorNamesStr ?? "无")
                .Replace("{count}", FormatHelper.GetIntString(item.WaitSignforCount));
        }

        private string AlreadySignedCount(MeetPagedDto item)
        {
            const string selectItem =
                "<a name='alreadySignCountLabel' namesStr='{namesStr}' style='cursor: pointer'>{count}</a>";
            return selectItem.Replace("{namesStr}", item.AlreadySignorNamesStr ?? "无")
                .Replace("{count}", FormatHelper.GetIntString(item.AlreadySignedCount));
        }
    }
}