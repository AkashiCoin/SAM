using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SAM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAM.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private DataContext dataContext;

        public UserController(ILogger<HomeController> logger, DataContext context) {
            _logger = logger;
            dataContext = context;
        }

        /// <summary>
        /// 用户信息首页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var query = from u in dataContext.Users
                        join r in dataContext.UserRoles on u.Id equals r.UserId
                        join x in dataContext.Roles on r.RoleId equals x.Id
                        where u.Id == int.Parse(HttpContext.User.FindFirst("UserId").Value)
                        select new UserInfo { Id = u.Id, UserName = u.UserName, NickName = u.NickName, RoleName = x.Name, RoleId = r.RoleId, LastLogin = u.LastLogin };
            return View(query.FirstOrDefault());
        }
        /// <summary>
        /// 401
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Query(string Name) {
            var users = new List<User>();
            if (string.IsNullOrEmpty(Name))
            {
                users = dataContext.Users.ToList();

            }
            else {
                users = dataContext.Users.Where(r => r.NickName.Contains(Name)).ToList();
            }
            
            return Json(users);
        }

        /// <summary>
        /// 返回权限信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Roles()
        {
            return Json(dataContext.Roles.ToList());
        }

        /// <summary>
        /// 编辑保存
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Save2(User user) {
            var id = user.Id;
            if (id != int.Parse(HttpContext.User.FindFirst("UserId").Value))
            {
                return Json("权限不足");
            }
            if (id == 0)
            {
                return Json("Error");
            }
            else {
                var tmp = dataContext.Users.FirstOrDefault(s => s.Id == id);
                if (tmp != null) {
                    tmp.UserName = user.UserName;
                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        tmp.Password = user.Password;
                    }
                    tmp.NickName = user.NickName;
                    int num = dataContext.SaveChanges();
                    if (num > 0)
                    {
                        return Json("Success");
                    }
                    else {
                        return Json("Failure");
                    }
                }
                return Json("Error");
            }
        }
    }
}
