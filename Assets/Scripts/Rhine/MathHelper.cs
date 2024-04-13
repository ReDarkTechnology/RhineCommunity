using System;

namespace Rhine
{
    public static class MathHelper
    {
        /// <summary>
        /// Does an epsilon comparison where 
        /// it doesn't care much about differences 
        /// that great in detail
        /// </summary>
        public static bool Epsilon(
            this float number, 
            float another, 
            float range = 0.0001f) =>
            Math.Abs(number - another) < range;
    }
}
