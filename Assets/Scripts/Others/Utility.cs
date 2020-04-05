using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyUtils
{
    public class Utility
    {
        public static Vector3 GetVectorFromAngle(float angle)
        {
            // angle 0 -> 360
            float angleRad = angle * (Mathf.PI / 180f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        public static float GetAngleFromVectorFloat(Vector3 dir)
        {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;

            return n;
        }

       
    }

    public class VectorConversion
    {
        public static Vector3Int V3IntFromV2Int(Vector2Int v2Int)
        {
            Vector3Int pos = new Vector3Int(v2Int.x, v2Int.y, 0);
            return pos;
        }

    }
}

