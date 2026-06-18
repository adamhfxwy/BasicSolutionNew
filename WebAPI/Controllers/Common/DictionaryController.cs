



using Solution.Core.EnumAndConstent;
using System;
using System.Collections.Generic;

namespace WebAPI.Models.Controllers.Common
{
    /// <summary>
    /// 字典相关
    /// </summary>
    [Authorize]
    [Route("customWeb/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "Common")]
    [ApiController]
    public class DictionaryController : ControllerBase
    {
        private readonly IDictionaryService _dictionaryService;

        public DictionaryController(IDictionaryService dictionaryService)
        {
            _dictionaryService = dictionaryService;
        }

        
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody))]
        [CheckPermission(nameof(WebDicMentMatching.DicMentadd))]
        public async Task<ActionResult> AddDictionaryAsync(AddEditDictionaryModel model, CancellationToken cancellationToken = default)
        {
                                                         
            if (string.IsNullOrEmpty(model.Key))
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"键不可为空"));
            }
            if (model.IsCreateType.HasValue && !model.IsCreateType.Value && string.IsNullOrEmpty(model.Value))
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"值不可为空"));
            }
            if (model.IsCreateType.HasValue && model.IsCreateType.Value)
            {
                model.Value = "CreateType";
            }
            if (!model.Type.HasValue)
            {
                model.Type = 0;
            }
           
            //if (!model.Type.HasValue)
            //{
            //    return Ok(new
            //    {
            //        Status = "Failed",
            //        Msg = "类型不可为空"
            //    });
            //}
            var tuple = await _dictionaryService.AddDictionaryAsync(model, cancellationToken);
            return Ok(new ResponseBody(tuple.Item1 > 0? ResponseStatus.Ok.ToString() : ResponseStatus.Failed.ToString(), tuple.Item2));       
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody))]
        [CheckPermission(nameof(WebDicMentMatching.DicMentedit))]
        public async Task<ActionResult> EditDictionaryAsync(AddEditDictionaryModel model, CancellationToken cancellationToken = default)
        {
            if (!model.Id.HasValue || model.Id.Value <= 0)
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"id不可为空"));
            }
            var tuple = await _dictionaryService.EditDictionaryAsync(model, cancellationToken);
            return Ok(new ResponseBody(tuple.Item1 ? ResponseStatus.Ok.ToString() : ResponseStatus.Failed.ToString(), tuple.Item2));
        }
        /// <summary>
        /// 根据条件查询字典列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody<DictionaryDTO>))]
        public async Task<ActionResult> GetDictionaryAsync(DictionaryQueryModel query, CancellationToken cancellationToken = default)
        {
            query.OrderBy = "createTime";
            query.IsDescending = true;
            var tuple = await _dictionaryService.GetDictionaryAsync(query, cancellationToken);
            var typeTuple = await _dictionaryService.GetDictionaryTypeAsync(new DictionaryQueryModel { Type = 0, Value= "CreateType" }, cancellationToken) ;
            var list = tuple.List.Select(x=>new DictionaryDTO 
            {
                CreateTime=x.CreateTime,
                Description = x.Description,
                Id = x.Id,
                Key = x.Key,
                Type = x.Type,
                Value = x.Value,
                TypeName= typeTuple.List.FirstOrDefault(i=>i.Id==x.Type)?.Key?? "无类型"
            }).ToArray();
            return Ok(new ResponseBody<DictionaryDTO[]>(ResponseStatus.Ok.ToString(), "Success", list, tuple.Total));           
        }
        /// <summary>
        /// 根据类型查询字典列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody<DictionaryDTO>))]
        public async Task<ActionResult> GetDictionaryByTypeNameAsync(string typeName, CancellationToken cancellationToken = default)
        {
            var tuple = await _dictionaryService.GetDictionaryByTypeNameAsync(typeName, cancellationToken);
            var list = tuple.Item1.Select(x => new DictionaryDTO
            {
                CreateTime = x.CreateTime,
                Description = x.Description,
                Id = x.Id,
                Key = x.Key,
                Type = x.Type,
                Value = x.Value
            }).ToArray();
            return Ok(new ResponseBody<DictionaryDTO[]>(ResponseStatus.Ok.ToString(), "Success", list, list.Count()));
        }
        /// <summary>
        /// 根据条件查询字典类型列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody<DictionaryDTO>))]
        public async Task<ActionResult> GetDictionaryTypeAsync(DictionaryQueryModel query, CancellationToken cancellationToken = default)
        {
            query.OrderBy = "createTime";
            query.IsDescending = true;
            query.Type = 0;
            query.Value = "CreateType";
            var tuple = await _dictionaryService.GetDictionaryTypeAsync(query, cancellationToken);
            tuple.List.Insert(0,new DictionaryDTO {Id=0,Key="无类型",Type=0});
            return Ok(new ResponseBody<DictionaryDTO[]>(ResponseStatus.Ok.ToString(), "Success", tuple.List.ToArray(), tuple.Total));
        }
        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody<DictionaryDTO>))]
        public async Task<ActionResult> GetDictionaryByIdAsync(GetByIdModel model)
        {
            if (model.Id <= 0)
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"id不可为空"));           
            }
            var obj = await _dictionaryService.GetDictionaryByIdAsync(model.Id);
            return Ok(new ResponseBody<DictionaryDTO>(ResponseStatus.Ok.ToString(), "Success", obj, 1));
        }
        /// <summary>
        /// 根据类型的key获取该类型下的字典数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody<Pagination<DictionaryDTO>>))]
        public async Task<ActionResult> GetDictionaryByKeyAsync(string key, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(key))
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"key不能为空"));             
            }

            var obj = await _dictionaryService.GetDictionaryByPropAsync(new DictionaryQueryModel {Key=key}, cancellationToken);
            if (obj==null)
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"key={key}的字典不存在"));
            }
            var list= await _dictionaryService.GetDictionaryAsync(new DictionaryQueryModel { Type=(int)obj.Id}, cancellationToken);
            return Ok(new ResponseBody<Pagination<DictionaryDTO>>(ResponseStatus.Ok.ToString(), "Success", list, list.Total));
        }
        /// <summary>
        /// 根据字段获取一条数据
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody<DictionaryDTO>))]
        public async Task<ActionResult> GetDictionaryByPropsAsync(DictionaryQueryModel query, CancellationToken cancellationToken = default)
        {
            var obj = await _dictionaryService.GetDictionaryByPropAsync(query, cancellationToken);
            return Ok(new ResponseBody<DictionaryDTO>(ResponseStatus.Ok.ToString(), "Success", obj, 1));
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission(nameof(WebDicMentMatching.DicMentdel))]
        [SwaggerResponse(0, "返回数据属性注释", typeof(ResponseBody))]
        public async Task<ActionResult> RemoveDictionaryAsync(GetByIdModel model, CancellationToken cancellationToken = default)
        {
            if (model.Id <= 0)
            {
                return Ok(new ResponseBody(ResponseStatus.Failed.ToString(), $"请选择Id"));             
            }

            var tuple = await _dictionaryService.RemoveDictionaryAsync(model.Id, cancellationToken);
            return Ok(new ResponseBody(tuple.Item1 ? ResponseStatus.Ok.ToString() : ResponseStatus.Failed.ToString(), tuple.Item2));         
        }
    }
}
