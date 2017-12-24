using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Algorithms  {

	public static int GetResultByWeight(int[] weights){

		int[] prop = new int[weights.Length];
		for (int i = 0; i < weights.Length; i++) {
			if (i == 0) {
				prop [i] = weights [i];	
			} else {
				prop [i] = weights [i] + prop [i - 1];
			}
		}
			
		int r = (int)(Random.Range (0, prop [prop.Length - 1] + 1));

		for (int i = 0; i < prop.Length; i++) {
			if (r <= prop [i]) {
				return i;
			}
		}

		return 0;
	}

	/// <summary>
	/// Gets the result by dic.
	/// </summary>
	/// <returns>The result by dic.</returns>
	/// <param name="d">id and weight</param>
	public static int GetResultByDic(Dictionary<int,int> d){
		int[] r = new int[d.Count];
		int[] w = new int[d.Count];
		int index = 0;
		foreach (int key in d.Keys){
			r [index] = key;
			w [index] = d [key];
			index++;
		}
		int i = GetResultByWeight (w);
		return r [i];
	}

	/// <summary>
	/// min <= get < max
	/// </summary>
	/// <returns>The index by range.</returns>
	/// <param name="min">Minimum.</param>
	/// <param name="max">Max.</param>
	public static int GetIndexByRange(int min,int max){
		int r = (int)(Random.Range (min, max - 0.0001f));
		return r;
	}

	/// <summary>
	/// Determines if is dodge or crit by hit/dodge/vitalSensibility/spirit.
	/// </summary>
	/// <returns><c>true</c> dodge:0 crit:2 hit:1 <c>false</c>.</returns>
	/// <param name="hit">Hit.</param>
	/// <param name="dodge">Dodge.</param>
	/// <param name="vitalSensibility">Vital sensibility.</param>
	/// <param name="spirit">Spirit.</param>
	public static int IsDodgeOrCrit(float hit, float dodge,float vitalSensibility ,float spirit){
		float hitRate = 1 - dodge * dodge / (dodge + 3 * hit)/100 * SpiritParam (spirit);
		float dodgeRate =1-hitRate;
		float critRate = hit * hit / (hit + dodge * 8)/100 * (1 - vitalSensibility/100f) * SpiritParam (spirit);
//		Debug.Log ("Dodge:" + dodgeRate + " Crit:" + critRate);
		float r = Random.Range (0F, 1F);
//		Debug.Log ("Random:" + r);
		if (r < dodgeRate)
			return 0;
		else if (r < dodgeRate + critRate)
			return 2;
		else
			return 1;
	}

	public static int CalculateDamage(float atk, float def, Skill s, int hitRate,bool isMyAtk){
		float dam = atk * (1 - def / (GameConfigs.defParam + def));
		if (!isMyAtk) {
			dam *= (s.power / 100f);
		}
		return (int)dam;
	}


	/// <summary>
	/// Spirit parameter to hit&dodge.
	/// </summary>
	/// <returns>The parameter.</returns>
	/// <param name="spirit">Spirit.</param>
	public static float SpiritParam(float spirit){
		return spirit / (spirit + (100 - spirit) / 10);
	}

	public static Dictionary<int,int> GetReward(Dictionary<int,float> d){
		Dictionary<int,int> r = new Dictionary<int, int> ();
		foreach (int key in d.Keys) {
			int i = (int)(d [key] + 0.001);	//int转换的时候会小1？
			float f = d [key] - i;//概率
			float rand = Random.Range (0f, 1f);
			if (rand < f)
				i++;
			if (i > 0) {
				if (r.ContainsKey (key))
					r [key] += i;
				else
					r.Add (key, i);
			}
		}
		return r;
	}

	/// <summary>
	/// item|num|pro
	/// </summary>
	/// <returns>The reward.</returns>
	/// <param name="items">Items.</param>
	/// <param name="pros">Pros.</param>
	public static Dictionary<int,int> GetReward(Dictionary<int,int> items,float[] pros){

		Dictionary<int,int> r = new Dictionary<int, int> ();
		if (items.Count != pros.Length) {
			return r;
		}

		int i = 0;
		foreach (int key in items.Keys) {
			if (pros[i] >= 1) {
				if (r.ContainsKey (key))
					r [key] += items [key];
				else
					r.Add (key, items [key]);
			} else {
				float rand = Random.Range(0f,1f);
				int num = 0;
				for (int j = 0; j < items [key]; j++) {
					if (rand < pros [i] * (j + 1f)) {
						num = items [key] - j;
						break;
					}
				}
				if (num >= 1) {
					if (r.ContainsKey (key))
						r [key] += num;
					else
						r.Add (key, num);
				}
			}
			i++;
		}

		return r;
	}

	public static Color GetDangerColor(float f){
		if (f > 0.3f)
			return new Color (1f, 1f, 1f, 1f);
		else if (f <= 0.1f)
			return new Color (1f, 0f, 0f, 1f);
		else
			return new Color (1f, (f - 0.1f) / 0.2f, 0f, 1f);
	}
		
}
