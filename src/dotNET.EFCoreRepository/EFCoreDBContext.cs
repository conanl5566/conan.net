using dotNET.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
namespace dotNET.EFCoreRepository
{
    public partial class EFCoreDBContext : DbContext
    {
        public EFCoreDBContext(DbContextOptions<EFCoreDBContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            // optionsBuilder.UseMySql("server=localhost;port=3306;database=acdb;uid=root;pwd=123456;CharSet=utf8;pooling=true;SslMode=None;");//配置连接字符串
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.AddEntityConfigurationsFromAssembly(GetType().Assembly);
        }

        public virtual DbSet<demo1> demo1 { get; set; }
        public virtual DbSet<AreaList> AreaList { get; set; }

        public virtual DbSet<Department> Department { get; set; }

        public virtual DbSet<ErrorLog> ErrorLog { get; set; }

        public virtual DbSet<ItemsData> ItemsData { get; set; }

        public virtual DbSet<Module> Module { get; set; }

        public virtual DbSet<ModuleButton> ModuleButton { get; set; }

        public virtual DbSet<OperateLog> OperateLog { get; set; }

        public virtual DbSet<Role> Role { get; set; }

        public virtual DbSet<RoleAuthorize> RoleAuthorize { get; set; }

        public virtual DbSet<User> User { get; set; }
    }
}