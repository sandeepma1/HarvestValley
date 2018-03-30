﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        protected List<string> GetAllLinesFromCSV(string fileName)
        {
            TextAsset itemCSV = Resources.Load("CSVs/" + fileName) as TextAsset;
            List<string> linesList = Regex.Split(itemCSV.text, "\r\n").ToList<string>();
            linesList.RemoveAt(0); // Remove first item as CSV has column names
            linesList.RemoveAt(linesList.Count - 1); // Warning: Remove last item as CSV one blank line at the end
            return linesList;
        }

        protected float FloatParse(string text)
        {
            float result = 0.01f;
            float.TryParse(text, out result);
            return result;
        }
    }
}