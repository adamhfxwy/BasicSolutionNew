

namespace Solution.Core.Infrastructure.Configs.Identity
{
    internal class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("T_Role");
            builder.Property(x => x.Id).UseIdentityColumn().HasComment("id主键");
            builder.Property(x => x.RoleName).HasMaxLength(255).HasComment("角色（职位）名称");
            builder.Property(x => x.Remark).HasComment("备注").HasMaxLength(255);
            builder.Property(x => x.CreateTime).HasColumnType("datetime").HasComment("创建时间");
            builder.Property(x => x.Permissions).HasComment("权限项");
            builder.Property(x => x.MobilePermissions).HasComment("移动端权限项");
            builder.Property(x => x.IsDeleted).HasComment("软删  1-未删除 2-已删除");
        }
    }
}
