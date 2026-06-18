using Solution.Core.DTO.Common;
using Solution.Core.ParameterModel.ChangeModel.Common;
using Solution.Core.ParameterModel.QueryModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Core.Domain.INpgSqlService
{
    public interface IShiftInfoService : IServiceSupport
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Tuple<long, string>> AddShiftInfoAsync(ShiftInfoChangeModel model, CancellationToken cancellationToken = default);
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Tuple<bool, string>> EditShiftInfoAsync(ShiftInfoChangeModel model, CancellationToken cancellationToken = default);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Tuple<bool, string>> RemoveShiftInfoAsync(long id, CancellationToken cancellationToken = default);
        /// <summary>
        /// 根据条件获取分页数据
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<Pagination<ShiftInfoDTO>> GetShiftInfoAsync(ShiftInfoQueryModel queryModel, CancellationToken cancellationToken = default, params string[] param);
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ShiftInfoDTO> GetShiftInfoByPropAsync(ShiftInfoQueryModel queryModel, CancellationToken cancellationToken = default, params string[] param);
        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ShiftInfoDTO> GetShiftInfoByIdAsync(long id);
        /// <summary>
        /// 根据条件查看是否有符合条件的数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> ShiftInfoAnyAsync(ShiftInfoQueryModel queryModel, CancellationToken cancellationToken = default);
    }
}
