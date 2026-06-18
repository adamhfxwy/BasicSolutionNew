


using Solution.Core.Infrastructure.Utils;
using Solution.Core.ParameterModel.ChangeModel.Common;
using Solution.Core.ParameterModel.QueryModel.Common;
using Solution.Core.ParameterModel.QueryModel.Identity;

namespace Solution.Core.Infrastructure.NpgSqlService
{
    public class VersionAppService : IVersionAppService
    {
        private readonly IRepository<VersionApp> _repository;
        public VersionAppService(IRepository<VersionApp> repository)
        {
            _repository = repository;
        }
        public async Task<Tuple<long, string>> AddVersionAppAsync(VersionAppChangeModel model, CancellationToken cancellationToken = default)
        {
            if (model.Source.HasValue)
            {
                bool exists = await _repository.AnyAsync(x => x.Source == model.Source && x.IsDeleted == IsDeletedEnum.未删除, cancellationToken);
                if (exists)
                {
                    return new Tuple<long, string>(0, "类型不能重复");
                }
            }
            VersionApp entity = new VersionApp(model.Version,Convert.ToDateTime(model.EditDate),model.IsUpdate,model.DataVersion,model.Source);
            entity = await _repository.InsertAsync(entity, true, cancellationToken);
            return new Tuple<long, string>(entity.Id, "success");
        }

        public async Task<Tuple<bool, string>> EditVersionAppAsync(VersionAppChangeModel model, CancellationToken cancellationToken = default)
        {
            var obj = await _repository.FindAsync(model.Id.Value);
            if (obj == null)
            {
                return new Tuple<bool, string>(false, $"id={model.Id}的信息不存在");
            }
            if (model.Source.HasValue)
            {
                bool exists = await _repository.AnyAsync(x => x.Source == model.Source && x.IsDeleted == IsDeletedEnum.未删除 && x.Id != model.Id, cancellationToken);
                if (exists)
                {
                    return new Tuple<bool, string>(false, "类型不能重复");
                }
                obj.ChangeSource(model.Source.Value);
            }
            if (model.IsUpdate.HasValue)
            {
                obj.ChangeIsUpdate(model.IsUpdate.Value);
            }
            if (!string.IsNullOrEmpty(model.Version))
            {
                obj.ChangeVersion(model.Version);
            }
            obj.ChangeEditDate(DateTime.Now);
            obj = await _repository.UpdateAsync(obj, true, cancellationToken);
            return new Tuple<bool, string>(true, "success");
        }

        public async Task<VersionAppDTO> GetVersionAppByIdAsync(long id)
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

        public async Task<VersionAppDTO> GetVersionAppByPropAsync(VersionAppQueryModel queryModel, CancellationToken cancellationToken = default, params string[] param)
        {
            var exp = await CommonWhereBuilder.WhereBuilderToExp<VersionApp, VersionAppQueryModel>(queryModel, cancellationToken);//<SimCardInfo, SimCardInfoQueryModel>
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
    }
}
