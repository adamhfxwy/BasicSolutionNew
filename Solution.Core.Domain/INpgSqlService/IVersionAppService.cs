using Solution.Core.DTO.Common;
using Solution.Core.DTO.Identity;
using Solution.Core.ParameterModel.ChangeModel.Common;
using Solution.Core.ParameterModel.ChangeModel.Identity;
using Solution.Core.ParameterModel.QueryModel.Common;
using Solution.Core.ParameterModel.QueryModel.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Core.Domain.INpgSqlService
{
    public interface IVersionAppService : IServiceSupport
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Tuple<long, string>> AddVersionAppAsync(VersionAppChangeModel model, CancellationToken cancellationToken = default);
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Tuple<bool, string>> EditVersionAppAsync(VersionAppChangeModel model, CancellationToken cancellationToken = default);
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<VersionAppDTO> GetVersionAppByPropAsync(VersionAppQueryModel queryModel, CancellationToken cancellationToken = default, params string[] param);
        /// <summary>
        /// 根据id获取一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<VersionAppDTO> GetVersionAppByIdAsync(long id);
    }
}
