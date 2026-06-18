

namespace Solution.Core.Infrastructure.Configs.Common
{
    internal class OperationLogConfig : IEntityTypeConfiguration<OperationLog>
    {
        public void Configure(EntityTypeBuilder<OperationLog> builder)
        {
            builder.ToTable("T_OperationLog");
            builder.Property(x => x.Id).UseIdentityColumn().HasComment("id主键");
            builder.Property(x => x.CreateTime).HasColumnType("datetime").HasComment("创建时间");
            builder.Property(x => x.IsDeleted).HasComment("软删  1-未删除 2-已删除");
            builder.Property(x => x.EmpId).HasComment("员工id");
            builder.Property(x => x.EmpName).HasMaxLength(255).HasComment("员工姓名");
            builder.Property(x => x.OperationName).HasMaxLength(255).HasComment("操作项目的名称");
            builder.Property(x => x.ApiPath).HasMaxLength(255).HasComment("接口地址");
            builder.Property(x => x.RequestMessage).HasComment("请求参数").IsRequired(false);
            builder.Property(x => x.ResponseMessage).HasComment("响应参数").IsRequired(false);
        }
    }
}
