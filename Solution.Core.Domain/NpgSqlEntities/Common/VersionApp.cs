using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Core.Domain.NpgSqlEntities.Common
{
    public class VersionApp:BaseEntity
    {
        /// <summary>
        /// app版本号
        /// </summary>
        public string? Version { get; private set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? EditDate { get; private set; }
        /// <summary>
        /// 是否需要强制更新 0：不需要 1：需要
        /// </summary>
        public int? IsUpdate { get; private set; }
        /// <summary>
        /// 数据库版本
        /// </summary>
        public string? DataVersion { get; private set; }
        /// <summary>
        /// 类型：1-app
        /// </summary>
        public VersionAppEnum? Source { get; private set; }
        private VersionApp() { }
        public VersionApp(string? version,DateTime? editDate,int? isUpdate, string? dataVersion , VersionAppEnum? sourse) 
        {
            Version = version;
            EditDate = editDate;
            IsUpdate = isUpdate;
            DataVersion = dataVersion;
            Source = sourse;
        }
        public void ChangeIsUpdate(int isUpdate)
        {
            IsUpdate = isUpdate;
        }
        public void ChangeSource(VersionAppEnum sourse)
        {
            Source = sourse;
        }
        public void ChangeVersion(string version)
        {
            Version = version;
        }
        public void ChangeEditDate(DateTime editDate)
        {
            EditDate = editDate;
        }

    }
}
