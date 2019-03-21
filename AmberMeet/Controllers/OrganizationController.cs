using System;
using System.Linq;
using System.Web.Mvc;
using AmberMeet.AppService.Organizations;
using AmberMeet.Domain.Data;
using AmberMeet.Domain.Organizations;
using AmberMeet.Infrastructure.Exceptions;
using AmberMeet.Infrastructure.Serialization;
using AmberMeet.Infrastructure.Utilities;
using AmberMeet.Models;
using HtmlHelper = AmberMeet.Infrastructure.Utilities.HtmlHelper;

namespace AmberMeet.Controllers
{
    public class OrganizationController : ControllerBase
    {
        private readonly OrganizationJsonService _orgJsonService;
        private readonly IOrgUserService _userService;

        public OrganizationController(IOrgUserService userService)
        {
            _userService = userService;
            _orgJsonService = new OrganizationJsonService();
        }

        public ActionResult UserList()
        {
            try
            {
                //只允许admin进入
                if (!IsValidAccount(UserRole.Manager))
                    return ErrorLoginView();
                ViewBag.userStates = UserState.Normal.GetDescriptions();
                ViewBag.userRoles = UserRole.Ordinay.GetDescriptions().Where(i => i.Key != (int) UserRole.System);
                return View();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                return ErrorView(ex.Message);
            }
        }

        public ActionResult UserDetail(string id)
        {
            try
            {
                if (!IsValidAccount(UserRole.Manager))
                    return ErrorLoginView();
                ViewBag.sexMan = (int) UserSex.Man;
                ViewBag.sexLady = (int) UserSex.Lady;
                if (!string.IsNullOrEmpty(id))
                {
                    var user = _userService.Get(id);
                    ViewData["userId"] = user.Id;
                    ViewData["status"] = user.Status;
                    ViewData["statusName"] = ((UserState) user.Status).ToEnumText();
                    ViewData["loginName"] = user.LoginName;
                    ViewData["name"] = user.Name;
                    ViewData["code"] = user.Code;
                    ViewData["mobile"] = user.Mobile;
                    ViewData["birthday"] = FormatHelper.GetIsoDateString(user.Birthday);
                    ViewData["mail"] = user.Mail;
                    if (user.Sex == (int) UserSex.Man)
                    {
                        ViewBag.sexManChecked = "checked";
                    }
                    else
                    {
                        ViewBag.sexLadyChecked = "checked";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                return ErrorView(ex.Message);
            }
        }

        public PartialViewResult UserSelectControl()
        {
            return PartialView();
        }

        /// <summary>
        ///     获取用户列表
        /// </summary>
        /// <param name="page">当前页</param>
        /// <param name="rows">每页显示行数</param>
        /// <param name="keywords">模糊条件</param>
        /// <param name="state">用户状态</param>
        /// <returns></returns>
        [HttpGet]
        public string GetUserList(int page, int rows, string keywords, int state)
        {
            try
            {
                if (!IsValidAccount(UserRole.Manager))
                {
                    return OkLoginError();
                }
                var result = _userService.GetPaged(page, rows, state, keywords);
                return _orgJsonService.GetJqGridJson(result, page, rows);
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                var result = HtmlHelper.Encode(ex.Message);
                return Ok(false, result);
            }
        }

        /// <summary>
        ///     获取选择用户控件用户列表数据
        /// </summary>
        /// <param name="keywords">模糊条件</param>
        /// <returns></returns>
        [HttpGet]
        public string GetUserSelectControlUserList(string keywords)
        {
            try
            {
                if (!IsValidAccount())
                {
                    return OkLoginError();
                }
                var userList = _userService.GetAll();
                if (!string.IsNullOrEmpty(keywords))
                {
                    userList = userList.Where(i => i.Name.Contains(keywords) ||
                                                   i.Mail.Contains(keywords) ||
                                                   i.Mobile.Contains(keywords)).ToList();
                }
                return Ok(userList.OrderBy(t => t.Name).ToList());
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                return Ok(false, HtmlHelper.Encode(ex.Message));
            }
        }

        [HttpGet]
        public string GetUser(string userId)
        {
            try
            {
                if (!IsValidAccount())
                {
                    return OkLoginError();
                }
                return _userService.Get(userId).ToJson();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                return Ok(false, HtmlHelper.Encode(ex.Message));
            }
        }

        [HttpPost]
        public string PutUser(FormCollection form)
        {
            try
            {
                if (!IsValidAccount(UserRole.Manager))
                {
                    return OkLoginError();
                }
                var adminUser = OrgUserDataService.FindAdmin();
                if (SessionUserId != adminUser.Id)
                {
                    throw new PreValidationException("当前用户无权限");
                }
                var dto = new OrgUser
                {
                    Account = form["loginName"],
                    Name = form["name"],
                    Code = form["code"],
                    Mobile = form["mobile"],
                    Mail = form["mail"]
                };
                if (!string.IsNullOrEmpty(form["userId"]))
                {
                    dto.Id = form["userId"];
                }
                if (!string.IsNullOrEmpty(form["status"]))
                {
                    dto.Status = int.Parse(form["status"]);
                }
                if (!string.IsNullOrEmpty(form["sex"]))
                {
                    dto.Sex = int.Parse(form["sex"]);
                }
                if (!string.IsNullOrEmpty(form["birthday"]))
                {
                    dto.Birthday = DateTimeHelper.GetNonNullIsoDateValue(form["birthday"]);
                }

                if (_userService.AnyLoginName(dto.Account, dto.Id))
                {
                    throw new PreValidationException("登录名不允许重复");
                }
                if (!string.IsNullOrEmpty(dto.Code) && _userService.AnyCode(dto.Code, dto.Id))
                {
                    throw new PreValidationException("编号不允许重复");
                }
                if (string.IsNullOrEmpty(dto.Id))
                {
                    _userService.AddUser(dto);
                }
                else
                {
                    _userService.ChangeUser(dto);
                }
                return Ok($"{dto.Id},{ConfigHelper.DefaultUserPwd}");
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
        public string PutPasswordReset(string id)
        {
            try
            {
                if (!IsValidAccount(UserRole.Manager))
                {
                    return OkLoginError();
                }
                var userPwd = ConfigHelper.DefaultUserPwd;
                if (string.IsNullOrEmpty(id))
                {
                    return Ok(false, "用户ID不允许为空。");
                }
                _userService.ChangeUserPassword(id, userPwd);
                return Ok(true, userPwd);
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                var result = HtmlHelper.Encode(ex.Message);
                return Ok(false, result);
            }
        }

        [HttpPost]
        public string PutUserCancle(string id)
        {
            try
            {
                if (!IsValidAccount(UserRole.Manager))
                {
                    return OkLoginError();
                }
                if (string.IsNullOrEmpty(id))
                {
                    return Ok(false, "用户ID不允许为空。");
                }
                _userService.CancleUser(id);
                return Ok();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                var result = HtmlHelper.Encode(ex.Message);
                return Ok(false, result);
            }
        }

        [HttpPost]
        public string PutUserReactivation(string id)
        {
            try
            {
                if (!IsValidAccount(UserRole.Manager))
                {
                    return OkLoginError();
                }
                if (string.IsNullOrEmpty(id))
                {
                    return Ok(false, "用户ID不允许为空。");
                }
                _userService.ReactivationUser(id);
                return Ok();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                var result = HtmlHelper.Encode(ex.Message);
                return Ok(false, result);
            }
        }

        [HttpPost]
        public string PutUserRole(string userId, int userRole)
        {
            try
            {
                if (!IsValidAccount(UserRole.Manager))
                {
                    return OkLoginError();
                }
                if (string.IsNullOrEmpty(userId))
                {
                    return Ok(false, "用户ID不允许为空。");
                }
                _userService.ChangeUserRole(userId, (UserRole) userRole);
                return Ok();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                var result = HtmlHelper.Encode(ex.Message);
                return Ok(false, result);
            }
        }
    }
}