using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HeadUiManager : MonoBehaviour {
	public Text hpNow;
	public Text hpMax;
	public Text spiritNow;
	public Text spiritMax;
	public Text foodNow;
	public Text foodMax;
	public Text waterNow;
	public Text waterMax;
	public Text strengthNow;
	public Text strengthMax;
	public Text tempNow;

	public LogManager _log;

	public Image HpImage;
	public Image SpiritImage;
	public Image FoodImage;
	public Image WaterImage;
	public Image StrengthImage;
	public Image TempImage;

	public Text dateNow;
	public Text timeNow;

	public Text seasonNow;

	public void UpdateHeadUI(){
		hpNow.text = GameData._playerData.hpNow.ToString ();
		hpMax.text = "/" + GameData._playerData.property[1];
		hpNow.color = Algorithms.GetDangerColor (GameData._playerData.hpNow / GameData._playerData.property [1]);
		HpImage.color = hpNow.color;

		spiritNow.text = GameData._playerData.spiritNow.ToString ();
		spiritMax.text = "/" + GameData._playerData.property[3];
		spiritNow.color = Algorithms.GetDangerColor (GameData._playerData.spiritNow / GameData._playerData.property [3]);
		SpiritImage.color = spiritNow.color;

		foodNow.text = GameData._playerData.foodNow.ToString ();
		foodMax.text = "/" + GameData._playerData.property[5];
		foodNow.color = Algorithms.GetDangerColor (GameData._playerData.foodNow / GameData._playerData.property [5]);
		FoodImage.color = foodNow.color;

		waterNow.text = GameData._playerData.waterNow.ToString ();
		waterMax.text = "/" + GameData._playerData.property[7];
		waterNow.color = Algorithms.GetDangerColor (GameData._playerData.waterNow / GameData._playerData.property [7]);
		WaterImage.color = waterNow.color;

		strengthNow.text = GameData._playerData.strengthNow.ToString ();
		strengthMax.text = "/" + GameData._playerData.property[9];
		strengthNow.color = new Color (1f, 1f, 1f, 1f);
		StrengthImage.color = strengthNow.color;

        tempNow.text = GameData._playerData.tempNow.ToString("#0.0");
		if ((GameData._playerData.tempNow >= (GameData._playerData.property[12] -5)) || (GameData._playerData.tempNow <= (GameData._playerData.property[11] +5)))
			tempNow.color = new Color(1f, 0f, 0f, 1f);
		else if ((GameData._playerData.tempNow >= (GameData._playerData.property[12] -15)) || (GameData._playerData.tempNow <= (GameData._playerData.property[11] +15)))
			tempNow.color = new Color(1f, 1f, 0f, 1f);
		else
			tempNow.color = new Color(1f, 1f, 1f, 1f);

		TempImage.color = tempNow.color;

		TempImage.color = tempNow.color;

		dateNow.text = GetDate ();
		timeNow.text = GetTime ();
		SetSeason ();
	}

	public void UpdateHeadUI(string propName){
		switch (propName) {
		case "hpNow":
			hpNow.text = GameData._playerData.hpNow.ToString ();
			hpNow.color = Algorithms.GetDangerColor (GameData._playerData.hpNow / GameData._playerData.property [1]);
			HpImage.color = hpNow.color;
			break;
		case "hpMax":
			hpMax.text = "/" + GameData._playerData.property[1];
			hpNow.color = Algorithms.GetDangerColor (GameData._playerData.hpNow / GameData._playerData.property [1]);
			HpImage.color = hpNow.color;
			break;
		case "spiritNow":
			spiritNow.text = GameData._playerData.spiritNow.ToString ();
			spiritNow.color = Algorithms.GetDangerColor (GameData._playerData.spiritNow / GameData._playerData.property [3]);
			SpiritImage.color = spiritNow.color;
			break;
		case "spiritMax":
			spiritMax.text = "/" + GameData._playerData.property[3];
			spiritNow.color = Algorithms.GetDangerColor (GameData._playerData.spiritNow / GameData._playerData.property [3]);
			SpiritImage.color = spiritNow.color;
			break;
		case "foodNow":
			foodNow.text = GameData._playerData.foodNow.ToString ();
			foodNow.color = Algorithms.GetDangerColor (GameData._playerData.foodNow / GameData._playerData.property [5]);
			FoodImage.color = foodNow.color;
			break;
		case "foodMax":
			foodMax.text = "/" + GameData._playerData.property[5];
			foodNow.color = Algorithms.GetDangerColor (GameData._playerData.foodNow / GameData._playerData.property [5]);
			FoodImage.color = foodNow.color;
			break;
		case "waterNow":
			waterNow.text = GameData._playerData.waterNow.ToString ();
			waterNow.color = Algorithms.GetDangerColor (GameData._playerData.waterNow / GameData._playerData.property [7]);
			WaterImage.color = waterNow.color;
			break;
		case "waterMax":
			waterMax.text = "/" + GameData._playerData.property[7];
			waterNow.color = Algorithms.GetDangerColor (GameData._playerData.waterNow / GameData._playerData.property [7]);
			WaterImage.color = waterNow.color;
			break;
		case "strengthNow":
			strengthNow.text = GameData._playerData.strengthNow.ToString ();
			strengthNow.color = new Color (1f, 1f, 1f, 1f);
			StrengthImage.color = strengthNow.color;
			break;
		case "strengthMax":
			strengthMax.text = "/" + GameData._playerData.property[9];
			strengthNow.color = new Color (1f, 1f, 1f, 1f);
			StrengthImage.color = strengthNow.color;
			break;
		case "tempNow":
			tempNow.text = GameData._playerData.tempNow.ToString ("#0.0");

            if ((GameData._playerData.tempNow >= (GameData._playerData.property[12] -5)) || (GameData._playerData.tempNow <= (GameData._playerData.property[11] +5)))
                tempNow.color = new Color(1f, 0f, 0f, 1f);
            else if ((GameData._playerData.tempNow >= (GameData._playerData.property[12] -15)) || (GameData._playerData.tempNow <= (GameData._playerData.property[11] +15)))
                tempNow.color = new Color(1f, 1f, 0f, 1f);
            else
                tempNow.color = new Color(1f, 1f, 1f, 1f);

            TempImage.color = tempNow.color;
            break;
		case "dateNow":
			dateNow.text = GetDate ();
			break;
		case "timeNow":
			timeNow.text = GetTime ();
			SetSeason ();
			break;
		default:
			Debug.Log ("Wrong propName with " + propName);
			break;
		}
	}


	string GetDate(){
		string s = "";
		s = "第" + GameData._playerData.dayNow + "天";

		return s;
	}

	void SetSeason(){
		string s = "";
		Color c = new Color();
		switch (GameData._playerData.seasonNow) {
		case 0:
			s += "春";
			c = Color.green;
			break;
		case 1:
			s += "夏";
			c = Color.red;
			break;
		case 2:
			s += "秋";
			c = Color.yellow;
			break;
		case 3:
			s += "冬";
			c = Color.white;
			break;
		default:
			break;
		}
		seasonNow.text = s;
		seasonNow.color = c;
	}

	string GetTime(){
		string s = "";
		s += GameData._playerData.hourNow >= 12 ? "下午 " : "上午 ";
		s += (GameData._playerData.hourNow > 9 ? "" : "0") + GameData._playerData.hourNow.ToString () + ":";
		s += (GameData._playerData.minuteNow > 9 ? "" : "0") + GameData._playerData.minuteNow.ToString ();
		return s;
	}

	public void OnProperty(string pName){
		switch (pName) {
		case "hp":
			_log.AddLog ("生命值，生存属性。");
			break;
		case "spirit":
			_log.AddLog ("精神，生存属性；施法媒介；影响命中。");
			break;
		case "food":
			_log.AddLog ("饱腹值，生存属性，会随时间降低。");
			break;
		case "water":
			_log.AddLog ("水分，生存属性，会随时间降低。");
			break;
		case "strength":
			_log.AddLog ("体力，采集活动需要体力才能进行。");
			break;
		case "temp":
			_log.AddLog ("体温，生存属性，-30℃~50℃。");
			break;
		case "time":
			_log.AddLog ("已存活天数和当前时间。");
			break;
		default:
			break;
		}
	}


}
