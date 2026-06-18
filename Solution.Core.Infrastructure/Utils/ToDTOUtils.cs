
using Microsoft.EntityFrameworkCore;

namespace Solution.Core.Infrastructure.Utils
{
    public class ToDTOUtils
    {
    
        public static OperationLogDTO ToDTO(OperationLog entity)
        {
            OperationLogDTO dto = new OperationLogDTO();
            dto = CommonUtils.Mapper<OperationLogDTO, OperationLog>(entity);
            return dto;
        }
        public static ShiftInfoDTO ToDTO(ShiftInfo entity)
        {
            ShiftInfoDTO dto = new ShiftInfoDTO();
            dto = CommonUtils.Mapper<ShiftInfoDTO, ShiftInfo>(entity);
            return dto;
        }
        public static UserInfoDTO ToDTO(UserInfo entity)
        {
            UserInfoDTO dto = new UserInfoDTO();
            dto = CommonUtils.Mapper<UserInfoDTO, UserInfo>(entity);
            return dto;
        }
        public static DictionaryDTO ToDTO(Dictionary entity)
        {
            DictionaryDTO dto = new DictionaryDTO();
            dto = CommonUtils.Mapper<DictionaryDTO, Dictionary>(entity);
            return dto;
        }
      
        public static DepartmentDTO ToDTO(Department entity)
        {
            DepartmentDTO dto = new DepartmentDTO();
            dto = CommonUtils.Mapper<DepartmentDTO, Department>(entity);
            if (entity.Employees != null && entity.Employees.Count() > 0)
            {
                dto.Employees = entity.Employees.Select(ToDTO).ToList();
            }
            //if (entity.DepartmentLeader != null)
            //{
            //    dto.LeaderCellphone = entity.DepartmentLeader.Cellphone;
            //    dto.EmployeeId = entity.DepartmentLeader.Id;
            //    dto.LeaderName = entity.DepartmentLeader.RealName;
            //}
            return dto;
        }
        public static EmployeeDTO ToDTO(Employee entity)
        {
            EmployeeDTO dto = new EmployeeDTO();
            dto = CommonUtils.Mapper<EmployeeDTO, Employee>(entity);
            if (entity.Department != null)
            {
                dto.DepartmentName = entity.Department.DepartmentName;
            }
            if (entity.ShiftInfo != null)
            {
                dto.ShiftName = entity.ShiftInfo.ShiftName;
            }
            dto.Sex = (int)entity.Sex;
            dto.SexStr = entity.Sex.ToString();
            dto.LoginPermissions = (int)entity.LoginPermissions;
            dto.LoginPermissionsStr = entity.LoginPermissions.ToString();
            dto.Status = (int)entity.Status;
            dto.StatusStr = entity.Status.ToString();
            if (entity.Roles != null && entity.Roles.Count() > 0)
            {
                dto.RoleId = string.Join(',', entity.Roles.Select(x => x.Id));
                dto.RoleIds = entity.Roles.Select(x => x.Id).ToArray();
                dto.RoleName = string.Join(',', entity.Roles.Select(x => x.RoleName));
                dto.Permissions = entity.Roles.Where(x => !string.IsNullOrWhiteSpace(x.Permissions))
                                                    .SelectMany(x => x.Permissions.Split(',', StringSplitOptions.RemoveEmptyEntries))
                                                    .Select(x => long.TryParse(x, out var n) ? (long?)n : null)
                                                    .Where(x => x.HasValue)
                                                    .Select(x => x.Value)
                                                    .Distinct()
                                                    .ToArray();
                dto.MobilePermissions = entity.Roles.Where(x => !string.IsNullOrWhiteSpace(x.MobilePermissions))
                                                    .SelectMany(x => x.MobilePermissions.Split(',', StringSplitOptions.RemoveEmptyEntries))
                                                    .Select(x => long.TryParse(x, out var n) ? (long?)n : null)
                                                    .Where(x => x.HasValue)
                                                    .Select(x => x.Value)
                                                    .Distinct()
                                                    .ToArray();
            }

            return dto;
        }
        public static MenuDTO ToDTO(Menu entity)
        {
            MenuDTO dto = new MenuDTO();
            dto = CommonUtils.Mapper<MenuDTO, Menu>(entity);
            dto.IsButton = (int)entity.IsButton;
            dto.IsButtonStr = entity.IsButton.ToString();
            return dto;
        }
        public static MobilePermissionsDTO ToDTO(MobilePermissions entity)
        {
            MobilePermissionsDTO dto = new MobilePermissionsDTO();
            dto = CommonUtils.Mapper<MobilePermissionsDTO, MobilePermissions>(entity);
            return dto;
        }
        public static RoleDTO ToDTO(Role entity)
        {
            RoleDTO dto = new RoleDTO();
            dto = CommonUtils.Mapper<RoleDTO, Role>(entity);
            if (entity.Employees != null && entity.Employees.Count() > 0)
            {
                dto.Employees = entity.Employees.Select(ToDTO).ToList();
            }
            dto.Permissions = entity.Permissions?
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Where(x => long.TryParse(x, out _))
                .Select(long.Parse)
                .ToArray() ?? Array.Empty<long>();
            dto.MobilePermissions = entity.MobilePermissions?
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Where(x => long.TryParse(x, out _))
                .Select(long.Parse)
                .ToArray() ?? Array.Empty<long>();
            return dto;
        }
        public static VersionAppDTO ToDTO(VersionApp entity)
        {
            VersionAppDTO dto = new VersionAppDTO();
            dto = CommonUtils.Mapper<VersionAppDTO, VersionApp>(entity);
            return dto;
        }
        public static int GetDays(DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                DateTime now = DateTime.Now;
                // 到期时间（取值）
                DateTime expirationDate = dateTime.Value;

                // 计算时间差
                TimeSpan diff = expirationDate - now;

                // 相差天数（向下取整）
                int days = diff.Days;
                return days <= 0 ? 0 : days;
            }
            else
            {
                return 0;
            }
        }
        
    }
}
