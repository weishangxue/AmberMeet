using System.Collections.Generic;
using System.Linq;
using AmberMeet.Dto.MeetSignfors;
using AmberMeet.Infrastructure.Search.Paging;
using AmberMeet.Models.JsonJqGrids;

namespace AmberMeet.Models
{
    internal class MeetSignforJsonService
    {
        public JqGridObject GetWaitSignforJqGridJson(
            PagedResult<MeetSignforPagedDto> pagedResult, int pageIndex, int pageSize)
        {
            IList<JqGridRowObject> rows =
                pagedResult.Entities.Select(item => new JqGridRowObject(item.Id, new[]
                {
                    GetSelectItem(item.Id, item.Subject, item.MeetId),
                    item.Place,
                    item.StartTimeStr,
                    item.EndTimeStr,
                    item.NeedFeedbackStr,
                    GetSelectBtn(item.Id)
                })).ToList();
            return new JqGridObject(rows, pagedResult.Count, pageIndex, pageSize);
        }

        public JqGridObject GetAlreadySignedJqGridJson(
            PagedResult<MeetSignforPagedDto> pagedResult, int pageIndex, int pageSize)
        {
            IList<JqGridRowObject> rows =
                pagedResult.Entities.Select(item => new JqGridRowObject(item.Id, new[]
                {
                    GetSelectItem(item.Id, item.Subject, item.MeetId),
                    item.Place,
                    item.StartTimeStr,
                    item.EndTimeStr,
                    item.NeedFeedbackStr
                })).ToList();
            return new JqGridObject(rows, pagedResult.Count, pageIndex, pageSize);
        }

        public JqGridObject GetAllSignforJqGridJson(
            PagedResult<MeetSignforPagedDto> pagedResult, int pageIndex, int pageSize)
        {
            IList<JqGridRowObject> rows =
                pagedResult.Entities.Select(item => new JqGridRowObject(item.Id, new[]
                {
                    GetSelectItem(item.Id, item.Subject, item.MeetId),
                    item.Place,
                    item.StartTimeStr,
                    item.EndTimeStr,
                    item.NeedFeedbackStr
                })).ToList();
            return new JqGridObject(rows, pagedResult.Count, pageIndex, pageSize);
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