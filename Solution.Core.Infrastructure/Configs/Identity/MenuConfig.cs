

namespace Solution.Core.Infrastructure.Configs.Identity
{
    internal class MenuConfig : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable("T_Menu");
            builder.Property(x => x.Id).UseIdentityColumn().HasComment("id主键");
            builder.Property(x => x.Path).HasMaxLength(255).HasComment("路由").IsRequired(false);
            builder.Property(x => x.Name).HasMaxLength(255).HasComment("菜单名称");
            builder.Property(x => x.ButtonName).HasMaxLength(255).HasComment("按钮名称");
            builder.Property(x => x.MenuName).HasMaxLength(255).HasComment("菜单名称");
            builder.Property(x => x.CreateTime).HasColumnType("datetime").HasComment("创建时间");
            builder.Property(x => x.ParentId).HasComment("父级id").IsRequired(false);
            builder.Property(x => x.Description).HasMaxLength(255).HasComment("描述").IsRequired(false);
            builder.Property(x => x.Component).HasMaxLength(255).HasComment("组件").IsRequired(false);
            builder.Property(x => x.Icon).HasComment("图标").IsRequired(false);
            builder.Property(x => x.IsButton).HasComment("是否是按钮 1-否 2-是");
            builder.Property(x => x.Level).HasComment("层级");
            builder.Property(x => x.MenuCode).HasMaxLength(255).HasComment("菜单或按钮编码").IsRequired(false);
            builder.Property(x => x.IsDeleted).HasComment("软删  1-未删除 2-已删除");
        }
    }
}
