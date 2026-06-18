

namespace Solution.Core.Infrastructure
{
    public class NpgSqlContext:DbContext
    {
        #region 实体
        /// <summary>
        /// 员工上下文
        /// </summary>
        public virtual DbSet<Employee>  Employees { get; set; } = null!;
        /// <summary>
        /// 部门上下文
        /// </summary>
        public virtual DbSet<Department>  Departments { get; set; } = null!;
        /// <summary>
        /// 角色上下文
        /// </summary>
        public virtual DbSet<Role>  Roles { get; set; } = null!;
        /// <summary>
        /// 菜单上下文
        /// </summary>
        public virtual DbSet<Menu>  Menus { get; set; } = null!;
        #endregion
        public NpgSqlContext(DbContextOptions<NpgSqlContext> options)
        : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //string connStr = "Host=127.0.0.1;Database=_server_core;Username=postgres;Password=root";
            //optionsBuilder.UseNpgsql(connStr);
            //optionsBuilder.LogTo(Console.WriteLine);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
