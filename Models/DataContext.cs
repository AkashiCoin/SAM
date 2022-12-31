using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAM.Models
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Menu> Menus { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<RoleMenu> RoleMenus { get; set; }
        public DbSet<Course> Courses { get; set; }

        public DbSet<Achieve> Achieves { get; set; }

        /// <summary>
        /// 学生
        /// </summary>
        public DbSet<Student> Students { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }
    }
}
