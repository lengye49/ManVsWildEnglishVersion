using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class ExploreActions : MonoBehaviour {

	public GameObject contentE;
    public RectTransform TunnelNotice;

	private GameObject mapCell;
	private ArrayList mapCells;
	public Maps mapGoing;
	private GameData _gameData;
	private PanelManager _panelManager;
	public LoadingBar _loadingBar;
	public LogManager _logManager;

	void Start () {
		mapCell = Instantiate (Resources.Load ("mapCell")) as GameObject;
		mapCell.SetActive (false);
		mapCells = new ArrayList ();
		mapGoing = new Maps ();
		_gameData = this.gameObject.GetComponentInParent<GameData> ();
		_panelManager = this.gameObject.GetComponentInParent<PanelManager> ();

        TunnelNotice.localPosition = Vector3.zero;
        TunnelNotice.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
        TunnelNotice.gameObject.SetActive (false);
	}
	
	public void UpdateExplore(){

		int openNum = 0;
		foreach (int key in GameData._playerData.MapOpenState.Keys) {
			if (GameData._playerData.MapOpenState [key] == 1)
				openNum++;
		}

		for (int i = 0; i < mapCells.Count; i++) {
			GameObject o = mapCells [i] as GameObject;
			ClearContents (o);
		}

		if (openNum-1 > mapCells.Count) {
			for (int i = mapCells.Count; i < openNum-1; i++) {
				GameObject o = Instantiate (mapCell) as GameObject;
				o.SetActive (true);
				o.transform.SetParent (contentE.transform);
				o.transform.localPosition = Vector3.zero;
				o.transform.localScale = Vector3.one;
				mapCells.Add (o);
				ClearContents (o);
			}
		}

		int j = 0;
		foreach (int key in GameData._playerData.MapOpenState.Keys) {
			if (GameData._playerData.MapOpenState [key] == 1 && key!=GameData._playerData.placeNowId) {
				GameObject o = mapCells [j] as GameObject;
				o.gameObject.name = key.ToString ();
				SetMapCell (o, key);
				j++;
			}
		}

		contentE.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(880,115 * mapCells.Count);
	}

	void SetMapCell(GameObject o,int mapId){
		Text[] t = o.GetComponentsInChildren<Text> ();
		t [0].text = LoadTxt.MapDic [mapId].name;
		int m = TravelTime (LoadTxt.MapDic [GameData._playerData.placeNowId].distances [mapId]);
		string s = GetTimeFormat (m);
		t [1].text = LoadTxt.MapDic [mapId].desc;
		t [2].text = s;
	}

	void ClearContents(GameObject o){
		Text[] t = o.GetComponentsInChildren<Text> ();
		for (int i = 0; i < t.Length; i++) {
			t [i].text = "";
		}
	}

    public void GoToPlace(int mapId){
        if (GameData._playerData.MapOpenState [mapId] == 0) {
            Debug.Log ("未知地域!");
            return;
        }
        mapGoing = LoadTxt.MapDic[mapId];
        if (mapId == 25)
        {
            CallInTunnelNotice();
        }
        else
        {
            OnTheRoad();
        }
    }

    void OnTheRoad(){
        StartCoroutine (StartLoading (60));
    }

	IEnumerator StartLoading(int costTime){
		int t = _loadingBar.CallInLoadingBar (costTime);
		yield return new WaitForSeconds (t);
		GoToMap();
	}

	void GoToMap(){
        int min = TravelTime (LoadTxt.MapDic [GameData._playerData.placeNowId].distances [mapGoing.id]);
        _gameData.ChangeTime (min);
        GameData._playerData.placeNowId = mapGoing.id;
        _gameData.StoreData ("PlaceNowId", mapGoing.id);

		_panelManager.MapGoing = mapGoing;
		if (mapGoing.id == 0) {
			_logManager.AddLog ("Get home.");
		} else
			_logManager.AddLog ("You get to " + mapGoing.name + ".");
		_panelManager.GoToPanel ("Place");

        //检测是否有新信息
        GetComponentInParent<SerendipityActions>().CheckSerendipity();
	}

	string GetTimeFormat(int m){
		string s = "";
		if (m < 60)
			s = m + "min";
		else if (m % 60 == 0)
			s = (int)(m / 60) + "h";
		else
			s = (int)(m / 60) + "h" + (m % 60) + "min";

		return s;
	}

	/// <summary>
	/// Travels the time,minutes.
	/// </summary>
	/// <returns>distance,km.</returns>
	int TravelTime(int distance){
		float speed = GameData._playerData.property [23];
		int min = (int)(distance * 60 / speed);
		return min;
	}

    void CallInTunnelNotice(){
        TunnelNotice.gameObject.SetActive (true);
        TunnelNotice.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
        TunnelNotice.gameObject.transform.DOBlendableScaleBy (new Vector3 (1f, 1f, 0f), 0.3f);
    }

    public void OnConfirmTunnelNotice(){
        CallOutComplete();
        OnTheRoad();
    }

    public void OnCancelTunnelNotice(){
        CallOutComplete();
    }

    void CallOutComplete(){
        TunnelNotice.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
        TunnelNotice.gameObject.SetActive (false);
    }
}
