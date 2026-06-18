using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Core.Domain.NpgSqlEntities.Common
{
    public class ShiftInfo:BaseEntity
    {
        /// <summary>
        /// 班次名称
        /// </summary>
        public string ShiftName { get; private set; } = null!;
        /// <summary>
        /// 开始时间
        /// </summary>
        public TimeOnly BeginTime { get; private set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public TimeOnly EndTime { get; private set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; private set; }
        private ShiftInfo()
        {

        }
        public ShiftInfo(string shiftName, TimeOnly beginTime, TimeOnly endTime, string? remark)
        {
            this.ShiftName = shiftName;
            this.BeginTime = beginTime;
            this.EndTime = endTime;
            this.Remark = remark;
        }
        public void ChangeShiftName(string shiftName)
        {
            this.ShiftName = shiftName;
        }
        public void ChangeBeginTime(TimeOnly beginTime)
        {
            this.BeginTime = beginTime;
        }
        public void ChangeEndTime(TimeOnly endTime)
        {
            this.EndTime = endTime;
        }
        public void ChangeRemark(string remark)
        {
            this.Remark = remark;
        }
    }
}
