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
    [Authorize(Roles ="Admin")]
    public class TeacherController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private DataContext dataContext;

        public TeacherController(ILogger<HomeController> logger, DataContext context) {
            _logger = logger;
            dataContext = context;
        }

        /// <summary>
        /// 教师信息首页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取学生成绩
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Query(string Name) {
            var teachers = new List<Teacher>();
            if (string.IsNullOrEmpty(Name))
            {
                teachers = dataContext.Teachers.ToList();

            }
            else {
                teachers = dataContext.Teachers.Where(r => r.Name.Contains(Name)).ToList();
            }
            
            return Json(teachers);
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
            var teacher = dataContext.Teachers.FirstOrDefault((s) => s.Id == id);
            return View(teacher);
        }

        /// <summary>
        /// 新增保存
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Save(Teacher teacher) {
           dataContext.Teachers.Add(teacher);
            dataContext.SaveChanges();
            if (teacher.Id > 0)
            {
                return Json("Success");
            }
            else {
               return Json("Failure");
            }
        }

        /// <summary>
        /// 编辑保存
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Save2(Teacher teacher) {
            var id = teacher.Id;
            if (id == 0)
            {
                //新增
                return Save(teacher);
            }
            else {
                var tmp = dataContext.Teachers.FirstOrDefault(s => s.Id == id);
                if (tmp != null) {
                    tmp.Name = teacher.Name;
                    tmp.Age = teacher.Age;
                    tmp.Title = teacher.Title;
                    tmp.Dept = teacher.Dept;
                    tmp.Sex = teacher.Sex;
                    tmp.No = teacher.No;
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
            var tmp = dataContext.Teachers.FirstOrDefault(s => s.Id == id);
            if (tmp != null)
            {
                dataContext.Teachers.Remove(tmp);
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
