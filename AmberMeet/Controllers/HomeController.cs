using System;
using System.Web.Mvc;
using AmberMeet.AppService.Organizations;
using AmberMeet.Domain.Organizations;
using AmberMeet.Infrastructure.Exceptions;
using AmberMeet.Infrastructure.Serialization;
using AmberMeet.Infrastructure.Utilities;
using HtmlHelper = AmberMeet.Infrastructure.Utilities.HtmlHelper;

namespace AmberMeet.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly IOrgUserService _orgUserService;

        public HomeController(IOrgUserService orgUserService)
        {
            _orgUserService = orgUserService;
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            if (!IsValidAccount())
            {
                return ErrorLoginView();
            }
            return View();
        }

        public ActionResult UserProfile()
        {
            try
            {
                if (!IsValidAccount())
                    return ErrorLoginView();
                var user = _orgUserService.Get(SessionUserId);
                ViewBag.Status = ((UserState) user.Status).ToEnumText();
                ViewBag.Sex = ((UserSex) user.Sex).ToEnumText();
                return View(user);
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                return ErrorView(ex.Message);
            }
        }

        [HttpPost]
        public string PostLogin(string loginName, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(loginName))
                {
                    throw new PreValidationException("用户名不允许为空");
                }
                if (string.IsNullOrEmpty(password))
                {
                    throw new PreValidationException("密码不允许为空");
                }
                var user = _orgUserService.GetByAccount(loginName);
                if (user == null)
                {
                    throw new PreValidationException("用户名或者密码错误");
                }
                if (user.Password != CryptographicHelper.Hash(password))
                {
                    throw new PreValidationException("用户名或者密码错误");
                }
                SessionUserId = user.Id;
                SessionUserRealName = user.Name;
                return Ok();
            }
            catch (PreValidationException ex)
            {
                return Ok(false, ex.Message);
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                var result = HtmlHelper.Encode(ex.Message);
                return Ok(false, result);
            }
        }

        [HttpPost]
        public string PostTestUserLogin()
        {
            try
            {
                //暂时直接使用admin进行测试
                var user = _orgUserService.GetByAccount("admin");
                SessionUserId = user.Id;
                SessionUserRealName = user.Name;
                return Ok();
            }
            catch (PreValidationException ex)
            {
                return Ok(false, ex.Message);
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                var result = HtmlHelper.Encode(ex.Message);
                return Ok(false, result);
            }
        }

        [HttpPost]
        public string PutPassword(string password, string newPassword)
        {
            try
            {
                if (!IsValidAccount())
                {
                    return OkLoginError();
                }
                if (string.IsNullOrEmpty(password))
                {
                    throw new PreValidationException("密码不允许为空");
                }
                var user = _orgUserService.Get(SessionUserId);
                if (user == null)
                {
                    throw new PreValidationException("用于已被注销");
                }
                if (user.Password != CryptographicHelper.Hash(password))
                {
                    throw new PreValidationException("原密码错误");
                }
                if (string.IsNullOrEmpty(newPassword))
                {
                    throw new PreValidationException("新密码不允许为空");
                }
                _orgUserService.ChangeUserPassword(user.Id, newPassword);
                return Ok();
            }
            catch (PreValidationException ex)
            {
                return Ok(false, ex.Message);
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                var result = HtmlHelper.Encode(ex.Message);
                return Ok(false, result);
            }
        }

        public RedirectResult Logout()
        {
            SessionUserId = null;
            SessionUserRealName = null;
            return Redirect("Login");
        }
    }
}