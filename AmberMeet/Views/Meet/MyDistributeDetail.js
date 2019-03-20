$(document).ready(function() { angel.myDistributeDetailControl.initialize(); });

angel.myDistributeDetailControl = (function($) {
    var _meet;
    var _signors = []; //KeyValueDto
    var _signorsHtml = "";

    var $controlContainer = $("#myDistributeDetailContainer");
    var $controlErrorContainer = $("#myDistributeDetailErrorContainer");

    var $subject = $("#subject");
    var $body = $("#body");
    var $startTime = $("#startTime");

    var $endTime = $("#endTime");
    var $place = $("#place");
    var $needFeedback = $("#needFeedback");
    var $needFeedbackNo = $("#needFeedbackNo");

    //var $signorsLabel = $("#signorsLabel");
    var $signors = $("#signors");
    var $signorNamesContainer = $("#signorNamesContainer");
    var $addSignorButton = $("#addSignorButton");

    var $submitButton = $("#myDistributeDetailSubmitButton");
    var $cancelButton = $("#myDistributeDetailCancelButton");

    var submitButtonHandler = function() {};
    var cancelButtonHandler = function() {};

    var initSignorsHtml = function() {
        _signorsHtml = "";
        $signorNamesContainer.empty();
        for (var isignor = 0; isignor < _signors.length; isignor++) {
            var signor = _signors[isignor];
            var userId = signor.Key;
            var userName = signor.Value;
            //_signorsHtml处理
            var newSignorHtml =
                '<span class="label">{userName} <a name="signorItem" userid={userId} username={userName} href="#" class="label label-badge">X</a></span>&nbsp;';
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
                //angel.alert(userName + "已存在");
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
                StartTime: $startTime.val(),
                EndTime: $endTime.val(),
                Signors: _signors //签收人发生改变时直接改变
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

        $startTime.addDatepicker(function(ev) {
            $endTime.setDatepickerStartDate(ev.date);
            $(this).blur();
        });

        $endTime.addDatepicker(function(ev) {
            $startTime.setDatepickerEndDate(ev.date);
            $(this).blur();
        });

        $addSignorButton.click(function() {
            angel.userSelectControl.show(function(selectItem) {
                addSignor(selectItem.userId, selectItem.userName);
            });
        });

        $subject.rules("add", { required: true, messages: { required: "主题不允许为空" } });
        $body.rules("add", { required: true, messages: { required: "内容不允许为空" } });
        $startTime.rules("add",
            {
                required: true,
                dateISO: true,
                compareDateFromTo: [$startTime, $endTime],
                messages: {
                    required: "开始时间不允许为空",
                    dateISO: "开始时间必需为日期格式(ISO)。例:2018-03-02",
                    compareDateFromTo: "开始时间必需小于结束时间"
                }
            });
        $endTime.rules("add",
            {
                dateISO: true,
                compareDateFromTo: [$startTime, $endTime],
                messages: {
                    dateISO: "结束时间必需为日期格式(ISO)。例:2018-03-02",
                    compareDateFromTo: "开始时间必需小于结束时间"
                }
            });
        $place.rules("add", { required: true, messages: { required: "会议地点不允许为空" } });
        $needFeedback.rules("add", { required: true, messages: { required: "是否需要反馈必须选择" } });
        $signors.rules("add", { required: true, messages: { required: "参与人不允许为空" } });

        angel.addRequiredMark($subject);
        angel.addRequiredMark($body);
        angel.addRequiredMark($startTime);
        angel.addRequiredMark($place);
        angel.addRequiredMark($needFeedback);
        angel.addRequiredMark($signors);
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
            debugger;
            _meet = dto;
            _signors = dto.Signors;

            $subject.val(_meet.Subject);
            $body.val(_meet.Body);
            $startTime.val(_meet.StartTimeStr);
            $endTime.val(_meet.EndTimeStr);
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
            $startTime.val("");
            $endTime.val("");
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