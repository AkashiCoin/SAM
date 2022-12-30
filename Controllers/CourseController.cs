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
    public class CourseController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private DataContext dataContext;

        public CourseController(ILogger<HomeController> logger, DataContext context) {
            _logger = logger;
            dataContext = context;
        }

        /// <summary>
        /// 学生课程首页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取课程信息
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Query(string Name) {
            var courses = new List<CourseInfo>();
            if (string.IsNullOrEmpty(Name))
            {
                var query = from c in dataContext.Courses
                            join t in dataContext.Teachers on c.TeacherId equals t.No
                            select new CourseInfo { Id = c.Id, TeacherId = c.TeacherId, No = c.No, TeacherName = t.Name, Type = c.Type, Name = c.Name };
                courses = query.ToList();
            }
            else {
                var query = from c in dataContext.Courses
                            join t in dataContext.Teachers on c.TeacherId equals t.No
                            where c.Name.Contains(Name) || t.Name.Contains(Name)
                            select new CourseInfo { Id = c.Id, TeacherId = c.TeacherId, No = c.No, TeacherName = t.Name, Type = c.Type, Name = c.Name };
                courses = query.ToList();
            }
            
            return Json(courses);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet]
        public IActionResult Add()
        {
            var query = from c in dataContext.Courses
                        join t in dataContext.Teachers on c.TeacherId equals t.No
                        select new CourseInfo { Id = c.Id, TeacherId = c.TeacherId, No = c.No, TeacherName = t.Name, Type = c.Type, Name = c.Name };
            return View(query.ToList());
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Teacher")]
        public IActionResult Edit(int id) {
            var query = from c in dataContext.Courses
                        join t in dataContext.Teachers on c.TeacherId equals t.No
                        where c.Id == id
                        select new CourseInfo { Id = c.Id, TeacherId = c.TeacherId, No = c.No, TeacherName = t.Name, Type = c.Type, Name = c.Name };
            var course = query.FirstOrDefault();
            return View(course);
        }

        /// <summary>
        /// 新增保存
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        public JsonResult Save(Course course) {
            dataContext.Courses.Add(course);
            dataContext.SaveChanges();
            if (course.Id > 0)
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
        /// <param name="course"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        public JsonResult Save2(Course course) {
            var id = course.Id;
            if (id == 0)
            {
                //新增
                return Save(course);
            }
            else {
                var tmp = dataContext.Courses.FirstOrDefault(s => s.Id == id);
                if (tmp != null) {
                    tmp.Name = course.Name;
                    tmp.No = course.No;
                    tmp.TeacherId = course.TeacherId;
                    tmp.Type = course.Type;
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
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        public JsonResult Delete(int id) {
            var tmp = dataContext.Courses.FirstOrDefault(s => s.Id == id);
            if (tmp != null)
            {
                dataContext.Courses.Remove(tmp);
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
