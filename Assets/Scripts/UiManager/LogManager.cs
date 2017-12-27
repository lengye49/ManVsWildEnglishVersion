using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogManager : MonoBehaviour {
	private Text[] logs;
	private int cNum = 36;

	void Start(){
		logs = this.gameObject.GetComponentsInChildren<Text> ();
		ClearLogs ();

		if (GameData._playerData.firstTimeInGame == 0) {
			GameData._playerData.firstTimeInGame = 1;
			PlayerPrefs.SetInt ("FirstTimeInGame", 1);
			AddLog ("After a big storm, you woke up.");
			StartCoroutine (FirstLog (1f, "It's a strange place."));
			StartCoroutine (FirstLog (2f, "You decided to keep alive."));
			StartCoroutine (FirstLog (3f, "There is water and food in the backpack."));
			StartCoroutine (FirstLog (4f, "Building a shelter is a good choice."));
			StartCoroutine (FirstLog (5f, "Click [Kitchen] to Build."));
			//你也可以看看页面左下角的[背包]，或者打开右下角的[地图]到四周转转。
		}
	}

	IEnumerator FirstLog(float t,string s){
		yield return new WaitForSeconds (t);
		AddLog (s);
	}

	void ClearLogs(){
		for (int i = 0; i < logs.Length; i++) {
			logs [i].text = string.Empty;
		}
	}

	/// <summary>
	/// 增加新的log
	/// </summary>
	/// <param name="t">等待时间.</param>
	/// <param name="s">S.</param>
	public void AddLog(float t,string s){
		StartCoroutine (FirstLog (t, s));
	}

	public void AddLog(string s){
		if (s.Length > cNum) {
			string s1 = s.Substring (0, cNum);
			string s2 = s.Substring (cNum, s.Length - cNum);
			AddNewLog (s1,false);
			AddNewLog (s2,false);
		} else
			AddNewLog (s,false);
	}


	public void AddLog(string s,bool isGreen){
		if (s.Length > cNum) {
			string s1 = s.Substring (0, cNum);
			string s2 = s.Substring (cNum, s.Length - cNum);
			AddNewLog (s1,isGreen);
			AddNewLog (s2,isGreen);
		} else
			AddNewLog (s,isGreen);
	}

	void AddNewLog(string s,bool isGreen){
		for (int i = logs.Length - 1; i > 0; i--) {
			logs [i].text = logs [i - 1].text;
			logs [i].color = logs [i - 1].color;
            logs[i].color = new Color(logs[i - 1].color.r, logs[i - 1].color.g, logs[i - 1].color.b, GetAlpha(i) / 255f);
		}
		logs [0].text = ">" + s;
		logs [0].color = isGreen ? Color.green : Color.white;
	}

    float GetAlpha(int index){
        switch (index)
        {
            case 0:
            case 1:
                return 255f;
            case 2:
            case 3:
                return 205f;
            case 4:
            case 5:
                return 155f;
            case 6:
            case 7:
                return 105f;
            case 8:
            case 9:
                return 55f;
            default:
                return 50f;
        }
    
    }

}
