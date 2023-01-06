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
    [Authorize]
    public class StudentController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private DataContext dataContext;

        public StudentController(ILogger<HomeController> logger, DataContext context) {
            _logger = logger;
            dataContext = context;
        }

        /// <summary>
        /// 学生成绩首页
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
            var students = new List<Student>();
            if (string.IsNullOrEmpty(Name))
            {
                students = dataContext.Students.ToList();

            }
            else {
                students = dataContext.Students.Where(r => r.Name.Contains(Name)).ToList();
            }
            
            return Json(students);
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
            var student = dataContext.Students.FirstOrDefault((s) => s.Id == id);
            return View(student);
        }

        /// <summary>
        /// 新增保存
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Save(Student student) {
            dataContext.Students.Add(student);
            dataContext.SaveChanges();
            if (student.Id > 0)
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
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Save2(Student student) {
            var id = student.Id;
            if (id == 0)
            {
                //新增
                return Save(student);
            }
            else {
                var tmp = dataContext.Students.FirstOrDefault(s => s.Id == id);
                if (tmp != null) {
                    tmp.Name = student.Name;
                    tmp.Age = student.Age;
                    tmp.Class = student.Class;
                    tmp.Dept = student.Dept;
                    tmp.Sex = student.Sex;
                    tmp.No = student.No;
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
            var tmp = dataContext.Students.FirstOrDefault(s => s.Id == id);
            if (tmp != null)
            {
                dataContext.Students.Remove(tmp);
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
