

namespace WebAPI.Controllers.Identity
{
    /// <summary>
    /// 部门相关
    /// </summary>
    [Authorize]
    [Route("customWeb/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "Identity")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly IEmployeeService _employeeService;

        public DepartmentController(IEmployeeService employeeService, IDepartmentService departmentService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission(nameof(WebDepartmentMatching.DepartmentMentadd))]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody))]
        public async Task<ActionResult> AddDepartmentAsync(AddEditDepartmentModel model, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(model.DepartmentName))
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"部门名称不可为空"));
            }
            //if (model.EmployeeId.HasValue && model.EmployeeId.Value > 0)
            //{
            //    var emp = await _employeeService.GetEmployeeByIdAsync(model.EmployeeId.Value);
            //    if (emp == null)
            //    {
            //        return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"id={model.EmployeeId.Value}的员工不存在"));
            //    }
            //    //model.DepartmentLeader = CommonUtils.Mapper<DepartmentLeaderEntity, EmployeeDTO>(emp);
            //}
            var tuple = await _departmentService.AddDepartmentAsync(model, cancellationToken);
            return Ok(new ResponseBody<long>(tuple.Item1 > 0 ? ResponseStatus.Ok.ToString() : ResponseStatus.Failed.ToString(), tuple.Item2, tuple.Item1, 0));
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission(nameof(WebDepartmentMatching.DepartmentMentedit))]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody))]
        public async Task<ActionResult> EditDepartmentAsync(AddEditDepartmentModel model, CancellationToken cancellationToken = default)
        {
            if (!model.Id.HasValue || model.Id.Value <= 0)
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"请选择Id"));
            }
            //if (model.EmployeeId.HasValue && model.EmployeeId.Value > 0)
            //{
            //    var emp=await _employeeService.GetEmployeeByIdAsync(model.EmployeeId.Value);
            //    if (emp == null)
            //    {
            //        return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"id={model.EmployeeId.Value}的员工不存在"));
            //    }
            //    model.DepartmentLeader = CommonUtils.Mapper<DepartmentLeaderEntity, EmployeeDTO>(emp);
            //}
            var tuple = await _departmentService.EditDepartmentAsync(model, cancellationToken);
            return Ok(new ResponseBody(tuple.Item1 ? ResponseStatus.Ok.ToString() : ResponseStatus.Failed.ToString(), tuple.Item2));
        }
        /// <summary>
        /// 根据条件查询列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost] 
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody<DepartmentDTO>))]
        public async Task<ActionResult> GetDepartmentAsync(DepartmentQueryModel query, CancellationToken cancellationToken = default)
        {
            query.OrderBy = "createTime";
            query.IsDescending = true;
            var tuple = await _departmentService.GetDepartmentAsync(query, cancellationToken);
            return Ok(new ResponseBody<DepartmentDTO[]>(ResponseStatus.Ok.ToString(), "Success", tuple.List.ToArray(), tuple.Total));
        }
        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody<DepartmentDTO>))]
        public async Task<ActionResult> GetDepartmentByIdAsync(GetByIdModel model)
        {
            if (model.Id <= 0)
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"请选择Id"));
            }
            var obj = await _departmentService.GetDepartmentByIdAsync(model.Id);
            return Ok(new ResponseBody<DepartmentDTO>(ResponseStatus.Ok.ToString(), "Success", obj, 1));
        }
        /// <summary>
        /// 根据字段获取一条数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody<DepartmentDTO>))]
        public async Task<ActionResult> GetDepartmentByPropsAsync(DepartmentQueryModel query, CancellationToken cancellationToken = default)
        {
            var obj = await _departmentService.GetDepartmentByPropAsync(query, cancellationToken);
            return Ok(new ResponseBody<DepartmentDTO>(ResponseStatus.Ok.ToString(), "Success", obj, 1));
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission(nameof(WebDepartmentMatching.DepartmentMentdel))]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody))]
        public async Task<ActionResult> RemoveDepartmentAsync(GetByIdModel model, CancellationToken cancellationToken = default)
        {
            if (model.Id <= 0)
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"请选择Id"));
            }

            var tuple = await _departmentService.RemoveDepartmentAsync(model.Id, cancellationToken);
            return Ok(new ResponseBody(tuple.Item1 ? ResponseStatus.Ok.ToString() : ResponseStatus.Failed.ToString(), tuple.Item2));
        }
    }
}
