$(document).ready(function() {
    var $boxContent = $("#boxContent");

    //设置被激活的菜单页签
    var setActiveTab = function(activeLink, linkUrl) {
        if (!linkUrl) {
            linkUrl = activeLink.attr("linkurl");
        }
        if (!linkUrl) {
            return;
        }
        angel.alertNonClose("正在加载......");
        $boxContent.attr("src", linkUrl);
        $(".tree-menu li.active").removeClass("active");
        activeLink.closest("li").addClass("active");
    };

    var boxContent = document.getElementById("boxContent");
    boxContent.onload = boxContent.onreadystatechange = function() {
        if (this.readyState && this.readyState != "complete") {
            angel.alertNonClose("正在加载......");
        } else {
            angel.hideAlert();
        }
    };

    //初始化BoxContent(界面)高度
    var initBoxContentHeight = function() {
        var height = $(window).height() * 1.08;
        $boxContent.css("height", height + "px");
    };

    var initLink = function() {
        //触发菜单页签激活
        $(".tree-menu a").each(function() { $(this).click(function() { setActiveTab($(this)); }); });
        //设置默认选中项
        setActiveTab($(".tree-menu li.active").find("a"));
    };

    initBoxContentHeight();
    initLink();
});