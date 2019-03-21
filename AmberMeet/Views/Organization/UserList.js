$(document).ready(function() {
    var $searchText = $("#searchText");
    var $userStatus = $("#userStatus");
    var $searchButton = $("#searchButton");
    var $createButton = $("#createButton");
    var $grid = $("#userList");
    var gridPager = "#userListPager";

    var gridUrl = function() {
        var listUrl = "GetUserList?keywords={keywords}&state={state}";
        listUrl = listUrl.replace("{keywords}", $searchText.val());
        listUrl = listUrl.replace("{state}", $userStatus.val());
        return listUrl;
    };

    var initializeGrid = function() {
        var colNames = ["姓名", "登录名", "邮箱", "手机", "性别", "出生日期", "角色", "选择"];
        var colModel = [
            { name: "name", index: "name", width: 80, sortable: false },
            { name: "loginName", index: "loginName", width: 80, sortable: false },
            { name: "mail", index: "mail", width: 100, sortable: false },
            { name: "mobile", index: "mobile", width: 100, sortable: false },
            { name: "sex", index: "sex", width: 60, sortable: false },
            { name: "birthday", index: "birthday", width: 80, sortable: false },
            { name: "role", index: "role", width: 60, sortable: false },
            { name: "select", index: "select", width: 60, sortable: false }
        ];
        var url = gridUrl();
        angel.jqGrid.setGridUrl(url);
        angel.jqGrid.setGridSortorder("");
        angel.jqGrid.setGridCompleteHandler(function() {
            $('a[name="selectItem"]', $grid).click(function() {
                angel.openWnd("../Home/UserProfile?userId=" + $(this).attr("itemId"));
            });
            $('a[name="editBtn"]', $grid).click(function() {
                window.location.href = "UserDetail?id=" + $(this).attr("itemId");
            });
            $('a[name="changeRoleBtn"]', $grid).click(function() {
                var itemId = $(this).attr("itemId");
                $.getJSON("GetUser?userId=" + itemId,
                    function(result) {
                        debugger;
                        angel.userRoleChangeControl.show(result, reloadGrid);
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

    $searchButton.click(function() { reloadGrid(); });

    $createButton.click(function() {
        window.location.href = "UserDetail";
    });

    $(window).bind("resize", function() { angel.jqGrid.initializeGridWidth($grid); });

    initializeGrid();
});