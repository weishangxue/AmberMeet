$(document).ready(function() { angel.meetSignforControl.initialize(); });

angel.meetSignforControl = (function($) {
    var $controlContainer = $("#signforContainer");
    var $controlErrorContainer = $("#signforErrorContainer");

    var $signforId = $("#signforId");
    var $subject = $("#subject");
    var $feedback = $("#feedback");

    var $submitButton = $("#signforSubmitButton");
    var $cancelButton = $("#signforCancelButton");

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

            angel.ajaxPost("PutSignfor",
                { signforId: $signforId.val(), feedback: $feedback.val() },
                function(data) {
                    var result = $.parseJSON(data);
                    if (result.result == 0) {
                        var msg = "签收成功";
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

        $feedback.rules("add", { required: true, messages: { required: "意见不允许为空" } });
        angel.addRequiredMark($feedback);
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
            $signforId.val(item.Id);
            $subject.val(item.Subject);
        } else {
            $signforId.val("");
            $subject.val("");
        }

        $controlContainer.modal({ backdrop: "static", keyboard: false });
        $controlContainer.modal("show");
        $submitButton.blur();
        $cancelButton.blur();
    };

    return that;
})(jQuery);