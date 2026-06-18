using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Solution.Core.EnumAndConstent.Enums.Enum;

namespace Solution.Core.DTO.Common
{
    public class VersionAppDTO
    {
        public long Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// app版本号
        /// </summary>
        public string? Version { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? EditDate { get; set; }
        /// <summary>
        /// 是否需要强制更新 0：不需要 1：需要
        /// </summary>
        public int? IsUpdate { get; set; }
        /// <summary>
        /// 数据库版本
        /// </summary>
        public string? DataVersion { get; set; }
        /// <summary>
        /// 类型：1-app
        /// </summary>
        public VersionAppEnum? Source { get; set; }
    }
}
