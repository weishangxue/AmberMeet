$(document).ready(function() {
    var $searchText = $("#searchText");
    var $activateDate = $("#activateDate");

    var $searchButton = $("#searchButton");
    var $createButton = $("#createButton");
    var $grid = $("#meetList");
    var gridPager = "#meetListPager";

    var gridUrl = function() {
        var listUrl = "GetMyDistributeList?keywords={keywords}&activateIsoDate={activateIsoDate}";
        listUrl = listUrl.replace("{keywords}", $searchText.val());
        listUrl = listUrl.replace("{activateIsoDate}", $activateDate.val());
        return listUrl;
    };

    var initCreate = function() {
        if (angel.getQueryString("createMeet")) {
            $createButton.click();
        }
    };
    var initializeGrid = function() {
        var colNames = ["主题", "会议室", "开始时间", "结束时间", "需要反馈", "选择"];
        var colModel = [
            { name: "subject", index: "subject", width: 130, sortable: false },
            { name: "room", index: "room", width: 80, sortable: false },
            { name: "startTime", index: "startTime", width: 60, sortable: false },
            { name: "endTime", index: "endTime", width: 60, sortable: false },
            { name: "needFeedback", index: "needFeedback", width: 50, sortable: false },
            { name: "selectBtn", index: "selectBtn", width: 50, sortable: false }
        ];
        var url = gridUrl();
        angel.jqGrid.setGridUrl(url);
        angel.jqGrid.setGridSortorder("");
        angel.jqGrid.setGridCompleteHandler(function() {
            //$('a[name="selectItem"]', $grid).click(function() {
            //    window.location.href = "UserDetail?id=" + $(this).attr("itemId");
            //});
            $('a[name="editBtn"]', $grid).click(function() {
                var itemId = $(this).attr("itemId");
                $.getJSON("GetMeetDetail?id=" + itemId,
                    function(result) {
                        angel.myDistributeDetailControl.show(result, reloadGrid);
                    });
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

    $createButton.click(function() {
        angel.myDistributeDetailControl.show(null, reloadGrid);
    });

    $(window).bind("resize", function() { angel.jqGrid.initializeGridWidth($grid); });

    initializeGrid();
    initCreate();
});