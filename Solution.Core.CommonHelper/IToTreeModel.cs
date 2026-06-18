namespace Solution.Core.CommonHelper
{
    public interface IToTreeModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        long Id { get; set; }
        /// <summary>
        /// 父级id
        /// </summary>
        long ParentId { get; set; }
        /// <summary>
        /// 子集
        /// </summary>
        List<IToTreeModel> Children { get; set; }
        void AddChilrden(IToTreeModel node);
    }
}
