
using LanTian.Solution.Core.DTO.DeviceMaintain;
using LanTian.Solution.Core.ParameterModel.ChangeModel.DeviceMaintain;
using LanTian.Solution.Core.ParameterModel.QueryModel.DeviceMaintain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PropertyManagementMobileAPI.Controllers
{
    /// <summary>
    /// 设备维护相关
    /// </summary>
    [Authorize]
    [Route("propertyMgtWeb/[controller]/[action]")]
    [ApiExplorerSettings(GroupName = "DeviceMaintain")]
    [ApiController]
    public class DeviceUpkeepStatementController : ControllerBase
    {
        private readonly DeviceUpkeepStatementApplication _deviceUpkeepStatementApplication;
        private readonly DeviceUpkeepApplication _deviceUpkeepApplication;

        public DeviceUpkeepStatementController(DeviceUpkeepStatementApplication deviceUpkeepStatementApplication, DeviceUpkeepApplication deviceUpkeepApplication)
        {
            _deviceUpkeepStatementApplication = deviceUpkeepStatementApplication;
            _deviceUpkeepApplication = deviceUpkeepApplication;
        }
        /// <summary>
        /// 设备保养维护流水上报
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission(nameof(MobileDeviceUpkeep.DeviceUpkeepRecordAdd))]
        public async Task<ActionResult> AddDeviceUpkeepStatementAsync(DeviceUpkeepStatementChangeModel model, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(model.DeviceNumber))
            {
                return Ok(new
                {
                    Status = "Failed",
                    Msg = "设备编号不可为空"
                });
            }
            if (string.IsNullOrEmpty(model.DeviceType))
            {
                return Ok(new
                {
                    Status = "Failed",
                    Msg = "设备类型不可为空"
                });
            }
            var deviceUpkeep = await _deviceUpkeepApplication.GetDeviceUpkeepByPropAsync(new DeviceUpkeepQueryModel { DeviceNumber = model.DeviceNumber, DeviceType = model.DeviceType });
            if (deviceUpkeep == null)
            {
                return Ok(new
                {
                    Status = "Failed",
                    Msg = $"设备编号={model.DeviceNumber}，设备类型={model.DeviceType}的设为维护数据不存在。"
                });
            }
            model.RealityUpkeepTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            model.UpkeepCycle = deviceUpkeep.UpkeepCycle;
            model.ThisUpkeepTime = deviceUpkeep.UpkeepLastDate;
            model.EmployeeId = Convert.ToInt64(User.FindFirst("UserId").Value);
            model.EmployeeName = User.FindFirst("RealName").Value;
            if (deviceUpkeep.UpkeepLastDate.HasValue)
            {
                if (Convert.ToDateTime(model.RealityUpkeepTime) > deviceUpkeep.UpkeepLastDate)
                {
                    model.IsTimeout = IsTimeoutEnum.是;
                }
                else
                {
                    model.IsTimeout = IsTimeoutEnum.否;
                }
            }
            else
            {
                model.IsTimeout = IsTimeoutEnum.否;
            }
            var tuple = await _deviceUpkeepStatementApplication.AddDeviceUpkeepStatementAsync(model, cancellationToken);
            return Ok(new
            {
                Status = tuple.Item1 != null ? "Ok" : "Failed",
                Id = tuple.Item1,
                Msg = tuple.Item2
            });
        }
        /// <summary>
        /// 设备保养维护流水列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [CheckPermission(nameof(MobileDeviceUpkeep.DeviceUpkeepSearch))]
        [SwaggerResponse(0, "返回数据属性注释", typeof(DeviceUpkeepStatementDTO))]
        public async Task<ActionResult> GetDeviceUpkeepStatementAsync(DeviceUpkeepStatementQueryModel query, CancellationToken cancellationToken = default)
        {
            query.OrderBy = "createTime";
            query.IsDescending = true;
            var tuple = await _deviceUpkeepStatementApplication.GetDeviceUpkeepStatementAsync(query, cancellationToken);
            return Ok(new
            {
                Status = "Ok",
                Data = tuple.List,
                TotalCount = tuple.Total
            });
        }
        /// <summary>
        /// 设备保养静态数据列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(0, "返回数据属性注释", typeof(DeviceUpkeepDTO))]
        public async Task<ActionResult> GetDeviceUpkeepAsync(DeviceUpkeepQueryModel query, CancellationToken cancellationToken = default)
        {
            query.OrderBy = "createTime";
            query.IsDescending = true;
            var tuple = await _deviceUpkeepApplication.GetDeviceUpkeepAsync(query, cancellationToken);
            return Ok(new
            {
                Status = "Ok",
                Data = tuple.List,
                TotalCount = tuple.Total
            });
        }
    }
}
