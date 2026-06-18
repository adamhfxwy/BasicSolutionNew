

using Solution.Core.Domain.INpgSqlService;
using Solution.Core.Infrastructure.Utils;
using Solution.Core.ParameterModel.ChangeModel.Common;
using Solution.Core.ParameterModel.QueryModel.Common;

namespace Solution.Core.Infrastructure.NpgSqlService
{
    public class ShiftInfoService : IShiftInfoService
    {
        private readonly IRepository<ShiftInfo> _repository;
        public ShiftInfoService(IRepository<ShiftInfo> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tuple<long, string>> AddShiftInfoAsync(ShiftInfoChangeModel model, CancellationToken cancellationToken = default)
        {
            bool exists = await _repository.AnyAsync(x => x.ShiftName == model.ShiftName && x.IsDeleted == IsDeletedEnum.未删除, cancellationToken);
            if (exists)
            {
                return new Tuple<long, string>(0, "排版名称不能重复");
            }
            ShiftInfo entity = new ShiftInfo(model.ShiftName, TimeOnly.Parse(model.BeginTime), TimeOnly.Parse(model.EndTime), model.Remark);

            entity = await _repository.InsertAsync(entity, true, cancellationToken);
            return new Tuple<long, string>(entity.Id, "success");
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> EditShiftInfoAsync(ShiftInfoChangeModel model, CancellationToken cancellationToken = default)
        {
            var obj = await _repository.FindAsync(model.Id.Value);
            if (obj == null)
            {
                return new Tuple<bool, string>(false, $"id={model.Id}的排班不存在");
            }

            if (!string.IsNullOrEmpty(model.ShiftName))
            {
                bool exists = await _repository.AnyAsync(x => x.ShiftName == model.ShiftName && x.IsDeleted == IsDeletedEnum.未删除 && x.Id != model.Id, cancellationToken);
                if (exists)
                {
                    return new Tuple<bool, string>(false, "排班名称不能重复");
                }
                obj.ChangeShiftName(model.ShiftName);
            }
            if (!string.IsNullOrEmpty(model.BeginTime))
            {
                obj.ChangeBeginTime(TimeOnly.Parse(model.BeginTime));
            }
            if (!string.IsNullOrEmpty(model.EndTime))
            {
                obj.ChangeEndTime(TimeOnly.Parse(model.EndTime));
            }
            obj = await _repository.UpdateAsync(obj, true, cancellationToken);
            return new Tuple<bool, string>(true, "success");
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> RemoveShiftInfoAsync(long id, CancellationToken cancellationToken = default)
        {
            var obj = await _repository.FindAsync(id);
            if (obj == null || obj.IsDeleted == IsDeletedEnum.已删除)
            {
                return new Tuple<bool, string>(false, $"id={id}的排班不存在");
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
        public async Task<Pagination<ShiftInfoDTO>> GetShiftInfoAsync(ShiftInfoQueryModel queryModel, CancellationToken cancellationToken = default, params string[] param)
        {
            var exp = await CommonWhereBuilder.WhereBuilderToExp<ShiftInfo, ShiftInfoQueryModel>(queryModel, cancellationToken);//<SimCardInfo, SimCardInfoQueryModel>        
            var list = await _repository.GetListOfWithIncludDataAsync(queryModel, exp, null, cancellationToken, true);
            var dto = list.List.Select(ToDTOUtils.ToDTO).ToList();
            return new Pagination<ShiftInfoDTO> { List = dto, Total = list.Total, Code = 1 };
        }
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ShiftInfoDTO> GetShiftInfoByPropAsync(ShiftInfoQueryModel queryModel, CancellationToken cancellationToken = default, params string[] param)
        {
            var exp = await CommonWhereBuilder.WhereBuilderToExp<ShiftInfo, ShiftInfoQueryModel>(queryModel, cancellationToken);//<SimCardInfo, SimCardInfoQueryModel>
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
        public async Task<ShiftInfoDTO> GetShiftInfoByIdAsync(long id)
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
        /// 根据条件查看是否有符合条件的数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> ShiftInfoAnyAsync(ShiftInfoQueryModel queryModel, CancellationToken cancellationToken = default)
        {
            var exp = await CommonWhereBuilder.WhereBuilderToExp<ShiftInfo, ShiftInfoQueryModel>(queryModel, cancellationToken);
            return await _repository.AnyAsync(exp, cancellationToken);
        }
    }
}
