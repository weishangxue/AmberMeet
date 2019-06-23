$(document).ready(function() { angel.myDistributeDetailControl.initialize(); });

angel.myDistributeDetailControl = (function($) {
    var _meet;
    var _signors = []; //KeyValueDto
    var _signorsHtml = "";

    var $controlContainer = $("#myDistributeDetailContainer");
    var $controlErrorContainer = $("#myDistributeDetailErrorContainer");

    var $subject = $("#distribute_subject");
    var $body = $("#distribute_body");
    var $startDate = $("#distribute_startDate");
    var $startHour = $("#distribute_startHour");
    var $startMinute = $("#distribute_startMinute");
    var $endDate = $("#distribute_endDate");
    var $endHour = $("#distribute_endHour");
    var $endMinute = $("#distribute_endMinute");
    var $place = $("#distribute_place");
    var $needFeedback = $("#distribute_needFeedback");
    var $needFeedbackNo = $("#distribute_needFeedbackNo");

    var $signors = $("#distribute_signors");
    var $signorNamesContainer = $("#distribute_signorNamesContainer");
    var $addSignorButton = $("#distribute_addSignorButton");

    var $submitButton = $("#myDistributeDetailSubmitButton");
    var $cancelButton = $("#myDistributeDetailCancelButton");

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

    var initSignorsHtml = function() {
        _signorsHtml = "";
        $signorNamesContainer.empty();
        for (var isignor = 0; isignor < _signors.length; isignor++) {
            var signor = _signors[isignor];
            var userId = signor.Key;
            var userName = signor.Value;
            //_signorsHtml处理
            var newSignorHtml =
                '<span class="label">{userName} <a name="signorItem" userid={userId} username={userName} href="javacript:void(0);" class="label label-badge">X</a></span>&nbsp;';
            newSignorHtml = newSignorHtml.replace("{userId}", userId)
                .replace("{userName}", userName).replace("{userName}", userName);
            _signorsHtml = _signorsHtml + newSignorHtml;
            $signorNamesContainer.empty();
            $signorNamesContainer.html(_signorsHtml);
            //删除按钮处理
            $('a[name="signorItem"]', $signorNamesContainer).click(function() {
                removeSignor($(this).attr("userid"), $(this).attr("username"));
            });
        }
        $signors.val(_signorsHtml);
    };
    var addSignor = function(userId, userName) {
        for (var isignor = 0; isignor < _signors.length; isignor++) {
            var signor = _signors[isignor];
            if (signor.Key == userId) {
                return;
            }
        }
        //_signors处理
        _signors.push({ Key: userId, Value: userName });
        initSignorsHtml();
    };

    var removeSignor = function(userId, userName) {
        angel.confirm("确认要移除<" + userName + ">吗?",
            function() {
                //signorsDto处理
                var signorsDto = [];
                for (var isignor = 0; isignor < _signors.length; isignor++) {
                    var signor = _signors[isignor];
                    if (signor.Key == userId) {
                        continue;
                    }
                    signorsDto.push(signor);
                }
                _signors = [];
                //_signors与_signorsHtml处理
                _signors = signorsDto;
                initSignorsHtml();
            });
    };
    var that = {};

    that.initialize = function() {
        $submitButton.click(function() {
            var validateElements = $controlContainer.find(":input:not(:hidden)");
            validateElements.push($signors);
            if (!angel.validator.validate(validateElements, $controlErrorContainer)) {
                $("html, body").animate({ scrollTop: 0 }, 0);
                return;
            }

            var needFeedback = true;
            if ($needFeedbackNo.prop("checked") == true) {
                needFeedback = false;
            }
            var newMeet = {
                Subject: $subject.val(),
                Body: $body.val(),
                Place: $place.val(),
                NeedFeedback: needFeedback,
                StartTime: $startDate.val(),
                StartHour: $startHour.val(),
                StartMinute: $startMinute.val(),
                EndTime: $endDate.val(),
                EndHour: $endHour.val(),
                EndMinute: $endMinute.val(),
                Signors: _signors
            };
            if (_meet != null) {
                newMeet.Id = _meet.Id;
                newMeet.OwnerId = _meet.OwnerId;
                newMeet.State = _meet.State;
            }
            angel.ajaxPost("PostMeet",
                newMeet,
                function(data) {
                    var result = $.parseJSON(data);
                    if (result.result == 0) {
                        var msg = "保存成功";
                        if (newMeet.Id == null || newMeet.Id == "") {
                            msg = "会议发起成功";
                        }
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
            $controlContainer.modal("hide");
            cancelButtonHandler();
        });

        $addSignorButton.click(function() {
            angel.userSelectControl.show(function(selectItem) {
                addSignor(selectItem.userId, selectItem.userName);
            });
        });

        $subject.rules("add", { required: true, messages: { required: "主题不允许为空" } });
        $body.rules("add", { required: true, messages: { required: "内容不允许为空" } });
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
                    dateISO: "结束日期必需为日期格式(ISO)。例:2018-03-02",
                    compareDateFromTo: "开始日期不允许大于结束日期"
                }
            });
        //$endHour.rules("add", { required: true, messages: { required: "结束时间必须选择" } });
        //$endMinute.rules("add", { required: true, messages: { required: "结束分钟必须选择" } });
        $place.rules("add", { required: true, messages: { required: "会议地点不允许为空" } });
        $needFeedback.rules("add", { required: true, messages: { required: "是否需要反馈必须选择" } });
        $signors.rules("add", { required: true, messages: { required: "参与人不允许为空" } });

        angel.addRequiredMark($subject);
        angel.addRequiredMark($body);
        angel.addRequiredMark($startDate);
        angel.addRequiredMark($place);
        angel.addRequiredMark($needFeedback);
        angel.addRequiredMark($signors);

        initElements();
    };

    that.show = function(dto, confirmHandler, cancelHandler) {
        if ($.isFunction(confirmHandler)) {
            submitButtonHandler = confirmHandler;
        }
        if ($.isFunction(cancelHandler)) {
            cancelButtonHandler = cancelHandler;
        }
        angel.validator.clearMessages($controlContainer.find(":input"), $controlErrorContainer);

        _signorsHtml = "";
        if (dto) {
            _meet = dto;
            _signors = dto.Signors;

            $subject.val(_meet.Subject);
            $body.val(_meet.Body);
            $startDate.val(_meet.StartDateStr);
            $startHour.val(_meet.StartHour);
            $startMinute.val(_meet.StartMinute);
            $endDate.val(_meet.EndDateStr);
            $endHour.val(_meet.endHour);
            $endMinute.val(_meet.endMinute);
            $place.val(_meet.Place);
            if (!_meet.NeedFeedback) {
                $needFeedbackNo.prop("checked", "checked");
            } else {
                $needFeedback.prop("checked", "checked");
            }
        } else {
            _meet = null;
            _signors = [];

            $subject.val("");
            $body.val("");
            $startDate.val("");
            $startHour.val("");
            $startMinute.val("");
            $endDate.val("");
            $endHour.val("");
            $endMinute.val("");
            $place.val("");
            $needFeedback.val("");
        }
        initSignorsHtml();

        $controlContainer.modal({ backdrop: "static", keyboard: false });
        $controlContainer.modal("show");
        $submitButton.blur();
        $cancelButton.blur();
    };

    return that;
})(jQuery);