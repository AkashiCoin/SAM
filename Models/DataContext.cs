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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Achieve>(entity =>
            {
                entity.HasKey(e => e.StudentId);//设置Book实体的BookCode属性为EF Core实体的Key属性

                entity.HasIndex(e => e.Id)
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();//设置Book实体的Id属性为插入数据到数据库Book表时自动生成，因为Book表的ID列为自增列
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.StudentId).HasMaxLength(50);

                entity.Property(e => e.CourseId).HasMaxLength(50);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.No);//设置Book实体的BookCode属性为EF Core实体的Key属性

                entity.HasIndex(e => e.No)
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();//设置Book实体的Id属性为插入数据到数据库Book表时自动生成，因为Book表的ID列为自增列
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.Property(e => e.TeacherId).HasMaxLength(20);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.No);//设置Person实体的PersonCode属性为EF Core实体的Key属性

                entity.HasIndex(e => e.No)
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();//设置Person实体的Id属性为插入数据到数据库Person表时自动生成，因为Person表的ID列为自增列
                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Name).HasMaxLength(50);

                //设置Person实体和Book实体之间的一对多关系，尽管我们并没有在数据库中建立Person表和Book表之间的一对多外键关系，但是我们可以用EF Core的Fluent API在实体层面设置外键关系
                //entity.HasMany(p => p.Achieves)//设置Person实体通过属性Book可以找到多个Book实体，表示Person表是一对多关系中的主表
                //.WithOne(b => b.Student)//设置Book实体通过属性Person可以找到一个Person实体，表示Book表是一对多关系中的从表
                //.HasPrincipalKey(p => p.No)//设置Person表的PersonCode列为一对多关系中的主表键
                //.HasForeignKey(b => b.StudentId)//设置Book表的PersonCode列为一对多关系中的从表外键
                //.OnDelete(DeleteBehavior.ClientSetNull);//设置一对多关系的级联删除效果为DeleteBehavior.ClientSetNull
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasKey(e => e.No);//设置Person实体的PersonCode属性为EF Core实体的Key属性

                entity.HasIndex(e => e.No)
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();//设置Person实体的Id属性为插入数据到数据库Person表时自动生成，因为Person表的ID列为自增列
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.Property(e => e.Dept).HasMaxLength(50);

                entity.Property(e => e.Sex).HasMaxLength(50);

                //设置Person实体和Book实体之间的一对多关系，尽管我们并没有在数据库中建立Person表和Book表之间的一对多外键关系，但是我们可以用EF Core的Fluent API在实体层面设置外键关系
                //entity.HasMany(p => p.Courses)//设置Person实体通过属性Book可以找到多个Book实体，表示Person表是一对多关系中的主表
                //.WithOne(b => b.Teacher)//设置Book实体通过属性Person可以找到一个Person实体，表示Book表是一对多关系中的从表
                //.HasPrincipalKey(p => p.No)//设置Person表的PersonCode列为一对多关系中的主表键
                //.HasForeignKey(b => b.TeacherId)//设置Book表的PersonCode列为一对多关系中的从表外键
                //.OnDelete(DeleteBehavior.ClientSetNull)//设置一对多关系的级联删除效果为DeleteBehavior.ClientSetNull
                //.HasConstraintName("FK_Courses_Teacher");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.No);//设置Person实体的PersonCode属性为EF Core实体的Key属性

                entity.HasIndex(e => e.No)
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();//设置Person实体的Id属性为插入数据到数据库Person表时自动生成，因为Person表的ID列为自增列
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Type).HasMaxLength(50);

                //设置Person实体和Book实体之间的一对多关系，尽管我们并没有在数据库中建立Person表和Book表之间的一对多外键关系，但是我们可以用EF Core的Fluent API在实体层面设置外键关系
                //entity.HasMany(p => p.Achieves)//设置Person实体通过属性Book可以找到多个Book实体，表示Person表是一对多关系中的主表
                //.WithOne(b => b.Course)//设置Book实体通过属性Person可以找到一个Person实体，表示Book表是一对多关系中的从表
                //.HasPrincipalKey(p => p.No)//设置Person表的PersonCode列为一对多关系中的主表键
                //.HasForeignKey(b => b.CourseId)//设置Book表的PersonCode列为一对多关系中的从表外键
                //.OnDelete(DeleteBehavior.ClientSetNull);//设置一对多关系的级联删除效果为DeleteBehavior.ClientSetNull
            });
        }
    }
}
