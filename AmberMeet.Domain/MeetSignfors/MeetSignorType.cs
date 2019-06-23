using System.ComponentModel;

namespace AmberMeet.Domain.MeetSignfors
{
    /// <summary>
    ///     会议签收者类型
    /// </summary>
    public enum MeetSignorType
    {
        /// <summary>
        ///     内部
        /// </summary>
        [Description("内部")] Org = 1,

        /// <summary>
        ///     外部
        /// </summary>
        [Description("外部")] External = 2
    }
}