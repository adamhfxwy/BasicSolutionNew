

using Solution.Core.Domain.INpgSqlService;
using Solution.Core.Infrastructure.Utils;
using Solution.Core.ParameterModel.ChangeModel.Identity;
using Solution.Core.ParameterModel.QueryModel.Identity;

namespace Solution.Core.Infrastructure.NpgSqlService
{
    public class DepartmentService : IDepartmentService
    {

        private readonly IRepository<Department> _repository;
        public DepartmentService(IRepository<Department> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tuple<long, string>> AddDepartmentAsync(AddEditDepartmentModel model, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(model.DepartmentName))
            {
                return new Tuple<long, string>(0, "部门名称不可为空");
            }
            bool exists = await _repository.AnyAsync(x => x.DepartmentName == model.DepartmentName && x.IsDeleted == IsDeletedEnum.未删除, cancellationToken);
            if (exists)
            {
                return new Tuple<long, string>(0, "部门名称不能重复");
            }

            Department entity = new Department(model.DepartmentName, model.Remark, model.EmployeeId);

            entity = await _repository.InsertAsync(entity, true, cancellationToken);
            return new Tuple<long, string>(entity.Id, "success");
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> EditDepartmentAsync(AddEditDepartmentModel model, CancellationToken cancellationToken = default)
        {
            var obj = await _repository.FindAsync(model.Id.Value);
            if (obj == null)
            {
                return new Tuple<bool, string>(false, $"id={model.Id}的部门不存在");
            }

            if (!string.IsNullOrEmpty(model.DepartmentName))
            {
                bool exists = await _repository.AnyAsync(x => x.DepartmentName == model.DepartmentName && x.IsDeleted == IsDeletedEnum.未删除 && x.Id != model.Id, cancellationToken);
                if (exists)
                {
                    return new Tuple<bool, string>(false, "部门名称不能重复");
                }
                obj.ChangeDepartMentName(model.DepartmentName);
            }
            if (!string.IsNullOrEmpty(model.Remark))
            {

                obj.ChangeRemark(model.Remark);
            }
            //if (model.DepartmentLeader != null)
            //{
            //    obj.ChangeDepartmentLeader(model.DepartmentLeader);
            //    obj.ChangeLeaderId(model.DepartmentLeader.Id);
            //}
            obj = await _repository.UpdateAsync(obj, true, cancellationToken);
            return new Tuple<bool, string>(true, "success");
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> RemoveDepartmentAsync(long id, CancellationToken cancellationToken = default)
        {
            var obj = await _repository.FindAsync(id);
            if (obj == null || obj.IsDeleted == IsDeletedEnum.已删除)
            {
                return new Tuple<bool, string>(false, $"id={id}的部门不存在");
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
        public async Task<Pagination<DepartmentDTO>> GetDepartmentAsync(DepartmentQueryModel queryModel, CancellationToken cancellationToken = default, params string[] param)
        {
            var exp = await CommonWhereBuilder.WhereBuilderToExp<Department, DepartmentQueryModel>(queryModel, cancellationToken);//<SimCardInfo, SimCardInfoQueryModel>        
            var list = await _repository.GetListOfWithIncludDataAsync(queryModel, exp, null, cancellationToken, true);
            var dto = list.List.Select(ToDTOUtils.ToDTO).ToList();
            return new Pagination<DepartmentDTO> { List = dto, Total = list.Total, Code = 1 };
        }
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DepartmentDTO> GetDepartmentByPropAsync(DepartmentQueryModel queryModel, CancellationToken cancellationToken = default, params string[] param)
        {
            var exp = await CommonWhereBuilder.WhereBuilderToExp<Department, DepartmentQueryModel>(queryModel, cancellationToken);//<SimCardInfo, SimCardInfoQueryModel>
            var obj = await _repository.FindAsync(exp, cancellationToken);
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
        public async Task<DepartmentDTO> GetDepartmentByIdAsync(long id)
        {
            var obj = await _repository.FindAsync(id);
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
        /// 根据条件获取一条数据及其子集
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DepartmentDTO> GetDepartmentAndEmployeesByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var obj = await _repository.GetAllAsync().Include(x => x.Employees.Where(x => x.IsDeleted == IsDeletedEnum.未删除))
             .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (obj == null)
            {
                return null;
            }
            return ToDTOUtils.ToDTO(obj);
        }
    }
}
