using System;
using System.Web.Mvc;
using AmberMeet.Domain.Organizations;
using AmberMeet.Infrastructure.Serialization;
using AmberMeet.Infrastructure.Utilities;
using AmberMeet.Models.JsonJqGrids;
using HtmlHelper = AmberMeet.Infrastructure.Utilities.HtmlHelper;

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

        protected void ValidationLoginV(UserRole role = UserRole.Ordinay)
        {
            try
            {
                ValidationLogin(role);
            }
            catch (Exception ex)
            {
                Response.Redirect($"~/Home/LoginError?msg={ex.Message}");
            }
        }

        protected void ValidationLogin(UserRole role = UserRole.Ordinay)
        {
            if (ConfigHelper.IsDebug)
            {
                SessionUserId = OrgUserDataService.FindAdmin().Id;
            }
            if (string.IsNullOrEmpty(SessionUserId))
            {
                throw new Exception("登录已失效,请重新登录再操作.");
            }
            try
            {
                var currentUser = OrgUserDataService.Find(SessionUserId);
                if (role == UserRole.Manager && (UserRole) currentUser.Role == UserRole.Ordinay)
                {
                    throw new Exception("权限不足");
                }
                SessionUserId = currentUser.Id;
                SessionUserRealName = currentUser.Name;
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                throw;
            }
        }

        protected ActionResult ErrorView(string errorMsg)
        {
            ViewData["errorMsg"] = $"Error message:{errorMsg}";
            return View("Error");
        }

        protected ActionResult OkJson(object resultValue)
        {
            ValidationLogin();
            return Json(resultValue, JsonRequestBehavior.AllowGet);
        }

        protected string OkJqGrid(JqGridObject value)
        {
            ValidationLogin();
            return value.ToJson(true);
        }

        protected ActionResult Ok(object resultValue = null, bool allowGet = false)
        {
            ValidationLogin();
            if (resultValue == null)
            {
                resultValue = "complete";
            }
            return Ok(true, resultValue, allowGet);
        }

        protected ActionResult OkLoginError()
        {
            return Ok(false, "登录已失效,请重新登录再操作.");
        }

        protected ActionResult Ok(bool result, object resultValue, bool allowGet = false)
        {
            //1代表false,0代表true
            var resultInt = 0;
            if (!result)
            {
                resultInt = 1;
            }
            if (allowGet)
            {
                return Json(new {result = resultInt, resultValue}, JsonRequestBehavior.AllowGet);
            }
            return Json(new {result = resultInt, resultValue});
        }

        protected string OkExceptionStr(Exception ex)
        {
            LogHelper.ExceptionLog(ex);
            var result = HtmlHelper.Encode(ex.Message);
            return new {result = 0, resultValue = result}.ToJson();
        }

        protected ActionResult OkException(Exception ex)
        {
            LogHelper.ExceptionLog(ex);
            var result = HtmlHelper.Encode(ex.Message);
            return Ok(false, result);
        }
    }
}