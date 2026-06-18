using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Core.Infrastructure.Configs.Common
{
    internal class VersionAppConfig : IEntityTypeConfiguration<VersionApp>
    {
        public void Configure(EntityTypeBuilder<VersionApp> builder)
        {
            builder.ToTable("T_VersionApp");
            builder.Property(x => x.Id).UseIdentityColumn().HasComment("id主键");
            builder.Property(x => x.CreateTime).HasColumnType("datetime").HasComment("创建时间");
            builder.Property(x => x.IsDeleted).HasComment("软删  1-未删除 2-已删除");
            builder.Property(x => x.Version).HasMaxLength(255).HasComment("app版本号");
            builder.Property(x => x.EditDate).HasColumnType("datetime").HasComment("更新时间");
            builder.Property(x => x.IsUpdate).HasComment("是否需要强制更新 0：不需要 1：需要");
            builder.Property(x => x.DataVersion).HasMaxLength(255).HasComment("数据库版本");
            builder.Property(x => x.Source).HasComment("类型 1:app");
        }
    }
}
