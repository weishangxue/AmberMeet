using System;
using System.Web.Mvc;
using AmberMeet.AppService.Organizations;
using AmberMeet.Domain.Organizations;
using AmberMeet.Infrastructure.Exceptions;
using AmberMeet.Infrastructure.Serialization;
using AmberMeet.Infrastructure.Utilities;

namespace AmberMeet.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly IOrgUserService _orgUserService;

        public HomeController(IOrgUserService orgUserService)
        {
            _orgUserService = orgUserService;
        }

        public ActionResult LoginError(string msg)
        {
            ViewBag.LoginErrorMsg = msg;
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            ValidationLoginV();
            return View();
        }

        public ActionResult UserProfile(string userId)
        {
            try
            {
                ValidationLoginV();
                if (userId == null)
                {
                    userId = SessionUserId;
                }
                var user = _orgUserService.Get(userId);
                ViewBag.Status = ((UserState) user.State).ToEnumText();
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
        public ActionResult PostLogin(string loginName, string password)
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
                return OkException(ex);
            }
        }

        [HttpPost]
        public ActionResult PostTestUserLogin()
        {
            try
            {
                //暂时直接使用test01进行测试
                var user = _orgUserService.GetByAccount("test0001");
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
                return OkException(ex);
            }
        }

        [HttpPost]
        public ActionResult PutPassword(string password, string newPassword)
        {
            try
            {
                ValidationLogin();
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
                return OkException(ex);
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