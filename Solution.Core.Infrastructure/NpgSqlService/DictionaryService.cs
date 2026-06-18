

using Solution.Core.Infrastructure.Utils;
using Solution.Core.ParameterModel.ChangeModel.Common;
using Solution.Core.ParameterModel.QueryModel.Common;

namespace Solution.Core.Infrastructure.NpgSqlService
{
    public class DictionaryService : IDictionaryService
    {
        private readonly IRepository<Dictionary> _repository;
        public DictionaryService(IRepository<Dictionary> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tuple<long, string>> AddDictionaryAsync(AddEditDictionaryModel model, CancellationToken cancellationToken = default)
        {
            bool exists = await _repository.AnyAsync(x => x.Key == model.Key && x.IsDeleted == IsDeletedEnum.未删除, cancellationToken);
            if (exists)
            {
                return new Tuple<long, string>(0, "Key不能重复");
            }

            Dictionary entity = new Dictionary(model.Key, model.Value, model.Description, model.Type.Value);

            entity = await _repository.InsertAsync(entity, true, cancellationToken);
            return new Tuple<long, string>(entity.Id, "success");
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> EditDictionaryAsync(AddEditDictionaryModel model, CancellationToken cancellationToken = default)
        {
            var obj = await _repository.FindAsync(model.Id.Value);
            if (obj == null)
            {
                return new Tuple<bool, string>(false, $"id={model.Id}的字典不存在");
            }


            if (!string.IsNullOrEmpty(model.Value))
            {

                obj.ChangeValue(model.Value);
            }
            if (!string.IsNullOrEmpty(model.Description))
            {

                obj.ChangeDescription(model.Description);
            }
            if (obj.Value != "CreateType" && model.Value == "CreateType")
            {
                return new Tuple<bool, string>(false, $"id={model.Id}的字典不能编辑为类型");
            }
            //if (model.Type.HasValue)
            //{
            //    obj.ChangeType(model.Type.Value);
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
        public async Task<Tuple<bool, string>> RemoveDictionaryAsync(long id, CancellationToken cancellationToken = default)
        {
            var obj = await _repository.FindAsync(id);
            if (obj == null || obj.IsDeleted == IsDeletedEnum.已删除)
            {
                return new Tuple<bool, string>(false, $"id={id}的字典不存在");
            }
            bool exists = await _repository.AnyAsync(x => x.IsDeleted == IsDeletedEnum.未删除 && x.Type == obj.Id, cancellationToken);
            if (exists)
            {
                return new Tuple<bool, string>(false, $"id={id}的字典有子集引用，无法删除");
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
        public async Task<Pagination<DictionaryDTO>> GetDictionaryAsync(DictionaryQueryModel queryModel, CancellationToken cancellationToken = default, params string[] param)
        {
            Expression<Func<Dictionary, bool>> exp = await CommonWhereBuilder.WhereBuilderToExp<Dictionary, DictionaryQueryModel>(queryModel, cancellationToken);
            exp = exp.And(x => x.Value != "CreateType");
            var list = await _repository.GetListOfWithIncludDataAsync(queryModel, exp, null, cancellationToken, true);
            var dto = list.List.Select(ToDTOUtils.ToDTO).ToList();
            return new Pagination<DictionaryDTO> { List = dto, Total = list.Total, Code = 1 };
        }

        /// <summary>
        /// 根据条件获取分页数据
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<Tuple<List<DictionaryDTO>, string>> GetDictionaryByTypeNameAsync(string typeName, CancellationToken cancellationToken = default, params string[] param)
        {
            var dicType = await GetDictionaryByPropAsync(new DictionaryQueryModel { Key = typeName });
            if (dicType == null)
            {
                return new Tuple<List<DictionaryDTO>, string>(null, $"key={typeName}的字典类型不存在");
            }
            var res = await _repository.GetAllAsync().Where(x => x.Type == dicType.Id).ToListAsync();
            return new Tuple<List<DictionaryDTO>, string>(res.Select(ToDTOUtils.ToDTO).ToList(), "success");
        }

        /// <summary>
        /// 根据条件获取类型数据
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<Pagination<DictionaryDTO>> GetDictionaryTypeAsync(DictionaryQueryModel queryModel, CancellationToken cancellationToken = default, params string[] param)
        {
            var exp = await CommonWhereBuilder.WhereBuilderToExp<Dictionary, DictionaryQueryModel>(queryModel, cancellationToken);//<SimCardInfo, SimCardInfoQueryModel>                                                                                                                                    //     
            var list = await _repository.GetListOfWithIncludDataAsync(queryModel, exp, null, cancellationToken, true);
            var dto = list.List.Select(ToDTOUtils.ToDTO).ToList();
            return new Pagination<DictionaryDTO> { List = dto, Total = list.Total, Code = 1 };
        }
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DictionaryDTO> GetDictionaryByPropAsync(DictionaryQueryModel queryModel, CancellationToken cancellationToken = default, params string[] param)
        {
            var exp = await CommonWhereBuilder.WhereBuilderToExp<Dictionary, DictionaryQueryModel>(queryModel, cancellationToken);//<SimCardInfo, SimCardInfoQueryModel>
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
        public async Task<DictionaryDTO> GetDictionaryByIdAsync(long id)
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
        public async Task<bool> AnyAsync(string key, CancellationToken cancellationToken)
        {
            return await _repository.AnyAsync(x => x.Key == key, cancellationToken);
        }
    }
}
