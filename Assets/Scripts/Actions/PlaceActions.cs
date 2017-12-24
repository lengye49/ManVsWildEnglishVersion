using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class PlaceActions : MonoBehaviour {

	public GameObject contentP;
	public RectTransform resourceDetail;
	public RectTransform observeDetail;
	public RectTransform placeRect;
	public RectTransform dungeonRect;
	public BattleActions _battleActions;
	public LoadingBar _loadingBar;
	public LogManager _logManager;
    public RectTransform Complete;
    public GameObject completeDungeon;
    public GameObject completeTunnel;
    public GameObject completeFinal;
    public ExploreActions _explore;

	private GameObject placeCell;
	private ArrayList placeCells = new ArrayList ();
	private Places _place;
	private LoadTxt _loadTxt;
	private FloatingActions _floating;
	private GameData _gameData;
	private PlaceUnit _puNow;
	private Maps _mapNow;
	private PanelManager _panelManager;

	private int[] dungeonCellState;//0 Cannot open;1 Can open;2 Opened;3 Door
	public Button[] dungeonCells;
	public Sprite frontImage;
	public Sprite backImage;
	public Sprite nextLevelImage;
    public Text dungeonLevelText;
	private int dungeonLevel;
	private int thisExitIndex;

    public int tunnelLevel;

	void Awake(){
		placeCell = Instantiate (Resources.Load ("placeCell")) as GameObject;
	}

	void Start(){
		placeCell.SetActive (false);
		_loadTxt = this.gameObject.GetComponentInParent<LoadTxt> ();
		_floating = GameObject.Find ("FloatingSystem").GetComponent<FloatingActions> ();
		_gameData = this.gameObject.GetComponentInParent<GameData> ();
		_panelManager = this.gameObject.GetComponentInParent<PanelManager> ();

        Complete.localPosition = Vector3.zero;
        Complete.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
        Complete.gameObject.SetActive (false);
	}

    /// <summary>
    /// 更新地图显示，否的情况是不更新地牢
    /// </summary>
    /// <param name="m">M.</param>
    /// <param name="newPlace">If set to <c>true</c> new place.</param>
	public void UpdatePlace(Maps m,bool newPlace){
		_mapNow = m;
		_place = LoadTxt.PlaceDic [m.id];

		if (_mapNow.id == 21) {
			if (!newPlace)
				return;
			dungeonLevel = GameData._playerData.dungeonLevelMax + 1;
            if (dungeonLevel > 100)
            {
                dungeonLevel = 100;
                _gameData.CloseMap(21);
            }
			InitializeDungeon ();
        } else if (_mapNow.id == 25){
            if (!newPlace)
                return;
			tunnelLevel = GameData._playerData.tunnelLevelMax + 1;
            if (tunnelLevel > 10)
            {
                tunnelLevel = 10;
                _gameData.CloseMap(25);
            }
			InitializeTunnel ();
        }else {
			if (newPlace) {
				SetDetailPosition ();
				SetPlace (_mapNow);
			}
		}
	}

	public void PlayBackGroundMusic(int mapId){
		int isRain = Random.Range (0, 100);
		string bgSoundName = "";
		if (mapId == 0)
			bgSoundName = "";
		else if (isRain < 10)
			bgSoundName = "rain_small";
		else if (isRain < 15)
			bgSoundName = "rain_heavy";
		else {
			switch (mapId) {
			case 0:
				bgSoundName = "";
				break;
			case 1:
				bgSoundName = "river";
				break;
			case 2:
				bgSoundName = "woods";
				break;
			case 3:
				bgSoundName = "mountain";
				break;
			case 4:
				bgSoundName = "";
				break;
			case 5:
				bgSoundName = "mountain";
				break;
			case 6:
				bgSoundName = "cave";
				break;
			case 7:
				bgSoundName = "island";
				break;
			case 8:
			case 9:
			case 10:
				bgSoundName = "";
				break;
			case 11:
				bgSoundName = "mountain";
				break;
			case 12:
				bgSoundName = "";
				break;
			case 13:
				bgSoundName = "woods";
				break;
			case 14:
				bgSoundName = "woods";
				break;
			case 15:
				bgSoundName = "cave";
				break;
			case 16:
			case 17:
			case 18:
				bgSoundName = "";
				break;
			case 19:
				bgSoundName = "hawk";
				break;
			case 20:
				bgSoundName = "";
				break;
			case 21:
				bgSoundName = "dungeon";
				break;
			case 22:
			case 23:
				bgSoundName = "island";
				break;
			case 24:
				bgSoundName = "sea";
				break;
			default:
				break;
			}
		}

		this.gameObject.GetComponentInParent<PlaySound> ().PlayEnvironmentSound (bgSoundName);
	}

	//******************************************地牢地图*******************************************

	/// <summary>
	/// 直接进入第lv层地牢
	/// </summary>
	/// <param name="lv">Lv.</param>
	public void UpdatePlace(int lv){
        dungeonLevel = lv;
		InitializeDungeon ();
	}

	void InitializeDungeon(){
        dungeonLevelText.text = "失落之地 - " + dungeonLevel + "/100层";

		dungeonRect.localPosition = Vector3.zero;
		placeRect.localPosition = new Vector3 (-10000, 0, 0);
		dungeonCellState = new int[20];
		for (int i = 0; i < dungeonCellState.Length; i++) {
			if (i == 0)
				dungeonCellState [i] = 1;
			else
				dungeonCellState [i] = 0;
		}
		SetDungeonState ();
		thisExitIndex = Algorithms.GetIndexByRange (1, 20);
	}

	void SetDungeonState(){
		for(int i=0;i<dungeonCells.Length;i++){
			SetDungeonState (i);
		}
	}

	void SetDungeonState(int i){
		if (dungeonCellState [i] == 0) {
			dungeonCells [i].image.sprite = frontImage;
			dungeonCells [i].image.color = new Color (200, 200, 200, 128);
			dungeonCells [i].interactable = false;
		} else if (dungeonCellState [i] == 1) {
			dungeonCells [i].image.sprite = frontImage;
			dungeonCells [i].image.color = new Color (255, 255, 255, 255);
			dungeonCells [i].interactable = true;
		} else if (dungeonCellState [i] == 2) {
			dungeonCells [i].image.sprite = backImage;				
			dungeonCells [i].image.color = new Color (255, 255, 255, 255);
			dungeonCells [i].interactable = true;			
		} else if (dungeonCellState [i] == 3) {
			dungeonCells [i].image.sprite = nextLevelImage;				
			dungeonCells [i].image.color = new Color (255, 255, 255, 255);
			dungeonCells [i].interactable = true;	
		}
	}

	public void OpenDungeonCover(int index){
        _gameData.ChangeTime(10);

        if (dungeonCellState[index] == 1)
        {
            if (index == thisExitIndex)
            {
                dungeonCellState[index] = 3;
            }
            else
            {
                dungeonCellState[index] = 2;

                if (_mapNow.id == 21)
                    DungeonEvent();
                else
                    TunnelEvent();
            }
            CheckRoad(index);
            ChangeImage(index);
        }
        else if (dungeonCellState[index] == 3 && _mapNow.id == 21)
        {
            if (GameData._playerData.dungeonLevelMax < dungeonLevel)
            {
                GameData._playerData.dungeonLevelMax = dungeonLevel;
                _gameData.StoreData("DungeonLevelMax", dungeonLevel);
            }

            if (dungeonLevel < 100)
            {
                dungeonLevel++;
                InitializeDungeon();
            }
            else
            {
                _gameData.OpenMap(25);
                _gameData.CloseMap(21);
                CallInComplete(0);
            }

            //成就
            GetComponentInParent<AchieveActions>().GetToNewDungeon(dungeonLevel);
        }
        else if (dungeonCellState[index] == 3 && _mapNow.id == 25)
        {
            if (GameData._playerData.tunnelLevelMax < tunnelLevel)
            {
                GameData._playerData.tunnelLevelMax = tunnelLevel;
                _gameData.StoreData("TunnelLevelMax", tunnelLevel);
            }
            if (tunnelLevel < 10)
            {
                tunnelLevel++;
                InitializeTunnel();
            }
            else
            {
                CallInComplete(1);
                _gameData.CloseMap(25);
                _gameData.OpenMap(26);
                _logManager.AddLog("通关模式已开启！");
            }
        }
	}

    /// <summary>
    /// 打开信息面板，0地牢通关 1隧道通关 2最终通关
    /// </summary>
    /// <param name="t">T.</param>
    public void CallInComplete(int t){
        Complete.gameObject.SetActive (true);
        Complete.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
        Complete.gameObject.transform.DOBlendableScaleBy (new Vector3 (1f, 1f, 0f), 0.3f);

        if (t == 0)
        {
            completeDungeon.SetActive(true);
            completeTunnel.SetActive(false);
            completeFinal.SetActive(false);
        }
        else if (t == 1)
        {
            completeDungeon.SetActive(false);
            completeTunnel.SetActive(true);
            completeFinal.SetActive(false);
        }
        else
        {
            completeDungeon.SetActive(false);
            completeTunnel.SetActive(false);
            completeFinal.SetActive(true);
        }
    }

   void CallOutComplete(){
        Complete.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
        Complete.gameObject.SetActive (false);
    }

    public void OnConfirmComplete(){
        CallOutComplete();
        _explore.GoToPlace(0);
    }

	void DungeonEvent(){
		
		//Get Dungeon Rewards According to the Level;
		//1 Reward 10%, 2 Monster 15%, 3 Buff&Debuff 10%, 4 Nothing: Text 13%, 5 Nothing 
		int r = Algorithms.GetIndexByRange(0,100);
		if (r < 10) {
			int r4 = (int)((dungeonLevel-1) / 10) + 1;
			int matId = Algorithms.GetResultByDic (LoadTxt.GetDungeonTreasure(r4).reward);
			int num = 1;
			if (LoadTxt.MatDic [matId].price < 100) {
				num = Algorithms.GetIndexByRange (1, 4);
			}
			_gameData.AddItem (matId * 10000, num);
            string s = LoadTxt.MatDic[matId].name + "+" + num;
//            _logManager.AddLog("你捡到了" + s + "。");
			_floating.CallInFloating ("你捡到了" + s + "。", 0);

		} else if (r < 25) {
			int r1 = Algorithms.GetIndexByRange (1, 3);
			Monster[] ms = new Monster[r1];
			for (int i = 0; i < r1; i++) {
				ms [i] = GetNewMonster (1);
			}
			_panelManager.GoToPanel ("Battle");
			r1 = Algorithms.GetIndexByRange(0,100);
			bool isSpoted = r1 < (GameData._playerData.SpotRate * 100);
			_battleActions.InitializeBattleField (ms, isSpoted);
		} else if (r < 35) {
			//Hp+3~5 10%,Hp-2~4 10%,Spirit+3~5 10% Spirit-2~4 10%,Water+4~8 10%,Food+4~8 10%,Temp+1~3 10%,Temp-1~3 10%,Hp+99 5%,Spirit+99 5%
			//HpMax+1 1%,SpiritMax+1 1%,StrengthMax+1 2%,WaterMax+1 2%,FoodMax+1 2%,TempMax+1 1%,TempMin-1 1%
			int r2 = Algorithms.GetIndexByRange (0, 100);
			int r3;

            //******************************************回血**********
            if (r2 < 5)
            {
                r3 = Algorithms.GetIndexByRange(3, 6);
                _gameData.ChangeProperty(0, r3);
//                _logManager.AddLog("你捡到一块绷带，生命+" + r3);
				_floating.CallInFloating ("你捡到一块绷带，生命+" + r3, 0);
            }
            else if (r2 < 8)
            {
                r3 = Algorithms.GetIndexByRange(10, 20);
                _gameData.ChangeProperty(0, r3);
//                _logManager.AddLog("一只猴子朝你扔了六个核桃，生命+" + r3);
				_floating.CallInFloating ("一只猴子朝你扔了六个核桃，生命+" + r3, 0);

            }
            else if (r2 < 10)
            {
                r3 = Algorithms.GetIndexByRange(20, 60);
                _gameData.ChangeProperty(0, r3);
//                _logManager.AddLog("一阵愉悦而纯净的力量扑面而来，生命+" + r3);
				_floating.CallInFloating("一阵愉悦而纯净的力量扑面而来，生命+" + r3,0);
            }

            //******************************************扣血**********
            else if (r2 < 14)
            {
                r3 = -Algorithms.GetIndexByRange(1, 5);
                _gameData.ChangeProperty(0, r3);
//                _logManager.AddLog("不小心踩到了陷阱，生命" + r3);
				_floating.CallInFloating("不小心踩到了陷阱，生命" + r3,1);
            }
            else if (r2 < 18)
            {
                r3 = -Algorithms.GetIndexByRange(3, 7);
                _gameData.ChangeProperty(0, r3);
//                _logManager.AddLog("打开宝箱，一股腐败气息铺面而来，生命" + r3 );
				_floating.CallInFloating("打开宝箱，一股腐败气息铺面而来，生命" + r3,1);
            }
            else if (r2 < 20) {
                r3 = -Algorithms.GetIndexByRange (4, 8);
                _gameData.ChangeProperty (0, r3);
//                _logManager.AddLog("你遭到莫名的袭击，生命" + r3);
				_floating.CallInFloating("你遭到莫名的袭击，生命" + r3,1);
            }  

            //******************************************恢复精神**********
            else if (r2 < 24) {
				r3 = Algorithms.GetIndexByRange (2, 6);
				_gameData.ChangeProperty (2, r3);
//                _logManager.AddLog("一个友善的精灵朝你使用精神之泉，精神+" + r3 );
				_floating.CallInFloating("一个友善的精灵朝你使用精神之泉，精神+" + r3 ,0);
            }
            else if (r2 < 28) {
                r3 = Algorithms.GetIndexByRange (5, 10);
                _gameData.ChangeProperty (2, r3);
//                _logManager.AddLog("你被远古战士雕像激励，精神+" + r3 );
				_floating.CallInFloating("你被远古战士雕像激励，精神+" + r3 ,0);
            }  
            else if (r2 < 30) {
                r3 = Algorithms.GetIndexByRange (10, 30);
                _gameData.ChangeProperty (2, r3);
//                _logManager.AddLog("你找到了精神之泉，精神+" + r3 );
				_floating.CallInFloating("你找到了精神之泉，精神+" + r3,0);
            } 

            //******************************************扣除精神**********
            else if (r2 < 35) {
				r3 = -Algorithms.GetIndexByRange (1, 4);
				_gameData.ChangeProperty (2, r3);
//				_logManager.AddLog("你感到一阵头晕目眩，精神" + r3 );
				_floating.CallInFloating ("你感到一阵头晕目眩，精神" + r3, 1);
			}
			else if (r2 < 40) {
				r3 = -Algorithms.GetIndexByRange (3, 6);
				_gameData.ChangeProperty (2, r3);
//				_logManager.AddLog("你受到恶魔雕像的惊吓，精神" + r3 );
				_floating.CallInFloating("你受到恶魔雕像的惊吓，精神" + r3,1);
			}

			//******************************************恢复食物**********
			else if (r2 < 43) {
				r3 = Algorithms.GetIndexByRange (2, 6);
				_gameData.ChangeProperty (4, r3);
//				_logManager.AddLog("你捡到了一片干硬的面包，食物+" + r3 );
				_floating.CallInFloating("你捡到了一片干硬的面包，食物+" + r3 ,0);
			} 
			else if (r2 < 50) {
				r3 = Algorithms.GetIndexByRange (5, 10);
				_gameData.ChangeProperty (4, r3);
//				_logManager.AddLog("好心的探险者分给你一些肉干，食物+" + r3 );
				_floating.CallInFloating("好心的探险者分给你一些肉干，食物+" + r3 ,0);
			} 

			//******************************************恢复水分**********
			else if (r2 < 54) {
				r3 = Algorithms.GetIndexByRange (2, 6);
				_gameData.ChangeProperty (6, r3);
//				_logManager.AddLog("捡到一个破旧的水囊，水+" + r3 );
				_floating.CallInFloating("捡到一个破旧的水囊，水+" + r3,0);
			} 
			else if (r2 < 58) {
				r3 = Algorithms.GetIndexByRange (5, 10);
				_gameData.ChangeProperty (6, r3);
//				_logManager.AddLog("好心的探险者分给你一些水，水+" + r3 );
				_floating.CallInFloating("好心的探险者分给你一些水，水+" + r3,0);
			}
			else if (r2 < 60) {
				r3 = Algorithms.GetIndexByRange (10, 25);
				_gameData.ChangeProperty (6, r3);
//				_logManager.AddLog("遇到一个地下泉眼，水+" + r3 );
				_floating.CallInFloating ("遇到一个地下泉眼，水+" + r3, 0);
			}

			//******************************************改变体温**********
			else if (r2 < 65) {
				r3 = Algorithms.GetIndexByRange (1, 3);
				_gameData.ChangeProperty (10, r3);
//				_logManager.AddLog("趟过一个温泉，温度+" + r3 );
				_floating.CallInFloating ("趟过一个温泉，温度+" + r3, 0);
			} 
			else if (r2 < 70) {
				r3 = Algorithms.GetIndexByRange (2, 5);
				_gameData.ChangeProperty (10, r3);
				_gameData.ChangeProperty(0, -r3);
//				_logManager.AddLog("地面突然冒出一团岩浆，温度+" + r3 + " ,生命-" + r3);
				_floating.CallInFloating ("地面突然冒出一团岩浆，温度+" + r3 + " ,生命-" + r3, 1);
			} 
			else if (r2 < 75) {
				r3 = -Algorithms.GetIndexByRange (1, 4);
				_gameData.ChangeProperty (10, r3);
//				_logManager.AddLog("路过冰面，摔了一跤，温度" + r3 );
				_floating.CallInFloating ("路过冰面，摔了一跤，温度" + r3, 1);
			} 
			else if (r2 < 80) {
				r3 = -Algorithms.GetIndexByRange (1, 4);
				_gameData.ChangeProperty (10, r3);
				_gameData.ChangeProperty(2, r3);
//				_logManager.AddLog("一阵刺骨的阴风吹过，温度" + r3 + " ,精神" + r3 );
				_floating.CallInFloating("一阵刺骨的阴风吹过，温度" + r3 + " ,精神" + r3,1);
			} 

			//******************************************特殊回复**********
			else if (r2 < 82) {
				_gameData.ChangeProperty (0, 999);
//				_logManager.AddLog("受到生命的祝福，生命回满。");
				_floating.CallInFloating ("受到生命的祝福，生命回满。", 0);
			} 
			else if (r2 < 84) {
				_gameData.ChangeProperty (2, 999);
//				_logManager.AddLog("受到生命的祝福，精神回满。");
				_floating.CallInFloating ("受到生命的祝福，精神回满。", 0);
			}
		} 
	}
        

	Monster GetNewMonster(int dType){
		int minLv = 1;
		int maxLv = 35;
		if (dType == 1) {
			maxLv += (int)(dungeonLevel / 10) * 5;
			if (dungeonLevel % 10 == 0) {
				minLv = maxLv - 5;
			} else {
				minLv = maxLv - 25;
			}
		} else {
			maxLv = 100;
		}

		ArrayList monsterList = new ArrayList ();
		monsterList = LoadTxt.GetMonster (minLv, maxLv, dType);

//        if (_mapNow.id == 21)
//        {
//			monsterList = LoadTxt.GetMonster (minLv, maxLv, 1);
//        }
//        else if (_mapNow.id == 25)
//        {
//			monsterList = LoadTxt.GetMonster (minLv, maxLv, 2);
//        }
//
		Debug.Log ("monsterList = " + monsterList.Count);
		Monster m = new Monster ();
		if (monsterList.Count > 0) {
			int index = Random.Range (0, monsterList.Count - 1);
			Debug.Log (index);
			m = monsterList [index] as Monster;
		} else {
			m = LoadTxt.GetMonster(100);
		}

		return m;
	}

	void CheckRoad(int index){
		if (index >= 5) {
			if (dungeonCellState [index - 5] == 0) {
				dungeonCellState [index - 5] = 1;
				SetDungeonState (index - 5);
			}
		}
		if (index <= 14) {
			if (dungeonCellState [index + 5] == 0) {
				dungeonCellState [index + 5] = 1;
				SetDungeonState (index + 5);
			}
		}
		if (index % 5 != 0) {
			if (dungeonCellState [index - 1] == 0) {
				dungeonCellState [index - 1] = 1;
				SetDungeonState (index - 1);
			}
		}
		if (index % 5 != 4) {
			if (dungeonCellState [index + 1] == 0) {
				dungeonCellState [index + 1] = 1;
				SetDungeonState (index + 1);
			}
		}
	}

	void ChangeImage(int index){
		dungeonCells [index].transform.DOBlendableRotateBy (new Vector3 (0, 90f, 0), 0.4f);
		StartCoroutine (WaitAndTurn (index));
	}

	IEnumerator WaitAndTurn(int index){
		yield return new WaitForSeconds (0.4f);
		dungeonCells [index].gameObject.SetActive (false);
		dungeonCells [index].image.sprite = (dungeonCellState [index] == 2) ? backImage : nextLevelImage;
		dungeonCells [index].transform.Rotate (new Vector3 (0, -180, 0));
		dungeonCells [index].gameObject.SetActive (true);
		dungeonCells [index].transform.DOBlendableRotateBy (new Vector3 (0, 0, 0), 0.3f);
	}

    //******************************************隧道地图*******************************************

    void InitializeTunnel(){
        dungeonLevelText.text = "时空隧道 - " + tunnelLevel + "/10层";

        dungeonRect.localPosition = Vector3.zero;
        placeRect.localPosition = new Vector3 (-10000, 0, 0);
        dungeonCellState = new int[20];
        for (int i = 0; i < dungeonCellState.Length; i++) {
            if (i == 0)
                dungeonCellState [i] = 1;
            else
                dungeonCellState [i] = 0;
        }
        SetDungeonState ();
        thisExitIndex = Algorithms.GetIndexByRange (1, 20);
    }

    void TunnelEvent(){

        //Get Tunnel Rewards According to the Level;
        //1 Reward 0%, 2 Monster 30%, 3 Buff&Debuff 20%, 4 Nothing: Text 13%, 5 Nothing 
        int r = Algorithms.GetIndexByRange(0,100);
        if (r < 0) {
            int r4 = tunnelLevel;
			int matId = Algorithms.GetResultByDic (LoadTxt.GetDungeonTreasure(r4).reward);
            int num = 1;
            if (LoadTxt.MatDic [matId].price < 100) {
                num = Algorithms.GetIndexByRange (1, 4);
            }
            _gameData.AddItem (matId * 10000, num);
            string s = LoadTxt.MatDic[matId].name + "+" + num;
//            _logManager.AddLog("你捡到了" + s + "。");
			_floating.CallInFloating("你捡到了" + s + "。",0);

        } else if (r < 20) {
            int r1 = Algorithms.GetIndexByRange (1, 3);
            Monster[] ms = new Monster[r1];
            for (int i = 0; i < r1; i++) {
                ms [i] = GetNewMonster (2);
            }
            _panelManager.GoToPanel ("Battle");
            r1 = Algorithms.GetIndexByRange(0,100);
            bool isSpoted = r1 < (GameData._playerData.SpotRate * 100);
            _battleActions.InitializeBattleField (ms, isSpoted);
        } else if (r < 50) {
            //Hp+3~5 10%,Hp-2~4 10%,Spirit+3~5 10% Spirit-2~4 10%,Water+4~8 10%,Food+4~8 10%,Temp+1~3 10%,Temp-1~3 10%,Hp+99 5%,Spirit+99 5%
            //HpMax+1 1%,SpiritMax+1 1%,StrengthMax+1 2%,WaterMax+1 2%,FoodMax+1 2%,TempMax+1 1%,TempMin-1 1%
            int r2 = Algorithms.GetIndexByRange (0, 100);
            int r3;

            //******************************************掉血**********
            if (r2 < 0)
            {
                r3 = Algorithms.GetIndexByRange(3, 6);
                _gameData.ChangeProperty(0, r3);
//                _logManager.AddLog("你捡到一块绷带，生命+" + r3);
				_floating.CallInFloating ("你捡到一块绷带，生命+" + r3, 0);
            }
            else if (r2 < 5)
            {
                r3 = Algorithms.GetIndexByRange(10, 30);
                _gameData.ChangeProperty(0, r3);
//                _logManager.AddLog("恢复之光照耀着你，生命+" + r3);
				_floating.CallInFloating("恢复之光照耀着你，生命+" + r3,0);
            }
            else if (r2 < 10)
            {
                r3 = Algorithms.GetIndexByRange(20, 60);
                _gameData.ChangeProperty(0, r3);
//                _logManager.AddLog("一阵愉悦而纯净的力量扑面而来，生命+" + r3);
				_floating.CallInFloating("一阵愉悦而纯净的力量扑面而来，生命+" + r3,0);
            }

            //******************************************回血**********
            else if (r2 < 15)
            {
                r3 = -Algorithms.GetIndexByRange(1, 5);
                _gameData.ChangeProperty(0, r3);
//                _logManager.AddLog("不小心踩到了陷阱，生命" + r3);
				_floating.CallInFloating("不小心踩到了陷阱，生命" + r3,1);
            }
            else if (r2 < 20) {
                r3 = -Algorithms.GetIndexByRange (4, 8);
                _gameData.ChangeProperty (0, r3);
//                _logManager.AddLog("你碰到了时空裂缝，生命" + r3);
				_floating.CallInFloating ("你碰到了时空裂缝，生命" + r3, 1);
            }  
            else if (r2 < 24) {
                r3 = Algorithms.GetIndexByRange (5, 10);
                _gameData.ChangeProperty (2, r3);
//                _logManager.AddLog("你被远古战士雕像激励，精神+" + r3 );
				_floating.CallInFloating ("你被远古战士雕像激励，精神+" + r3, 0);
            }  
            else if (r2 < 30) {
                r3 = Algorithms.GetIndexByRange (10, 30);
                _gameData.ChangeProperty (2, r3);
//                _logManager.AddLog("你找到了精神之泉，精神+" + r3 );
				_floating.CallInFloating("你找到了精神之泉，精神+" + r3,0);
            } 

            //******************************************扣除精神**********
            else if (r2 < 40) {
                r3 = -Algorithms.GetIndexByRange (1, 4);
                _gameData.ChangeProperty (2, r3);
//                _logManager.AddLog("你感到一阵头晕目眩，精神" + r3 );
				_floating.CallInFloating ("你感到一阵头晕目眩，精神" + r3, 1);
            }
            //******************************************恢复食物**********
            else if (r2 < 43) {
                r3 = Algorithms.GetIndexByRange (2, 6);
                _gameData.ChangeProperty (4, r3);
//                _logManager.AddLog("你捡到了一片干硬的面包，食物+" + r3 );
				_floating.CallInFloating ("你捡到了一片干硬的面包，食物+" + r3, 0);
            } 
            else if (r2 < 50) {
                r3 = Algorithms.GetIndexByRange (5, 10);
                _gameData.ChangeProperty (4, r3);
//                _logManager.AddLog("好心的时空旅者分给你一些肉干，食物+" + r3 );
				_floating.CallInFloating ("好心的时空旅者分给你一些肉干，食物+" + r3, 0);
            } 

            //******************************************恢复水分**********
            else if (r2 < 54) {
                r3 = Algorithms.GetIndexByRange (2, 6);
                _gameData.ChangeProperty (6, r3);
//                _logManager.AddLog("捡到一个破旧的水囊，水+" + r3 );
				_floating.CallInFloating("捡到一个破旧的水囊，水+" + r3 ,0);
            } 
            else if (r2 < 60) {
                r3 = Algorithms.GetIndexByRange (5, 10);
                _gameData.ChangeProperty (6, r3);
//                _logManager.AddLog("好心的时空旅者分给你一些水，水+" + r3 );
				_floating.CallInFloating("好心的时空旅者分给你一些水，水+" + r3,0 );
            }
            //******************************************改变体温**********
            else if (r2 < 70) {
                r3 = Algorithms.GetIndexByRange (2, 5);
                _gameData.ChangeProperty (10, r3);
                _gameData.ChangeProperty(0, -r3);
//                _logManager.AddLog("地面突然冒出一团热量，温度+" + r3 + " ,生命-" + r3);
				_floating.CallInFloating("地面突然冒出一团热量，温度+" + r3 + " ,生命-" + r3,1);
            } 
            else if (r2 < 80) {
                r3 = -Algorithms.GetIndexByRange (1, 4);
                _gameData.ChangeProperty (10, r3);
                _gameData.ChangeProperty(2, r3);
//                _logManager.AddLog("一阵刺骨的阴风吹过，温度" + r3 + " ,精神" + r3 );
				_floating.CallInFloating ("一阵刺骨的阴风吹过，温度" + r3 + " ,精神" + r3, 1);
            } 

            //******************************************特殊回复**********
            else if (r2 < 85) {
                _gameData.ChangeProperty (0, 999);
//                _logManager.AddLog("受到生命的祝福，生命回满。");
				_floating.CallInFloating("受到生命的祝福，生命回满。",0);
            } 
            else if (r2 < 90) {
                _gameData.ChangeProperty (2, 999);
//                _logManager.AddLog("受到精神的祝福，精神回满。");
				_floating.CallInFloating("受到精神的祝福，精神回满。",0);
            }else {
                Debug.Log ("Nothing Happened");
            }
        } 
        else {
            Debug.Log ("Nothing Happened");
        }
    }

	//******************************************正常地图*******************************************

	void SetPlace(Maps m){
		dungeonRect.localPosition = new Vector3 (-6000, 420, 0);
		placeRect.localPosition = new Vector3 (0, 420, 0);

		int count = 0;

		for (int i = 0; i < _place.placeUnits.Count; i++) {
			PlaceUnit pu = _place.placeUnits [i] as PlaceUnit;
			string[] s = pu.actionParam.Split (';');
			if (s [0] == "1")
				count++;
		}

		for (int i = 0; i < placeCells.Count; i++) {
			GameObject o = placeCells [i] as GameObject;
			o.SetActive (true);
			ClearContents (o);
		}

		if (count > placeCells.Count) {
			for (int i = placeCells.Count; i < count; i++) {
				GameObject o = Instantiate (placeCell) as GameObject;
				o.SetActive (true);
				o.transform.SetParent (contentP.transform);
				o.transform.localPosition = Vector3.zero;
				o.transform.localScale = Vector3.one;
				placeCells.Add (o);
				ClearContents (o);
			}
		}
			
        int j = 0;
		for (int i = 0; i < _place.placeUnits.Count; i++) {
			PlaceUnit pu = _place.placeUnits [i] as PlaceUnit;
            string[] s = pu.actionParam.Split (';');
            if (s[0] == "1")
            {
                GameObject o = placeCells [j] as GameObject;
                o.gameObject.name = pu.unitId.ToString ();
                SetPlaceCellState (o, pu);
                j++;
            }
		}

		if (placeCells.Count > count)
			for (int i = count; i < placeCells.Count; i++) {
				GameObject o = placeCells [i] as GameObject;
				o.SetActive (false);
			}
		contentP.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2 (900, 115 * count);
	}

	void SetPlaceCellState(GameObject o,PlaceUnit pu){
		Text[] t = o.GetComponentsInChildren<Text> ();
		t [0].text = pu.name;
		t [1].text = pu.desc;
		t [2].text = "";
		switch (pu.actionType) {
		case 0:
			t [3].text ="伐木";
			break;
		case 1:
			t [3].text ="挖掘";
			break;
		case 2:
			t [3].text ="提水";
			break;
		case 3:
			t [3].text = "探索";
			break;
		case 4:
			t [3].text = "收获";
			break;
		case 5:
			t [3].text = "出发";
			break;
		case 6:
			t [3].text = "交谈";
			break;
		default:
			t [3].text = "";
			break;
		}

	}
		
	void ClearContents(GameObject o){
		Text[] t = o.GetComponentsInChildren<Text> ();
		for (int i = 0; i < t.Length; i++) {
			t[i].text = "";
		}
	}

	/*ActionType
	 * 0 Cutting trees
	 * 1 Mining
	 * 2 Fetching water
	 * 3 Searching
	 * 4 Collect
	 * 5 Hunting
	 * 6 Tasks&Observe
	 * 7 
	 * 8 Treasure Box
	 */
	public void CallInDetail(int unitId){
		PlaceUnit pu = new PlaceUnit ();
		foreach (PlaceUnit p in _place.placeUnits) {
			if (p.unitId == unitId)
				pu = p;
		}
			
		switch (pu.actionType) {
		case 0:
			CallInResourceDetail ();
			SetCuttingWood (pu);
			break;
		case 1:
			CallInResourceDetail ();
			SetMining (pu);
			break;
		case 2:
			CallInResourceDetail ();
			SetFetchingWater (pu);
			break;
		case 3:
			CallInResourceDetail ();
			SetSearching (pu);
			break;
		case 4:
			CallInResourceDetail ();
			SetCollect (pu);
			break;
		case 5:
			CallInResourceDetail ();
			SetHunting (pu);
			break;
		case 6:
			CallInObserveDetail ();
			SetObserving (pu);
			break;
		default:
			Debug.Log ("wrong unitId " + unitId);
			break;
		}
	}

	void SetDetailPosition(){
		observeDetail.localPosition = new Vector3 (0, 0, 0);
		resourceDetail.localPosition = new Vector3 (150, 0, 0);
		observeDetail.gameObject.SetActive (false);
		resourceDetail.gameObject.SetActive (false);
	}

	void CallInResourceDetail(){
		observeDetail.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
		observeDetail.gameObject.SetActive (false);

		resourceDetail.gameObject.SetActive (true);
		resourceDetail.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
		resourceDetail.gameObject.transform.DOBlendableScaleBy (new Vector3 (1f, 1f, 0f), 0.3f);
	}

	void CallInObserveDetail(){
		resourceDetail.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
		resourceDetail.gameObject.SetActive (false);

		observeDetail.gameObject.SetActive (true);
		observeDetail.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
		observeDetail.gameObject.transform.DOBlendableScaleBy (new Vector3 (1f, 1f, 0f), 0.3f);
	}

	public void CallOutDetail(){
		resourceDetail.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
		resourceDetail.gameObject.SetActive (false);
		observeDetail.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
		observeDetail.gameObject.SetActive (false);
	}

	void SetCuttingWood(PlaceUnit pu){
		_puNow = pu;
		float lastTrees = 0f;
		int lastTime = 0;
		//open;lastTrees;lastTime;id|num|prop;...
		string[] s = pu.actionParam.Split (';');
		lastTrees = float.Parse (s [1]);
		lastTime = int.Parse (s [2]);
		float nowTrees = lastTrees + (GameData._playerData.minutesPassed - lastTime) / 60 / 24 * GameConfigs.TreeGrowSpeed;

		string a = "1;" + nowTrees + ";" + GameData._playerData.minutesPassed + ";";
		for (int i = 3; i < s.Length; i++) {
			a += s [i]+";";
		}
		a = a.Substring (0, a.Length - 1);
		_puNow.actionParam = a;
		_loadTxt.StorePlaceUnit (_puNow);

		string ac = "";
		for (int i = 3; i < s.Length; i++) {
			string[] ss = s [i].Split ('|');
			ac += LoadTxt.MatDic [int.Parse (ss [0])].name + ",";
		}		
		ac = ac.Substring (0, ac.Length - 1);

		Text[] t = resourceDetail.gameObject.GetComponentsInChildren<Text> ();
		t [0].text = _puNow.name;
		t [1].text = "";
		t [2].text = "增速: " + GameConfigs.TreeGrowSpeed + " /天" + "\n剩余: " + ((int)nowTrees).ToString ();
		t [3].text = "可能获得:";
		t [4].text = ac;
		t [5].text = "耗时: "+GameConfigs.CuttingTime + "分";
		t [6].text = "消耗:"+ GameConfigs.CuttingStrength + "体力";

		Button[] b = resourceDetail.gameObject.GetComponentsInChildren<Button> ();
		b [0].gameObject.GetComponentInChildren<Text> ().text = "伐木";
		b [0].interactable = (nowTrees >= 1);
	}

	void SetMining(PlaceUnit pu){
		_puNow = pu;
		//open;totalAmount;nowAmount;id|num|prop;...
		string[] s = _puNow.actionParam.Split (';');
		int total = int.Parse (s [1]);
		int now = int.Parse (s [2]);
		string ac = "";
		for (int i = 3; i < s.Length; i++) {
			string[] ss = s [i].Split ('|');
			ac += LoadTxt.MatDic [int.Parse (ss [0])].name + ",";
		}
		ac = ac.Substring (0, ac.Length - 1);
			
		Text[] t = resourceDetail.gameObject.GetComponentsInChildren<Text> ();
		t [0].text = pu.name;
		t [1].text = "";
		t [2].text = "剩余:" + now.ToString()+"/"+total.ToString ();
		t [3].text = "可能获得:";
		t [4].text = "石料,各类矿产";
		t [5].text = "耗时: "+GameConfigs.DiggingTime + "分";
		t [6].text = "消耗: " + GameConfigs.DiggingStrength + " 体力";

		Button b = resourceDetail.gameObject.GetComponentInChildren<Button> ();
		b.gameObject.GetComponentInChildren<Text> ().text = "挖掘";
		b.interactable = (now > 0);
	}

	void SetFetchingWater(PlaceUnit pu){
		_puNow = pu;
		//open;id|num|prop;id|num|prop...
		string[] s = _puNow.actionParam.Split (';');
		string ac = "";
		for (int i = 1; i < s.Length; i++) {
			string[] ss = s [i].Split ('|');
			ac += LoadTxt.MatDic [int.Parse (ss [0])].name + ",";
		}
		ac = ac.Substring (0, ac.Length - 1);

		Text[] t = resourceDetail.gameObject.GetComponentsInChildren<Text> ();
		t [0].text = pu.name;
		t [1].text = "";
		t [2].text = "";
		t [3].text = "可能获得:";
		t [4].text = ac;
		t [5].text = "耗时: "+GameConfigs.FetchingTime + "分";
		t [6].text = "消耗: " + GameConfigs.FetchingStrength + " 体力";

		Button b = resourceDetail.gameObject.GetComponentInChildren<Button> ();
		b.gameObject.GetComponentInChildren<Text> ().text = "提水";
		b.interactable = true;
	}

	void SetSearching(PlaceUnit pu){
		_puNow = pu;
		//open;total;left;id|weight;id|weight;...
		string[] s=pu.actionParam.Split(';');
		int total = int.Parse (s [1]);
		int now = int.Parse (s [2]);

		Text[] t = resourceDetail.gameObject.GetComponentsInChildren<Text> ();
		t[0].text = pu.name;
		t [1].text = "";
		t [2].text = "";
		t [3].text = "探索进度:";
		t [4].text = (100 - (int)(now * 100 / total)) + "%";
		t [5].text = "耗时: "+GameConfigs.SearchTime + "分";
		t [6].text = "";

		Button b = resourceDetail.gameObject.GetComponentInChildren<Button> ();
		b.gameObject.GetComponentInChildren<Text>().text = "探索";
		b.interactable = (now > 0);
	}

	void SetCollect(PlaceUnit pu){
		_puNow = pu;
		//open;id|num|prop;...
		string	[] s = pu.actionParam.Split(';');
		string ac = "";
		for (int i = 1; i < s.Length; i++) {
			string[] ss = s [i].Split ('|');
			ac+=LoadTxt.MatDic[int.Parse(ss[0])].name + ",";
		}
		ac = ac.Substring (0, ac.Length - 1);

		Text[] t = resourceDetail.gameObject.GetComponentsInChildren<Text> ();
		t[0].text = pu.name;
		t [1].text = "";
		t [2].text = "";
		t [3].text = "可能获得:";
		t [4].text = ac;
		t [5].text = "耗时: "+GameConfigs.CollectTime + "分";
		t [6].text = "消耗: " + GameConfigs.CollectStrength + " 体力";

		Button b = resourceDetail.gameObject.GetComponentInChildren<Button> ();
		b.gameObject.GetComponentInChildren<Text>().text = "收获";
		b.interactable = true;

	}

	void SetHunting(PlaceUnit pu){
		_puNow = pu;
		//open;id|weight;...
		Text[] t = resourceDetail.gameObject.GetComponentsInChildren<Text> ();
		t[0].text = pu.name;
		t [1].text = "";
		t [2].text = "";
		t [3].text = "猎物:";
		t [4].text = "各类野兽";
		t [5].text = "耗时: "+GameConfigs.HuntTime + "分";
		t [6].text = "";

		Button b = resourceDetail.gameObject.GetComponentInChildren<Button> ();
		b.gameObject.GetComponentInChildren<Text>().text = "捕猎";
		b.interactable = true;
	}

	void SetObserving(PlaceUnit pu){
		_puNow = pu;
		//param格式:open;Name;Description;MonsterId
		string[] s = pu.actionParam.Split (';');
		Text[] t = observeDetail.gameObject.GetComponentsInChildren<Text> ();
		t [0].text = s [1];
		t [1].text = s [2];
		Button[] b = observeDetail.gameObject.GetComponentsInChildren<Button> ();
		b [0].interactable = (s.Length >= 4);
	}
		
	public void ResourceAct(){
		if (GameData._playerData.dayNow > GameConfigs.StartGhostEvent) {
			bool ghostComing = CheckGhost ();
			if (ghostComing) {
				_panelManager.GoToPanel ("Battle");
				Monster[] m = new Monster[1];
				m [0] = FindGhost ();

				int r = Algorithms.GetIndexByRange (0, 100);
				bool isSpoted = r < (GameData._playerData.SpotRate * 100);
				_battleActions.InitializeBattleField (m, isSpoted);

				return;
			}
		}

		string t = resourceDetail.GetComponentInChildren<Button> ().gameObject.GetComponentInChildren<Text> ().text;
		switch (t) {
		case "伐木":
			if (GameData._playerData.strengthNow < GameConfigs.CuttingStrength) {
				_floating.CallInFloating ("体力不足", 1);
				break;
			}
			StartCoroutine (StartCut ());
			break;
		case "挖掘":
			if (GameData._playerData.strengthNow < GameConfigs.DiggingStrength) {
				_floating.CallInFloating ("体力不足", 1);
				return;
			}
			StartCoroutine (StartDig ());
			break;
		case "提水":
			if (GameData._playerData.strengthNow < GameConfigs.FetchingStrength) {
				_floating.CallInFloating ("体力不足", 1);
				return;
			}
			StartCoroutine (StartFetch ());
			break;
		case "收获":
			if (GameData._playerData.strengthNow < GameConfigs.FetchingStrength) {
				_floating.CallInFloating ("体力不足", 1);
				return;
			}
			StartCoroutine (StartFetch ());
			break;
		case "探索":
			StartCoroutine (StartSearch ());
			break;
		case "捕猎":
			Hunt ();
			break;
		default:
			Debug.Log ("Wrong action : " + t);
			break;
		}
		SetPlace (_mapNow);
	}

	IEnumerator StartCut(){
		int t = _loadingBar.CallInLoadingBar (60);
		yield return new WaitForSeconds (t);
		Cut();
	}

	IEnumerator StartDig(){
		int t = _loadingBar.CallInLoadingBar (60);
		yield return new WaitForSeconds (t);
		Dig();
	}

	IEnumerator StartFetch(){
		int t = _loadingBar.CallInLoadingBar (60);
		yield return new WaitForSeconds (t);
		Fetch();
	}

	IEnumerator StartSearch(){
		int t = _loadingBar.CallInLoadingBar (60);
		yield return new WaitForSeconds (t);
		Search();
	}

	bool CheckGhost(){

		if (GameData._playerData.hourNow >= 5 && GameData._playerData.hourNow <= 19)
			return false;
		float r = Random.Range (0f, 100f);
//		Debug.Log ("出现幽灵的概率为" + GameData._playerData.GhostComingProp + ",随机数为" + r + "。");

		if (r < (100f * GameData._playerData.GhostComingProp))
			return true;
		return false;
	}

	Monster FindGhost(){
		int[] weight = new int[GameConfigs.GhostDic.Count];
		Monster[] m = new Monster[weight.Length];
		int i = 0;
		foreach(int key in GameConfigs.GhostDic.Keys){
			m [i] = LoadTxt.GetMonster(key);
			weight [i++] = GameConfigs.GhostDic [key];
		}
		i = Algorithms.GetResultByWeight (weight);
		return m [i];
	}

	void Cut(){
		//open;lastTrees;lastTime;id|num|prop;...
		float lastTrees = 0f;
		int lastTime = 0;
		string[] s = _puNow.actionParam.Split (';');
		lastTrees = float.Parse (s [1]);
		lastTime = int.Parse (s [2]);
		float nowTrees = lastTrees + (GameData._playerData.minutesPassed - lastTime) / 60 / 24 * GameConfigs.TreeGrowSpeed;
		if (nowTrees < 1)
			return;
		
		_gameData.ChangeProperty (8, -GameConfigs.CuttingStrength);
		_gameData.ChangeTime (GameConfigs.CuttingTime);
		nowTrees--;

		Dictionary<int,int> ac = new Dictionary<int, int> ();
		float[] pro = new float[s.Length - 3];
		for (int i = 3; i < s.Length; i++) {
			string[] ss = s [i].Split ('|');
			ac.Add (int.Parse (ss [0]), int.Parse (ss [1]));
			pro [i - 3] = float.Parse (ss [2]);
		}
		Dictionary<int,int> rewards = Algorithms.GetReward (ac, pro);
		if (rewards.Count >= 1) {
			string newItems = "";
			foreach (int key in rewards.Keys) {
				_gameData.AddItem (key * 10000, rewards [key]);
				newItems += LoadTxt.MatDic [key].name + " +" + rewards [key] + "\t";
			}
			newItems = newItems.Substring (0, newItems.Length - 1);
			_floating.CallInFloating (newItems, 0);
		}

		string a = "1;" + nowTrees + ";" + GameData._playerData.minutesPassed + ";";
		for (int i = 3; i < s.Length; i++) {
			a += s [i] + ";";
		}
		a = a.Substring (0, a.Length - 1);
		_puNow.actionParam = a;
		_loadTxt.StorePlaceUnit (_puNow);

		SetCuttingWood (_puNow);
	}

	void Dig(){
		//open;totalAmount;nowAmount;id|num|prop;...

		string[] s = _puNow.actionParam.Split (';');
		int total = int.Parse (s [1]);
		int now = int.Parse (s [2]);

		_gameData.ChangeProperty (8, -GameConfigs.DiggingStrength);
		_gameData.ChangeTime (GameConfigs.DiggingTime );
		now--;

		Dictionary<int,int> ac = new Dictionary<int, int> ();
		float[] pro = new float[s.Length - 3];
		for (int i = 3; i < s.Length; i++) {
			string[] ss = s [i].Split ('|');
			ac.Add (int.Parse (ss [0]), int.Parse (ss [1]));
			pro [i - 3] = float.Parse (ss [2]);
		}
		Dictionary<int,int> rewards = Algorithms.GetReward (ac, pro);
		if (rewards.Count >= 1) {
			string newItems = "";
			foreach (int key in rewards.Keys) {
				_gameData.AddItem (key * 10000, rewards [key]);
				newItems += LoadTxt.MatDic [key].name + " +" + rewards [key] + "\t";
			}
			newItems = newItems.Substring (0, newItems.Length - 1);
			_floating.CallInFloating (newItems, 0);
		}

		string a = "1;" + total + ";" + now + ";";
		for (int i = 3; i < s.Length; i++) {
			a += s [i] + ";";
		}
		a = a.Substring (0, a.Length - 1);
		_puNow.actionParam = a;
		_loadTxt.StorePlaceUnit (_puNow);

		SetMining (_puNow);
	}

	void Fetch(){
		//open;id|num|prop;id|num|prop...
		_gameData.ChangeProperty (8, -GameConfigs.FetchingStrength);
		_gameData.ChangeTime (GameConfigs.FetchingTime);

		string[] s = _puNow.actionParam.Split (';');
		Dictionary<int,int> ac = new Dictionary<int, int> ();
		float[] pro = new float[s.Length - 1];
		for (int i = 1; i < s.Length; i++) {
			string[] ss = s [i].Split ('|');
			ac.Add (int.Parse (ss [0]), int.Parse (ss [1]));
			pro [i - 1] = float.Parse (ss [2]);
		}
		Dictionary<int,int> rewards = Algorithms.GetReward (ac, pro);
		if (rewards.Count >= 1) {
			string newItems = "";
			foreach (int key in rewards.Keys) {
				_gameData.AddItem (key * 10000, rewards [key]);
				newItems += LoadTxt.MatDic [key].name + " +" + rewards [key] + "\t";
			}
			newItems = newItems.Substring (0, newItems.Length - 1);
			_floating.CallInFloating (newItems, 0);
		}
	}

	void Search(){

		//open;total;left;id|weight;id|weight;...
		_gameData.ChangeTime (GameConfigs.SearchTime);

		string[] s=_puNow.actionParam.Split(';');
		int total = int.Parse (s [1]);
		int now = int.Parse (s [2]);
		int[] weight = new int[s.Length - 3];
		int[] ids = new int[s.Length - 3];
		for (int i = 3; i < s.Length; i++) {
			string[] ss = s [i].Split ('|');
			ids [i - 3] = int.Parse (ss [0]);
			weight [i - 3] = int.Parse (ss [1]);
		}

		int num = Algorithms.GetIndexByRange ((int)(GameConfigs.SearchRewardsNum * GameData._playerData.SearchRate * 0.8f), (int)(GameConfigs.SearchRewardsNum * GameData._playerData.SearchRate * 1.2f));
		num = (num > now) ? now : num;
		now = now - num;
		Dictionary<int,int> rewards = new Dictionary<int, int> ();
		for (int i = 0; i < num; i++) {
			int index = Algorithms.GetResultByWeight (weight);

            //随出来的没有东西
            if (index == 0)
                continue;
            
			if (rewards.ContainsKey (ids [index])) {
				rewards [ids [index]]++;
			} else {
				rewards.Add (ids [index], 1);
			}
		}

		if (rewards.Count >= 1) {
			string newItems = "获得 ";
			foreach (int key in rewards.Keys) {

                //容错
                if (key == 0)
                    continue;
				
                _gameData.AddItem (key * 10000, rewards [key]);
				newItems += LoadTxt.MatDic [key].name + " +" + rewards [key] + "\t";
			}
			newItems = newItems.Substring (0, newItems.Length - 1);
			if (newItems != "获得")
				_floating.CallInFloating (newItems, 0);
			else
				_floating.CallInFloating ("一无所获。", 0);

			//Achievement
			if (now == 0)
				this.gameObject.GetComponentInParent<AchieveActions> ().TotalSearch ();
		}

		string a = "";
		if (now > 0)
			a = "1;" + total + ";" + now + ";";
		else
			a = "0;" + total + ";" + now + ";";
		for (int i = 3; i < s.Length; i++) {
			a += s [i] + ";";
		}
		a = a.Substring (0, a.Length - 1);
		_puNow.actionParam = a;
		_loadTxt.StorePlaceUnit (_puNow);

		SetSearching (_puNow);
		_gameData.SearchNewPlace (_mapNow.id, now * 100 / total);

        //检测是否有新信息
        GetComponentInParent<SerendipityActions>().CheckSerendipity();
	}

	void Hunt(){
		//open;id|weight;...
		_gameData.ChangeTime (GameConfigs.HuntTime);
		string[] s=_puNow.actionParam.Split(';');
		int[] weight = new int[s.Length - 1];
		int[] ids = new int[s.Length - 1];
		for (int i = 1; i < s.Length; i++) {
			string[] ss = s [i].Split ('|');
			ids [i - 1] = int.Parse (ss [0]);
			weight [i - 1] = int.Parse (ss [1]);
		}
		int index = Algorithms.GetResultByWeight (weight);
		int num = Algorithms.GetIndexByRange (1, 1 + LoadTxt.GetMonster (ids [index]).groupNum);

		Monster[] m = new Monster[num];
		for (int i = 0; i < m.Length; i++)
			m [i] = LoadTxt.GetMonster (ids [index]);
		_panelManager.GoToPanel ("Battle");

		int r = Algorithms.GetIndexByRange(0,100);
		bool isSpoted = r < (GameData._playerData.SpotRate * 100);
		_battleActions.InitializeBattleField (m, isSpoted);
	}


	public void Rob(){
		//param格式:open;Name;Description;MonsterId
		string[] s = _puNow.actionParam.Split (';');
		if (s.Length < 4)
			return;
		int monsterId = int.Parse (s [3]);
		Monster[] m = new Monster[1];
		m [0] = LoadTxt.GetMonster(monsterId);
//		Debug.Log ("Challange Monster : " + m [0].name);
		_panelManager.GoToPanel ("Battle");
		_battleActions.InitializeBattleField (m, false);
	}


}
