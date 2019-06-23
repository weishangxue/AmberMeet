using System.ComponentModel;

namespace AmberMeet.Domain.MeetSignfors
{
    /// <summary>
    ///     会议签收状态
    /// </summary>
    public enum MeetSignforState
    {
        /// <summary>
        ///     未签收
        /// </summary>
        [Description("未签收")] WaitSign = 1,

        /// <summary>
        ///     已签收(当会议需要反馈时，签收需填写反馈信息)=待参与
        /// </summary>
        [Description("已签收")] AlreadySigned = 2,

        /// <summary>
        ///     自动签收(发起者确认会议激活，未签收者自动签收并添加到日程)=待参与
        /// </summary>
        [Description("自动签收")] AutoSigned = 3,

        /// <summary>
        ///     已取消(发起者取消会议)
        /// </summary>
        [Description("已取消")] Cancelled = 4
    }
}