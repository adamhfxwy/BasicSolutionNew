using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Core.Infrastructure.Configs.Common
{
    internal class ShiftInfoConfig : IEntityTypeConfiguration<ShiftInfo>
    {
        public void Configure(EntityTypeBuilder<ShiftInfo> builder)
        {
            builder.ToTable("T_ShiftInfo");
            builder.Property(x => x.Id).UseIdentityColumn().HasComment("id主键");
            builder.Property(x => x.CreateTime).HasColumnType("datetime").HasComment("创建时间");
            builder.Property(x => x.IsDeleted).HasComment("软删  1-未删除 2-已删除");
            builder.Property(x => x.ShiftName).HasMaxLength(255).HasComment("班次名称");
            builder.Property(x => x.Remark).HasComment("备注");
            builder.Property(x => x.BeginTime).HasConversion(
                  v => v.ToTimeSpan(),
                  v => TimeOnly.FromTimeSpan(v)
              )
              .HasColumnType("time").HasComment("开始时间");
            builder.Property(x => x.EndTime).HasConversion(
                   v => v.ToTimeSpan(),
                   v => TimeOnly.FromTimeSpan(v)
               )
               .HasColumnType("time").HasComment("结束时间");

        }
    }
}
