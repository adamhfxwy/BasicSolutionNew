
using Solution.Core.Domain.INpgSqlService;
using Newtonsoft.Json.Linq;
using MobileAPI.Models;
using System.Data;

namespace MobileAPI.Identity
{
    /// <summary>
    /// 登录相关
    /// </summary>
    [Authorize]
    [Route("propertyMgtApp/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "Identity")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMenuService _menuService;
        private readonly IMobilePermissionsService _mobilePermissionsService;
        private readonly IRoleService _roleService;
        private readonly JwtParam _jwtParam;
        private readonly IMemoryCacheHelper _memoryCacheHelper;
        private readonly IVersionAppService _versionAppService;
        private readonly IConfiguration _config;
        public MainController(IEmployeeService employeeService, IMenuService menuService, IMobilePermissionsService mobilePermissionsService, IRoleService roleService, JwtParam jwtParam, IMemoryCacheHelper memoryCacheHelper, IVersionAppService versionAppService, IConfiguration config)
        {
            _employeeService = employeeService;
            _menuService = menuService;
            _mobilePermissionsService = mobilePermissionsService;
            _roleService = roleService;
            _jwtParam = jwtParam;
            _memoryCacheHelper = memoryCacheHelper;
            _versionAppService = versionAppService;
            _config = config;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginModel model, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(model.Cellphone) || string.IsNullOrEmpty(model.Password))
            {
                return Ok(new { Status = "Failed", Msg = "用户名或密码不能为空" });
            }
            var tuple = await _employeeService.CheckLoginAsync(model.Cellphone.Trim(), model.Password.Trim(), cancellationToken);
            if (!tuple.Item1)
            {
                return Ok(new
                {
                    Status = "Failed",
                    Msg = tuple.Item2
                });
            }
            var user = tuple.Item3;
            string roleName = user.RoleName ?? "";
            if (user.LoginPermissions != (int)LoginPermissionsEnum.app权限 && user.LoginPermissions != (int)LoginPermissionsEnum.所有权限)
            {
                return Ok(new
                {
                    Status = "Failed",
                    Msg = "无app管理系统权限，无法登录。"
                });
            }
            var claims = new List<Claim>();
            var memoryCacheKey = $"MobilePermission-{user.Id}-{user.UserName}";
            claims.Add(new Claim("RealName", user.RealName));
            claims.Add(new Claim("UserId", user.Id.ToString()));
            claims.Add(new Claim("RoleId", user.RoleId == null ? "" : user.RoleId));
            claims.Add(new Claim("MemoryCacheKey", memoryCacheKey));
            claims.Add(new Claim("RoleName", roleName));

            string token = BuildToken(claims, _jwtParam);
            if (user.RealName != "超级管理员")
            {
                if (user.MobilePermissions != null && user.MobilePermissions.Count() > 0)
                {
                    _memoryCacheHelper.Remove(memoryCacheKey);
                    var mobilePermissionRes = await _memoryCacheHelper.GetOrCreateAsync(memoryCacheKey, async (e) =>
                    {
                        var mobilePermissionRes = await _mobilePermissionsService.GetPermissionsByIdsAsync(user.MobilePermissions, cancellationToken);
                        var mobilePermissions = mobilePermissionRes.Where(x => x.ParentId != 0).Select(x => x.PermissionCode).ToList();
                        return mobilePermissions;
                    }, Convert.ToInt32(_jwtParam.ValidLifetime), false);
                }
            }
            return Ok(new
            {
                Status = "Ok",
                UserId = user.Id,
                User = user,
                Token = token
            });
        }
        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(RoleDTO))]
        [AllowAnonymous]
        public async Task<ActionResult> GetRoleByIdAsync(GetByIdModel model)
        {
            if (model.Id <= 0)
            {
                return Ok(new
                {
                    Status = "Failed",
                    Msg = "请选择Id"
                });
            }
            var obj = await _roleService.GetRoleByIdAsync(model.Id);
            return Ok(new
            {
                Status = "Ok",
                Data = obj
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> RemoveMobilePermissionByRole(RoleDTO role, CancellationToken cancellationToken = default)
        {
            foreach (var item in role.Employees ?? new List<EmployeeDTO>())//
            {
                string key = $"MobilePermission-{item.Id}-{item.UserName}";
                _memoryCacheHelper.Remove(key);
                //await _memoryCacheHelper.GetOrCreateAsync(key, async (e) =>
                //{
                //    var mobilePermissionRes = await _mobilePermissionsService.GetPermissionsByIdsAsync(role.MobilePermissions, cancellationToken);
                //    var mobilePermissions = mobilePermissionRes.Where(x => x.ParentId != 0).Select(x => x.PermissionCode).ToList();
                //    return mobilePermissions;
                //}, Convert.ToInt32(_jwtParam.ValidLifetime), false);
            }
          

           
            return Ok(new
            {
                Status = "Ok"
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> RemoveMobilePermissionByEmployee(EmployeeDTO employee, CancellationToken cancellationToken = default)
        {
            string mobileKey = $"MobilePermission-{employee.Id}-{employee.UserName}";
            _memoryCacheHelper.Remove(mobileKey);
            return Ok(new
            {
                Status = "Ok"
            });
        }

        /// <summary>
        /// 更新程序
        /// </summary>
        /// <param name="curVersion"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateApk(string curVersion)
        {
            #region 程序远程升级
            if (string.IsNullOrEmpty(curVersion))
            {
                return Ok(new
                {
                    Status = "Faied",
                    msg = "没有版本数据"
                });
            }
            string version = curVersion;
            var versionAppTempForSystemUpdate = await _versionAppService.GetVersionAppByPropAsync(new VersionAppQueryModel {Source=VersionAppEnum.App});
            if (versionAppTempForSystemUpdate == null)
            {
                return Ok(new
                {
                    Status = "Faied",
                    msg = "没有版本数据"
                });
            }

            versionAppTempForSystemUpdate.Version = version;

            await _versionAppService.EditVersionAppAsync(new VersionAppChangeModel 
            {
                Id=versionAppTempForSystemUpdate.Id,
                Version= versionAppTempForSystemUpdate.Version ,
                DataVersion= versionAppTempForSystemUpdate.DataVersion ,
                Source=VersionAppEnum.App,
                IsUpdate=versionAppTempForSystemUpdate.IsUpdate           
            });

            if (version != versionAppTempForSystemUpdate.DataVersion)
            {
                var data = new
                {
                    Download_URL = _config["Curdomain"] + "/Storage/AppUpload/ManagerApp.apk",
                    Version = versionAppTempForSystemUpdate.DataVersion
                };
                return Ok(new
                {
                    Status = "Ok",
                    data = data,
                    msg = "操作成功"
                });
            }
            else
            {
                return Ok(new
                {
                    Status = "Ok",
                    data = "",
                    msg = "无需更新"
                });
            }
            #endregion
        }

        private static string BuildToken(IEnumerable<Claim> claims, JwtParam options)
        {
            var authTime = DateTime.UtcNow;
            DateTime expires = authTime.AddSeconds(options.ValidLifetime);
            byte[] keyBytes = Encoding.UTF8.GetBytes(options.ValidIssuerSigningKey);
            var secKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(secKey,
                SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(issuer: options.ValidIssuer, audience: options.ValidAudience, claims: claims, expires: expires, notBefore: authTime,
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
        /// <summary>
        /// 查询所有移动端权限项
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[HttpPost]
        //[SwaggerResponse(0, "返回数据属性注释", typeof(PermissionsTreeModel))]
        //public async Task<ActionResult> GetAllPermissionsAsync(CancellationToken cancellationToken = default)
        //{
        //    var data = await _mobilePermissionsService.GetAllPermissionsAsync(cancellationToken);
        //    List<PermissionsTreeModel> treeData = data.Select(x => new PermissionsTreeModel
        //    {
        //        Id = x.Id,
        //        ParentId = x.ParentId.HasValue ? x.ParentId.Value : 0,
        //        CreateTime = x.CreateTime,
        //        PermissionCode = x.PermissionCode,
        //        PermissionName = x.PermissionName,
        //        Remark = x.Remark
        //    }).OrderBy(x => x.Id).ToList();

        //    return Ok(new
        //    {
        //        Status = "Ok",
        //        Data = treeData == null ? null : CommonUtils.ToTreeData(null, treeData)
        //    });
        //}

        
     
    }
}
