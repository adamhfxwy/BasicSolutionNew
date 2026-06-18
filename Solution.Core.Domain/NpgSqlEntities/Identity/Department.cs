
using Solution.Core.EnumAndConstent.SubEntitys;

namespace Solution.Core.Domain.NpgSqlEntities.Identity
{
    /// <summary>
    /// 部门实体
    /// </summary>
    public class Department : BaseEntity
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; private set; } = null!;
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; private set; }
        /// <summary>
        /// 员工导航属性
        /// </summary>
        public List<Employee>? Employees { get; private set; }
        /// <summary>
        /// 部门负责人id
        /// </summary>
        public long? LeaderId { get; private set; }
        /// <summary>
        /// 部门负责人
        /// </summary>
       // public DepartmentLeaderEntity? DepartmentLeader { get; private set; }
        private Department()
        {

        }
        public Department(string departmentName, string? remark, long? leaderId)
        {
            this.DepartmentName = departmentName;
            this.Remark = remark;
            //this.DepartmentLeader = departmentLeader;
            LeaderId = leaderId;
        }
        public void ChangeDepartMentName(string departmentName)
        {
            this.DepartmentName = departmentName;
        }
        public void ChangeRemark(string remark)
        {
            Remark = remark;
        }
        //public void ChangeDepartmentLeader(DepartmentLeaderEntity departmentLeader)
        //{
        //    this.DepartmentLeader = departmentLeader;
        //}
        public void ChangeLeaderId(long leaderId)
        {
            this.LeaderId = leaderId;
        }
    }
}
