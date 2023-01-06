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
    public class AchieveController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private DataContext dataContext;

        public AchieveController(ILogger<HomeController> logger, DataContext context)
        {
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
        public JsonResult Query(string Name)
        {
            var achieves = new List<AchieveInfo>();
            if (string.IsNullOrEmpty(Name))
            {
                var query = from a in dataContext.Achieves
                            join s in dataContext.Students on a.StudentId equals s.No
                            join c in dataContext.Courses on a.CourseId equals c.No
                            select new AchieveInfo { Id = a.Id, CourseId = a.CourseId, StudentId = a.StudentId, Score = a.Score, StudentName = s.Name, CourseName = c.Name, Credit = GetCredit(c.Credit, a.Score) };
                achieves = query.ToList();

            }
            else
            {
                var query = from a in dataContext.Achieves
                            join s in dataContext.Students on a.StudentId equals s.No
                            join c in dataContext.Courses on a.CourseId equals c.No
                            where s.Name.Contains(Name) || c.Name.Contains(Name)
                            select new AchieveInfo { Id = a.Id, CourseId = a.CourseId, StudentId = a.StudentId, Score = a.Score, StudentName = s.Name, CourseName = c.Name, Credit = GetCredit(c.Credit, a.Score) };
                achieves = query.ToList();
            }

            return Json(achieves);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Admin,Teacher")]
        [HttpGet]
        public IActionResult Add()
        {
            var query = from a in dataContext.Achieves
                        join s in dataContext.Students on a.StudentId equals s.No
                        join c in dataContext.Courses on a.CourseId equals c.No
                        select new AchieveInfo { Id = a.Id, CourseId = a.CourseId, StudentId = a.StudentId, Score = a.Score, StudentName = s.Name, CourseName = c.Name, Credit = GetCredit(c.Credit, a.Score) };
            return View(query.ToList());
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Teacher")]
        public IActionResult Edit(int id)
        {
            var query = from a in dataContext.Achieves
                        join s in dataContext.Students on a.StudentId equals s.No
                        join c in dataContext.Courses on a.CourseId equals c.No
                        where a.Id == id
                        select new AchieveInfo { Id = a.Id, CourseId = a.CourseId, StudentId = a.StudentId, Score = a.Score, StudentName = s.Name, CourseName = c.Name, Credit = GetCredit(c.Credit, a.Score) };
            return View(query.First());
        }

        /// <summary>
        /// 新增保存
        /// </summary>
        /// <param name="achieve"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        public JsonResult Save(Achieve achieve)
        {
            dataContext.Achieves.Add(achieve);
            dataContext.SaveChanges();
            if (achieve.Id > 0)
            {
                return Json("Success");
            }
            else
            {
                return Json("Failure");
            }
        }

        /// <summary>
        /// 编辑保存
        /// </summary>
        /// <param name="achieve"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        public JsonResult Save2(Achieve achieve)
        {
            var id = achieve.Id;
            if (id == 0)
            {
                //新增
                return Save(achieve);
            }
            else
            {
                var tmp = dataContext.Achieves.FirstOrDefault(s => s.Id == id);
                if (tmp != null)
                {
                    tmp.CourseId = achieve.CourseId;
                    tmp.StudentId = achieve.StudentId;
                    tmp.Score = achieve.Score;
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

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var tmp = dataContext.Achieves.FirstOrDefault(s => s.Id == id);
            if (tmp != null)
            {
                dataContext.Achieves.Remove(tmp);
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

        static private int GetCredit(int credit, double score)
        {
            if (score >= 60)
            {
                return credit;
            }
            return 0;
        }
    }
}
