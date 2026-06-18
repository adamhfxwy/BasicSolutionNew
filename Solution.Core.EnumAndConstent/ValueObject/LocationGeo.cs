

namespace Solution.Core.EnumAndConstent.ValueObject
{
    public class LocationGeo
    {
        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; set; } = null!;
        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude { get; set; } = null!;
        private LocationGeo()
        {

        }
        public LocationGeo(string latitude, string longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
        public void ChangeLatitude(string latitude)
        {
            this.Latitude = latitude;
        }
        public void ChangeLongitude(string longitude)
        {
            this.Longitude = longitude;
        }
    }
}
