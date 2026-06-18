namespace Solution.Core.EnumAndConstent;

public class Pagination<TResult>
{
    /// <summary>
    /// 总条数
    /// </summary>
    public int Total { get; set; }
    /// <summary>
    /// 数据
    /// </summary>
    public List<TResult> List { get; set; }
    public string? Message {  get; set; }
    /// <summary>
    /// 返回状态 1-success 2-failed
    /// </summary>
    public int Code {  get; set; }
}
