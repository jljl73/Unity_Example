using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UK.Util
{
    public class UtilFunc
    {
        public static bool CheckRange(Vector3 pos1,  Vector3 pos2, float range)
        {
            if (Vector3.Distance(pos1, pos2) < range)
                return true;
            return false;
        }
    }
}
