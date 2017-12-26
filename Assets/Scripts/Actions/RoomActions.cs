using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoomActions : MonoBehaviour {

	public Text restTimeText;
	public Text restRecoverText;
	public Text bathMatText;
	public Text hotBathMat1Text;
	public Text hotBathMat2Text;
	public Text normalBathRecoverText;
	public Text hotBathRecoverText;

	public Button addButton;
	public Button reduceButton;
	public Button normalBathButton;
	public Button hotBathButton;
	public Button upgradeButton;

	public GameObject normalBath;
	public GameObject hotBath;

	public LoadingBar _loading;

	private int restTimeMax = 20;
	private int restTimeMin = 1;
	private int restTime;

	private GameData _gameData;

	void Start(){
		_gameData = this.gameObject.GetComponentInParent<GameData> ();
	}

	public void UpdateRoomStates(){
		restTime = 10;
		SetRestState ();
		SetUpgradeState ();
		if (GameData._playerData.BedRoomOpen >= 2) {
			normalBath.SetActive (true);
			SetNormalBathState ();
		} else
			normalBath.SetActive (false);

		if (GameData._playerData.BedRoomOpen >= 3) {
			hotBath.SetActive (true);
			SetHotBathState ();
		} else
			hotBath.SetActive (false);
	}

	public void AddTime(){
		restTime = (restTime + 1) > restTimeMax ? restTime : (restTime + 1);
		SetRestState ();
	}

	public void ReduceTime(){
		restTime = (restTime - 1) < restTimeMin ? restTime : (restTime - 1);
		SetRestState ();
	}

	void SetRestState(){
		restTimeText.text = restTime + "h";
		restRecoverText.text = "Energy +" + GameConfigs.StrengthRecoverPerRestHour [GameData._playerData.BedRoomOpen - 1] * restTime + ", Spirit +" + GameConfigs.SpiritRecoverPerRestHour * restTime + ".";
		addButton.interactable = !(restTime >= restTimeMax);
		reduceButton.interactable = !(restTime <= restTimeMin);
	}

	void SetUpgradeState(){
		if (GameData._playerData.BedRoomOpen >= GameConfigs.MaxLv_BedRoom)
			upgradeButton.gameObject.SetActive (false);
		else
			upgradeButton.gameObject.SetActive (true);
	}

	void SetNormalBathState(){
		bathMatText.text = "Water ×" + GameConfigs.WaterForBath;
		normalBathRecoverText.text = "Temp. " + GameConfigs.TempRecoverPerNormalBath + "℃, Spirit +" + GameConfigs.SpiritRecoverPerBath+".";

		if (_gameData.CountInHome (4100) < GameConfigs.WaterForBath) {
			bathMatText.color = Color.red;
			normalBathButton.interactable = false;
		}else {
			bathMatText.color = Color.green;
			normalBathButton.interactable = true;
		}
	}

	void SetHotBathState(){
		hotBathMat1Text.text = "Water ×" + GameConfigs.WaterForBath;
		hotBathMat2Text.text = "Wood ×" + GameConfigs.WoodForHotBath;
		hotBathRecoverText.text = "Temp. +" + GameConfigs.TempRecoverPerHotBath + "℃, Spirit +" + GameConfigs.SpiritRecoverPerBath+".";

		if (_gameData.CountInHome (4100) < GameConfigs.WaterForBath) {
			hotBathMat1Text.color = Color.red;
		}else {
			hotBathMat1Text.color = Color.green;
		}

		if (_gameData.CountInHome (GameConfigs.WoodId) < GameConfigs.WoodForHotBath) {
			hotBathMat2Text.color = Color.red;
		}else {
			hotBathMat2Text.color = Color.green;
		}

		hotBathButton.interactable = (hotBathMat1Text.color == Color.green && hotBathMat2Text.color == Color.green);
	}

	public void Rest(){
		StartCoroutine (WaitAndRest ());
	}

	IEnumerator WaitAndRest(){
		int t = _loading.CallInLoadingBar (0);
		yield return new WaitForSeconds (t);
		ConfirmRest ();
	}

	void ConfirmRest(){
		_gameData.ChangeProperty (8, GameConfigs.StrengthRecoverPerRestHour[GameData._playerData.BedRoomOpen-1]  * restTime);
		_gameData.ChangeProperty (2, GameConfigs.SpiritRecoverPerRestHour * restTime);
		_gameData.ChangeTime (restTime * 60);
        Debug.Log("Rest Time = " + restTime);
		//Achievement
		this.gameObject.GetComponentInParent<AchieveActions>().Sleep(restTime);
	}

	public void NormalBath(){
		StartCoroutine (WaitAndNormalBath ());
	}

	IEnumerator WaitAndNormalBath(){
		int t = _loading.CallInLoadingBar (0);
		yield return new WaitForSeconds (t);
		ConfirmNormalBath ();
	}

	void ConfirmNormalBath(){
		_gameData.ChangeProperty (10, GameConfigs.TempRecoverPerNormalBath);
		_gameData.ChangeProperty (2, GameConfigs.SpiritRecoverPerBath);
		_gameData.ConsumeItemInHome (4100, GameConfigs.WaterForBath);
		_gameData.ChangeTime (GameConfigs.TimeForBath * 60);
	}
		
	public void HotBath(){
		StartCoroutine (WaitAndHotBath ());
	}

	IEnumerator WaitAndHotBath(){
		int t = _loading.CallInLoadingBar (0);
		yield return new WaitForSeconds (t);
		ConfirmHotBath ();
	}

	void ConfirmHotBath(){
		_gameData.ChangeProperty (10, GameConfigs.TempRecoverPerHotBath);
		_gameData.ChangeProperty (2, GameConfigs.SpiritRecoverPerBath);
		_gameData.ConsumeItemInHome (4100, GameConfigs.WaterForBath);
		_gameData.ConsumeItemInHome (GameConfigs.WoodId, GameConfigs.WoodForHotBath);
		_gameData.ChangeTime (GameConfigs.TimeForBath * 60);
	}
}
