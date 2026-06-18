

namespace Solution.Core.Infrastructure.Configs.Common
{
    internal class UserConfig : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.ToTable("T_UserInfo");
            builder.Property(x => x.Id).UseIdentityColumn().HasComment("id主键");
            builder.Property(x => x.CreateTime).HasColumnType("datetime").HasComment("创建时间");
            builder.Property(x => x.IsDeleted).HasComment("软删  1-未删除 2-已删除");
            builder.Property(x => x.Name).HasMaxLength(255).HasComment("用户名称");
            builder.Property(x => x.Cellphone).HasMaxLength(255).HasComment("联系方式");
            builder.Property(x => x.Address).HasMaxLength(255).HasComment("地址");
        }
    }
}
