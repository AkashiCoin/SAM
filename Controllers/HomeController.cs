using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SAM.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SAM.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private DataContext dataContext;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            dataContext = context;
        }

        public IActionResult Index()
        {
            #region session登录
            //int? userId = HttpContext.Session.GetInt32("UserId");
            //string lastLogin = HttpContext.Session.GetString("LastLogin");
            ////判断是否登录
            //if (userId != null)
            //{
            //    var user = dataContext.Users.FirstOrDefault(u=>u.Id== userId);
            //    if (user != null) {
            //        ViewBag.NickName = user.NickName;
            //        ViewBag.LastLogin = lastLogin;
            //        ViewBag.UserRights = GetUserRights();
            //    }
            //    return View();
            //}
            //else
            //{
            //    return Redirect("/Login");
            //}
            #endregion
            var claim = HttpContext.User.FindFirst("UserId");
            if (claim == null)
            {
                return Redirect("/Login");
            }
            int.TryParse(claim.Value, out int userId);
            string lastLogin = HttpContext.User.FindFirst("LastLogin").Value;
            var user = dataContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                ViewBag.NickName = user.NickName;
                ViewBag.LastLogin = lastLogin;
                ViewBag.UserRights = GetUserRights();
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Welcome() {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public List<UserRight> GetUserRights()
        {
            // int? userId = HttpContext.Session.GetInt32("UserId");
            int.TryParse(HttpContext.User.FindFirst("UserId").Value, out int userId);
            if (userId != 0)
            {
                var query = from u in dataContext.UserRoles
                            join r in dataContext.Roles on u.RoleId equals r.Id
                            join x in dataContext.RoleMenus on r.Id equals x.RoleId
                            join m in dataContext.Menus on x.MenuId equals m.Id
                            where u.UserId == userId
                            select new UserRight { Id=m.Id, RoleName = r.Name, MenuName = m.Name,  Url = m.Url, ParentId = m.ParentId, SortId=m.SortId };

                return query.ToList();
            }
            return null;
        }

        /// <summary>
        /// 退出
        /// </summary>
        public IActionResult Logout()
        {
            // HttpContext.Session.Clear();
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login");
        }
    }
}
