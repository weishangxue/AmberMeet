using System;
using System.Collections.Generic;
using AmberMeet.Domain.Meets;
using AmberMeet.Infrastructure.Serialization;
using AmberMeet.Infrastructure.Utilities;

namespace AmberMeet.Dto.Meets
{
    /// <summary>
    ///     ������ϸ-(�漰ǰ��js ajax����ӳ��)
    /// </summary>
    public class MeetDto
    {
        /// <summary>
        ///     ����ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     ������ID
        /// </summary>
        public string OwnerId { get; set; }

        /// <summary>
        ///     ����������
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        ///     ����״̬
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