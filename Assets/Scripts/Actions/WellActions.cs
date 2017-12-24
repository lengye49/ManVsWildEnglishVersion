using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WellActions : MonoBehaviour {

	public Text waterStored;
	public Text waterPerDay;
	public Text waterStoreMax;

	private GameData _gameData;
	private int waterStoreNow;

	// Use this for initialization
	void Start () {
		waterPerDay.text = "(" + (int)(GameConfigs.WaterInWellPerDay * GameData._playerData.WaterCollectingRate) + "/天):";
		waterStoreMax.text = "/" + GameConfigs.WaterStoreMax.ToString ();
		_gameData = this.gameObject.GetComponentInParent<GameData> ();
	}
	
	public void UpdateWell(){
		int min = (GameData._playerData.minutesPassed - GameData._playerData.LastWithdrawWaterTime);
		waterPerDay.text = "(" + (int)(GameConfigs.WaterInWellPerDay * GameData._playerData.WaterCollectingRate) + "/天):";
		waterStoreNow = (int)(min / 60 / 24 * GameConfigs.WaterInWellPerDay * GameData._playerData.WaterCollectingRate);
		waterStoreNow = (waterStoreNow < 0) ? 0 : waterStoreNow;
		waterStoreNow = (waterStoreNow > GameConfigs.WaterStoreMax) ? GameConfigs.WaterStoreMax : waterStoreNow;
		waterStored.text = waterStoreNow.ToString ();
	}

	public void Withdraw(){
		if (waterStoreNow <= 0)
			return;
		_gameData.WithDrawWater (waterStoreNow);
		UpdateWell ();
	}
}
