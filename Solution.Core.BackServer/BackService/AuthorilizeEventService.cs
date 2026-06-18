using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.Extensions.DependencyInjection;

namespace Solution.Core.BackServer.BackService
{
    public class AuthorilizeEventService
    {
        //private readonly RoleApplication _roleApplication;
        //private readonly MenuApplication _menuApplication;
        //private readonly MobilePermissionsApplication _mobilePermissionsApplication;
        //private readonly IServiceScopeFactory _service;
        //private IServiceScope _serviceScope;

        //public AuthorilizeEventService(IServiceScopeFactory service)
        //{
        //    _service = service;
        //    _serviceScope = _service.CreateScope();
        //    _roleApplication = _serviceScope.ServiceProvider.GetRequiredService<RoleApplication>();
        //    _menuApplication = _serviceScope.ServiceProvider.GetRequiredService<MenuApplication>();
        //    _mobilePermissionsApplication = _serviceScope.ServiceProvider.GetRequiredService<MobilePermissionsApplication>();
        //}

        ///// <summary>
        ///// 根据角色id获取权限
        ///// </summary>
        ///// <param name="roleId"></param>
        ///// <param name="keyType"></param>
        ///// <returns></returns>
        //public async Task<List<string>> GetPermissionByRoleIdAsync(long roleId, string keyType)
        //{
        //    var role = await _roleApplication.GetRoleByIdAsync(roleId);
        //    if (role != null)
        //    {
        //        if (keyType == "web")
        //        {
        //            var menus = await _menuApplication.GetMenusByIdsAsync(role.Permissions, CancellationToken.None);
        //            var webPermissions = menus.Where(x => x.ParentId != 0).Select(x => x.MenuCode).ToList();
        //            return webPermissions.Where(x => !string.IsNullOrEmpty(x)).ToList();
        //        }
        //        else if (keyType == "mobile")
        //        {
        //            var mobilePermissionRes = await _mobilePermissionsApplication.GetPermissionsByIdsAsync(role.MobilePermissions, CancellationToken.None);
        //            var mobilePermissions = mobilePermissionRes.Where(x => x.ParentId != 0).Select(x => x.PermissionCode).ToList();
        //            return mobilePermissions.Where(x => !string.IsNullOrEmpty(x)).ToList();
        //        }
        //        else
        //        {
        //            return new List<string>();
        //        }
        //    }
        //    else
        //    {
        //        return new List<string>();
        //    }
        //}
    }
}
