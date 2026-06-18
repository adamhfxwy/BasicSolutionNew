
namespace Solution.Core.EnumAndConstent.Enums
{
    public class Enum
    {

        #region 状态枚举
        /// <summary>
        /// 员工状态 1-正常 2-离职
        /// </summary>
        public enum EmployeeStatusEnum
        {
            正常 = 1,
            离职 = 2,
        }
        /// <summary>
        /// 软删 1-未删除  2-已删除
        /// </summary>
        public enum IsDeletedEnum
        {
            未删除 = 1,
            已删除 = 2,
        }
        /// <summary>
        /// 水电表状态 1-未启用   2-正常  3-欠费停用  4-欠费提醒 (新增时无需传参)
        /// </summary>
        public enum MeterStatusEnum
        {
            未启用 = 1,
            正常 = 2,
            欠费提醒 = 3,
            欠费停用 = 4
        }
        /// <summary>
        /// 计费模板表状态  1-未生效  2-已生效
        /// </summary>
        public enum ChargingTemplateStatus
        {
            未生效 = 1,
            已生效 = 2
        }
        /// <summary>
        /// 在线状态   1-在线  2-离线
        /// </summary>
        public enum OnlineStatusEnum
        {
            在线 = 1,
            离线 = 2
        }
        /// <summary>
        /// 开关状态   1-关  2-开
        /// </summary>
        public enum SwitchStatusEnum
        {
            关 = 1,
            开 = 2
        }
        /// <summary>
        /// 工单状态 1-未处理 2-已处理
        /// </summary>
        public enum WorkOrderStatusEnum
        {
            未处理 = 1,
            已处理 = 2
        }
        /// <summary>
        /// 是否超时保养：1-否 2-是（无需传参）
        /// </summary>
        public enum IsTimeoutEnum
        {
            否 = 1,
            是 = 2
        }
        /// <summary>
        /// 状态： 成功-1:Ok  失败-2:Failed
        /// </summary>
        public enum ResponseStatus
        {
            Ok = 1,
            Failed = 2
        }
        /// <summary>
        /// 是否完成：1-未完成   2-已完成
        /// </summary>
        public enum IsFinishEnum
        {
            未完成 = 1,
            已完成 =2
        }
        #endregion

        #region 其他枚举业务性枚举
        /// <summary>
        /// 性别 1-男 2-女
        /// </summary>
        public enum SexEnum
        {
            男 = 1,
            女 = 2
        }
        /// <summary>
        /// 是否 1-否  2-是
        /// </summary>
        public enum YesOrNoEnum
        {
            否 = 1,
            是 = 2,
        }
        /// <summary>
        /// 登录权限 1-无权限 2-web权限  3-app权限 4-所有权限
        /// </summary>
        public enum LoginPermissionsEnum
        {
            无权限 = 1,
            web权限 = 2,
            app权限 = 3,
            所有权限 = 4,
        }

        /// <summary>
        /// 是否是按钮 1-否  2-是
        /// </summary>
        public enum IsButtonEnum
        {
            否 = 1,
            是 = 2,
        }
        /// <summary>
        /// 工单类型  1-维修类、2-安全隐患、3-卫生类
        /// </summary>
        public enum OrderTypeEnum
        {
            维修类 = 1,
            安全隐患 = 2,
            卫生类 = 3
        }
        /// <summary>
        /// 上报途径  1-巡检上报、2-用户上报、3-设备上报
        /// </summary>
        public enum ReportingChannelsEnum
        {
            巡检上报 = 1,
            用户上报 = 2,
            设备上报 = 3
        }
        /// <summary>
        /// 状态 1-空闲 2-占用
        /// </summary>
        public enum WorkstationStatusEnum
        {
            空闲 = 1,
            占用 = 2
        }

        public enum DoorEnum
        {
            东门 = 1,
            南门 = 2,
            西门 = 3,
            北门 = 4
        }
        /// <summary>
        /// 类型：1-app
        /// </summary>
        public enum VersionAppEnum
        {
            App = 1
        }
        #endregion

        #region web端权限匹配词
        /// <summary>
        /// web菜单管理权限匹配词
        /// </summary>
        public enum WebSystemMentMatching
        {
            SystemMent = 1,
            SystemMentadd = 2,
            SystemMentedit = 3,
            SystemMentdel = 4
        }       
        /// <summary>
        /// web部门管理权限匹配词
        /// </summary>
        public enum WebDepartmentMatching
        {
            DepartmentMent = 1,
            DepartmentMentadd = 2,
            DepartmentMentedit = 3,
            DepartmentMentdel = 4
        }
        /// <summary>
        /// web角色管理权限匹配词
        /// </summary>
        public enum WebRoleMentMatching
        {
            RoleMent = 1,
            RoleMentadd = 2,
            RoleMentedit = 3,
            RoleMentdel = 4
        }
        /// <summary>
        /// web员工管理权限匹配词
        /// </summary>
        public enum WebEmployeeMatching
        {
            EmployeeMent = 1,
            EmployeeMentadd = 2,
            EmployeeMentedit = 3,
            EmployeeMentdel = 4
        }
        /// <summary>
        /// Web字典管理权限匹配词
        /// </summary>
        public enum WebDicMentMatching
        {
            DicMent = 1,
            DicMentadd = 2,
            DicMentedit = 3,
            DicMentdel = 4
        }
        /// <summary>
        /// Web用户管理权限匹配词
        /// </summary>
        public enum WebUserMentMatching
        {
            UserMent = 1,
            UserMentadd = 2,
            UserMentedit = 3,
            UserMentdel = 4
        }
        /// <summary>
        /// Web操作日志管理权限匹配词
        /// </summary>
        public enum WebOperationLogMatching
        {
            OperationLog = 1
        }
        /// <summary>
        /// Web电表管理权限匹配词
        /// </summary>
        public enum WebElectricityMeterMatching
        {
            ElectricityMeterInfo = 1,
            ElectricityMeterInfoadd = 2,
            ElectricityMeterInfoedit = 3,
            ElectricityMeterInfodel = 4
        }
        /// <summary>
        /// Web电表缴费流水管理权限匹配词
        /// </summary>
        public enum WebElectricityMeterPaymentStatement
        {
            ElectricityPayment = 1,
            ElectricityPaymentadd = 2
        }
        /// <summary>
        /// Web电表运行流水管理权限匹配词
        /// </summary>
        public enum WebElectricityMeterRunStatement
        {
            ElectricityOperationFlow = 1
        }
        /// <summary>
        /// Web水表缴费流水管理权限匹配词
        /// </summary>
        public enum WebWaterMeterPaymentStatement
        {
            WaterPayment = 1,
            WaterPaymentadd = 2
        }
        /// <summary>
        /// Web水表管理权限匹配词
        /// </summary>
        public enum WebWaterMeterMatching
        {
            WaterMeterInfo = 1,
            WaterMeterInfoadd = 2,
            WaterMeterInfoedit = 3,
            WaterMeterInfodel = 4
        }
        /// <summary>
        /// Web电表运行流水管理权限匹配词
        /// </summary>
        public enum WebWaterMeterRunStatement
        {
            WaterOperationFlow = 1
        }
        /// <summary>
        /// Web计费模板权限匹配词
        /// </summary>
        public enum WebChargingTemplate
        {
            ChargingTemplate = 1,
            ChargingTemplateadd = 2,
            ChargingTemplateedit = 3,
            ChargingTemplatedel = 4
        }
        /// <summary>
        /// Web房间维护权限匹配词
        /// </summary>
        public enum WebRoomMaintenance
        {
            RoomMaintenance = 1,
            RoomMaintenanceadd = 2,
            RoomMaintenanceedit = 3,
            RoomMaintenancedel = 4
        }
        /// <summary>
        /// Web开关基础数据权限匹配词
        /// </summary>
        public enum WebSwitchData
        {
            SwitchData = 1,
            SwitchDataadd = 2,
            SwitchDataedit = 3,
            SwitchDatadel = 4
        }
        /// <summary>
        /// Web开关运行统计权限匹配词
        /// </summary>
        public enum WebSwtichOperationStatisc
        {
            SwtichOperationStatisc = 1,
            SwitchDataadd = 2,
            SwitchDataedit = 3,
            SwitchDatadel = 4
        }
        /// <summary>
        /// Web工单管理权限匹配词
        /// </summary>
        public enum WebWorkOrder
        {
            WorkOrderList = 1,
            WorkOrderListadd = 2,
            WorkOrderListedit = 3
        }
        /// <summary>
        /// Web房间维护权限匹配词
        /// </summary>
        public enum WebFloorMent
        {
            FloorMent = 1,
            FloorMentadd = 2,
            FloorMentedit = 3,
            FloorMentdel = 4
        }
        /// <summary>
        /// Web工位维护权限匹配词
        /// </summary>
        public enum WebWorkstationMent
        {
            WorkstationMent = 1,
            WorkstationMentadd = 2,
            WorkstationMentedit = 3,
            WorkstationMentdel = 4
        }
        /// <summary>
        /// Web员工、人员访问记录权限匹配词
        /// </summary>
        public enum WebVistorInoutRecord
        {
            VistorInoutRecord = 1,
            VistorInoutRecordadd = 2,
            VistorInoutRecordedit = 3,
            VistorInoutRecorddel = 4
        }
        /// <summary>
        /// Web车辆访问记录权限匹配词
        /// </summary>
        public enum WebVistorCarRecord
        {
            VistorCarRecord = 1,
            VistorCarRecordadd = 2,
            VistorCarRecordedit = 3,
            VistorCarRecorddel = 4
        }
        /// <summary>
        /// 电梯权限匹配词
        /// </summary>
        public enum WebElevator
        {
            Elevator = 1,
            Elevatoradd = 2,
            Elevatoredit = 3,
            Elevatordel = 4
        }
        /// <summary>
        /// 电梯权限匹配词
        /// </summary>
        public enum WebElevatorDynamicData
        {
            ElevatorDynamicData = 1,
            ElevatorDynamicDataadd = 2,
            ElevatorDynamicDataedit = 3,
            ElevatorDynamicDatadel = 4
        }
        /// <summary>
        /// Web巡检线路权限匹配词
        /// </summary>
        public enum WebInspectionLine
        {
            InspectionLine = 1,
            InspectionLineadd = 2,
            InspectionLineedit = 3,
            InspectionLinedel = 4
        }
        /// <summary>
        /// Web巡检点位权限匹配词
        /// </summary>
        public enum WebInspectionPoint
        {
            InspectionPoint = 1,
            InspectionPointadd = 2,
            InspectionPointedit = 3,
            InspectionPointdel = 4
        }
        /// <summary>
        /// Web巡检记录权限匹配词
        /// </summary>
        public enum WebInspectionRecords
        {
            InspectionRecords = 1,
            InspectionRecordsadd = 2,
            InspectionRecordsedit = 3,
            InspectionRecordsdel = 4
        }
        /// <summary>
        /// Web班次管理权限匹配词
        /// </summary>
        public enum WebShiftMent
        {
            ShiftMent = 1,
            ShiftMentadd = 2,
            ShiftMentedit = 3,
            ShiftMentdeldel1 = 4
        }
        /// <summary>
        /// Web空气质量权限匹配词
        /// </summary>
        public enum WebAirMent
        {
            AirMent = 1
        }
        /// <summary>
        /// Web设备保养维护管理权限匹配词
        /// </summary>
        public enum WebEquipmentWh
        {
            EquipmentWh = 1,
            EquipmentWhadd = 2,
            EquipmentWhedit = 3,
            EquipmentWhdel = 4
        }
        /// <summary>
        /// Web设备保养维护流水管理权限匹配词
        /// </summary>
        public enum WebEquipmentFlow
        {
            EquipmentFlow = 1,
            EquipmentFlowadd = 2,
        }
        /// <summary>
        /// Web物业费管理权限匹配词
        /// </summary>
        public enum WebWyfjfData
        {
            WyfjfData = 1,
            WyfjfDataadd = 2,
            WyfjfDataedit = 3,
            WyfjfDatadel = 4
        }
        #region 停车场相关
        /// <summary>
        /// 停车场权限匹配词
        /// </summary>
        public enum WebCarPark
        {
            CarPark = 1,
            CarParkadd = 2,
            CarParkedit = 3,
            CarParkdel = 4
        }
        /// <summary>
        /// 停车场车位权限匹配词
        /// </summary>
        public enum WebCarParkStall
        {
            CarParkStall = 1,
            CarParkStalladd = 2,
            CarParkStalledit = 3,
            CarParkStalldel = 4
        }
        /// <summary>
        /// 停车场摄像头权限匹配词
        /// </summary>
        public enum WebCarParkCamera
        {
            CarParkCamera = 1,
            CarParkCameraadd = 2,
            CarParkCameraedit = 3,
            CarParkCameradel = 4
        }
        /// <summary>
        /// 摄像头拍照记录权限匹配词
        /// </summary>
        public enum WebCarParkCameraRecord
        {
            CarParkCameraRecord = 1,
            CarParkCameraRecordadd = 2,
            CarParkCameraRecordedit = 3,
            CarParkCameraRecorddel = 4
        }
        #endregion

        #endregion
        #region  Mobile端权限匹配词
        /// <summary>
        /// 移动端巡检相关权限匹配词
        /// </summary>
        public enum MobileInspection
        {
            MobileInspectionPointadd = 1,
            MobileInspectionRecordsadd = 2,
            MobileInspectionRecord=3
        }
        /// <summary>
        /// 移动端工单相关权限匹配词
        /// </summary>
        public enum MobileWorkOrder
        {         
            WorkOrderAdd = 1,
            WorkOrderFinish = 2,
            WorkOrderSearch = 3
        }
        /// <summary>
        /// 移动端水电表相关权限匹配词
        /// </summary>
        public enum MobileElectricityAndWater
        {
            ElectricityAndWaterAdd = 1,
            ElectricityAndWaterSearch = 2
        }
        /// <summary>
        /// 移动端开关相关权限匹配词
        /// </summary>
        public enum MobileSwitchInfo
        {
            SwitchInfoAdd = 1,
            ChangeSwitchStatus = 2,
            SwitchInfoSearch = 3
        }
        /// <summary>
        /// 移动端设备维护相关权限匹配词
        /// </summary>
        public enum MobileDeviceUpkeep
        {
            EquipmentWh = 1,
            EquipmentWhadd = 2,
            EquipmentWhedit = 3,
            EquipmentWhdel = 4
        }

        /// <summary>
        /// 停车场摄像头权限匹配词
        /// </summary>
        public enum MobileCarParkCamera
        {
            CarParkCamera = 1,
            CarParkCameraadd = 2,
            CarParkCameraedit = 3,
            CarParkCameradel = 4
        }
        /// <summary>
        /// 移动端物业费管理权限匹配词
        /// </summary>
        public enum MobileWyfjfData
        {
            WyfjfData = 1,
            WyfjfDataadd = 2,
            WyfjfDataedit = 3,
            WyfjfDatadel = 4
        }
        #endregion
    }
}
