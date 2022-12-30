using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SAM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAM.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private DataContext dataContext;

        public UsersController(ILogger<HomeController> logger, DataContext context) {
            _logger = logger;
            dataContext = context;
        }

        /// <summary>
        /// 用户成绩首页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
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
            var users = new List<UserInfo>();
            if (string.IsNullOrEmpty(Name))
            {
                var query = from u in dataContext.Users
                            join r in dataContext.UserRoles on u.Id equals r.UserId
                            join x in dataContext.Roles on r.RoleId equals x.Id
                            select new UserInfo { Id = u.Id, UserName = u.UserName, NickName = u.NickName, RoleName = x.Name, LastLogin = u.LastLogin };
                users = query.ToList();
            }
            else {
                var query = from u in dataContext.Users
                            join r in dataContext.UserRoles on u.Id equals r.UserId
                            join x in dataContext.Roles on r.RoleId equals x.Id
                            where x.Name.Contains(Name) || u.NickName.Contains(Name) || u.UserName.Contains(Name)
                            select new UserInfo { Id = u.Id, UserName = u.UserName, NickName = u.NickName, RoleName = x.Name, LastLogin = u.LastLogin };
                users = query.ToList();
            }
            
            return Json(users);
        }

        /// <summary>
        /// 获取权限信息
        /// </summary>
        /// 
        [Authorize(Roles = "Teacher,Student")]
        [HttpPost]
        public JsonResult Roles() {
            List<Role> roles = dataContext.Roles.ToList();
            return Json(roles);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add() {
            return View();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Edit(int id) {
            var query = from u in dataContext.Users
                        join r in dataContext.UserRoles on u.Id equals r.UserId
                        join x in dataContext.Roles on r.RoleId equals x.Id
                        where u.Id == id
                        select new UserInfo { Id = u.Id, UserName = u.UserName, NickName = u.NickName, RoleName = x.Name, RoleId = r.RoleId, LastLogin = u.LastLogin };
            return View(query.FirstOrDefault());
        }

        /// <summary>
        /// 新增保存
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Save(UserInfo user) {
            user.LastLogin = DateTime.MaxValue;
            var tmp_u = new User();
            var tmp_r = new UserRole();
            tmp_u.UserName = user.UserName;
            tmp_u.Password = user.Password;
            tmp_u.NickName = user.NickName;
            tmp_u.LastLogin = user.LastLogin;
            dataContext.Users.Add(tmp_u);
            dataContext.SaveChanges();
            if (tmp_u.Id <= 0)
            {
               return Json("Failure");
            }

            tmp_r.RoleId = user.RoleId;
            tmp_r.UserId = tmp_u.Id;
            dataContext.UserRoles.Add(tmp_r);
            dataContext.SaveChanges();
            if (tmp_r.Id > 0)
            {
                return Json("Success");
            }
            else
            {
                dataContext.Users.Remove(tmp_u);
                dataContext.SaveChanges();
                return Json("Failure");
            }
        }

        /// <summary>
        /// 编辑保存
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Save2(UserInfo user) {
            var id = user.Id;
            if (id == 0)
            {
                //新增
                return Save(user);
            }
            else {
                var tmp = dataContext.Users.FirstOrDefault(s => s.Id == id);
                var tmp_r = dataContext.UserRoles.FirstOrDefault(r => r.UserId == id);
                if (tmp_r != null)
                {
                    tmp_r.RoleId = user.RoleId;
                }
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

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete(int id) {
            var tmp = dataContext.Users.FirstOrDefault(s => s.Id == id);
            var tmp_r = dataContext.UserRoles.FirstOrDefault(r => r.UserId == id);
            if (tmp != null)
            {
                dataContext.Users.Remove(tmp);
                dataContext.UserRoles.Remove(tmp_r);
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
