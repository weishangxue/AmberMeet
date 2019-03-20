$(document).ready(function() {
    var $loginForm = $("#loginForm");
    var $loginErrorContainer = $("#loginErrorContainer");
    var $loginErrorMsg = $("#loginErrorMsg");
    var $loginName = $("#loginName");
    var $password = $("#password");
    var $loginButton = $("#loginButton");
    var loginButtonText = $loginButton.val();
    var $testUserLoginButton = $("#testUserLoginButton");
    var testUserLoginButtonText = $testUserLoginButton.val();

    var initLoginButton = function() {
        $testUserLoginButton.removeAttr("disabled");
        $testUserLoginButton.val(testUserLoginButtonText);

        $loginButton.removeAttr("disabled");
        $loginButton.val(loginButtonText);
    };

    var checkValidation = function() {
        $testUserLoginButton.val("正在登录");
        $testUserLoginButton.attr("disabled", "disabled");

        $loginButton.val("正在登录");
        $loginButton.attr("disabled", "disabled");

        $loginErrorContainer.hide();
        $loginErrorMsg.text("用户名或者密码错误。");
        if (!$loginName.val() || !$password.val()) {
            initLoginButton();
            return false;
        }
        if ($loginName.val() == "Login Name" || $password.val() == "Password") {
            $loginErrorContainer.show();
            initLoginButton();
            return false;
        }
        return true;
    };

    var keydownHadler = function(event) {
        if (event.which === 13) {
            $loginButton.click();
        }
    };

    $loginButton.click(function() {
        if (!checkValidation()) {
            return;
        }
        $loginForm.ajaxSubmit({
            url: "../Home/PostLogin",
            type: "post",
            dataType: "json",
            clearForm: false,
            success: function(result) {
                if (result.result == 0) {
                    window.location.href = "../Meet/Index";
                } else {
                    $loginErrorMsg.text(result.resultValue);
                    $loginErrorContainer.show();
                    initLoginButton();
                }
            }
        });
    });

    $testUserLoginButton.click(function() {
        $loginName.val("测试用户");
        $password.val("123456");
        if (!checkValidation()) {
            return;
        }

        $loginForm.ajaxSubmit({
            url: "../Home/PostTestUserLogin",
            type: "post",
            dataType: "json",
            clearForm: false,
            success: function(result) {
                if (result.result == 0) {
                    window.location.href = "../Meet/Index";
                } else {
                    $loginErrorMsg.text(result.resultValue);
                    $loginErrorContainer.show();
                    initLoginButton();
                }
            }
        });
    });

    $(document).add($loginName).add($password).keydown(keydownHadler);
});