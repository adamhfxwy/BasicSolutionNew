

using System.Collections.Generic;

namespace WebAPI.Controllers.Common
{
    /// <summary>
    /// 排班相关
    /// </summary>
    [Authorize]
    [Route("customWeb/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "Common")]
    [ApiController]
    public class ShiftInfoController : ControllerBase
    {
        private readonly IShiftInfoService  _shiftInfoService;
        private readonly IEmployeeService _employeeService;

        public ShiftInfoController(IShiftInfoService shiftInfoService, IEmployeeService employeeService)
        {
            _shiftInfoService = shiftInfoService;
            _employeeService = employeeService;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody))]
        [CheckPermission(nameof(WebShiftMent.ShiftMentadd))]
        public async Task<ActionResult> AddShiftInfoAsync(ShiftInfoChangeModel model, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(model.ShiftName))
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"排版名称不可为空"));              
            }
            if (string.IsNullOrEmpty(model.BeginTime))
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"开始时间不可为空"));              
            }
            if (string.IsNullOrEmpty(model.EndTime))
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"结束时间不可为空"));            
            }
            var tuple = await _shiftInfoService.AddShiftInfoAsync(model, cancellationToken);
            return Ok(new ResponseBody(tuple.Item1 > 0 ? ResponseStatus.Ok.ToString() : ResponseStatus.Failed.ToString(), tuple.Item2));        
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission(nameof(WebShiftMent.ShiftMentedit))]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody))]
        public async Task<ActionResult> EditShiftInfoAsync(ShiftInfoChangeModel model, CancellationToken cancellationToken = default)
        {
            if (!model.Id.HasValue || model.Id.Value <= 0)
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"id不可为空"));
              
            }
            var tuple = await _shiftInfoService.EditShiftInfoAsync(model, cancellationToken);
            return Ok(new ResponseBody(tuple.Item1 ? ResponseStatus.Ok.ToString() : ResponseStatus.Failed.ToString(), tuple.Item2));
        }
        /// <summary>
        /// 根据条件查询列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody<ShiftInfoDTO>))]
        public async Task<ActionResult> GetShiftInfoAsync(ShiftInfoQueryModel query, CancellationToken cancellationToken = default)
        {
            query.OrderBy = "createTime";
            query.IsDescending = true;
            var tuple = await _shiftInfoService.GetShiftInfoAsync(query, cancellationToken);
            return Ok(new ResponseBody<ShiftInfoDTO[]>(ResponseStatus.Ok.ToString(), "Success", tuple.List.ToArray(), tuple.Total));          
        }
        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody<ShiftInfoDTO>))]
        public async Task<ActionResult> GetShiftInfoByIdAsync(GetByIdModel model)
        {
            if (model.Id <= 0)
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"请选择Id"));             
            }
            var obj = await _shiftInfoService.GetShiftInfoByIdAsync(model.Id);
            return Ok(new ResponseBody<ShiftInfoDTO>(ResponseStatus.Ok.ToString(), "Success", obj, 1));
        }
        /// <summary>
        /// 根据字段获取一条数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody<ShiftInfoDTO>))]
        public async Task<ActionResult> GetShiftInfoByPropsAsync(ShiftInfoQueryModel query, CancellationToken cancellationToken = default)
        {
            var obj = await _shiftInfoService.GetShiftInfoByPropAsync(query, cancellationToken);
            return Ok(new ResponseBody<ShiftInfoDTO>(ResponseStatus.Ok.ToString(), "Success", obj, 1));
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission(nameof(WebShiftMent.ShiftMentdeldel1))]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody))]
        public async Task<ActionResult> RemoveShiftInfoAsync(GetByIdModel model, CancellationToken cancellationToken = default)
        {
            if (model.Id <= 0)
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"请选择Id"));
            }
            bool exists = await _employeeService.EmployeeAnyAsync(new EmployeeQueryModel { ShiftId = model.Id });
            if (exists)
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"Id={model.Id}的排班已被员工绑定，无法删除"));               
            }
            var tuple = await _shiftInfoService.RemoveShiftInfoAsync(model.Id, cancellationToken);
            return Ok(new ResponseBody(tuple.Item1 ? ResponseStatus.Ok.ToString() : ResponseStatus.Failed.ToString(), tuple.Item2));
        }
    }
}
