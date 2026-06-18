

using Solution.Core.Domain.INpgSqlService;
using Solution.Core.Infrastructure.Utils;
using Solution.Core.ParameterModel.ChangeModel.Common;
using Solution.Core.ParameterModel.QueryModel.Common;

namespace Solution.Core.Infrastructure.NpgSqlService
{
    public class OperationLogService : IOperationLogService
    {
        private readonly IRepository<OperationLog> _repository;
        public OperationLogService(IRepository<OperationLog> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tuple<long, string>> AddOperationLogAsync(AddEditOperationLogModel model, CancellationToken cancellationToken = default)
        {
            OperationLog entity = new OperationLog(model.EmpId.Value, model.EmpName, model.OperationName, model.ApiPath, model.RequestMessage, model.ResponseMessage);
            entity = await _repository.InsertAsync(entity, true, cancellationToken);
            return new Tuple<long, string>(entity.Id, "success");
        }
        /// <summary>admin
        /// 根据条件获取分页数据
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<Pagination<OperationLogDTO>> GetOperationLogAsync(OperationLogQueryModel queryModel, CancellationToken cancellationToken = default, params string[] param)
        {
            var exp = await CommonWhereBuilder.WhereBuilderToExp<OperationLog, OperationLogQueryModel>(queryModel, cancellationToken);//<SimCardInfo, SimCardInfoQueryModel>        
            var list = await _repository.GetListOfWithIncludDataAsync(queryModel, exp, null, cancellationToken, true);
            var dto = list.List.Select(ToDTOUtils.ToDTO).ToList();
            return new Pagination<OperationLogDTO> { List = dto, Total = list.Total, Code = 1 };
        }
    }
}
