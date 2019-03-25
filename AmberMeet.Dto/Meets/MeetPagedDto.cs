﻿using System;
using AmberMeet.Infrastructure.Utilities;

namespace AmberMeet.Dto.Meets
{
    public class MeetPagedDto
    {
        /// <summary>
        ///     主键ID
        /// </summary>
        public string Id { get; set; }

        public string Subject { get; set; }
        public string Place { get; set; }
        public bool NeedFeedback { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        /*********Signfor Extensions*******************************************************/

        public int WaitSignforCount { get; set; }
        public string WaitSignorNamesStr { get; set; }
        public int AlreadySignedCount { get; set; }
        public string AlreadySignorNamesStr { get; set; }

        /*********Extensions***************************************************************/

        /// <summary>
        ///     签收摘要
        /// </summary>
        public string SignforSummary { get; set; }

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