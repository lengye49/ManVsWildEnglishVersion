using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PetsActions : MonoBehaviour {

	public GameObject contentP;
	public RectTransform Detail;
	public FloatingActions _floating;
	public Text spaceText;
	public GameObject upgradeButton;
	public LogManager _logManager;

	private GameObject petCell;
	private ArrayList petCells = new ArrayList ();
	private int openPetCell;

	private int petSpace;
	private int usedSpace;

	private GameData _gameData;
	private int _localIndex;
	private Pet _localPet;

	void Start(){
		_gameData = this.gameObject.GetComponentInParent<GameData> ();
		_floating = GameObject.Find ("FloatingSystem").GetComponent<FloatingActions> ();
	}

	public void UpdatePets(){
		Detail.localPosition = new Vector3 (150, -2000, 0);
		SetPetCells ();
		upgradeButton.SetActive (!(GameData._playerData.PetsOpen >= GameConfigs.MaxLv_Pets));
	}

	void SetPetCells(){
		OnLeave ();
		petCell = Instantiate (Resources.Load ("petCell")) as GameObject;
		petCell.SetActive (false);

		petSpace = GameData._playerData.PetsOpen * 10;

		openPetCell = 0;
		usedSpace = 0;

		for (int i = 0; i < petCells.Count; i++) {
			GameObject o = petCells [i] as GameObject;
			ClearContents (o);
		}
			
		foreach (int key in GameData._playerData.Pets.Keys) {
			openPetCell++;
			Monster m = LoadTxt.GetMonster (GameData._playerData.Pets [key].monsterId);
			usedSpace += m.canCapture;
		}
		spaceText.text = "Space(" + usedSpace + "/" + petSpace + ")";

		if (openPetCell > petCells.Count) {
			for (int i = petCells.Count; i < openPetCell; i++) {
				GameObject o = Instantiate (petCell) as GameObject;
				o.transform.SetParent (contentP.transform);
				o.transform.localPosition = Vector3.zero;
				o.transform.localScale = Vector3.one;
				petCells.Add (o);
			}
		}
		if (openPetCell < petCells.Count) {
			for (int i = openPetCell; i < petCells.Count; i++) {
				GameObject o = petCells [i] as GameObject;
				petCells.RemoveAt (i);
				GameObject.Destroy (o);
			}
		}

		int j = 0;
		foreach (int key in GameData._playerData.Pets.Keys) {
			GameObject o = petCells [j] as GameObject;
			o.SetActive (true);
			o.gameObject.name = key.ToString ();
			SetPetCellState (o, GameData._playerData.Pets [key]);
			j++;
		}

		contentP.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(800,100 * openPetCell);
	}

	void SetPetCellState(GameObject o,Pet p){
		
		Text[] t = o.GetComponentsInChildren<Text> ();
		Monster m = LoadTxt.GetMonster (p.monsterId);
		t [0].text = m.name;
		switch (p.state) {
		case 0:
			t [1].text = "Free Range";
			break;
		case 1:
			t [1].text = "Riding";
			break;
		case 2:
			t [1].text = "Patrolling";
			break;
		default:
			t [1].text = "Free Range";
			break;
		}
		t[2].text = m.canCapture.ToString();
	}

	public void CallInDetail(Pet p,int index){
		_localPet = p;
		_localIndex = index;
		Detail.localPosition = new Vector3 (150, 0, 0);
		UpdateDetail ();
		foreach (int key in GameData._playerData.Pets.Keys) {
			Debug.Log ("Key = " + key + ", State = "+GameData._playerData.Pets[key].state);
		}
	}

	void UpdateDetail(){
		Text[] t = Detail.gameObject.GetComponentsInChildren<Text> ();
		Monster m = LoadTxt.GetMonster (_localPet.monsterId);
		t [0].text = _localPet.name;
		t [1].text = "Desc.";

		switch (_localPet.state) {
		case 0:
			t [2].text = "Free Range";
			break;
		case 1:
			t [2].text = "Ride";
			break;
		case 2:
			t [2].text = "Patrolling";
			break;
		default:
			break;
		}
		t [2].color = Color.green;
		t [3].text = m.canCapture.ToString ();
		t [4].text = m.name;
		t [5].text = _localPet.speed.ToString ();
		t [6].text = _localPet.alertness.ToString ();
	}

	public void CallOutDetail(){
		if (Detail.localPosition.y > -1 && Detail.localPosition.y < 1)
			Detail.localPosition = new Vector3 (0, -2000, 0);
	}

	void ClearContents(GameObject o){
		Text[] t = o.GetComponentsInChildren<Text> ();
		for (int i = 0; i < t.Length; i++) {
			t [i].text = "";
		}
	}

	public void Guard(){
		if (_localPet.state == 1) {
			RemoveMount ();
		}
		_localPet.state = (_localPet.state == 2) ? 0 : 2;

		if (_localPet.state == 2)
			_logManager.AddLog (_localPet.name + " started to patrol.");
		else
			_logManager.AddLog (_localPet.name + " stopped patrolling");

		foreach (int key in GameData._playerData.Pets.Keys) {
			Debug.Log ("Key = " + key + ", State = "+GameData._playerData.Pets[key].state);
		}
		GameData._playerData.Pets [_localIndex] = _localPet;
		StorePetState ();
		UpdateDetail ();
		SetPetCells();
	}

	public void Ride(){
		float speed1 = GameData._playerData.property [23];
		string s = "";
		if (_localPet.state == 1) {
			RemoveMount ();
			_localPet.state = 0;
			s += "Stop riding" + _localPet.name+".";
		} else {
            foreach (int key in GameData._playerData.Pets.Keys)
            {
                if (GameData._playerData.Pets[key].state == 1)
                    GameData._playerData.Pets[key].state = 0;
            }

			_localPet.state = 1;
            GameData._playerData.Mount = _localPet;
            _gameData.StoreData ("Mount", _gameData.GetStrFromMount (GameData._playerData.Mount));
            _gameData.UpdateProperty ();

			s += "Start to ride " + _localPet.name+".";
			//Achievement
			this.gameObject.GetComponentInParent<AchieveActions> ().MountPet (_localPet.monsterId);
		}
		float speed2 = GameData._playerData.property [23];
		if (speed1 > speed2) {
			s += "Move speed -" + (speed1 - speed2) + ".";
		}else if(speed1<speed2){
			s += "Move speed +" + (speed2 - speed1) + ".";
		}
		_logManager.AddLog (s);

		GameData._playerData.Pets [_localIndex] = _localPet;
		StorePetState ();
		UpdateDetail ();
		SetPetCells();
	}

	public void SetFree(){
		if (_localPet.state == 1) {
			RemoveMount ();
		}

		GameData._playerData.Pets.Remove (_localIndex);
		_logManager.AddLog (_localPet.name + " left you.");
		StorePetState ();
		CallOutDetail ();
		SetPetCells();
	}

	public void Kill(){
		if (_localPet.state == 1) {
			RemoveMount ();
		}
		string s = "You slaughtered " + _localPet.name + ".";
		Monster m = LoadTxt.GetMonster(_localPet.monsterId);
		Dictionary<int,int> r = Algorithms.GetReward (m.drop);
		if (r.Count > 0) {
			foreach (int key in r.Keys) {
				_gameData.AddItem (key * 10000, r [key]);
				s += LoadTxt.MatDic [key].name + " ×" + r [key] + ",";
			}
			s = s.Substring (0, s.Length - 1) + ".";
		}
		_logManager.AddLog (s);

		GameData._playerData.Pets.Remove (_localIndex);
		StorePetState ();
		CallOutDetail ();
		SetPetCells();
	}

	void RemoveMount(){
		GameData._playerData.Mount = new Pet ();
		_gameData.StoreData ("Mount", _gameData.GetStrFromMount (GameData._playerData.Mount));
		_gameData.UpdateProperty ();
	}

	void StorePetState(){
		_gameData.StoreData ("Pets", _gameData.GetstrFromPets (GameData._playerData.Pets));
		GameData._playerData.Pets = _gameData.GetPetListFromStr (PlayerPrefs.GetString ("Pets", ""));
	}

	public void OnLeave(){
		Destroy (petCell);
		if (petCells == null)
			return;
		foreach (GameObject o in petCells)
			Destroy (o);	
		petCells.Clear ();
	}
}
