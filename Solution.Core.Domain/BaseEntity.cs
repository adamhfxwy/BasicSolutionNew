


namespace Solution.Core.Domain;

public abstract class BaseEntity
{
    /// <summary>
    /// id主键
    /// </summary>
    public long Id { get; init; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; init; } = DateTime.Now;
    /// <summary>
    /// 软删  1-未删除 2-已删除
    /// </summary>
    public IsDeletedEnum IsDeleted { get; private set; } = IsDeletedEnum.未删除;
    public void ChangeIsDeleted()
    {
        this.IsDeleted = IsDeletedEnum.已删除;
    }
}
