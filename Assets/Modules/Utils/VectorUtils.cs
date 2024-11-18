using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UtilsModule
{
    public static class VectorUtils
    {
        /// <summary>
        /// returns true if the signs of the x and z values of the vectors are different
        /// </summary>
        public static bool CompareVectorSigns(Vector3 vectorA, Vector3 vectorB)
        {
            bool xDiff = false;
            
            bool zDiff = false;

            if ( vectorA.x >0 && vectorB.x < 0 || vectorA.x < 0 && vectorB.x > 0)
            {
                xDiff = true;
            }
            if (vectorA.z > 0 && vectorB.z < 0 || vectorA.z < 0 && vectorB.z > 0)
            {
                zDiff = true;
            }

            if (xDiff && zDiff)
            {
                return true;
            }
            return false;
            
        }
    }
}

