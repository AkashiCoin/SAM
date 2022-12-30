using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAM.Models
{
    public class User
    {
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 最后登录
        /// </summary>
        public DateTime LastLogin { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string NickName { get; set; }
    }
}
