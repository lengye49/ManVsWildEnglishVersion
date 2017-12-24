using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogManager : MonoBehaviour {
	private Text[] logs;

	void Start(){
		logs = this.gameObject.GetComponentsInChildren<Text> ();
		ClearLogs ();

		if (GameData._playerData.firstTimeInGame == 0) {
			GameData._playerData.firstTimeInGame = 1;
			PlayerPrefs.SetInt ("FirstTimeInGame", 1);
			AddLog ("一阵电闪雷鸣过后，你睁开双眼...");
			StartCoroutine (FirstLog (1f, "这个世界是如此的陌生..."));
			StartCoroutine (FirstLog (2f, "你决定努力活下去，搞清这一切..."));
			StartCoroutine (FirstLog (3f, "[背包]里还有食物和水，"));
			StartCoroutine (FirstLog (4f, "或许可以先建个简单的庇护所。"));
			StartCoroutine (FirstLog (5f, "点击[厨房]可以进行建造。"));
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
		if (s.Length > 20) {
			string s1 = s.Substring (0, 20);
			string s2 = s.Substring (20, s.Length - 20);
			AddNewLog (s1,false);
			AddNewLog (s2,false);
		} else
			AddNewLog (s,false);
	}


	public void AddLog(string s,bool isGreen){
		if (s.Length > 20) {
			string s1 = s.Substring (0, 20);
			string s2 = s.Substring (20, s.Length - 20);
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
