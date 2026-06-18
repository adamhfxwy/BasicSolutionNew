

namespace WebAPI.Controllers.Identity
{
    /// <summary>
    /// 角色相关
    /// </summary>
    [Authorize]
    [Route("customWeb/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "Identity")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IMemoryCacheHelper _memoryCacheHelper;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly JwtParam _jwtParam;
        public RoleController(IRoleService roleService, IMemoryCacheHelper memoryCacheHelper, HttpClient httpClient, IConfiguration config, JwtParam jwtParam)
        {
            _roleService = roleService;
            _memoryCacheHelper = memoryCacheHelper;
            _httpClient = httpClient;
            _config = config;
            _jwtParam = jwtParam;
        }

      

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission(nameof(WebRoleMentMatching.RoleMentadd))]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody))]
        public async Task<ActionResult> AddRoleAsync(AddEditRoleModel model, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(model.RoleName))
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"id不可为空"));
            }
            var tuple = await _roleService.AddRoleAsync(model, cancellationToken);
            return Ok(new ResponseBody(tuple.Item1 > 0 ? ResponseStatus.Ok.ToString() : ResponseStatus.Failed.ToString(), tuple.Item2));
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission(nameof(WebRoleMentMatching.RoleMentedit))]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody))]
        public async Task<ActionResult> EditRoleAsync(AddEditRoleModel model, CancellationToken cancellationToken = default)
        {
            if (!model.Id.HasValue || model.Id.Value <= 0)
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"id不可为空"));
            }
            var role = await _roleService.GetRoleAndEmployeesByIdAsync(model.Id.Value);
            if (role == null)
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"id={model.Id}的角色（职位）不存在"));     
            }
            bool isChangeWeb = false;
            bool isChangeMobile = false;
            if (model.Permissions != null && !model.Permissions.OrderBy(x => x).ToArray()
                .SequenceEqual(role.Permissions == null ? Array.Empty<long>() : role.Permissions.OrderBy(x => x).ToArray()))
            {
                isChangeWeb = true;
            }
            if (model.MobilePermissions != null && !model.MobilePermissions.OrderBy(x => x).ToArray()
                .SequenceEqual(role.MobilePermissions == null ? Array.Empty<long>() : role.MobilePermissions.OrderBy(x => x).ToArray()))
            {
                isChangeMobile = true;
            }
            var tuple = await _roleService.EditRoleAsync(model, cancellationToken);
            //if (tuple.Item1 && isChangeWeb)
            //{
            //    foreach (var item in role.Employees ?? new List<EmployeeDTO>())//
            //    {
            //        string key = $"WebPermission-{item.Id}-{item.UserName}";
            //        _memoryCacheHelper.Remove(key);
            //        await _memoryCacheHelper.GetOrCreateAsync(key, async (e) =>
            //        {
            //            return new Tuple<bool, List<string>>(false, null);
            //        }, Convert.ToInt32(_jwtParam.ValidLifetime), false);
            //    }
            //}
            //if (tuple.Item1 && isChangeMobile)
            //{
            //    var baseUrl= _config["Curdomain"];
            //    var url = $"{baseUrl}/customWeb/Main/RemoveMobilePermissionByRole";
            //    string json = JsonConvert.SerializeObject(role);
            //    var content = new StringContent(json, Encoding.UTF8, "application/json");
            //    var response = await _httpClient.PostAsync(url, content);
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
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody<RoleDTO>))]
        public async Task<ActionResult> GetRoleAsync(RoleQueryModel query, CancellationToken cancellationToken = default)
        {
            query.OrderBy = "createTime";
            query.IsDescending = true;
            var tuple = await _roleService.GetRoleAsync(query, cancellationToken);
            return Ok(new ResponseBody<RoleDTO[]>(ResponseStatus.Ok.ToString(), "Success", tuple.List.ToArray(), tuple.Total));
        }
        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody<RoleDTO>))]
        public async Task<ActionResult> GetRoleByIdAsync(GetByIdModel model)
        {
            if (model.Id <= 0)
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"id不可为空"));
            }
            var obj = await _roleService.GetRoleByIdAsync(model.Id);
            return Ok(new ResponseBody<RoleDTO>(ResponseStatus.Ok.ToString(), "Success", obj, 1));
        }
        /// <summary>
        /// 根据字段获取一条数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody<RoleDTO>))]
        public async Task<ActionResult> GetRoleByPropsAsync(RoleQueryModel query, CancellationToken cancellationToken = default)
        {
            var obj = await _roleService.GetRoleByPropAsync(query, cancellationToken);
            return Ok(new ResponseBody<RoleDTO>(ResponseStatus.Ok.ToString(), "Success", obj, 1));
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission(nameof(WebRoleMentMatching.RoleMentdel))]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody))]
        public async Task<ActionResult> RemoveRoleAsync(GetByIdModel model, CancellationToken cancellationToken = default)
        {
            if (model.Id <= 0)
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"id不可为空"));
            }

            var tuple = await _roleService.RemoveRoleAsync(model.Id, cancellationToken);
            return Ok(new ResponseBody(tuple.Item1 ? ResponseStatus.Ok.ToString() : ResponseStatus.Failed.ToString(), tuple.Item2));
        }
    }
}
