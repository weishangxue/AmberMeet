using System;
using System.Collections.Generic;
using AmberMeet.Domain.Meets;
using AmberMeet.Infrastructure.Serialization;
using AmberMeet.Infrastructure.Utilities;

namespace AmberMeet.Dto.Meets
{
    /// <summary>
    ///     会议详细-(涉及前端js ajax对象映射)
    /// </summary>
    public class MeetDto
    {
        /// <summary>
        ///     主键ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     发起人ID
        /// </summary>
        public string OwnerId { get; set; }

        /// <summary>
        ///     发起人姓名
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        ///     会议状态
        /// </summary>
        public int State { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }
        public string Place { get; set; }
        public bool NeedFeedback { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public IList<KeyValueDto> Signors { get; set; }

        /*********Extensions***************************************************************/

        public string StartTimeStr
        {
            get { return FormatHelper.GetIsoDateTimeString(StartTime); }
        }

        public string EndTimeStr
        {
            get { return FormatHelper.GetIsoDateTimeString(EndTime); }
        }

        public string NeedFeedbackStr
        {
            get { return FormatHelper.GetBooleanString(NeedFeedback); }
        }

        public string StateStr
        {
            get { return ((MeetState) State).ToEnumText(); }
        }

        public string SignorIdsStr
        {
            get
            {
                if (Signors == null || Signors.Count == 0)
                {
                    return string.Empty;
                }
                return Signors.ToJson();
            }
        }
    }
}