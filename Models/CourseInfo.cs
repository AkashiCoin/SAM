using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAM.Models
{
    public class CourseInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 课程编号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// TeaderID
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// 老师姓名
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 课程类型
        /// </summary>
        public string Type { get; set; }
    }
}
