using System.Collections;

namespace MobileAPI.Models
{
    public class ResponseBody<T>
    {
        /// <summary>
        /// 状态： 成功-Ok  失败-Failed
        /// </summary>
        public string Status {  get; set; }  
        /// <summary>
        /// 消息 ，无消息时为空
        /// </summary>
        public string Msg { get; set; } = "";
        /// <summary>
        /// 返回对象
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 总条数，数据为集合，并需要分页时返回
        /// </summary>
        public int? TotalCount {  get; set; }     
        public ResponseBody(string status, string msg, T data, int? totalCount)
        {
            Status = status;
            Msg = msg;
            Data = data;
            if (!totalCount.HasValue || totalCount.Value == 0)
            {
                if (data==null)
                {
                    TotalCount = null;
                }
                else if (data is ICollection collection)
                {
                    TotalCount = collection.Count;
                }
                else
                {
                    TotalCount = 1;
                }
            }
            else
            {
                TotalCount = totalCount;
            }
           
        }
    }
    public class ResponseBody
    {
        /// <summary>
        /// 状态： 成功-Ok  失败-Failed
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 消息 ，无消息时为空
        /// </summary>
        public string Msg { get; set; } = "";
        public ResponseBody(string status, string msg)
        {
            Status = status;
            Msg = msg;
        }
    }
}
