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
        public string GetMyDistributeJqGridJson(PagedResult<MeetWaitActivatePagedDto> pagedResult, int pageIndex,
            int pageSize)
        {
            IList<JqGridRowObject> rows =
                pagedResult.Entities.Select(item => new JqGridRowObject(item.Id, new[]
                {
                    GetSelectItem(item.Id, item.Subject),
                    item.Place,
                    item.StartTimeStr,
                    item.EndTimeStr,
                    item.NeedFeedbackStr,
                    GetWaitSignforCount(item),
                    AlreadySignedCount(item),
                    GetSelectBtn(item.Id, item.Subject)
                })).ToList();
            var jsonJqGridObject = new JqGridObject(rows, pagedResult.Count, pageIndex, pageSize);
            return jsonJqGridObject.ToJson(true);
        }

        public string GetMyActivateJqGridJson(PagedResult<MeetPagedDto> pagedResult, int pageIndex, int pageSize)
        {
            IList<JqGridRowObject> rows =
                pagedResult.Entities.Select(item => new JqGridRowObject(item.Id, new[]
                {
                    GetSelectItem(item.Id, item.Subject),
                    item.Place,
                    item.StartTimeStr,
                    item.EndTimeStr,
                    item.NeedFeedbackStr,
                    GetSignforCount(item)
                })).ToList();
            var jsonJqGridObject = new JqGridObject(rows, pagedResult.Count, pageIndex, pageSize);
            return jsonJqGridObject.ToJson(true);
        }

        public string GetMyAllDistributeJqGridJson(PagedResult<MeetPagedDto> pagedResult, int pageIndex, int pageSize)
        {
            IList<JqGridRowObject> rows =
                pagedResult.Entities.Select(item => new JqGridRowObject(item.Id, new[]
                {
                    GetSelectItem(item.Id, item.Subject),
                    item.Place,
                    item.StartTimeStr,
                    item.EndTimeStr,
                    item.NeedFeedbackStr,
                    GetSignforCount(item)
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
                "&nbsp;|&nbsp;<a name='activateBtn' itemId='{itemId}' style='cursor: pointer'>激活</a>" +
                "&nbsp;|&nbsp;<a name='signforListBtn' itemId='{itemId}' style='cursor: pointer'>签收详情</a>";
            return selectBtn.Replace("{itemId}", itemId).Replace("{itemName}", itemName);
        }

        private string GetWaitSignforCount(MeetWaitActivatePagedDto item)
        {
            const string selectItem =
                "<a name='waitSignforCountLabel' namesStr='{namesStr}' style='cursor: pointer'>{count}</a>";
            return selectItem.Replace("{namesStr}", item.WaitSignorNamesStr ?? "无")
                .Replace("{count}", $"{FormatHelper.GetIntString(item.WaitSignforCount)}位");
        }

        private string AlreadySignedCount(MeetWaitActivatePagedDto item)
        {
            const string selectItem =
                "<a name='alreadySignCountLabel' namesStr='{namesStr}' style='cursor: pointer'>{count}</a>";
            return selectItem.Replace("{namesStr}", item.AlreadySignorNamesStr ?? "无")
                .Replace("{count}", $"{FormatHelper.GetIntString(item.AlreadySignedCount)}位");
        }

        private string GetSignforCount(MeetPagedDto item)
        {
            const string selectItem =
                "<a name='signforCountLabel' itemId='{itemId}' namesStr='{namesStr}' style='cursor: pointer'>{count}</a>";
            return selectItem.Replace("{namesStr}", item.SignorNamesStr)
                .Replace("{count}", $"{FormatHelper.GetIntString(item.SignorCount)}位")
                .Replace("{itemId}", item.Id);
        }
    }
}