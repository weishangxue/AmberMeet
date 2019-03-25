using System;
using System.Collections.Generic;
using System.Linq;
using AmberMeet.Domain.Meets;
using AmberMeet.Infrastructure.Serialization;
using AmberMeet.Infrastructure.Utilities;

namespace AmberMeet.Dto.Meets
{
    /// <summary>
    ///     会议详细
    /// </summary>
    public class MeetDto
    {
        private int? _endHour;
        private int? _endMinute;
        private int _startHour;
        private int _startMinute;

        /// <summary>
        ///     主键ID
        /// </summary>
        public string Id { get; set; }

        public int IdentityId { get; set; }

        /// <summary>
        ///     发起人ID
        /// </summary>
        public string OwnerId { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Place { get; set; }
        public bool NeedFeedback { get; set; }

        /// <summary>
        ///     会议状态
        /// </summary>
        public int State { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime ModifiedTime { get; set; }

        /*********use to js*******************************/

        public int StartHour
        {
            get
            {
                if (_startHour > 0)
                {
                    return _startHour;
                }
                return StartTime.Hour;
            }
            set { _startHour = value; }
        }

        public int StartMinute
        {
            get
            {
                if (_startMinute > 0)
                {
                    return _startMinute;
                }
                return StartTime.Minute;
            }
            set { _startMinute = value; }
        }

        public int? EndHour
        {
            get
            {
                if (_endHour != null && _endHour > 0)
                {
                    return _endHour;
                }
                if (EndTime == null)
                {
                    return null;
                }
                return EndTime.Value.Hour;
            }
            set { _endHour = value; }
        }

        public int? EndMinute
        {
            get
            {
                if (_endMinute != null && _endMinute > 0)
                {
                    return _endMinute;
                }
                if (EndTime == null)
                {
                    return null;
                }
                return EndTime.Value.Minute;
            }
            set { _endMinute = value; }
        }

        /*********Extensions***************************************************************/

        /// <summary>
        ///     发起人姓名
        /// </summary>
        public string OwnerName { get; set; }

        public IList<KeyValueDto> Signors { get; set; }

        public string StartDateStr
        {
            get { return FormatHelper.GetIsoDateString(StartTime); }
        }

        public string StartTimeStr
        {
            get { return FormatHelper.GetIsoDateTimeString(StartTime); }
        }

        public string EndDateStr
        {
            get { return FormatHelper.GetIsoDateString(EndTime); }
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

        public string[] SignorNames
        {
            get { return Signors.Select(i => i.Value).ToArray(); }
        }
    }
}