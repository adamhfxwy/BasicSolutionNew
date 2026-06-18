

using Solution.Core.Domain.INpgSqlService;
using Solution.Core.Infrastructure.Utils;
using Solution.Core.ParameterModel.ChangeModel.Common;
using Solution.Core.ParameterModel.QueryModel.Common;
using Solution.Core.ParameterModel.QueryModel.Identity;

namespace Solution.Core.Infrastructure.NpgSqlService
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IRepository<UserInfo> _repository;
        public UserInfoService(IRepository<UserInfo> repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tuple<long, string>> AddUserInfoAsync(AddEditUserInfoModel model, CancellationToken cancellationToken = default)
        {
            //if (string.IsNullOrEmpty(model.))
            //{
            //    return new Tuple<UserInfoDTO, string>(null, "部门名称不可为空");
            //}
            bool exists = await _repository.AnyAsync(x => x.Cellphone == model.Cellphone && x.IsDeleted == IsDeletedEnum.未删除, cancellationToken);
            if (exists)
            {
                return new Tuple<long, string>(0, "用户联系方式不能重复");
            }
            UserInfo entity = new UserInfo(model.Name, model.Cellphone, model.Address);

            entity = await _repository.InsertAsync(entity, true, cancellationToken);
            return new Tuple<long, string>(entity.Id, "success");
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> EditUserInfoAsync(AddEditUserInfoModel model, CancellationToken cancellationToken = default)
        {
            var obj = await _repository.FindAsync(model.Id.Value);
            if (obj == null)
            {
                return new Tuple<bool, string>(false, $"id={model.Id}的用户不存在");
            }

            if (!string.IsNullOrEmpty(model.Cellphone))
            {
                bool exists = await _repository.AnyAsync(x => x.Cellphone == model.Cellphone && x.IsDeleted == IsDeletedEnum.未删除 && x.Id != model.Id, cancellationToken);
                if (exists)
                {
                    return new Tuple<bool, string>(false, "用户联系方式不能重复");
                }
                obj.ChangeCellphone(model.Cellphone);
            }
            if (!string.IsNullOrEmpty(model.Name))
            {

                obj.ChangeName(model.Name);
            }
            if (!string.IsNullOrEmpty(model.Address))
            {

                obj.ChangeAddress(model.Address);
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
        public async Task<Tuple<bool, string>> RemoveUserInfoAsync(long id, CancellationToken cancellationToken = default)
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
        public async Task<Pagination<UserInfoDTO>> GetUserInfoAsync(UserInfoQueryModel queryModel, CancellationToken cancellationToken = default, params string[] param)
        {

            var exp = await CommonWhereBuilder.WhereBuilderToExp<UserInfo, UserInfoQueryModel>(queryModel, cancellationToken);//<SimCardInfo, SimCardInfoQueryModel>        
            var list = await _repository.GetListOfWithIncludDataAsync(queryModel, exp, null, cancellationToken, true);
            var dto = list.List.Select(ToDTOUtils.ToDTO).ToList();
            return new Pagination<UserInfoDTO> { List = dto, Total = list.Total, Code = 1 };
        }
        /// <summary>
        /// 根据条件获取一条数据
        /// </summary>
        /// <param name="queryModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<UserInfoDTO> GetUserInfoByPropAsync(UserInfoQueryModel queryModel, CancellationToken cancellationToken = default, params string[] param)
        {
            var exp = await CommonWhereBuilder.WhereBuilderToExp<UserInfo, UserInfoQueryModel>(queryModel, cancellationToken);//<SimCardInfo, SimCardInfoQueryModel>
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
        public async Task<UserInfoDTO> GetUserInfoByIdAsync(long id)
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
        public async Task<bool> UserInfoAnyAsync(UserInfoQueryModel queryModel, CancellationToken cancellationToken = default)
        {
            var exp = await CommonWhereBuilder.WhereBuilderToExp<UserInfo, UserInfoQueryModel>(queryModel, cancellationToken);
            return await _repository.AnyAsync(exp, cancellationToken);
        }
    }
}
