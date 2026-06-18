using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Core.CommonHelper
{
    /// <summary>
    /// 通过地图上的两个坐标计算距离
    /// </summary>
    public class MapHelper
    {
        /// <summary>
        /// 地球半径
        /// </summary>
        private const double EarthRadius = 6378.137;


        /// <summary>
        /// 经纬度转化成弧度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private static double Rad(double d)
        {
            return d * Math.PI / 180d;
        }


        /// <summary>
        /// 计算两个坐标点之间的距离
        /// </summary>
        /// <param name="firstLatitude">第一个坐标的纬度</param>
        /// <param name="firstLongitude">第一个坐标的经度</param>
        /// <param name="secondLatitude">第二个坐标的纬度</param>
        /// <param name="secondLongitude">第二个坐标的经度</param>
        /// <returns>返回两点之间的距离，单位：公里/千米</returns>
        public static double GetDistance(double firstLatitude, double firstLongitude, double secondLatitude, double secondLongitude)
        {
            var firstRadLat = Rad(firstLatitude);
            var firstRadLng = Rad(firstLongitude);
            var secondRadLat = Rad(secondLatitude);
            var secondRadLng = Rad(secondLongitude);


            var a = firstRadLat - secondRadLat;
            var b = firstRadLng - secondRadLng;
            var cal = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(firstRadLat)
                * Math.Cos(secondRadLat) * Math.Pow(Math.Sin(b / 2), 2))) * EarthRadius;
            var result = Math.Round(cal * 10000) / 10000;
            return result;
        }


        /// <summary>
        /// 计算两个坐标点之间的距离
        /// </summary>
        /// <param name="firstPoint">第一个坐标点的（纬度,经度）</param>
        /// <param name="secondPoint">第二个坐标点的（纬度,经度）</param>
        /// <returns>返回两点之间的距离，单位：公里/千米</returns>
        public static double GetPointDistance(string firstPoint, string secondPoint)
        {
            var firstArray = firstPoint.Split(',');
            var secondArray = secondPoint.Split(',');
            var firstLatitude = Convert.ToDouble(firstArray[0].Trim());
            var firstLongitude = Convert.ToDouble(firstArray[1].Trim());
            var secondLatitude = Convert.ToDouble(secondArray[0].Trim());
            var secondLongitude = Convert.ToDouble(secondArray[1].Trim());
            return GetDistance(firstLatitude, firstLongitude, secondLatitude, secondLongitude);
        }

        /// <summary>
        /// 围栏计算(点是否在围栏内)
        /// </summary>
        /// <param name="latlon">单点坐标</param>
        /// <param name="APoints">坐标集合</param>
        /// <returns></returns>
        public static bool MBR(Gps latlon, List<Gps> APoints)
        {
            if (MBR_zerOne(latlon, APoints))
            {
                return IsPtInPoly(latlon, APoints);//内判断
            }
            else
            {
                return false;//外判断
            }
        }
        /// <summary>
        /// 最小外包法
        /// </summary>
        /// <param name="latlon"></param>
        /// <param name="APoints"></param>
        /// <returns></returns>
        private static bool MBR_zerOne(Gps latlon, List<Gps> APoints)
        {
            double max_lon = APoints.Max(x => x.getWgLon());
            double max_lat = APoints.Max(x => x.getWgLat());
            double min_lon = APoints.Min(x => x.getWgLon());
            double min_lat = APoints.Min(x => x.getWgLat());
            double aLon = latlon.getWgLon();
            double aLat = latlon.getWgLat();
            if (aLon >= max_lon || aLon <= min_lon || aLat >= max_lat || aLat <= min_lat)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 点在围栏内（数学计算方法）
        /// </summary>
        /// <param name="latlon">Gps坐标</param>
        /// <param name="APoints">Gps坐标点集合</param>
        /// <returns></returns>
        private static bool IsPtInPoly(Gps latlon, List<Gps> APoints)
        {
            int iSum = 0, iCount;
            double dLon1, dLon2, dLat1, dLat2, dLon;
            double ALat = latlon.getWgLat();
            double ALon = latlon.getWgLon();
            if (APoints.Count < 3)
                return false;
            iCount = APoints.Count;
            for (int i = 0; i < iCount; i++)
            {
                if (i == iCount - 1)
                {
                    dLon1 = APoints[i].getWgLon();
                    dLat1 = APoints[i].getWgLat();
                    dLon2 = APoints[0].getWgLon();
                    dLat2 = APoints[0].getWgLat();
                }
                else
                {
                    dLon1 = APoints[i].getWgLon();
                    dLat1 = APoints[i].getWgLat();
                    dLon2 = APoints[i + 1].getWgLon();
                    dLat2 = APoints[i + 1].getWgLat();
                }
                //以下语句判断A点是否在边的两端点的水平平行线之间，在则可能有交点，开始判断交点是否在左射线上
                if (((ALat >= dLat1) && (ALat < dLat2)) || ((ALat >= dLat2) && (ALat < dLat1)))
                {
                    if (Math.Abs(dLat1 - dLat2) > 0)
                    {
                        //得到 A点向左射线与边的交点的x坐标：
                        dLon = dLon1 - ((dLon1 - dLon2) * (dLat1 - ALat)) / (dLat1 - dLat2);

                        // 如果交点在A点左侧（说明是做射线与 边的交点），则射线与边的全部交点数加一：
                        if (dLon < ALon)
                            iSum++;
                    }
                }
            }
            if (iSum % 2 != 0)
                return true;
            return false;
        }
    }
}
