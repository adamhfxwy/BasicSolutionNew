




namespace Solution.Core.DTO.Identity
{
    public class DepartmentDTO
    {
        public long Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; } = null!;
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
        /// <summary>
        /// 角色下的员工
        /// </summary>
        public List<EmployeeDTO>? Employees { get; set; }
        /// <summary>
        /// 部门负责人
        /// </summary>
        public DepartmentLeaderEntity? DepartmentLeader { get; set; }
        /// <summary>
        /// 负责人姓名
        /// </summary>
        public string? LeaderName {  get; set; }
        /// <summary>
        /// 负责人id
        /// </summary>
        public long? EmployeeId { get; set; }
        /// <summary>
        /// 负责人手机号
        /// </summary>
        public string? LeaderCellphone { get; set; }

      

    }
}
