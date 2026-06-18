namespace WebAPI.Models.TaskManager
{
    public class TaskFinishModel
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 任务完成时间
        /// </summary>
        public string FinishTime { get; set; } = null!;
    }
}
