$(document).ready(function() {
    var $searchText = $("#searchText");
    var $activateDate = $("#activateDate");
    var $signforStates = $("#signforStates");

    var $searchButton = $("#searchButton");
    var $grid = $("#allSignforList");
    var gridPager = "#allSignforListPager";

    var gridUrl = function() {
        var listUrl =
            "../MeetSignfor/GetAllSignforList?keywords={keywords}&activateIsoDate={activateIsoDate}&state={state}";
        listUrl = listUrl.replace("{keywords}", $searchText.val());
        listUrl = listUrl.replace("{activateIsoDate}", $activateDate.val());
        listUrl = listUrl.replace("{state}", $signforStates.val());
        return listUrl;
    };
    var initializeGrid = function() {
        var colNames = ["主题", "会议室", "开始时间", "结束时间", "需要反馈"];
        var colModel = [
            { name: "subject", index: "subject", width: 130, sortable: false },
            { name: "room", index: "room", width: 80, sortable: false },
            { name: "startTime", index: "startTime", width: 100, sortable: false },
            { name: "endTime", index: "endTime", width: 100, sortable: false },
            { name: "needFeedback", index: "needFeedback", width: 50, sortable: false }
        ];
        var url = gridUrl();
        angel.jqGrid.setGridUrl(url);
        angel.jqGrid.setGridSortorder("");
        angel.jqGrid.setGridCompleteHandler(function() {
            $('a[name="selectItem"]', $grid).click(function() {
                var itemId = $(this).attr("itemId");
                var meetId = $(this).attr("meetId");
                angel.openWnd("../Meet/MeetDetail?meetId=" + meetId + "&subSignforId=" + itemId, 993, 600);
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