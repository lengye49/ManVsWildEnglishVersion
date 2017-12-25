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
		s = "Day " + GameData._playerData.dayNow;

		return s;
	}

	void SetSeason(){
		string s = "";
		Color c = new Color();
		switch (GameData._playerData.seasonNow) {
		case 0:
			s += "Spring";
			c = Color.green;
			break;
		case 1:
			s += "Autumn";
			c = Color.red;
			break;
		case 2:
			s += "Autumn";
			c = Color.yellow;
			break;
		case 3:
			s += "Winter";
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
		s += (GameData._playerData.hourNow > 9 ? "" : "0") + GameData._playerData.hourNow.ToString () + ":";
		s += (GameData._playerData.minuteNow > 9 ? "" : "0") + GameData._playerData.minuteNow.ToString ();
		s += GameData._playerData.hourNow >= 12 ? "am" : "pm";
		return s;
	}

	public void OnProperty(string pName){
		switch (pName) {
		case "hp":
			_log.AddLog ("Hp, Can not below 0.");
			break;
		case "spirit":
			_log.AddLog ("Spirit, Can not below 0.");
			break;
		case "food":
			_log.AddLog ("Food,Can not below 0.");
			break;
		case "water":
			_log.AddLog ("Water, Can not below 0.");
			break;
		case "strength":
			_log.AddLog ("Energy");
			break;
		case "temp":
			_log.AddLog ("Temperature,-30℃~50℃.");
			break;
		case "time":
			_log.AddLog ("Day and time.");
			break;
		default:
			break;
		}
	}


}
