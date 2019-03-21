$(document).ready(function() { angel.userRoleChangeControl.initialize(); });

angel.userRoleChangeControl = (function($) {
    var $controlContainer = $("#userRoleChangeContainer");
    var $controlErrorContainer = $("#userRoleChangeErrorContainer");

    var $userId = $("#userRoleChangeUserId");
    var $userName = $("#userName");
    var $userRole = $("#userRole");

    var $submitButton = $("#userRoleChangeSubmitButton");
    var $cancelButton = $("#userRoleChangeCancelButton");

    var submitButtonHandler = function() {};
    var cancelButtonHandler = function() {};

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

            angel.ajaxPost("PutUserRole",
                { userId: $userId.val(), userRole: $userRole.val() },
                function(data) {
                    var result = $.parseJSON(data);
                    if (result.result == 0) {
                        var msg = "保存成功";
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

        $userRole.rules("add", { required: true, messages: { required: "必需选择所属角色" } });
        angel.addRequiredMark($userRole);
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
            $userId.val(item.Id);
            $userName.val(item.Name);
            $userRole.val(item.Role);
        } else {
            $userId.val("");
            $userName.val("");
            $userRole.val("");
        }

        $controlContainer.modal({ backdrop: "static", keyboard: false });
        $controlContainer.modal("show");
        $submitButton.blur();
        $cancelButton.blur();
    };

    return that;
})(jQuery);