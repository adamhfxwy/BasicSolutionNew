

using Solution.Core.DTO.Common;
using Solution.Core.ParameterModel.ChangeModel.Common;
using Solution.Core.ParameterModel.QueryModel.Common;

namespace Solution.Core.Domain.INpgSqlService
{
    public interface IOperationLogService :IServiceSupport
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Tuple<long, string>> AddOperationLogAsync(AddEditOperationLogModel model, CancellationToken cancellationToken = default);
        /// <summary>admin
        /// 根据条件获取分页数据
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<Pagination<OperationLogDTO>> GetOperationLogAsync(OperationLogQueryModel queryModel, CancellationToken cancellationToken = default, params string[] param);
    }
}
