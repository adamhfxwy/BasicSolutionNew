namespace Solution.Core.customWebAPI.Models
{
    public class VehicleDetectDTO
    {
        public VehicleNum vehicle_num { get; set; }
        public List<VehicleInfo> vehicle_info { get; set; }
    }

    public class VehicleNum
    {
        /// <summary>
        /// 摩托车
        /// </summary>
        public int motorbike { get; set; }
        /// <summary>
        /// 三轮车
        /// </summary>
        public int tricycle { get; set; } 
        /// <summary>
        /// 轿车
        /// </summary>
        public int car { get; set; }
        /// <summary>
        /// 车牌
        /// </summary>
        public int carplate { get; set; }
        /// <summary>
        /// 卡车
        /// </summary>
        public int truck { get; set; }
        /// <summary>
        /// 巴士
        /// </summary>
        public int bus { get; set; }
    }
    public class VehicleInfo
    {
        public string type { get; set; }
        public VehicleLocation location { get; set; }
        public decimal probability { get; set; }
    }
    public class VehicleLocation
    {
        public int width { get; set; }
        public int top { get; set; }
        public int left { get; set; }
        public int height { get; set; }

    }
}
