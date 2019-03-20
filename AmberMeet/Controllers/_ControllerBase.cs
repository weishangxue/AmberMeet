using System;
using System.Web.Mvc;
using AmberMeet.Domain.Organizations;
using AmberMeet.Infrastructure.Serialization;
using AmberMeet.Infrastructure.Utilities;

namespace AmberMeet.Controllers
{
    public class ControllerBase : Controller
    {
        protected string SessionUserId
        {
            get { return Session["UserId"]?.ToString() ?? string.Empty; }
            set
            {
                if (Session["UserId"] != null)
                {
                    Session.Remove("UserId");
                }
                Session.Add("UserId", value);
            }
        }

        protected string SessionUserRealName
        {
            get { return Session["UserRealName"] == null ? string.Empty : Session["UserRealName"].ToString(); }
            set
            {
                if (Session["UserRealName"] != null)
                {
                    Session.Remove("UserRealName");
                }
                Session.Add("UserRealName", value);
                ViewBag.sessionUserRealName = value;
            }
        }

        protected bool IsValidAccount(UserRole role = UserRole.Ordinay)
        {
            if (ConfigHelper.IsDebug)
            {
                SessionUserId = OrgUserDataService.FindAdmin().Id;
            }
            if (string.IsNullOrEmpty(SessionUserId))
            {
                ViewData["loginErrorMsg"] = "当前用户为空";
                return false;
            }
            try
            {
                var currentUser = OrgUserDataService.Find(SessionUserId);
                if (role == UserRole.Manager && (UserRole) currentUser.Role == UserRole.Ordinay)
                {
                    ViewData["loginErrorMsg"] = "权限不足";
                    return false;
                }
                SessionUserId = currentUser.Id;
                SessionUserRealName = currentUser.Name;
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                ViewData["loginErrorMsg"] = $"ex.Message={ex.Message}";
                return false;
            }
        }

        protected ActionResult ErrorLoginView()
        {
            return View("../Home/Login");
        }

        protected RedirectResult ErrorLoginRedirect()
        {
            return Redirect("LoginError");
        }

        protected ActionResult ErrorView(string errorMsg)
        {
            ViewData["errorMsg"] = $"Error message:{errorMsg}";
            return View("Error");
        }

        protected string Ok(object resultValue = null)
        {
            if (resultValue == null)
            {
                resultValue = "complete";
            }
            return Ok(true, resultValue);
        }

        protected string OkLoginError()
        {
            return Ok(false, "登录已失效,请重新登录再操作.");
        }

        protected string Ok(bool result, object resultValue)
        {
            //1代表false,0代表true
            var resultInt = 0;
            if (!result)
            {
                resultInt = 1;
            }

            var objectMsg = new {result = resultInt, resultValue}.ToJson();
            return objectMsg;
        }
    }
}