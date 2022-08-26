using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWork
{
    public class MathHelper
    {
        /// <summary>
        /// 弧度转角度
        /// </summary>
        /// <param name="rad"></param>
        /// <returns></returns>
        public static double RadToDeg(double rad)
        {
            double Deg = rad * 180/Math.PI;
            return Deg;
        }
        /// <summary>
        /// 角度转弧度
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        public static double DegToRad(double deg)
        {
            double Rad = deg *  Math.PI/180;
            return Rad;
        }


    }
}
