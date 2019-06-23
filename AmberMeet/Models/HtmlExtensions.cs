using System.Web.Mvc;

namespace AmberMeet.Models
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString SearchGroup_Data(this HtmlHelper helper,
            string searchId = "searchText",
            string searchPlaceholder = "关键字",
            string dateId = "searchDate",
            string datePlaceholder = "日期",
            string searchBtnId = "searchButton",
            string searchBtnText = "搜索",
            int top = 3)
        {
            var topStyle = $"style='margin-bottom: {top}px;'";
            if (top <= 0)
            {
                topStyle = string.Empty;
            }
            var str =
                $"<div class='input-group' {topStyle}>" +
                "<div class='input-control has-icon-left has-icon-right'>" +
                $"<input id='{searchId}' class='form-control' type='search' placeholder='{searchPlaceholder}' style='width: 85%'>" +
                "<label for='searchText' class='input-control-icon-left search-icon'>" +
                "<i class='icon icon-search'></i>" +
                "</label>" +
                $"<input type='text' id='{dateId}' name='{dateId}' class='form-control' placeholder='{datePlaceholder}' style='width: 15%;' />" +
                "</div>" +
                "<span class='input-group-btn'>" +
                $"<button type='button' id='{searchBtnId}' class='btn btn-primary'>{searchBtnText}</button>" +
                "</span>" +
                "</div>";
            return new MvcHtmlString(str);
        }

        public static MvcHtmlString JqGrid(this HtmlHelper helper,
            string listId = "dataList", string pagerId = "listPager")
        {
            var str = "<div class='jqGrid_wrapper'>" +
                      $"<table id='{listId}'></table>" +
                      $"<div id='{pagerId}'></div></div>";
            return new MvcHtmlString(str);
        }

        public static MvcHtmlString LkLabel(this HtmlHelper helper, string fortarget, string text)
        {
            var str = $"<label for='{fortarget}'>{text}</label>";
            return new MvcHtmlString(str);
        }
    }
}