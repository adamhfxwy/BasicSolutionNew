
using Solution.Core.Domain.INpgSqlService;
using Solution.Core.EnumAndConstent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileAPI.Models;

namespace MobileAPI.Controllers
{
    /// <summary>
    /// 公共数据相关
    /// </summary>
    [Authorize]
    [Route("propertyMgtApp/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "Common")]
    [ApiController]
    public class CommonDataController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly IDictionaryService _dictionaryService;

        public CommonDataController(IDictionaryService dictionaryService, IDepartmentService departmentService, IEmployeeService employeeService)
        {
            _dictionaryService = dictionaryService;
            _departmentService = departmentService;
            _employeeService = employeeService;
        }


        /// <summary>
        /// 员工列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody<EmployeeDTO>))]
        public async Task<ActionResult> GetEmployeeAsync(EmployeeQueryModel query, CancellationToken cancellationToken = default)
        {
            query.OrderBy = "createTime";
            query.IsDescending = true;
            var tuple = await _employeeService.GetEmployeeAsync(query, cancellationToken);
            return Ok(new ResponseBody<EmployeeDTO[]>(ResponseStatus.Ok.ToString(), "Success", tuple.List.ToArray(), tuple.Total));
        }
        /// <summary>
        /// 根据id获取员工
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody<EmployeeDTO>))]
        public async Task<ActionResult> GetEmployeeByIdAsync(GetByIdModel model, CancellationToken cancellationToken = default)
        {
            if (model.Id <= 0)
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"请选择Id"));
            }
            var obj = await _employeeService.GetEmployeeByIdAsync(model.Id, cancellationToken);
            return Ok(new ResponseBody<EmployeeDTO>(ResponseStatus.Ok.ToString(), "Success", obj, 1));
        }
        /// <summary>
        /// 部门列表
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
        /// 根据id获取部门数据
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
        /// 根据条件查询字典类型列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody<DictionaryDTO>))]
        public async Task<ActionResult> GetDictionaryTypeAsync(DictionaryQueryModel query, CancellationToken cancellationToken = default)
        {
            query.OrderBy = "createTime";
            query.IsDescending = true;
            query.Type = 0;
            query.Value = "CreateType";
            var tuple = await _dictionaryService.GetDictionaryTypeAsync(query, cancellationToken);
            tuple.List.Insert(0, new DictionaryDTO { Id = 0, Key = "无类型", Type = 0 });
            return Ok(new ResponseBody<DictionaryDTO[]>(ResponseStatus.Ok.ToString(), "Success", tuple.List.ToArray(), tuple.Total));
        }
        /// <summary>
        /// 根据类型的key获取该类型下的字典数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody<DictionaryDTO>))]
        public async Task<ActionResult> GetDictionaryByKeyAsync(string key, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(key))
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"key不能为空"));
            }

            var obj = await _dictionaryService.GetDictionaryByPropAsync(new DictionaryQueryModel { Key = key }, cancellationToken);
            if (obj == null)
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"key={key}的字典不存在")); 
            }
            var list = await _dictionaryService.GetDictionaryAsync(new DictionaryQueryModel { Type = (int)obj.Id }, cancellationToken);
            return Ok(new ResponseBody<Pagination<DictionaryDTO>>(ResponseStatus.Ok.ToString(), "Success", list, list.Total));
        }
      
    }
}
