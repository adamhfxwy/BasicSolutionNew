

namespace Solution.Core.Infrastructure.Configs.Common
{
    internal class DictionaryConfig : IEntityTypeConfiguration<Dictionary>
    {
        public void Configure(EntityTypeBuilder<Dictionary> builder)
        {
            builder.ToTable("T_Dictionary");
            builder.Property(x => x.Id).UseIdentityColumn().HasComment("id主键");
            builder.Property(x => x.CreateTime).HasColumnType("datetime").HasComment("创建时间");
            builder.Property(x => x.IsDeleted).HasComment("软删  1-未删除 2-已删除");
            builder.Property(x => x.Key).HasMaxLength(255).HasComment("键");
            builder.Property(x => x.Value).HasMaxLength(255).HasComment("值");
            builder.Property(x => x.Description).HasMaxLength(255).HasComment("描述");
            builder.Property(x => x.Type).HasComment("类型  0-创建类型");
        }
    }
}
