using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ShaderSupport
{
    public static partial class ShaderSupport
    {
        /// <summary>
        /// LDR Color と intensity → HDR Color に変換する
        /// </summary>
        /// <param name="ldrColor">LDR Color</param>
        /// <param name="intensity">輝度(intensity)</param>
        /// <returns>HDR Color</returns>
        public static Color ToHDRColor(this Color ldrColor, float intensity)
        {
            var factor = Mathf.Pow(2, intensity);
            return new Color(
                ldrColor.r * factor,
                ldrColor.g * factor,
                ldrColor.b * factor,
                ldrColor.a
            );
        }
    }
}

