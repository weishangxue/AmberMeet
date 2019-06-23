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
                ValidationLoginV(UserRole.Manager);

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
                ValidationLoginV(UserRole.Manager);
                ViewBag.sexMan = (int) UserSex.Man;
                ViewBag.sexLady = (int) UserSex.Lady;
                if (!string.IsNullOrEmpty(id))
                {
                    var user = _userService.Get(id);
                    ViewData["userId"] = user.Id;
                    ViewData["state"] = user.State;
                    ViewData["stateName"] = ((UserState) user.State).ToEnumText();
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
                ValidationLogin(UserRole.Manager);
                var result = _userService.GetPaged(page, rows, state, keywords);
                return OkJqGrid(_orgJsonService.GetJqGridJson(result, page, rows));
            }
            catch (Exception ex)
            {
                return OkExceptionStr(ex);
            }
        }

        /// <summary>
        ///     获取选择用户控件用户列表数据
        /// </summary>
        /// <param name="keywords">模糊条件</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUserSelectControlUserList(string keywords)
        {
            try
            {
                ValidationLogin();
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
                return OkException(ex);
            }
        }

        [HttpGet]
        public ActionResult GetUser(string userId)
        {
            try
            {
                ValidationLogin();
                return OkJson(_userService.Get(userId));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost]
        public ActionResult PutUser(FormCollection form)
        {
            try
            {
                ValidationLogin(UserRole.Manager);
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
                    dto.State = int.Parse(form["status"]);
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
                return OkException(ex);
            }
        }

        [HttpPost]
        public ActionResult PutPasswordReset(string id)
        {
            try
            {
                ValidationLogin(UserRole.Manager);
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
                return OkException(ex);
            }
        }

        [HttpPost]
        public ActionResult PutUserCancle(string id)
        {
            try
            {
                ValidationLogin(UserRole.Manager);
                if (string.IsNullOrEmpty(id))
                {
                    return Ok(false, "用户ID不允许为空。");
                }
                _userService.CancleUser(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost]
        public ActionResult PutUserReactivation(string id)
        {
            try
            {
                ValidationLogin(UserRole.Manager);
                if (string.IsNullOrEmpty(id))
                {
                    return Ok(false, "用户ID不允许为空。");
                }
                _userService.ReactivationUser(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost]
        public ActionResult PutUserRole(string userId, int userRole)
        {
            try
            {
                ValidationLogin(UserRole.Manager);
                if (string.IsNullOrEmpty(userId))
                {
                    return Ok(false, "用户ID不允许为空。");
                }
                _userService.ChangeUserRole(userId, (UserRole) userRole);
                return Ok();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}