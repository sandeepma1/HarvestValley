using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HarvestValley.IO
{
    public abstract class DatabaseBase<T> : Singleton<T> where T : DatabaseBase<T>
    {
        protected int IntParse(string text)
        {
            int num;
            if (int.TryParse(text, out num))
            {
                return num;
            }
            else
            {
                return 0;
            }
        }

        protected float FloatParse(string text)
        {
            float result = 0.01f;
            float.TryParse(text, out result);
            return result;
        }
    }
}
