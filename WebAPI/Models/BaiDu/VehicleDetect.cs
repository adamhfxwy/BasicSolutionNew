namespace WebAPI.Models.BaiDu
{
    public class VehicleDetect
    {
        public VehicleNum vehicle_num { get; set; }
        public List<VehicleInfo> vehicle_info { get; set; }
    }

    public class VehicleNum
    {
        public int motorbike { get; set; }
        public int tricycle { get; set; } 
        public int car { get; set; }
        public int carplate { get; set; }
        public int truck { get; set; }
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
