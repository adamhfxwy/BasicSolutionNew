

using Solution.Core.Domain.INpgSqlService;
using Solution.Core.DTO.Identity;
using Solution.Core.Infrastructure.Utils;
using Solution.Core.ParameterModel.ChangeModel.Identity;
using Solution.Core.ParameterModel.QueryModel.Identity;
using static Solution.Core.EnumAndConstent.Enums.Enum;

namespace Solution.Core.Infrastructure.NpgSqlService
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _repository;
        private readonly IRepository<Role> _roleRepository;
        public EmployeeService(IRepository<Employee> repository, IRepository<Role> roleRepository)
        {
            _repository = repository;
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tuple<long, string>> AddEmployeeAsync(AddEditEmployeeModel model, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(model.RealName))
            {
                return new Tuple<long, string>(0, "姓名不可为空");
            }
            if (string.IsNullOrEmpty(model.Cellphone))
            {
                return new Tuple<long, string>(0, "手机号不可为空");
            }
            bool exists = await _repository.AnyAsync(x => x.Cellphone == model.Cellphone && x.IsDeleted == IsDeletedEnum.未删除, cancellationToken);
            if (exists)
            {
                return new Tuple<long, string>(0, "手机号不能重复");
            }
            if (model.LoginPermissions == null)
            {
                return new Tuple<long, string>(0, "权限配置不可为空");
            }
            if (model.Sex == null)
            {
                return new Tuple<long, string>(0, "性别不可为空");
            }
            // 从数据库获取已有员工实体
            var existingRoles = await _roleRepository.GetAllAsync()
                .Where(x => model.RoleIds.Contains(x.Id))
                .ToListAsync(cancellationToken);
            Employee entity = new Employee(model.RealName, model.Cellphone, model.Sex.Value, model.Remark
                , model.LoginPermissions.Value, model.DepartmentId, model.Age, model.PhotoPath, model.ShiftId, model.UserName, existingRoles,model.JobName);
            if (!string.IsNullOrEmpty(model.Password))
            {
                var tuple = entity.ChangePassword(model.Password);
                if (!tuple.Item1)
                {
                    return new Tuple<long, string>(0, tuple.Item2);
                }
            }
            entity = await _repository.InsertAsync(entity, true, cancellationToken);
            return new Tuple<long, string>(entity.Id, "success");
        }
        public async Task<Tuple<bool, string>> ChangePasswordAsync(AddEditEmployeeModel model, CancellationToken cancellationToken = default)
        {
            var obj = await _repository.FindAsync(model.Id.Value);
            if (obj == null)
            {
                return new Tuple<bool, string>(false, $"id={model.Id}的员工不存在");
            }
            if (!string.IsNullOrEmpty(model.Password))
            {
                var tuple = obj.ChangePassword(model.Password);
                if (!tuple.Item1)
                {
                    return new Tuple<bool, string>(true, tuple.Item2);
                }
            }
            obj = await _repository.UpdateAsync(obj, true, cancellationToken);
            return new Tuple<bool, string>(true, "success");
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> EditEmployeeAsync(AddEditEmployeeModel model, CancellationToken cancellationToken = default)
        {
            var obj = await _repository.FindAsync(model.Id.Value);
            if (obj == null)
            {
                return new Tuple<bool, string>(false, $"id={model.Id}的员工不存在");
            }

            if (!string.IsNullOrEmpty(model.Cellphone))
            {
                bool exists = await _repository.AnyAsync(x => x.Cellphone == model.Cellphone && x.IsDeleted == IsDeletedEnum.未删除 && x.Id != model.Id, cancellationToken);
                if (exists)
                {
                    return new Tuple<bool, string>(false, "手机号不能重复");
                }
                obj.ChangeCellPhone(model.Cellphone);
            }
            if (!string.IsNullOrEmpty(model.Password))
            {
                var tuple = obj.ChangePassword(model.Password);
                if (!tuple.Item1)
                {
                    return new Tuple<bool, string>(false, tuple.Item2);
                }
            }
            if (!string.IsNullOrEmpty(model.Remark))
            {
                obj.ChangeRemark(model.Remark);
            }
            if (!string.IsNullOrEmpty(model.RealName))
            {
                obj.ChangeRealName(model.RealName);
            }
            if (!string.IsNullOrEmpty(model.JobName))
            {
                obj.ChangeJobName(model.JobName);
            }
            if (!string.IsNullOrEmpty(model.PhotoPath) && !model.PhotoPath.Contains("http") && !model.PhotoPath.Contains("https"))
            {
                obj.ChangePhotoPath(model.PhotoPath);
            }
            if (model.Sex.HasValue)
            {
                obj.ChangeSex(model.Sex.Value);
            }
            if (model.Status.HasValue)
            {
                if (obj.Status == EmployeeStatusEnum.正常)
                {
                    obj.ChangeStatus(EmployeeStatusEnum.离职);
                }
                else if (obj.Status == EmployeeStatusEnum.离职)
                {
                    obj.ChangeStatus(EmployeeStatusEnum.正常);
                }
                else
                {
                    return new Tuple<bool, string>(false, $"请输入正确的状态");
                }
            }
            if (model.LoginPermissions.HasValue)
            {
                obj.ChangeLoginPermissions(model.LoginPermissions.Value);
            }
            //if (model.RoleId.HasValue && model.RoleId.Value > 0)
            //{
            //    obj.ChangeRoles(model.RoleId.Value);
            //}
            if (model.RoleIds != null && model.RoleIds.Count() > 0)
            {
                obj.Roles?.Clear();
                var roles = await _roleRepository.GetAllAsync().Where(x => model.RoleIds.Any(i => x.Id == i)).ToListAsync();
                if (roles.Count() <= 0)
                {
                    return new Tuple<bool, string>(false, "职位不存在");
                }
                obj.ChangeRoles(roles);
            }
            if (model.ShiftId.HasValue && model.ShiftId.Value > 0)
            {
                obj.ChangeShiftId(model.ShiftId.Value);
            }
            if (model.DepartmentId.HasValue && model.DepartmentId.Value > 0)
            {
                obj.ChangeDepartmentId(model.DepartmentId.Value);
            }
            if (model.Age.HasValue)
            {
                obj.ChangeAge(model.Age.Value);
            }
            obj = await _repository.UpdateAsync(obj, true, cancellationToken);
            return new Tuple<bool, string>(true, "success");
        }
        /// <summary>
        /// 登录校验
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<Tuple<bool, string, EmployeeDTO>> CheckLoginAsync(string cellphone, string password, CancellationToken cancellationToken = default)
        {
            var user = await _repository.FindAsync(x => x.Cellphone == cellphone && x.IsDeleted == IsDeletedEnum.未删除, cancellationToken, x => x.Include(i => i.Roles));
            if (user == null)
            {
                return new Tuple<bool, string, EmployeeDTO>(false, $"手机号={cellphone}的用户不存在", null);
            }
            bool isOK = user.CheckPassword(password);
            if (isOK)
            {
                var dto = CommonUtils.Mapper<EmployeeDTO, Employee>(user);
                if (user.Department != null)
                {
                    dto.DepartmentName = user.Department.DepartmentName;
                }
                if (user.ShiftInfo != null)
                {
                    dto.ShiftName = user.ShiftInfo.ShiftName;
                }
                dto.Sex = (int)user.Sex;
                dto.SexStr = user.Sex.ToString();
                dto.LoginPermissions = (int)user.LoginPermissions;
                dto.LoginPermissionsStr = user.LoginPermissions.ToString();
                dto.Status = (int)user.Status;
                dto.StatusStr = user.Status.ToString();
                //if (user.role != null)
                //{
                //    dto.rolename = user.role.rolename;
                //    dto.permissions = user.role.permissions;
                //    dto.mobilepermissions = user.role.mobilepermissions;
                //}
                if (user.Roles != null && user.Roles.Count() > 0)
                {
                    dto.RoleId = string.Join(',', user.Roles.Select(x => x.Id));
                    dto.RoleIds = user.Roles.Select(x => x.Id).ToArray();
                    dto.RoleName = string.Join(',', user.Roles.Select(x => x.RoleName));
                    dto.Permissions = user.Roles.Where(x => !string.IsNullOrWhiteSpace(x.Permissions))
                                                    .SelectMany(x => x.Permissions.Split(',', StringSplitOptions.RemoveEmptyEntries))
                                                    .Select(x => long.TryParse(x, out var n) ? (long?)n : null)
                                                    .Where(x => x.HasValue)
                                                    .Select(x => x.Value)
                                                    .Distinct()
                                                    .ToArray();
                    dto.MobilePermissions = user.Roles.Where(x => !string.IsNullOrWhiteSpace(x.MobilePermissions))
                                                    .SelectMany(x => x.MobilePermissions.Split(',', StringSplitOptions.RemoveEmptyEntries))
                                                    .Select(x => long.TryParse(x, out var n) ? (long?)n : null)
                                                    .Where(x => x.HasValue)
                                                    .Select(x => x.Value)
                                                    .Distinct()
                                                    .ToArray(); 
                }
                return new Tuple<bool, string, EmployeeDTO>(true, $"success", dto);
            }
            else
            {
                return new Tuple<bool, string, EmployeeDTO>(false, $"密码错误", null);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> RemoveEmployeeAsync(long id, CancellationToken cancellationToken = default)
        {
            var obj = await _repository.FindAsync(id);
            if (obj == null || obj.IsDeleted == IsDeletedEnum.已删除)
            {
                return new Tuple<bool, string>(false, $"id={id}的员工不存在");
            }
            obj.ChangeIsDeleted();
            await _repository.UpdateAsync(obj, true, cancellationToken);
            return new Tuple<bool, string>(true, "success");
        }
        /// <summary>
        /// 根据条件获取分页数据
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<Pagination<EmployeeDTO>> GetEmployeeAsync(EmployeeQueryModel queryModel, CancellationToken cancellationToken = default, params string[] param)
        {

            var data = _repository.GetAllAsync().Include(x => x.Roles).Include(x => x.ShiftInfo).Include(x => x.Department).Where(x => x.RealName != "超级管理员");
            var tuple = await CommonWhereBuilder.WhereBuilder(data, queryModel, cancellationToken);
            //var list = await tuple.Item1.Select(x => new EmployeeDTO
            //{
            //    Id = x.Id,
            //    DepartmentId = x.DepartmentId,
            //    DepartmentName = x.Department.DepartmentName,
            //    Age = x.Age,
            //    Cellphone = x.Cellphone,
            //    LoginPermissions = (int)x.LoginPermissions,
            //    LoginPermissionsStr = x.LoginPermissions.ToString(),
            //    Sex = (int)x.Sex,
            //    SexStr = x.Sex.ToString(),
            //    PhotoPath = x.PhotoPath,
            //    CreateTime = x.CreateTime,
            //    RealName = x.RealName,
            //    Remark = x.Remark,
            //    RoleId = x.RoleId,
            //    RoleName = x.Role.RoleName,
            //    Status = (int)x.Status,
            //    StatusStr = x.Status.ToString(),
            //    UserName = x.UserName,
            //    ShiftId = x.ShiftId,
            //    ShiftName = x.ShiftInfo.ShiftName

            //}).ToListAsync(cancellationToken);
            var list = tuple.Item1.Select(ToDTOUtils.ToDTO).ToList();
            return new Pagination<EmployeeDTO> { List = list, Total = tuple.Item2, Code = 1 };
        }
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<EmployeeDTO> GetEmployeeByPropAsync(EmployeeQueryModel queryModel, CancellationToken cancellationToken = default)
        {
            var exp = await CommonWhereBuilder.WhereBuilderToExp<Employee, EmployeeQueryModel>(queryModel, cancellationToken);//<SimCardInfo, SimCardInfoQueryModel>
            var obj = await _repository.FindAsync(exp, cancellationToken, x => x.Include(e => e.Roles).Include(x => x.ShiftInfo).Include(x => x.Department));
            if (obj != null)
            {
                return ToDTOUtils.ToDTO(obj);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<EmployeeDTO> GetEmployeeByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var obj = await _repository.FindAsync(id, x => x.Include(e => e.ShiftInfo).Include(e => e.Department).Include(x => x.Roles));
            if (obj != null)
            {
                return ToDTOUtils.ToDTO(obj);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 根据条件查看是否有符合条件的数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> EmployeeAnyAsync(EmployeeQueryModel queryModel, CancellationToken cancellationToken = default)
        {
            var exp = await CommonWhereBuilder.WhereBuilderToExp<Employee, EmployeeQueryModel>(queryModel, cancellationToken);
            return await _repository.AnyAsync(exp, cancellationToken);
        }


    }
}
