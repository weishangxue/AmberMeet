using System;
using AmberMeet.Domain.MeetSignfors;
using AmberMeet.Infrastructure.Serialization;
using AmberMeet.Infrastructure.Utilities;

namespace AmberMeet.Dto.MeetSignfors
{
    public class MeetSignforDto
    {
        /// <summary>
        ///     主键ID
        /// </summary>
        public string Id { get; set; }

        public string MeetId { get; set; }
        public string SignorId { get; set; }
        public int SignorType { get; set; }
        public string Feedback { get; set; }
        public bool IsRemind { get; set; }
        public int State { get; set; }
        public DateTime SignTime { get; set; }
        public DateTime ModifiedTime { get; set; }

        /*********Meet Extensions*************/

        public string Subject { get; set; }
        public string Body { get; set; }
        public string Place { get; set; }
        public bool NeedFeedback { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        /*********Signor Extensions*************/

        public string SignorName { get; set; }

        /*********Extensions***************************************************************/

        public string FeedbackStr
        {
            get
            {
                if (Feedback == null)
                {
                    return string.Empty;
                }
                if (Feedback.Length > 20)
                {
                    Feedback = $"{Feedback.Substring(0, 15)}......";
                }
                return Feedback;
            }
        }

        public string StateStr
        {
            get { return ((MeetSignforState) State).ToEnumText(); }
        }

        public string SignTimeStr
        {
            get { return FormatHelper.GetIsoDateTimeString(SignTime); }
        }

        public string StartTimeStr
        {
            get { return FormatHelper.GetIsoDateTimeString(StartTime); }
        }

        public string EndTimeStr
        {
            get { return FormatHelper.GetIsoDateTimeString(EndTime); }
        }
    }
}