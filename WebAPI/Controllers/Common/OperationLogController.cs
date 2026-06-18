

namespace WebAPI.Models.Controllers.Common
{
    /// <summary>
    /// 操作日志相关
    /// </summary>
    [Authorize]
    [Route("customWeb/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "Common")]
    [ApiController]
    public class OperationLogController : ControllerBase
    {
        private readonly IOperationLogService  _operationLogService;

        public OperationLogController(IOperationLogService operationLogService)
        {
            _operationLogService = operationLogService;
        }
        /// <summary>
        /// 根据条件查询列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody<OperationLogDTO>))]
        public async Task<ActionResult> GetOperationLogAsync(OperationLogQueryModel query, CancellationToken cancellationToken = default)
        {
            query.OrderBy = "createTime";
            query.IsDescending = true;
            var tuple = await _operationLogService.GetOperationLogAsync(query, cancellationToken);
            return Ok(new ResponseBody<OperationLogDTO[]>(ResponseStatus.Ok.ToString(), "Success", tuple.List.ToArray(), tuple.Total));         
        }
    }
}
