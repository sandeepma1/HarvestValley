using UnityEngine;
using System.Collections;
using System;
using System.Text.RegularExpressions;

public class MyCSVReader : MonoBehaviour
{
	public static MyCSVReader m_instance = null;
	string[] lines = new string[100];
	string[] chars = new string[100];

	void Awake ()
	{
		m_instance = this;
		//Initialize ();
	}

	public void Initialize (string fileName)
	{		
		TextAsset itemCSV =	Resources.Load ("CSVs/" + fileName) as TextAsset;
		lines = Regex.Split (itemCSV.text, "\r\n");
		Item[] itemz = new Item[lines.Length - 1];
		for (int i = 1; i < 5; i++) {			
			chars = Regex.Split (lines [i], ",");
			itemz [i - 1] = new Item (IntParse (chars [0]), chars [1], chars [2], IntParse (chars [3]), IntParse (chars [4]), 
				IntParse (chars [5]), (ItemType)Enum.Parse (typeof(ItemType), chars [6]), IntParse (chars [7]), (ItemSource)Enum.Parse (typeof(ItemSource), chars [8]), IntParse (chars [9]), IntParse (chars [10]),
				IntParse (chars [11]), IntParse (chars [12]), IntParse (chars [13]), IntParse (chars [14]), IntParse (chars [15]), IntParse (chars [16]), IntParse (chars [17]));		
		}
	}

	int IntParse (string text)
	{
		int num;
		if (int.TryParse (text, out num)) {
			return num;
		} else
			return 0;
	}
}

/*ReadDemo.cs

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class ReadDemo : MonoBehaviour {
 
	public TextAsset csv; 
 
	void Start () {
		CSVReader.DebugOutputGrid( CSVReader.SplitCsvGrid(csv.text) ); 
	}
}*/