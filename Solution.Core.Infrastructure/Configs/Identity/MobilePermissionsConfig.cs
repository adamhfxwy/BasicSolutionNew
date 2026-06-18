

namespace Solution.Core.Infrastructure.Configs.Identity
{
    internal class MobilePermissionsConfig : IEntityTypeConfiguration<MobilePermissions>
    {
        public void Configure(EntityTypeBuilder<MobilePermissions> builder)
        {
            builder.ToTable("T_MobilePermissions");
            builder.Property(x => x.Id).UseIdentityColumn().HasComment("id主键");
            builder.Property(x => x.PermissionName).HasMaxLength(255).HasComment("权限项名称");
            builder.Property(x => x.Remark).HasComment("备注").HasMaxLength(255);
            builder.Property(x => x.ParentId).HasComment("父级id");
            builder.Property(x => x.PermissionCode).HasComment("权限项编码").HasMaxLength(255);
            builder.Property(x => x.CreateTime).HasColumnType("datetime").HasComment("创建时间");
            builder.Property(x => x.IsDeleted).HasComment("软删  1-未删除 2-已删除");
        }
    }
}
