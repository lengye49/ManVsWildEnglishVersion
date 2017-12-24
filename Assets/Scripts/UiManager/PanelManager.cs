using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class PanelManager : MonoBehaviour {

	public RectTransform Home;
	public RectTransform BedRoom;
	public RectTransform Warehouse;
	public RectTransform Workshop;
	public RectTransform Study;
	public RectTransform Farm;
	public RectTransform Pets;
	public RectTransform Well;
	public RectTransform Achievement;
	public RectTransform Altar;
	public RectTransform Death;
	public RectTransform Making;
	public RectTransform Backpack;
	public RectTransform Explore;
	public RectTransform Place;
	public RectTransform Battle;
	public LogManager _logManager;

	private RectTransform _FatherPanel;
	private RectTransform _PanelNow;
	private RectTransform _GrandFatherPanel;
	private float restPointLeftX = -3000f;
	private float restPointRightX = 3000f;
	private float tweenerTime = 0.5f;
	private Maps mapGoing;
	private GameData _gameData;

	public Maps MapGoing{
		set{ mapGoing = value;}
	}

	private TipManager _tipManager;

	void Start(){
		ResetPanels ();
		_tipManager = this.gameObject.GetComponentInParent<TipManager> ();
		_gameData = this.gameObject.GetComponentInParent<GameData> ();
		if (GameData._playerData.placeNowId == 0)
			GoToPanel ("Home");
		else {
			mapGoing = LoadTxt.MapDic [GameData._playerData.placeNowId];
			GoToPanel ("Place");
		}
			
	}

	void ResetPanels(){
		Home.localPosition = new Vector3 (-5000f, 0f, 0f);
		BedRoom.localPosition = new Vector3 (-5000f, 0f, 0f);
		Warehouse.localPosition = new Vector3 (-5000f, 0f, 0f);
		Workshop.localPosition = new Vector3 (-5000f, 0f, 0f);
		Study.localPosition = new Vector3 (-5000f, 0f, 0f);
		Farm.localPosition = new Vector3 (-5000f, 0f, 0f);
		Pets.localPosition = new Vector3 (-5000f, 0f, 0f);
		Well.localPosition = new Vector3 (-5000f, 0f, 0f);
		Achievement.localPosition = new Vector3 (-5000f, 0f, 0f);
		Altar.localPosition = new Vector3 (-5000f, 0f, 0f);
		Death.localPosition = new Vector3 (-5000f, 0f, 0f);
		Making.localPosition = new Vector3 (-5000f, 0f, 0f);
		Backpack.localPosition = new Vector3 (-5000f, 0f, 0f);
		Explore.localPosition = new Vector3 (-5000f, 0f, 0f);
		Place.localPosition = new Vector3 (-5000f, 0f, 0f);
		Battle.localPosition = new Vector3 (-5000f, 0f, 0f);
	}

	public void GoToPanel(string panelName){
		if (panelName=="Home" && GameData._playerData.placeNowId!=0) {
			Explore.GetComponent<ExploreActions> ().GoToPlace (0);
			return;
		}
		switch (panelName) {
			case "Home":
				Home.DOLocalMoveX (0, tweenerTime);
				Home.gameObject.GetComponentInChildren<HomeManager> ().UpdateContent ();
                if (_PanelNow == Place || _FatherPanel == Place)
					CheckThiefActivities ();
				_FatherPanel = null;
				if(_PanelNow!=Home)
					_PanelNow.DOLocalMoveX (restPointLeftX, tweenerTime);
				OnHome ();
                _PanelNow = Home;
                GameData._playerData.placeNowId = 0;
                _gameData.StoreData("PlaceNowId", 0);
                break;
            case "BedRoom":
                if (GameData._playerData.BedRoomOpen > 0)
                {
                    if (BedRoom.localPosition.x > 10)
                        _PanelNow.DOLocalMoveX(restPointLeftX, tweenerTime);
                    else
                        _PanelNow.DOLocalMoveX(restPointRightX, tweenerTime);
                    BedRoom.DOLocalMoveX(0, tweenerTime);
                    BedRoom.gameObject.GetComponent<RoomActions>().UpdateRoomStates();
                    _FatherPanel = Home;
                    _PanelNow = BedRoom;
                }
                else
                {
                    _tipManager.ShowBuildingConstruct("BedRoom");
                }
                break;
            case "Warehouse":
                if (GameData._playerData.WarehouseOpen > 0)
                {
                    if (Warehouse.localPosition.x > 10)
                    {
                        _PanelNow.DOLocalMoveX(restPointLeftX, tweenerTime);
                    }
                    else
                        _PanelNow.DOLocalMoveX(restPointRightX, tweenerTime);
                    Warehouse.DOLocalMoveX(0, tweenerTime);
                    Warehouse.gameObject.GetComponent<WarehouseActions>().UpdatePanel();
                    _FatherPanel = Home;
                    _PanelNow = Warehouse;
                }
                else
                {
                    _tipManager.ShowBuildingConstruct("Warehouse");
                }
                break;
            case "Kitchen":
                if (GameData._playerData.KitchenOpen > 0)
                {
                    if (Making.localPosition.x > 10)
                        _PanelNow.DOLocalMoveX(restPointLeftX, tweenerTime);
                    else
                        _PanelNow.DOLocalMoveX(restPointRightX, tweenerTime);
                    Making.DOLocalMoveX(0, tweenerTime);
                    this.gameObject.GetComponentInChildren<MakingActions>().UpdatePanel(panelName);
                    _FatherPanel = _PanelNow;
                    _PanelNow = Making;
                    break;
                }
                else
                {
                    _tipManager.ShowBuildingConstruct("Kitchen");
                }
                break;
            case "Workshop":
                if (GameData._playerData.WorkshopOpen > 0)
                {
                    if (Workshop.localPosition.x > 10)
                        _PanelNow.DOLocalMoveX(restPointLeftX, tweenerTime);
                    else
                        _PanelNow.DOLocalMoveX(restPointRightX, tweenerTime);
                    Workshop.DOLocalMoveX(0, tweenerTime);
                    this.gameObject.GetComponentInChildren<WorkshopActions>().UpdateWorkshop();
                    _FatherPanel = Home;
                    _PanelNow = Workshop;
                }
                else
                {
                    _tipManager.ShowBuildingConstruct("Workshop");
                }
                break;
            case "Well":
                if (GameData._playerData.WellOpen > 0)
                {
                    if (Well.localPosition.x > 10)
                        _PanelNow.DOLocalMoveX(restPointLeftX, tweenerTime);
                    else
                        _PanelNow.DOLocalMoveX(restPointRightX, tweenerTime);
                    Well.DOLocalMoveX(0, tweenerTime);
                    this.gameObject.GetComponentInChildren<WellActions>().UpdateWell();
                    _FatherPanel = Home;
                    _PanelNow = Well;
                }
                else
                {
                    _tipManager.ShowBuildingConstruct("Well");
                }
                break;
            case "Backpack":
                if (_PanelNow == Backpack)
                {
                    if (_FatherPanel == Explore)
                    {
                        if (GameData._playerData.placeNowId == 0)
                            GoToPanel("Home");
                        else
                            GoToPanel("Place");
                    }
                    else
                        GoToPanel("Father");
                    break;
                }
                if (Backpack.localPosition.x > 10)
                    _PanelNow.DOLocalMoveX(restPointLeftX, tweenerTime);
                else
                    _PanelNow.DOLocalMoveX(restPointRightX, tweenerTime);
                Backpack.DOLocalMoveX(0, tweenerTime);
                Backpack.gameObject.GetComponent<BackpackActions>().UpdataPanel();
                _GrandFatherPanel = _FatherPanel;
                _FatherPanel = _PanelNow;
                _PanelNow = Backpack;
                break;
            case "Explore":
                if (_PanelNow == Explore)
                {
					if (_FatherPanel == Backpack && GameData._playerData.placeNowId == 0)
						GoToPanel ("Home");
					else if (_FatherPanel == Backpack)
						GoToPanel ("Place");
					else
                        GoToPanel("Father");
                    break;
                }
                if (Explore.localPosition.x > 10)
                    _PanelNow.DOLocalMoveX(restPointLeftX, tweenerTime);
                else
                    _PanelNow.DOLocalMoveX(restPointRightX, tweenerTime);
                Explore.DOLocalMoveX(0, tweenerTime);
                Explore.gameObject.GetComponent<ExploreActions>().UpdateExplore();
                _GrandFatherPanel = _FatherPanel;
                _FatherPanel = _PanelNow;
                _PanelNow = Explore;
                break;
            case "Battle":
                if (Battle.localPosition.x > 10)
                    _PanelNow.DOLocalMoveX(restPointLeftX, tweenerTime);
                else
                    _PanelNow.DOLocalMoveX(restPointRightX, tweenerTime);
                Battle.DOLocalMoveX(0, tweenerTime);
                _GrandFatherPanel = _FatherPanel;
                _FatherPanel = _PanelNow;
                _PanelNow = Battle;
                break;
            case "Place":
                if (_PanelNow != Battle && _PanelNow != Backpack && _PanelNow != Explore)
                    Place.gameObject.GetComponent<PlaceActions>().PlayBackGroundMusic(mapGoing.id);

                if (mapGoing.id == 0)
                {
                    GoToPanel("Home");
                    break;
                }
                if (Place.localPosition.x > 0)
                    _PanelNow.DOLocalMoveX(restPointLeftX, tweenerTime);
                else
                    _PanelNow.DOLocalMoveX(restPointRightX, tweenerTime);	
                Place.DOLocalMoveX(0, tweenerTime);

                //如果当前面板是战斗面板，则不用更新进度
                //如果当前在place面板再打开背包或地图，关闭后也不刷新
                if (_PanelNow == Battle || (_PanelNow == Backpack && _FatherPanel == Place))
                {
                    Place.gameObject.GetComponent<PlaceActions>().UpdatePlace(mapGoing, false);
                }
                else
                {
                    Place.gameObject.GetComponent<PlaceActions>().UpdatePlace(mapGoing, true);
                }

                GameData._playerData.placeNowId = mapGoing.id;
                _gameData.StoreData("PlaceNowId", mapGoing.id);
                _GrandFatherPanel = _FatherPanel;
                _FatherPanel = _PanelNow;
                _PanelNow = Place;
                break;
            case "Study":
                if (GameData._playerData.StudyOpen > 0)
                {
                    if (Study.localPosition.x > 0)
                        _PanelNow.DOLocalMoveX(restPointLeftX, tweenerTime);
                    else
                        _PanelNow.DOLocalMoveX(restPointRightX, tweenerTime);
                    Study.DOLocalMoveX(0, tweenerTime);
                    Study.gameObject.GetComponent<StudyActions>().UpdateStudy();
                    _FatherPanel = Home;
                    _PanelNow = Study;
                }
                else
                {
                    _tipManager.ShowBuildingConstruct("Study");
                }
                break;
            case "Farm":
                if (GameData._playerData.FarmOpen > 0)
                {
                    if (Farm.localPosition.x > 0)
                        _PanelNow.DOLocalMoveX(restPointLeftX, tweenerTime);
                    else
                        _PanelNow.DOLocalMoveX(restPointRightX, tweenerTime);
                    Farm.DOLocalMoveX(0, tweenerTime);
                    Farm.gameObject.GetComponent<FarmActions>().UpdateFarm();
                    _FatherPanel = Home;
                    _PanelNow = Farm;
                }
                else
                {
                    _tipManager.ShowBuildingConstruct("Farm");
                }
                break;
            case "Pets":
                if (GameData._playerData.PetsOpen > 0)
                {
                    if (Pets.localPosition.x > 0)
                        _PanelNow.DOLocalMoveX(restPointLeftX, tweenerTime);
                    else
                        _PanelNow.DOLocalMoveX(restPointRightX, tweenerTime);
                    Pets.DOLocalMoveX(0, tweenerTime);
                    Pets.gameObject.GetComponent<PetsActions>().UpdatePets();
                    _FatherPanel = Home;
                    _PanelNow = Pets;
                }
                else
                {
                    _tipManager.ShowBuildingConstruct("Pets");
                }
                break;
            case "Achievement":
                if (GameData._playerData.AchievementOpen > 0)
                {
                    if (Achievement.localPosition.x > 0)
                        _PanelNow.DOLocalMoveX(restPointLeftX, tweenerTime);
                    else
                        _PanelNow.DOLocalMoveX(restPointRightX, tweenerTime);
                    Achievement.DOLocalMoveX(0, tweenerTime);
                    Achievement.gameObject.GetComponent<AchievementActions>().UpdateAchievement();
                    _FatherPanel = Home;
                    _PanelNow = Achievement;
                }
                else
                {
                    _tipManager.ShowBuildingConstruct("Achievement");
                }
                break;
            case "Altar":
                if (GameData._playerData.AltarOpen > 0)
                {
                    if (Altar.localPosition.x > 0)
                        _PanelNow.DOLocalMoveX(restPointLeftX, tweenerTime);
                    else
                        _PanelNow.DOLocalMoveX(restPointRightX, tweenerTime);
                    Altar.DOLocalMoveX(0, tweenerTime);
                    this.gameObject.GetComponentInChildren<AlterActions>().UpdateAltar();
				this.gameObject.GetComponentInChildren<AlterActions>().UpdateSacrifice();
                    _FatherPanel = Home;
                    _PanelNow = Altar;
                }
                else
                {
                    _tipManager.ShowBuildingConstruct("Altar");
                }
                break;
            case "Death":
                Death.DOLocalMoveX(0, tweenerTime);
                break;
            case "Melee":
            case "Ranged":
            case "Magic":
            case "Head":
            case "Body":
            case "Shoe":
            case "Accessory":
                if (Making.localPosition.x > 0)
                    _PanelNow.DOLocalMoveX(restPointLeftX, tweenerTime);
                else
                    _PanelNow.DOLocalMoveX(restPointRightX, tweenerTime);
                Making.DOLocalMoveX(0, tweenerTime);
                this.gameObject.GetComponentInChildren<MakingActions>().UpdatePanel(panelName);
                _FatherPanel = _PanelNow;
                _PanelNow = Making;
                break;
            case "Making":
                if (Making.localPosition.x > 0)
                    _PanelNow.DOLocalMoveX(restPointLeftX, tweenerTime);
                else
                    _PanelNow.DOLocalMoveX(restPointRightX, tweenerTime);
                Making.DOLocalMoveX(0, tweenerTime);
                _FatherPanel = _GrandFatherPanel;
                _PanelNow = Making;
                break;
            case "Father":
                GoToPanel(_FatherPanel.name);
                break;
            default:
                Debug.Log("The panel name is wrong with " + panelName);
                break;
		}
	}

	/// <summary>
	/// 用于销毁所有临时生成的游戏物体
	/// </summary>
	void OnHome(){
		Achievement.GetComponent<AchievementActions> ().OnLeave ();
		Backpack.GetComponent<BackpackActions> ().OnLeave ();
		Farm.GetComponent<FarmActions> ().OnLeave ();
		Making.GetComponent<MakingActions> ().OnLeave ();
		Pets.GetComponent<PetsActions> ().OnLeave ();
		Study.GetComponent<StudyActions> ().OnLeave ();
		Warehouse.GetComponent<WarehouseActions> ().OnLeave ();
	}

	void CheckThiefActivities(){

        //开始日期
		if (GameData._playerData.dayNow <= GameConfigs.StartThiefEvent)
			return;
        //上次盗贼光顾时间
		if (GameData._playerData.lastThiefTime >= GameData._playerData.dayNow - 2)
			return;

        //触发盗贼概率
        int r=Algorithms.GetIndexByRange (0, 100);
//        Debug.Log(r);
        if (r >= (int)(GameData._playerData.ThiefDefence * 100f))
            return;

		GameData._playerData.lastThiefTime = GameData._playerData.dayNow;
		_gameData.StoreData ("LastThiefTime", GameData._playerData.lastThiefTime);

		Thief[] tList = LoadTxt.GetThiefList ();
		int[] weight = new int[tList.Length];
		int[] ids = new int[weight.Length];
		int i = 0;
		for (int key = 0;key<tList.Length;key++) {
			weight [i] = tList[key].weight;
			ids [i] = key;
			i++;
		}
		i = Algorithms.GetResultByWeight (weight);
		Thief _thisThief = tList[ids [i]];

		int antiAlert = LoadTxt.GetMonster(_thisThief.monsterId).level;
		int alert = GetGuardAlert ();

		//发现概率
		int p = 0;
		if (alert > 2 * antiAlert)
			p = 10000;
		else if (antiAlert > alert * 5)
			p = 0;
		else
			p = alert * 10000 / (alert + antiAlert);

		r = Random.Range (0, 10000);

		if (r <= p) {
			CatchThief (_thisThief);
		} else {
			BeStolen (_thisThief);
		}
	}

	int GetGuardAlert(){
		//警惕性 = 宠物的等级
		int alert = 0;
		foreach (int key in GameData._playerData.Pets.Keys) {
			if (GameData._playerData.Pets [key].state == 2) {
				alert += GameData._playerData.Pets [key].alertness;
			}
		}
		//获得警惕性之和再除以3
		return (int)(alert / 3f);
	}

	void CatchThief(Thief t){
		
		string s = "You get ";
		Dictionary<int,int> drop = Algorithms.GetReward (LoadTxt.GetMonster(t.monsterId).drop);
		foreach (int key in drop.Keys) {
			_gameData.AddItem (key * 10000, drop [key]);
			s += LoadTxt.MatDic [key].name + " ×" + drop [key];
			break;
		}
        if (drop.Count <= 0)
            s = "";
		_logManager.AddLog (t.name + " tried to steal, but was caught by your guard." + s);

		//Achievement
		this.gameObject.GetComponentInParent<AchieveActions>().CatchThief(t.id);
	}

	void BeStolen(Thief t){

		Dictionary<int,int> target = new Dictionary<int, int> ();

		int totalValue = 0;//计算总价值
		foreach (int key in GameData._playerData.wh.Keys) {
			totalValue += LoadTxt.MatDic [(int)(key / 10000)].price * GameData._playerData.wh [key];
		}

		int max = (int)(10000f / GameData._playerData.wh.Count);//把仓库物品分成max份
		int num = 0;
		int value = 0;
		int i = 1;

		foreach (int key in GameData._playerData.wh.Keys) {
			int r = Algorithms.GetIndexByRange (0, 10000);
			if (r < i * max && r > ((i - 1) * max)) {
				float f = Algorithms.GetIndexByRange (0, 30) / 100f * GameData._playerData.wh [key];
				f *= GameData._playerData.TheftLossDiscount;

				num = (int)Mathf.Round (f);
				if (num > 0) {
					target.Add (key, num);
					value += LoadTxt.MatDic [(int)(key / 10000)].price * num;
				}
			}

			//价值保护
			if (value >= totalValue * 0.2f || value >= 2000)
				break;
		}
		_gameData.DeleteItemInWh (target);
		string s = "";
		foreach (int key in target.Keys) {
			s += LoadTxt.MatDic [(int)(key / 10000)].name + " -" + target [key] + ",";
		}
		if (s != "") {
			s = s.Substring (0, s.Length - 1) + "。";
			_logManager.AddLog ("Warning!" + t.name + "robbed your house." + s);
		} else {
			_logManager.AddLog ("Warning!" + t.name + "robbed your house, but got nothing.");
		}
		_logManager.AddLog ("You should upgrade techs or arrange better guards.");
	}
}

