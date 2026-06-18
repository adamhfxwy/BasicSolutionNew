

namespace Solution.Core.Infrastructure.Configs.Identity;

internal class EmployeeConfig : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("T_Employee");
        builder.Property(x => x.Id).UseIdentityColumn().HasComment("id主键");
        builder.Property(x => x.RealName).HasMaxLength(255).HasComment("真实姓名");
        builder.Property(x => x.UserName).HasComment("用户名");
        builder.Property(x => x.Cellphone).HasMaxLength(255).HasComment("手机号");
        builder.Property(x => x.CreateTime).HasColumnType("datetime").HasComment("创建时间");
        builder.Property(x => x.PasswordHash).IsRequired(false).HasComment("密码").HasMaxLength(255);
        builder.Property(x => x.PasswordSalt).IsRequired(false).HasComment("盐").HasMaxLength(255);
        builder.Property(x => x.Remark).HasMaxLength(255).HasComment("备注");
        builder.Property(x => x.JobName).HasMaxLength(255).HasComment("岗位名称");
        builder.Property(x => x.ShiftId).HasComment("班次id").IsRequired(false);
        builder.Property(x => x.Sex).HasComment(" 性别 1-男 2-女");
        builder.Property(x => x.LoginPermissions).HasComment("登录权限 1-无权限 2-web权限  3-app权限 4-小程序权限  5-所有权限");
        builder.Property(x => x.DepartmentId).HasComment("部门id").IsRequired(false); ;
        builder.Property(x => x.Status).HasComment("员工状态 1-正常 2-离职");
        builder.Property(x => x.Age).HasComment("年龄");
        builder.Property(x => x.PhotoPath).HasMaxLength(255).HasComment("照片地址");
        builder.Property(x => x.IsDeleted).HasComment("软删  1-未删除 2-已删除");
        //一对多配置
        
        builder.HasOne(x => x.Department).WithMany(x => x.Employees).IsRequired(false).HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ShiftInfo).WithMany().HasForeignKey(x => x.ShiftId);
        builder.HasMany(x => x.Roles).WithMany(x => x.Employees)
            .UsingEntity<Dictionary<string, object>>("UsersRoles",
            x => x.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
            x => x.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId"),
            x => x.ToTable("T_EmployeeRoleRelation"));
    }
}
