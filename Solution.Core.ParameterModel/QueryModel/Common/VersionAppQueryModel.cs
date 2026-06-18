using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Core.ParameterModel.QueryModel.Common
{
    public class VersionAppQueryModel
    {
        /// <summary>
        /// 类型 1-app
        /// </summary>
        [SearchProperty("Source")]
        public VersionAppEnum? Source { get; set; }
    }
}
