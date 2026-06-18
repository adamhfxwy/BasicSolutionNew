using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Core.EnumAndConstent.Resources
{
    public class MessageResources
    {
        /// <summary>
        /// 工单短信模板 0-order_type 1-order_number  2-problem_description  3-create_time
        /// </summary>
        public static string WorkOrderMessage = "{{\"order_class\":\"{0}\",\"order_number\":\"{1}\",\"problem_description\":\"{2}\",\"create_time\":\"{3}\"}}";
    }
}
