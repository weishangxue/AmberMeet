using System;
using AmberMeet.Infrastructure.Utilities;

namespace AmberMeet.Dto.MeetSignfors
{
    public class MeetSignforPagedDto
    {
        /// <summary>
        ///     主键ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     会议ID
        /// </summary>
        public string MeetId { get; set; }

        public string Subject { get; set; }
        public string Place { get; set; }
        public bool NeedFeedback { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        /*********Extensions***************************************************************/

        public string StartTimeText
        {
            get { return FormatHelper.GetIsoDateTimeString(StartTime); }
        }

        public string EndTimeText
        {
            get { return FormatHelper.GetIsoDateTimeString(EndTime); }
        }

        public string NeedFeedbackText
        {
            get { return FormatHelper.GetBooleanString(NeedFeedback); }
        }
    }
}