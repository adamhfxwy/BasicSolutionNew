
using Solution.Core.Domain.INpgSqlService;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace MobileAPI.Filters
{
    public class AuthorilizeFilter : IAuthorizationFilter
    {
        private readonly IRoleService _roleService;
        private readonly IMobilePermissionsService _mobilePermissionsService;
        private readonly IEmployeeService _employeeService;
        private readonly IMemoryCacheHelper _memoryCacheHelper;
        private readonly IServiceScopeFactory _service;
        private IServiceScope _serviceScope;
        public AuthorilizeFilter(IMemoryCacheHelper memoryCacheHelper, IServiceScopeFactory service)
        {
            _memoryCacheHelper = memoryCacheHelper;
            _service = service;
            _serviceScope = _service.CreateScope();
            _roleService = _serviceScope.ServiceProvider.GetRequiredService<IRoleService>();
            _mobilePermissionsService = _serviceScope.ServiceProvider.GetRequiredService<IMobilePermissionsService>();
            _employeeService = _serviceScope.ServiceProvider.GetRequiredService<IEmployeeService>();
        }
        private async Task<List<string>> GetPermissionsFromEvent(long userId)
        {
            bool isOk = true;
            List<string> list = null;
            while (isOk)
            {
                var user = await _employeeService.GetEmployeeByIdAsync(Convert.ToInt64(userId));
                if (user != null && !string.IsNullOrEmpty(user.RoleId))
                {
                    var roleIds = user.RoleId.Split(',')
                               .Select(long.Parse)
                               .ToArray();

                    var role = await _roleService.GetRoleByIdsAsync(roleIds);
                    if (role != null)
                    {
                        var mobilePermissions = role.SelectMany(x => x.MobilePermissions).Distinct().ToArray();
                        var permissions = await _mobilePermissionsService.GetPermissionsByIdsAsync(mobilePermissions);
                        list = permissions.Where(x => x.ParentId != 0).Select(x => x.PermissionCode).ToList();
                    }
                }

                if (list != null)
                {
                    isOk = false;
                }
            }

            return list;
        }
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            var mvcContext = context;
            var descriptor = mvcContext?.ActionDescriptor as ControllerActionDescriptor;

            if (descriptor != null)
            {
                bool isNext = false;
                CheckPermissionAttribute permAttr = (CheckPermissionAttribute)descriptor.MethodInfo.GetCustomAttribute(typeof(CheckPermissionAttribute), false);
                if (permAttr == null)
                {
                    return;
                }
                string token = string.Empty;
                string authHeader = context.HttpContext.Request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer"))
                {
                    token = authHeader.Substring("Bearer ".Length).Trim();
                }
                else
                {
                    string message = "NoAuthorilize";
                    StringBuilder str = new StringBuilder();
                    str.Append("{");
                    str.Append($"\"message\":\"{message}\",");
                    str.Append($"\"status\":\"error\",");
                    str.Append($"\"code\":\"{(int)HttpStatusCode.Forbidden}\"");
                    str.Append("}");
                    context.Result = new ContentResult()
                    {
                        Content = str.ToString(),
                        ContentType = "application/json",
                        StatusCode = (int)HttpStatusCode.Forbidden
                    };
                }
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token).Payload;
                string memoryCacheKey = string.Empty;
                var RealName = jwtToken["RealName"].ToString();
                bool hasPerms = false;
                if (RealName != "超级管理员")
                {
                    if (jwtToken.ContainsKey("MemoryCacheKey"))
                    {
                        memoryCacheKey = jwtToken["MemoryCacheKey"].ToString();
                    }

                    List<string> permissions = _memoryCacheHelper.Get(memoryCacheKey) as List<string>;
                    var userId = jwtToken["UserId"].ToString();
                    if (!string.IsNullOrEmpty(userId))
                    {
                        if (permissions == null)
                        {
                            permissions = await _memoryCacheHelper.GetOrCreateAsync(memoryCacheKey, async (e) =>
                            {
                                Task<List<string>> newTask = Task.Run(() => GetPermissionsFromEvent(Convert.ToInt64(userId)));
                                newTask.Wait();
                                var res = newTask.Result;
                                return res;
                            });
                        }
                    }
                    if (permissions != null)
                    {
                        hasPerms = permissions.Any(x => x == permAttr.Permission);
                    }

                }
                else
                {
                    hasPerms = true;
                }

                if (!hasPerms)
                {
                    string message = "NoAuthorilize";
                    StringBuilder str = new StringBuilder();
                    str.Append("{");
                    str.Append($"\"message\":\"{message}\",");
                    str.Append($"\"status\":\"error\",");
                    str.Append($"\"code\":\"{(int)HttpStatusCode.Unauthorized}\"");
                    str.Append("}");
                    context.Result = new ContentResult()
                    {
                        Content = str.ToString(),
                        ContentType = "application/json",
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                    };

                }
            }
        }
    }
}
