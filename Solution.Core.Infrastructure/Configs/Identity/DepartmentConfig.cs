

using Solution.Core.EnumAndConstent.SubEntitys;
using Newtonsoft.Json;

namespace Solution.Core.Infrastructure.Configs.Identity;

internal class DepartmentConfig : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("T_Department");
        builder.Property(x => x.Id).UseIdentityColumn().HasComment("id主键");
        builder.Property(x => x.IsDeleted).HasComment("软删  1-未删除 2-已删除");
        builder.Property(x => x.DepartmentName).HasMaxLength(255).HasComment("部门名称");
        builder.Property(x => x.Remark).HasMaxLength(255).HasComment("备注");
        builder.Property(x => x.LeaderId).HasComment("部门负责人id");
        builder.Property(x => x.CreateTime).HasColumnType("datetime").HasComment("创建时间");
        //builder.Property(x => x.DepartmentLeader).HasColumnType("jsonb")
        //    .HasConversion(
        //    v => JsonConvert.SerializeObject(v),
        //     v => JsonConvert.DeserializeObject<DepartmentLeaderEntity>(v))
        //    .HasColumnName("department_leader").HasComment("部门负责人");
    }
}
