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

        public int SignorCount { get; set; }
        public string[] SignorNames { get; set; }

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

        public string SignorNamesStr
        {
            get
            {
                if (SignorNames.Length == 0)
                {
                    return string.Empty;
                }
                var signorNamesStr = string.Empty;
                foreach (var signorName in SignorNames)
                {
                    signorNamesStr = $"{signorNamesStr}{signorName};";
                }
                return signorNamesStr;
            }
        }
    }
}