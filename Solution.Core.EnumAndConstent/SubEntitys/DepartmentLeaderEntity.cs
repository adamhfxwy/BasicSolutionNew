using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Solution.Core.EnumAndConstent.SubEntitys
{

    public class DepartmentLeaderEntity
    {
        public long Id { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; } = null!;
        /// <summary>
        /// 手机号
        /// </summary>
        public string Cellphone { get; set; } = null!;
    }
}
