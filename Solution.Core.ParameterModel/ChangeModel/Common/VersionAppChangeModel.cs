using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Core.ParameterModel.ChangeModel.Common
{
    public class VersionAppChangeModel
    {
        public long? Id { get; set; }
        /// <summary>
        /// app版本号
        /// </summary>
        public string? Version { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public string? EditDate { get; set; }
        /// <summary>
        /// 是否需要强制更新 0：不需要 1：需要
        /// </summary>
        public int? IsUpdate { get; set; }
        /// <summary>
        /// 数据库版本
        /// </summary>
        public string? DataVersion { get; set; }
        /// <summary>
        /// 1：智能秤 2：智能箱
        /// </summary>
        public VersionAppEnum? Source { get; set; }
    }
}
