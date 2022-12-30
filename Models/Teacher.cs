using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAM.Models
{
    public class Teacher
    {

        /// <summary>
        /// 唯一标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 教师编号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 教师姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 年纪
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Boolean Sex { get; set; }

        /// <summary>
        /// 专业
        /// </summary>
        public string Dept { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Title { get; set; }
    }
}
