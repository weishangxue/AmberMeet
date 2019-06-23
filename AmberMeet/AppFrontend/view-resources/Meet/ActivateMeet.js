$(document).ready(function() { angel.meetActivateControl.initialize(); });

angel.meetActivateControl = (function($) {
    var $controlContainer = $("#activateMeetContainer");
    var $controlErrorContainer = $("#activateMeetErrorContainer");

    var $meetId = $("#activate_meetId");
    var $subject = $("#activate_subject");
    var $startDate = $("#activate_startDate");
    var $startHour = $("#activate_startHour");
    var $startMinute = $("#activate_startMinute");
    var $endDate = $("#activate_endDate");
    var $endHour = $("#activate_endHour");
    var $endMinute = $("#activate_endMinute");
    var $place = $("#activate_place");

    var $submitButton = $("#activateMeetSubmitButton");
    var $cancelButton = $("#activateMeetCancelButton");

    var submitButtonHandler = function() {};
    var cancelButtonHandler = function() {};

    var initElements = function() {
        $startDate.addDatepicker(function(ev) {
            $endDate.setDatepickerStartDate(ev.date);
            $(this).blur();
        });

        $endDate.addDatepicker(function(ev) {
            $startDate.setDatepickerEndDate(ev.date);
            $(this).blur();
        });
        //小时初始化
        $startHour.empty();
        $endHour.empty();
        var hourSelects = "";
        for (var ihour = 0; ihour < 24; ihour++) {
            var hourSelect = '<option value="{hour}">{hourText}</option>';
            hourSelect = hourSelect.replace("{hour}", ihour).replace("{hourText}", ihour + "点");
            hourSelects = hourSelects + hourSelect;
        }
        $startHour.html(hourSelects).val(10);
        $endHour.html(hourSelects).val(10);
        //分钟初始化
        $startMinute.empty();
        $endMinute.empty();
        var minuteSelects = "";
        for (var iminute = 0; iminute < 60; iminute++) {
            if (iminute != 0 && iminute % 5 != 0) {
                continue;
            }
            var minuteSelect = '<option value="{minute}">{minuteText}</option>';
            minuteSelect = minuteSelect.replace("{minute}", iminute).replace("{minuteText}", iminute + "分");
            minuteSelects = minuteSelects + minuteSelect;
        }
        $startMinute.html(minuteSelects);
        $endMinute.html(minuteSelects);
    };
    var that = {};

    that.initialize = function() {
        $submitButton.click(function() {
            var isValidateSuccess = angel.validator.validate(
                $controlContainer.find(":input:not(:hidden)"),
                $controlErrorContainer);
            if (!isValidateSuccess) {
                $("html, body").animate({ scrollTop: 0 }, 0);
                return;
            }
            var activateMeet = {
                Id: $meetId.val(),
                Place: $place.val(),
                StartTime: $startDate.val(),
                StartHour: $startHour.val(),
                StartMinute: $startMinute.val(),
                EndTime: $endDate.val(),
                EndHour: $endHour.val(),
                EndMinute: $endMinute.val()
            };
            angel.ajaxPost("PutMeetActivate",
                activateMeet,
                function(data) {
                    var result = $.parseJSON(data);
                    if (result.result == 0) {
                        var msg = "激活成功";
                        angel.alert(msg,
                            function() {
                                $controlContainer.modal("hide");
                                submitButtonHandler();
                            });
                    } else {
                        angel.alert(result.resultValue);
                    }
                });
        });

        $cancelButton.click(function() {
            cancelButtonHandler();
            $controlContainer.modal("hide");
            return false;
        });
        $startDate.rules("add",
            {
                required: true,
                dateISO: true,
                isFutureTime: [$startDate, $startHour, $startMinute],
                compareDateFromTo: [$startDate, $endDate],
                messages: {
                    required: "开始日期不允许为空",
                    dateISO: "开始日期必需为日期格式(ISO)。例:2018-03-02",
                    isFutureTime: "开始时间不允许小于当前时间",
                    compareDateFromTo: "开始日期不允许大于结束日期"
                }
            });
        $startHour.rules("add", { required: true, messages: { required: "开始时间必须选择" } });
        $startMinute.rules("add", { required: true, messages: { required: "开始分钟必须选择" } });
        $endDate.rules("add",
            {
                dateISO: true,
                compareDateFromTo: [$startDate, $endDate],
                messages: {
                    dateISO: "结束时间必需为日期格式(ISO)。例:2018-03-02",
                    compareDateFromTo: "开始时间必需小于结束时间"
                }
            });
        $place.rules("add", { required: true, messages: { required: "会议地点不允许为空" } });
        angel.addRequiredMark($startDate);
        angel.addRequiredMark($endDate);
        angel.addRequiredMark($place);

        initElements();
    };

    that.show = function(item, confirmHandler, cancelHandler) {
        if ($.isFunction(confirmHandler)) {
            submitButtonHandler = confirmHandler;
        }
        if ($.isFunction(cancelHandler)) {
            cancelButtonHandler = cancelHandler;
        }
        angel.validator.clearMessages($controlContainer.find(":input"), $controlErrorContainer);

        if (item) {
            $meetId.val(item.Id);
            $subject.val(item.Subject);
            $startDate.val(item.StartDateStr);
            $startHour.val(item.StartHour);
            $startMinute.val(item.StartMinute);
            $endDate.val(item.EndDateStr);
            $endHour.val(item.endHour);
            $endMinute.val(item.endMinute);
            $place.val(item.Place);
        } else {
            $meetId.val("");
            $subject.val("");
            $startDate.val("");
            $startHour.val("");
            $startMinute.val("");
            $endDate.val("");
            $endHour.val("");
            $endMinute.val("");
            $place.val("");
        }

        $controlContainer.modal({ backdrop: "static", keyboard: false });
        $controlContainer.modal("show");
        $submitButton.blur();
        $cancelButton.blur();
    };

    return that;
})(jQuery);