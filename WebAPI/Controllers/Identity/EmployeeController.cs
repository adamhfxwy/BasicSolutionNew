

using Solution.Core.Domain.INpgSqlService;
using Solution.Core.Domain.NpgSqlEntities.Identity;
using Solution.Core.Infrastructure.NpgSqlService;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http;

namespace WebAPI.Controllers.Identity
{
    /// <summary>
    /// 员工相关
    /// </summary>
    [Authorize]
    [Route("customWeb/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "Identity")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMemoryCacheHelper _memoryCacheHelper;
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly IDepartmentService _departmentService;
        public EmployeeController(IEmployeeService employeeService, IMemoryCacheHelper memoryCacheHelper, IConfiguration config, HttpClient httpClient, IDepartmentService departmentService)
        {
            _employeeService = employeeService;
            _memoryCacheHelper = memoryCacheHelper;
            _config = config;
            _httpClient = httpClient;
            _departmentService = departmentService;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission(nameof(WebEmployeeMatching.EmployeeMentadd))]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody<long>))]
        public async Task<ActionResult> AddEmployeeAsync(AddEditEmployeeModel model, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(model.RealName))
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"真实姓名不可为空"));
            }
            if (string.IsNullOrEmpty(model.Cellphone))
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"手机号不可为空"));
            }
            if (!model.LoginPermissions.HasValue)
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"登陆权限不可为空"));
            }
            if (!model.Sex.HasValue)
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"性别不可为空"));
            }
            if (string.IsNullOrEmpty(model.JobName))
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"岗位名称不可为空"));
            }
            if (!string.IsNullOrEmpty(model.PhotoPath))
            {
                if (ValidateHelper.IsBase64(model.PhotoPath))
                {
                    string basePath = _config["FilePathBase"];
                    var picSuffix = _config["PicSuffix"];
                    DateTime date = DateTime.Now;
                    string DirUrl = "/Storage/okrMgt/Employee/" + date.Year + "/" + date.Month + "/" + date.Day + "/" + model.RealName + "/";

                    if (!Directory.Exists(basePath + DirUrl)) //检测文件夹是否存在，不存在则创建
                    {
                        Directory.CreateDirectory(basePath + DirUrl);
                    }
                    model.PhotoPath = CommonUtils.SavaImg(model.PhotoPath, basePath, DirUrl, null, picSuffix);
                }
            }
            model.UserName = Guid.NewGuid().ToString("N").Substring(0, 16);
            var tuple = await _employeeService.AddEmployeeAsync(model, cancellationToken);
            return Ok(new ResponseBody<long>(tuple.Item1 > 0 ? ResponseStatus.Ok.ToString() : ResponseStatus.Failed.ToString(),tuple.Item2, tuple.Item1,0));
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission(nameof(WebEmployeeMatching.EmployeeMentedit))]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody))]
        public async Task<ActionResult> EditEmployeeAsync(AddEditEmployeeModel model, CancellationToken cancellationToken = default)
        {
            if (!model.Id.HasValue || model.Id.Value <= 0)
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"id不可为空"));
            }
            if (!string.IsNullOrEmpty(model.PhotoPath))
            {
                if (ValidateHelper.IsBase64(model.PhotoPath))
                {
                    string basePath = _config["FilePathBase"];
                    var picSuffix = _config["PicSuffix"];
                    DateTime date = DateTime.Now;
                    string DirUrl = "/Storage/okrMgt/Employee/" + date.Year + "/" + date.Month + "/" + date.Day + "/" + model.RealName + "/";

                    if (!Directory.Exists(basePath + DirUrl)) //检测文件夹是否存在，不存在则创建
                    {
                        Directory.CreateDirectory(basePath + DirUrl);
                    }
                    model.PhotoPath = CommonUtils.SavaImg(model.PhotoPath, basePath, DirUrl, null, picSuffix);
                }
            }
            var tuple = await _employeeService.EditEmployeeAsync(model, cancellationToken);
            //var emp = await _employeeService.GetEmployeeByIdAsync(model.Id.Value);
            //var depts = await _departmentService.GetDepartmentAsync(new DepartmentQueryModel { LeaderId = emp.Id });
            //if (depts.List != null && depts.List.Count() > 0)
            //{
            //    foreach (var dept in depts.List)
            //    {
            //        await _departmentService.EditDepartmentAsync(new AddEditDepartmentModel { Id = dept.Id, DepartmentLeader = CommonUtils.Mapper<DepartmentLeaderEntity, EmployeeDTO>(emp) });
            //    }
            //}
            return Ok(new ResponseBody(tuple.Item1 ? ResponseStatus.Ok.ToString() : ResponseStatus.Failed.ToString(), tuple.Item2));
        }
        /// <summary>
        /// 根据条件查询列表
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
            foreach (var item in tuple.List)
            {
                if (!string.IsNullOrEmpty(item.PhotoPath))
                {
                    item.PhotoPath = $"{_config["Curdomain"]}{item.PhotoPath}";
                }
            }
            return Ok(new ResponseBody<EmployeeDTO[]>(ResponseStatus.Ok.ToString(), "Success", tuple.List.ToArray(), tuple.Total));
        }
        /// <summary>
        /// 根据id获取数据
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
        /// 根据字段获取一条数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody<EmployeeDTO>))]
        public async Task<ActionResult> GetEmployeeByPropsAsync(EmployeeQueryModel query, CancellationToken cancellationToken = default)
        {
            var obj = await _employeeService.GetEmployeeByPropAsync(query, cancellationToken);
            return Ok(new ResponseBody<EmployeeDTO>(ResponseStatus.Ok.ToString(), "Success", obj, 1));
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission(nameof(WebEmployeeMatching.EmployeeMentdel))]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody))]
        public async Task<ActionResult> RemoveEmployeeAsync(GetByIdModel model, CancellationToken cancellationToken = default)
        {
            if (model.Id <= 0)
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"请选择Id"));
            }

            var tuple = await _employeeService.RemoveEmployeeAsync(model.Id, cancellationToken);
            return Ok(new ResponseBody(tuple.Item1 ? ResponseStatus.Ok.ToString() : ResponseStatus.Failed.ToString(), tuple.Item2));
        }
    }
}
