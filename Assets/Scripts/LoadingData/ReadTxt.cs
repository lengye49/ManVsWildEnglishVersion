//using System;
using UnityEngine;
using System.Collections;

public class ReadTxt 
{
	public static string[][] ReadText (string txtName) 
	{
		string[][] textArray;
		TextAsset binAsset = Resources.Load (txtName, typeof(TextAsset)) as TextAsset;
		string[] lineArray = binAsset.text.Split ("\r" [0]);//split the txt by return("/r"[0]);

		textArray = new string[lineArray.Length][];

		for (int i=0; i<lineArray.Length; i++)	 {
			textArray[i] = lineArray[i].Split(',');	//split the line by ','
		}

		return textArray;

	}

	public static string GetDataByRowAndCol(string[][] textArray, int nRow,int nCol)
	{
		if (textArray.Length <= 0 || nRow >= textArray.Length)
			return "";
		if (nCol >= textArray [0].Length)
			return "";

		return textArray [nRow] [nCol];
	}

//	public static string GetDataByIdAndName(string[][] textArray, int id, string name)
//	{
//		if (textArray.Length <= 0)
//			return "";
//		int nRow = textArray.Length;
//		int nCol = textArray [0].Length;
//
//		for (int i=0; i<nRow; ++i) 
//		{
//			string strId = string.Format("\r{0}",id);
//
//			if(textArray[i][0] == strId)
//			{
//				for(int j=0;j<nCol;++j)
//				{
//					if(textArray[0][j] == name)
//						return textArray[i][j];
//				}
//			}
//		}
//
//		return "";
//	}

	public static string[][] GetRequire(string req)
	{
		if (!req.Contains ("|"))
			return null;
		string[] reqs;
		if (req.Contains (";")) {
			reqs = req.Split (';');
		} else {
			reqs = new string[1];
			reqs [0] = req;
		}
		string[][] back = new string[reqs.Length][];
		for (int i=0; i<reqs.Length; i++) {
			back[i] = reqs[i].Split('|');
		}
		return back;
	}

	public static string[][] GetFoodRequire(string req)
	{
		string[] reqs;
		if (req.Contains (";")) {
			reqs = req.Split (';');
		} else {
			reqs = new string[1];
			reqs [0] = req;
		}
		string[][] back = new string[reqs.Length][];
		for (int i=0; i<reqs.Length; i++) {
			back[i] = new string[2];
			back[i][0] = reqs[i];
			back[i][1] = "1";
		}
		return back;
	}

	public static int[] GetIntsByString(string s)
	{
		string[] ss=s.Split('|');
		int[] ints = new int[ss.Length];
		for (int i=0; i<ss.Length; i++)
			ints [i] = int.Parse (ss [i]);
		return ints;
	}

}
