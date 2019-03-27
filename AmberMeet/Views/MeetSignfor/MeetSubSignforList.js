$(document).ready(function() {
    var $searchText = $("#searchText");
    var $meetId = $("#meetId");
    var $searchButton = $("#searchButton");
    var $grid = $("#meetSignforList");
    var gridPager = "#meetSignforListPager";

    var initializeGridWidth = function() {
        var gviewElement = $("#gview_" + $grid.attr("id"));
        var width = $(".jqGrid_wrapper").width() - 2;

        $grid.setGridWidth(width);
        $(".ui-jqgrid-htable", gviewElement).width($(".ui-jqgrid-hdiv", gviewElement).width() - 1);
        $(".ui-jqgrid-btable", gviewElement).width($(".ui-jqgrid-hdiv", gviewElement).width());
        //$('.ui-jqgrid-title', gviewElement).css('color', '#F5F5F5');
        $(".ui-jqgrid-title", gviewElement).css("color", "white");
    };

    var initializeGrid = function() {
        var greidHeight = $(window).height() * 0.8;
        $grid.jqGrid({
            datatype: "local",
            colNames: ["签收者", "状态", "签收时间", "反馈信息"],
            colModel: [
                { name: "SignorName", index: "SignorName", width: 100, sortable: false },
                { name: "StateStr", index: "StateStr", width: 80, sortable: false },
                { name: "SignTimeStr", index: "SignTimeStr", width: 100, sortable: false },
                { name: "FeedbackStr", index: "FeedbackStr", width: 200, sortable: false }
            ],
            caption: null,
            hidegrid: false,
            sortname: null,
            sortorder: "",
            shrinkToFit: true, //false
            pager: gridPager,
            rowNum: 10,
            rowList: [10, 20, 30],
            viewrecords: true,
            rownumbers: false,
            multiselect: false,
            height: greidHeight,
            width: 300,
            autowidth: true,
            styleUI: "Bootstrap"
        });
        var pager = { refresh: true, search: false, edit: false, add: false, del: false };
        $grid.jqGrid("navGrid", gridPager, pager, { height: 200, reloadAfterSubmit: true });
    };

    var initGridData = function() {
        var listUrl = "../MeetSignfor/GetMeetSubSignforList?meetId={meetId}&keywords={keywords}";
        listUrl = listUrl.replace("{meetId}", $meetId.val());
        listUrl = listUrl.replace("{keywords}", $searchText.val());

        angel.ajaxGet(listUrl,
            function(data) {
                var result = $.parseJSON(data);
                if (result.result != 0) {
                    angel.alert(result.resultValue);
                    return;
                }
                $grid.clearGridData();
                var list = result.resultValue;
                for (var i = 0; i < list.length; i++) {
                    var signfor = list[i];
                    var feedbackStr =
                        "<span tittle='{feedback}'>{feedbackStr}</span>";
                    feedbackStr = feedbackStr
                        .replace("{feedback}", signfor.Feedback)
                        .replace("{feedbackStr}", signfor.FeedbackStr);
                    list[i].FeedbackStr = feedbackStr;
                }
                $grid.setGridParam({ data: list });
                $grid.trigger("reloadGrid");
            });
    };

    $searchButton.click(function() { initGridData(); });

    $(window).bind("resize", function() { initializeGridWidth(); });
    initializeGrid();
    initializeGridWidth();
    initGridData();
});