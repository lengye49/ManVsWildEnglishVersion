using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FarmActions : MonoBehaviour {

	public GameObject ContentF;
	public RectTransform plantingTip;
	public Button upgradeButton;
    public LoadingBar _loading;

	private GameObject farmCell;
	private ArrayList farmCells = new ArrayList ();

	private int openFarmlands;
	private GameData _gameData;
	private FloatingActions _floating;

	void Start () {
		_gameData = this.gameObject.GetComponentInParent<GameData> ();
		_floating = GameObject.Find ("FloatingSystem").GetComponent<FloatingActions> ();
	}

	public void UpdateFarm(){
		Destroy (farmCell);
		farmCell = Instantiate (Resources.Load ("farmCell")) as GameObject;
		farmCell.SetActive (false);

		plantingTip.localPosition = new Vector3 (150, 2000, 0);
		openFarmlands = 0;

		upgradeButton.gameObject.SetActive (!(GameData._playerData.FarmOpen >= GameConfigs.MaxLv_Farm));

		foreach (int key in GameData._playerData.Farms.Keys) {
			if (GameData._playerData.Farms [key].open == 1)
				openFarmlands++;
		}

		if (openFarmlands > farmCells.Count) {
			for (int i = farmCells.Count; i < openFarmlands; i++) {
				GameObject o = Instantiate (farmCell) as GameObject;
				o.transform.SetParent (ContentF.transform);
				o.transform.localPosition = Vector3.zero;
				o.transform.localScale = Vector3.one;
				farmCells.Add (o);
				ClearContents (o);
			}
		}

		int j = 0;
		foreach (int key in GameData._playerData.Farms.Keys) {
			if (GameData._playerData.Farms [key].open > 0) {
				GameObject o = farmCells [j] as GameObject;
				o.SetActive (true);
				SetFarmState (o, GameData._playerData.Farms [key], j,key);
				j++;
			}
		}
		ContentF.GetComponent<RectTransform> ().sizeDelta = new Vector2(900,250 * j);
	}

	void ClearContents(GameObject o){
		Text[] t = o.GetComponentsInChildren<Text> ();
		for (int i = 0; i < t.Length; i++) {
			t [i].text = "";
		}
	}

	void SetFarmState(GameObject o, FarmState f,int j,int key){
		Text[] t = o.GetComponentsInChildren<Text> ();
		Button b = o.GetComponentInChildren<Button> ();
		Plants p = LoadTxt.GetPlant (f.plantType);

		if (f.plantType==0)
			t[0].text = "Farm";
		else
			t[0].text = "Wine Cellar";

		if (f.plantTime <= 0) {
			t [1].text = "(Empty)";
			t [1].color = Color.grey;
			if (f.plantType == 0)
				t [2].text = "Grow crops here.";
			else if (f.plantType == 1)
				t [2].text = "Make wine here.";
			else if (f.plantType == 2)
				t [2].text = "Make beer here.";
			else if (f.plantType == 3)
				t [2].text = "Make whiskey here.";
			else
				Debug.Log ("wrong plantType!!");
			b.interactable = true;
			b.name = key.ToString()+"|Prepare";
			t [3].text = "Prepare";
		} else {
			bool isMature = IsMature (f.plantTime, p);
			t [1].text = isMature ? "(Collect)" : "(Wait)";
			t [1].color = isMature ? Color.green : Color.black;
			if (isMature) {
				if (f.plantType == 0)
					t [2].text = "Crops are ready.";
				else if (f.plantType == 1)
					t [2].text = "Wine is ready.";
				else if (f.plantType == 2)
					t [2].text = "Beer is ready.";
				else if (f.plantType == 3)
					t [2].text = "Whiskey is ready.";
				else
					Debug.Log ("wrong plantType!!");

				b .interactable = true;
				b.name = key.ToString()+"|Charge";
				t [3].text = "Collect";
			} else {
				t [2].text = "Time: " + GetLeftTime (f.plantTime, p);
				b .interactable = false;
				b .name = key.ToString()+"|Charge";
				t [3].text = "Collect";
			}
		}
	}

	bool IsMature(int t,Plants p){
		return (t + p.plantGrowCycle * 24 * 60 <= GameData._playerData.minutesPassed);
	}

	string GetLeftTime(int t,Plants p){
		int t1 = t + p.plantGrowCycle * 24 * 60;
		int t2 = t1 - GameData._playerData.minutesPassed;
		if (t2 >= 24 * 60)
			return (int)((t2) / 60 / 24) + " day";
		else {
			int h = (int)(t2 / 60);
			int m = t2 - 60 * h;
			return h + " h" + m + " min";
		}
	}

	public void RemoveCrop(int index){
		GameData._playerData.Farms [index].plantTime = 0;
		_gameData.StoreData ("Farms", _gameData.GetStrFromFarmState (GameData._playerData.Farms));
		UpdateFarm ();
	}

	public void ChargeCrop(int index){
		Dictionary<int,int> r = new Dictionary<int, int> ();
		Plants p = LoadTxt.GetPlant (GameData._playerData.Farms [index].plantType);
		int num;
		switch (p.plantType) {
		case 0:
			foreach (int key in p.plantObtain.Keys) {
				num = (int)(p.plantObtain [key] * Algorithms.GetIndexByRange (80, 120) / 100 * GameData._playerData.HarvestIncrease);
				if (num > 0)
					r.Add (key, num);
			}
			break;
		default:
			foreach (int key in p.plantObtain.Keys) {
				num = (int)(p.plantObtain [key] * Algorithms.GetIndexByRange (75, 125) / 100 * GameData._playerData.OenologyIncrease);
				if (num > 0)
					r.Add (key, num);
			}
			break;
		}

		foreach (int key in r.Keys) {
			_gameData.AddItem (key * 10000, r [key]);
			_floating.CallInFloating (LoadTxt.MatDic [key].name + " +" + r [key], 0);
		}
		GameData._playerData.Farms [index].plantTime = 0;
		_gameData.StoreData ("Farms", _gameData.GetStrFromFarmState (GameData._playerData.Farms));
		UpdateFarm ();
	}
		
	public void CallInPlantingTip(int index){
		plantingTip.gameObject.SetActive (true);
		if (plantingTip.transform.localPosition.y > 1 || plantingTip.transform.localPosition.y < -1)
			plantingTip.localPosition = new Vector3 (150, 0, 0);
		int plantType = GameData._playerData.Farms [index].plantType;
		plantingTip.gameObject.name = index.ToString();
		Text[] t = plantingTip.gameObject.GetComponentsInChildren<Text> ();

		switch (plantType) {
		case 0:
			t [0].text = "Crops";
			break;
		case 1:
			t [0].text = "Wine";
			break;
		case 2:
			t [0].text = "Beer";
			break;
		case 3:
			t [0].text = "Whiskey";
			break;
		default:
			break;
		}
                
        int i = 2;
        bool canPrepare = true;

		Plants p = LoadTxt.GetPlant(plantType);
		Dictionary<int,int> d = p.plantReq;
        foreach (int key in d.Keys) {
            t[i].text= LoadTxt.MatDic [key].name + " ×" + d [key];
            if (_gameData.CountInHome(key) < d[key])
            {
                t[i].color = Color.red;
                canPrepare = false;
            }
            else
                t[i].color = Color.green;
            i++;
        }
        plantingTip.GetComponentInChildren<Button>().interactable = canPrepare;

		t[5].text= p.plantTime + " h";
		t[7].text = p.plantGrowCycle + " day";
		t[8].text= "Prepare";
	}

	public void Prepare(){
		int index = int.Parse (plantingTip.gameObject.name);
		int plantType = GameData._playerData.Farms [index].plantType;;
		Plants p = LoadTxt.GetPlant (plantType);

		foreach (int key in p.plantReq.Keys)
        {
			if (_gameData.CountInHome(key) < p.plantReq[key])
                return;
        }
            
        int t = _loading.CallInLoadingBar(60);

		StartCoroutine(GetPrepared(p, index, t));
	}

	IEnumerator GetPrepared(Plants p,int index,int waitTime){
        yield return new WaitForSeconds(waitTime);

		_gameData.ChangeTime (p.plantTime * 60);
		foreach (int key in p.plantReq.Keys) {
			_gameData.ConsumeItemInHome (key, p.plantReq [key]);
        }
        GameData._playerData.Farms [index].plantTime = GameData._playerData.minutesPassed;
        _gameData.StoreData ("Farms", _gameData.GetStrFromFarmState (GameData._playerData.Farms));
        CallOutPlantingTip ();
        UpdateFarm ();
    }

	public void CallOutPlantingTip(){
		plantingTip.localPosition = new Vector3 (150, 2000, 0);
	}

	public void OnLeave(){
		Destroy (farmCell);
		if (farmCells == null)
			return;
		foreach (GameObject o in farmCells)
			Destroy (o);
		farmCells.Clear ();
	}
}
