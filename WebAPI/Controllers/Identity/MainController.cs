
namespace WebAPI.Controllers.Identity
{
    /// <summary>
    /// 登录相关
    /// </summary>
    [Authorize]
    [Route("customWeb/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "Identity")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMenuService _menuService;
        private readonly IMobilePermissionsService _mobilePermissionsService;
        private readonly JwtParam _jwtParam;
        private readonly IRoleService _roleService;
        private readonly IMemoryCacheHelper _memoryCacheHelper;

        public MainController(IMemoryCacheHelper memoryCacheHelper, IRoleService roleService, JwtParam jwtParam, IMobilePermissionsService mobilePermissionsService, IMenuService menuService, IEmployeeService employeeService)
        {
            _memoryCacheHelper = memoryCacheHelper;
            _roleService = roleService;
            _jwtParam = jwtParam;
            _mobilePermissionsService = mobilePermissionsService;
            _menuService = menuService;
            _employeeService = employeeService;
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
            if (user.LoginPermissions != (int)LoginPermissionsEnum.web权限 && user.LoginPermissions != (int)LoginPermissionsEnum.所有权限)
            {
                return Ok(new
                {
                    Status = "Failed",
                    Msg = "无web管理系统权限，无法登录。"
                });
            }
            var claims = new List<Claim>();
            string roleName = user.RoleName ?? "";
            var memoryCacheKey = $"WebPermission-{user.Id}-{user.UserName}";
            claims.Add(new Claim("RealName", user.RealName));
            claims.Add(new Claim("RoleName", roleName));
            claims.Add(new Claim("UserId", user.Id.ToString()));
            claims.Add(new Claim("RoleId", user.RoleId == null ? "" : user.RoleId));
            claims.Add(new Claim("MemoryCacheKey", memoryCacheKey));

            List<MenuTreeModel> treeData = new List<MenuTreeModel>();
            string token = BuildToken(claims, _jwtParam);
            if (user.RealName != "超级管理员")
            {
                //var role = await _roleService.GetRoleByIdAsync(user.RoleId.Value);
                //if (role != null)
                //{

                //}
                if (user.Permissions != null && user.Permissions.Count() > 0)
                {
                    var menus = await _menuService.GetMenusByIdsAsync(user.Permissions, cancellationToken);
                    _memoryCacheHelper.Remove(memoryCacheKey);
                    var permissions = await _memoryCacheHelper.GetOrCreateAsync(memoryCacheKey, async (e) =>
                    {
                        //var mobilePermissionRes = await _mobilePermissionsApplication.GetPermissionsByIdsAsync(role.MobilePermissions, cancellationToken);
                        //var mobilePermissions = mobilePermissionRes.Select(x => x.PermissionCode).ToList();
                        var webPermissions = menus.Where(x => x.ParentId != 0).Select(x => x.MenuCode).ToList();
                        return new Tuple<bool, List<string>>(true, webPermissions);
                    }, Convert.ToInt32(_jwtParam.ValidLifetime), false);


                    treeData = menus.Where(x => x.IsButton == (int)YesOrNoEnum.否).Select(x => new MenuTreeModel
                    {
                        Component = x.Component,
                        Createtime = x.CreateTime,
                        Description = x.Description,
                        Icon = x.Icon,
                        Id = x.Id,
                        Name = x.Name,
                        MenuName = x.MenuName,
                        ParentId = x.ParentId.HasValue ? x.ParentId.Value : 0,
                        Path = x.Path,
                        Level = x.Level,
                        Meta = new Meta { Buttons = menus.Where(i => i.ParentId == x.Id && i.IsButton == (int)YesOrNoEnum.是).Select(i => i.ButtonName).ToList() }
                        //Buttons = menus.Where(i => i.ParentId == x.Id && i.IsButton == (int)YesOrNoEnum.是).Select(i => i.ButtonName).ToList()
                    }).OrderByDescending(x => x.Createtime).ToList();
                }
            }
            else if (user.RealName == "超级管理员")
            {
                var menus = await _menuService.GetAllMenusAndButtonAsync();
                //var bottens = await _menuApplication.GetAllButtonsAsync(cancellationToken);
                treeData = menus.Where(x => x.IsButton == (int)YesOrNoEnum.否).Select(x => new MenuTreeModel
                {
                    Component = x.Component,
                    Createtime = x.CreateTime,
                    Description = x.Description,
                    Icon = x.Icon,
                    Id = x.Id,
                    Name = x.Name,
                    MenuName = x.MenuName,
                    ParentId = x.ParentId.HasValue ? x.ParentId.Value : 0,
                    Path = x.Path,
                    Level = x.Level,
                    //Buttons = menus.Where(i => i.ParentId == x.Id && i.IsButton == (int)YesOrNoEnum.是).Select(i => new WebMenuBotten
                    //{
                    //    Component = i.Component,
                    //    Createtime = i.CreateTime,
                    //    Description = i.Description,
                    //    Icon = i.Icon,
                    //    MenuCode = i.MenuCode,
                    //    Id = i.Id,
                    //    MenuName = i.MenuName,
                    //    Name = i.Name,
                    //    ParentId = i.ParentId,
                    //    Path = i.Path,
                    //    //IsShow = menus.Any(u => u.Id == i.Id)
                    //}).ToList()
                    Meta = new Meta { Buttons = menus.Where(i => i.ParentId == x.Id && i.IsButton == (int)YesOrNoEnum.是).Select(i => i.ButtonName).ToList() }

                }).OrderByDescending(x => x.Createtime).ToList();
            }

            return Ok(new
            {
                Status = "Ok",
                UserId = user.Id,
                User = user,
                Menus = treeData == null ? null : CommonUtils.ToTreeData(null, treeData),
                Token = token
            });
        }
        /// <summary>
        /// 跳转页登录接口
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SignIn(string username, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(username) )
            {
                return Ok(new { Status = "Failed", Msg = "用户名不能为空" });
            }
            var tuple = await _employeeService.GetEmployeeByPropAsync(new EmployeeQueryModel { Cellphone = username }, cancellationToken);
            if (tuple==null)
            {
                return Ok(new
                {
                    Status = "Failed",
                    Msg = "用户不存在"
                });
            }
            var user = tuple;
            if (user.LoginPermissions != (int)LoginPermissionsEnum.web权限 && user.LoginPermissions != (int)LoginPermissionsEnum.所有权限)
            {
                return Ok(new
                {
                    Status = "Failed",
                    Msg = "无web管理系统权限，无法登录。"
                });
            }
            var claims = new List<Claim>();
            string roleName = user.RoleName ?? "";
            var memoryCacheKey = $"WebPermission-{user.Id}-{user.UserName}";
            claims.Add(new Claim("RealName", user.RealName));
            claims.Add(new Claim("RoleName", roleName));
            claims.Add(new Claim("UserId", user.Id.ToString()));
            claims.Add(new Claim("RoleId", user.RoleId == null ? "" : user.RoleId));
            claims.Add(new Claim("MemoryCacheKey", memoryCacheKey));

            List<MenuTreeModel> treeData = new List<MenuTreeModel>();
            string token = BuildToken(claims, _jwtParam);
            if (user.RealName != "超级管理员")
            {
                //var role = await _roleService.GetRoleByIdAsync(user.RoleId.Value);
                //if (role != null)
                //{

                //}
                if (user.Permissions != null && user.Permissions.Count() > 0)
                {
                    var menus = await _menuService.GetMenusByIdsAsync(user.Permissions, cancellationToken);
                    _memoryCacheHelper.Remove(memoryCacheKey);
                    var permissions = await _memoryCacheHelper.GetOrCreateAsync(memoryCacheKey, async (e) =>
                    {
                        //var mobilePermissionRes = await _mobilePermissionsApplication.GetPermissionsByIdsAsync(role.MobilePermissions, cancellationToken);
                        //var mobilePermissions = mobilePermissionRes.Select(x => x.PermissionCode).ToList();
                        var webPermissions = menus.Where(x => x.ParentId != 0).Select(x => x.MenuCode).ToList();
                        return new Tuple<bool, List<string>>(true, webPermissions);
                    }, Convert.ToInt32(_jwtParam.ValidLifetime), false);


                    treeData = menus.Where(x => x.IsButton == (int)YesOrNoEnum.否).Select(x => new MenuTreeModel
                    {
                        Component = x.Component,
                        Createtime = x.CreateTime,
                        Description = x.Description,
                        Icon = x.Icon,
                        Id = x.Id,
                        Name = x.Name,
                        MenuName = x.MenuName,
                        ParentId = x.ParentId.HasValue ? x.ParentId.Value : 0,
                        Path = x.Path,
                        Level = x.Level,
                        Meta = new Meta { Buttons = menus.Where(i => i.ParentId == x.Id && i.IsButton == (int)YesOrNoEnum.是).Select(i => i.ButtonName).ToList() }
                        //Buttons = menus.Where(i => i.ParentId == x.Id && i.IsButton == (int)YesOrNoEnum.是).Select(i => i.ButtonName).ToList()
                    }).OrderByDescending(x => x.Createtime).ToList();
                }
            }
            else if (user.RealName == "超级管理员")
            {
                var menus = await _menuService.GetAllMenusAndButtonAsync();
                //var bottens = await _menuApplication.GetAllButtonsAsync(cancellationToken);
                treeData = menus.Where(x => x.IsButton == (int)YesOrNoEnum.否).Select(x => new MenuTreeModel
                {
                    Component = x.Component,
                    Createtime = x.CreateTime,
                    Description = x.Description,
                    Icon = x.Icon,
                    Id = x.Id,
                    Name = x.Name,
                    MenuName = x.MenuName,
                    ParentId = x.ParentId.HasValue ? x.ParentId.Value : 0,
                    Path = x.Path,
                    Level = x.Level,
                    //Buttons = menus.Where(i => i.ParentId == x.Id && i.IsButton == (int)YesOrNoEnum.是).Select(i => new WebMenuBotten
                    //{
                    //    Component = i.Component,
                    //    Createtime = i.CreateTime,
                    //    Description = i.Description,
                    //    Icon = i.Icon,
                    //    MenuCode = i.MenuCode,
                    //    Id = i.Id,
                    //    MenuName = i.MenuName,
                    //    Name = i.Name,
                    //    ParentId = i.ParentId,
                    //    Path = i.Path,
                    //    //IsShow = menus.Any(u => u.Id == i.Id)
                    //}).ToList()
                    Meta = new Meta { Buttons = menus.Where(i => i.ParentId == x.Id && i.IsButton == (int)YesOrNoEnum.是).Select(i => i.ButtonName).ToList() }

                }).OrderByDescending(x => x.Createtime).ToList();
            }

            return Ok(new
            {
                Status = "Ok",
                UserId = user.Id,
                User = user,
                Menus = treeData == null ? null : CommonUtils.ToTreeData(null, treeData),
                Token = token
            });
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
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(PermissionsTreeModel))]
        public async Task<ActionResult> GetAllPermissionsAsync(CancellationToken cancellationToken = default)
        {
            var data = await _mobilePermissionsService.GetAllPermissionsAsync(cancellationToken);
            List<PermissionsTreeModel> treeData = data.Select(x => new PermissionsTreeModel
            {
                Id = x.Id,
                ParentId = x.ParentId.HasValue ? x.ParentId.Value : 0,
                CreateTime = x.CreateTime,
                PermissionCode = x.PermissionCode,
                PermissionName = x.PermissionName,
                Remark = x.Remark
            }).OrderBy(x => x.Id).ToList();

            return Ok(new
            {
                Status = "Ok",
                Data = treeData == null ? null : CommonUtils.ToTreeData(null, treeData)
            });
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody))]
        public async Task<ActionResult> ChangePasswordAsync(ChangePasswordModel model, CancellationToken cancellationToken = default)
        {
            if (model.Id <= 0)
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"请选择Id"));
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"密码不能为空"));
            }
            var tuple = await _employeeService.ChangePasswordAsync(new AddEditEmployeeModel { Id = model.Id, Password = model.Password }, cancellationToken);
            return Ok(new ResponseBody(tuple.Item1 ? ResponseStatus.Ok.ToString() : ResponseStatus.Failed.ToString(), tuple.Item2));
        }
    }
}
