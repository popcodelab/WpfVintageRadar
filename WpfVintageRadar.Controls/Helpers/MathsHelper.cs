using System;

namespace WpfVintageRadar.Controls.Helpers
{
    /// <summary>
    /// Trigonometric functions
    /// </summary>
    internal class MathsHelper
    {

        //TODO Remove this class if not used

        /// <summary>
        /// Convert radians to degrees
        /// </summary>
        /// <param name="radians">Radians value to convert</param>
        /// <returns>Degrees value</returns>
        public static double ConvertRadiansToDegrees(double radians)
        {
            double degrees = (180 / Math.PI) * radians;
            return (degrees);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="degrees">Degrees value to convert</param>
        /// <returns>Radians value</returns>
        public static double ConvertDegreesToRadians(double degrees)
        {
            double radians = (Math.PI / 180) * degrees;
            return (radians);
        }

        
    }
}
