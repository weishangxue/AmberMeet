$(document).ready(function() {
    var $searchText = $("#searchText");
    var $activateDate = $("#activateDate");

    var $searchButton = $("#searchButton");
    var $grid = $("#activateMeetList");
    var gridPager = "#activateMeetListPager";

    var gridUrl = function() {
        var listUrl = "../MeetQuery/GetMyActivateList?keywords={keywords}&activateIsoDate={activateIsoDate}";
        listUrl = listUrl.replace("{keywords}", $searchText.val());
        listUrl = listUrl.replace("{activateIsoDate}", $activateDate.val());
        return listUrl;
    };

    var initializeGrid = function() {
        var colNames = ["主题", "会议室", "开始时间", "结束时间", "需要反馈", "接收者"];
        var colModel = [
            { name: "subject", index: "subject", width: 130, sortable: false },
            { name: "room", index: "room", width: 80, sortable: false },
            { name: "startTime", index: "startTime", width: 100, sortable: false },
            { name: "endTime", index: "endTime", width: 100, sortable: false },
            { name: "needFeedback", index: "needFeedback", width: 50, sortable: false },
            { name: "signforCount", index: "signforCount", width: 50, sortable: false }
        ];
        var url = gridUrl();
        angel.jqGrid.setGridUrl(url);
        angel.jqGrid.setGridSortorder("");
        angel.jqGrid.setGridCompleteHandler(function() {
            $('a[name="selectItem"]', $grid).click(function() {
                angel.openWnd("../Meet/MeetDetail?meetId=" + $(this).attr("itemId"), 993, 530);
            });
            $('a[name="signforCountLabel"]', $grid).click(function() {
                angel.openWnd("../MeetSignfor/MeetSubSignforList?meetId=" + $(this).attr("itemId"), 993, 530);
            });
        });
        angel.jqGrid.initialize($grid, gridPager, colNames, colModel, null);
        angel.jqGrid.initializeGridWidth($grid);
    };

    var reloadGrid = function() {
        $grid.clearGridData();
        $grid.setGridParam({ url: gridUrl() });
        $grid.trigger("reloadGrid");
    };

    $activateDate.addDatepicker();

    $searchButton.click(function() { reloadGrid(); });

    $(window).bind("resize", function() { angel.jqGrid.initializeGridWidth($grid); });

    initializeGrid();
});