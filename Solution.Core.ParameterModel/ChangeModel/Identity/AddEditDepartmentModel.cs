

using Solution.Core.EnumAndConstent.SubEntitys;

namespace Solution.Core.ParameterModel.ChangeModel.Identity
{
    /// <summary>
    /// 部门新增编辑模型
    /// </summary>
    public class AddEditDepartmentModel
    {
        public long? Id { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string? DepartmentName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
        /// <summary>
        /// 员工id
        /// </summary>
        public long ? EmployeeId { get; set; }
        /// <summary>
        /// 部门负责人（无需传参）
        /// </summary>
        //public DepartmentLeaderEntity? DepartmentLeader { get; set; }
    }
}
