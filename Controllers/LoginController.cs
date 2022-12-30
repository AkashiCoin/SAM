using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SAM.Controllers
{

    public class LoginController : Controller
    {
        private DataContext dataContext;

        public LoginController(DataContext context)
        {
            dataContext = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            var claim = HttpContext.User.FindFirst("UserId");
            if (claim == null)
            {
                return View();
            }
            int.TryParse(claim.Value, out int userId);
            if (userId != 0)
            {
                return Redirect("/Home");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(User user)
        {
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                ViewBag.Msg = "用户名或密码为空";
                return View("Index", user);
            }
            else
            {
                var item = dataContext.Users.FirstOrDefault(i => i.UserName == user.UserName && i.Password == user.Password);
                if (item != null)
                {
                    //HttpContext.Session.SetInt32("UserId", item.Id);
                    //HttpContext.Session.SetString("LastLogin", item.LastLogin.ToString());

                    //用户角色列表，实际操作是读数据库
                    var query = from u in dataContext.Users
                                join r in dataContext.UserRoles on u.Id equals r.UserId
                                join x in dataContext.Roles on r.RoleId equals x.Id
                                where u.Id == item.Id
                                select new { u.Id, u.UserName, x.Name };
                    var roleList = query.ToList();
                    var claims = new List<Claim>() //用Claim保存用户信息
                    {
                        new Claim(ClaimTypes.Name,item.UserName),
                        new Claim("UserId",item.Id.ToString()),
                        new Claim("LastLogin",item.LastLogin.ToString()),
                    };
                    //填充角色
                    foreach (var role in roleList)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Name));
                    }
                    //把用户信息装到ClaimsPrincipal
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Customer"));
                    //登录，把用户信息写入到cookie
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,
                        new AuthenticationProperties
                        {
                            ExpiresUtc = DateTime.Now.AddMinutes(30)//过期时间30分钟
                        }).Wait();
                    SaveLastLogin(item);
                    return Redirect("/Home");
                }
                else
                {
                    ViewBag.Msg = "用户名或密码验证错误";
                    return View("Index", user);
                }

            }
        }

        /// <summary>
        /// 保存登录时间
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public JsonResult SaveLastLogin(User user)
        {
            var id = user.Id;
            var tmp = dataContext.Users.FirstOrDefault(s => s.Id == id);
            if (tmp != null)
            {
                tmp.LastLogin = DateTime.Now;
                int num = dataContext.SaveChanges();
                if (num > 0)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("Failure");
                }
            }
            return Json("Error");
        }
    }
}
