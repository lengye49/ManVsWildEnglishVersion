using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WarehouseActions : MonoBehaviour {

	public GameObject contentW;
	public GameObject contentB;
	public Text stateW;
	public Text stateB;
	public Button upgradeWarehouse;
	private GameObject bpCell;
	private GameObject whCell;

	private int _warehouseNum;
	private int _warehouseUsed;

	private ArrayList whCells = new ArrayList ();
	private ArrayList bpCells = new ArrayList ();

	public void UpdatePanel(){
		Destroy (bpCell);
		Destroy (whCell);
		whCell = Instantiate (Resources.Load ("whCell")) as GameObject;
		whCell.SetActive (false);
		bpCell = Instantiate (Resources.Load ("bpCell")) as GameObject;
		bpCell.SetActive (false);

		_warehouseNum = GameConfigs.warehouseMin + GameConfigs.warehouseAdd * (GameData._playerData.WarehouseOpen - 1);
		_warehouseUsed = GameData._playerData.wh.Count;
		SetState ();
		upgradeWarehouse.gameObject.SetActive (GameData._playerData.WarehouseOpen < GameConfigs.MaxLv_Warehouse);
		UpdateBpContent ();
		UpdateWhContent ();
	}

	void SetState(){
		stateW.text="("+_warehouseUsed+"/"+_warehouseNum+")";
		stateW.color = (_warehouseUsed >= _warehouseNum) ? Color.yellow : Color.white;
		stateB.text="("+GameData._playerData.bp.Count+"/"+GameData._playerData.bpNum+")";
		stateB.color = (GameData._playerData.bp.Count >= GameData._playerData.bpNum) ? Color.yellow : Color.white;
	}

	void UpdateWhContent(){
		for (int i = 0; i < whCells.Count; i++) {
			ClearContent (whCells [i] as GameObject);
		}

		if (whCells.Count<_warehouseUsed) {
			int n = whCells.Count;
			for (int i = n; i < _warehouseUsed; i++) {
				GameObject o = Instantiate (whCell) as GameObject;
				o.transform.SetParent (contentW.transform);
				o.transform.localPosition = Vector3.zero;
				o.transform.localScale = Vector3.one;
				whCells.Add (o);
				ClearContent (o);
			}
			contentW.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(375,100 * _warehouseUsed);
		}

		int j = 0;
		foreach (int key in GameData._playerData.wh.Keys) {
			GameObject o = whCells [j] as GameObject;
			o.SetActive (true);
			o.GetComponentInChildren<Image> ().color = new Color (1f, 1f, 1f, 100f / 255f);
			o.gameObject.name = key.ToString ();
			o.GetComponent<Button> ().interactable = true;
			Text[] t = o.GetComponentsInChildren<Text> ();
			t [0].text = LoadTxt.MatDic [(int)(key / 10000)].name;
			t [1].text = GameData._playerData.wh [key].ToString();
			j++;
		}
	}
		

	void UpdateBpContent(){
		for (int i = 0; i < bpCells.Count; i++) {
			ClearContent (bpCells [i] as GameObject);
		}

		if (bpCells.Count<GameData._playerData.bp.Count) {
			int n = bpCells.Count;
			for (int i = n; i < GameData._playerData.bp.Count; i++) {
				GameObject o = Instantiate (bpCell) as GameObject;
				o.transform.SetParent (contentB.transform);
				o.transform.localPosition = Vector3.zero;
				o.transform.localScale = Vector3.one;
				bpCells.Add (o);
				ClearContent (o);
			}
			contentB.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(375,100 * GameData._playerData.bp.Count);
		}

		int j = 0;
		foreach (int key in GameData._playerData.bp.Keys) {
			GameObject o = bpCells [j] as GameObject;
			o.SetActive (true);
			o.GetComponentInChildren<Image> ().color = new Color (1f, 1f, 1f, 100f / 255f);
			o.gameObject.name = key.ToString ();
			o.GetComponent<Button> ().interactable = true;
			Text[] t = o.GetComponentsInChildren<Text> ();
			t [0].text = LoadTxt.MatDic [(int)(key / 10000)].name;
			t [1].text = GameData._playerData.bp [key].ToString();
			j++;
		}
	}

	void ClearContent(GameObject o){
		o.SetActive (true);
		Text[] ts = o.gameObject.GetComponentsInChildren<Text> ();
		for (int i = 0; i < ts.Length; i++)
			ts [i].text = "";
		o.GetComponent<Button> ().interactable = false;
		o.GetComponentInChildren<Image> ().color = new Color (0f, 0f, 0f, 0f);
	}

	public void OnLeave(){
		Destroy (bpCell);
		Destroy (whCell);
		if (bpCells != null) {
			foreach (GameObject o in bpCells)
				Destroy (o);
			bpCells.Clear ();
		}
		if (whCells != null) {
			foreach (GameObject o in whCells)
				Destroy (o);
			whCells.Clear ();
		}
	}
}
