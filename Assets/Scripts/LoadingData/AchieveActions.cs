using UnityEngine;
using System.Collections;

public class AchieveActions : MonoBehaviour {

	private GameData _gameData;
	private FloatingActions _floating;
	private LogManager _log;
	void Start(){
		_gameData = GetComponent<GameData> ();
		_floating = GetComponentInChildren<FloatingActions> ();
		_log = GetComponentInChildren<LogManager> ();
	}

	public static string GetProgress(int id){
        int openCount = 0;
        string s = "";
        switch (id)
        {
            case 1:
                s += GameData._playerData.thiefCaught + "/" + LoadTxt.GetAchievement(1).req;
                break;
            case 2:
                s += GameData._playerData.ghostKill + "/" + LoadTxt.GetAchievement(2).req;
                break;
            case 3:
                s += GameData._playerData.ghostBossKill + "/" + LoadTxt.GetAchievement(3).req;
                break;
            case 4:
                s += GameData._playerData.ghostKingKill + "/" + LoadTxt.GetAchievement(4).req;
                break;
            case 5:
                s += GameData._playerData.wineTasted.Length + "/" + LoadTxt.GetAchievement(5).req;
                break;
            case 6:
                s += GameData._playerData.foodCooked.Length + "/" + LoadTxt.GetAchievement(6).req;
                break;
            case 7:
                s += GameData._playerData.Renown + "/" + LoadTxt.GetAchievement(7).req;
                break;
            case 8:
                s += GameData._playerData.Renown + "/" + LoadTxt.GetAchievement(8).req;
                break;
            case 9:
                s += GameData._playerData.Renown + "/" + LoadTxt.GetAchievement(9).req;
                break;
            case 10:
                s += GameData._playerData.Renown + "/" + LoadTxt.GetAchievement(10).req;
                break;
            case 11:
                s += GameData._playerData.meleeCollected.Length + "/" + LoadTxt.GetAchievement(11).req;
                break;
            case 12:
                s += GameData._playerData.magicCollected.Length + "/" + LoadTxt.GetAchievement(12).req;
                break;
            case 13:
                s += GameData._playerData.rangedCollected.Length + "/" + LoadTxt.GetAchievement(13).req;
                break;
            case 14:
                s += GameData._playerData.petsCaptured + "/" + LoadTxt.GetAchievement(14).req;
                break;
            case 15:
                if (GameData._playerData.Achievements[15] == 0)
                    s = "0/1";
                else
                    s = "1/1";
                break;
            case 16:
                s += GameData._playerData.monsterKilled + "/" + LoadTxt.GetAchievement(16).req;
                break;
            case 17:
                if (GameData._playerData.Achievements[17] == 0)
                    s = "0/1";
                else
                    s = "1/1";
                break;
            case 18:
                if (GameData._playerData.Achievements[18] == 0)
                    s = "0/1";
                else
                    s = "1/1";
                break;
            case 19:
                foreach (int key in GameData._playerData.MapOpenState.Keys)
                {
                    openCount += GameData._playerData.MapOpenState[key];
                }
                s += openCount + "/" + LoadTxt.GetAchievement(19).req;
                break;
            case 20:
                s += GameData._playerData.dayNow + "/" + LoadTxt.GetAchievement(20).req;
                break;
            case 21:
                s += GameData._playerData.sleepTime + "/" + LoadTxt.GetAchievement(21).req;
                break;
            case 22:
                s += GameData._playerData.wineDrinked + "/" + LoadTxt.GetAchievement(22).req;
                break;
            case 23:
                if (GameData._playerData.Achievements[23] == 0)
                    s = "0/1";
                else
                    s = "1/1";
                break;
            case 24:
                s += GameData._playerData.dungeonLevelMax + "/" + LoadTxt.GetAchievement(24).req;
                break;
            case 25:
                s += GameData._playerData.meleeAttackCount + "/" + LoadTxt.GetAchievement(25).req;
                break;
            case 26:
                s += GameData._playerData.rangedAttackCount + "/" + LoadTxt.GetAchievement(26).req;
                break;
            case 27:
                s += GameData._playerData.magicAttackCount + "/" + LoadTxt.GetAchievement(27).req;
                break;
            case 28:
                break;
            case 29:
                break;
            case 30:
                break;
            case 31:
                foreach (int key in GameData._playerData.MapOpenState.Keys)
                {
                    openCount += GameData._playerData.MapOpenState[key];
                }
                s += openCount + "/" + LoadTxt.GetAchievement(31).req;
                break;
            case 32:
                s += GameData._playerData.dragonKilled + "/" + LoadTxt.GetAchievement(32).req;
                break;
            case 33:
                if (GameData._playerData.Achievements[23] == 0)
                    s = "0/1";
                else
                    s = "1/1";
                break;
            case 34:
                s += GameData._playerData.dayNow + "/" + LoadTxt.GetAchievement(34).req;
                break;
            case 35:
                if (GameData._playerData.Achievements[35] == 0)
                    s = "0/1";
                else
                    s = "1/1";
                break;
            case 36:
                if (GameData._playerData.Achievements[36] == 0)
                    s = "0/1";
                else
                    s = "1/1";
                break;
            case 37:
                s += s += GameData._playerData.dungeonLevelMax + "/" + LoadTxt.GetAchievement(37).req;
                break;
            case 38:
                s += GameData._playerData.legendThiefCaught + "/" + LoadTxt.GetAchievement(38).req;
                break;
            case 39:
                s += ReconizeCamp() + "/" + LoadTxt.GetAchievement(39).req;
                break;
            case 40:
                break;
            default:
                break;
        }
        return s;
    }

    public static int ReconizeCamp(){
        int i = 0;
        if (GameData._playerData.orderCamp > 0)
            i++;
        if (GameData._playerData.truthCamp > 0)
            i++;
        if (GameData._playerData.lifeCamp > 0)
            i++;
        if (GameData._playerData.chaosCamp > 0)
            i++;
        if (GameData._playerData.deathCamp > 0)
            i++;
        return i;
    }

	public void DefeatEnemy(int monsterId){
		Achievement a = new Achievement ();
        if (monsterId == 1)
        {
            //Ghost
            GameData._playerData.ghostKill++;
            _gameData.StoreData("GhostKill", GameData._playerData.ghostKill);
            a = LoadTxt.GetAchievement(2);
            if (GameData._playerData.Achievements[2] == 0)
            {
                if (GameData._playerData.ghostKill >= a.req)
                {
                    StoreAchievement(2);
					_floating.CallInFloating("Achievement:" + a.name, 0);
					_log.AddLog("Achievement:" + a.name, true);
                }
            }
        }
        else if (monsterId == 2)
        {
            //GhostBoss
            GameData._playerData.ghostBossKill++;
            _gameData.StoreData("GhostBossKill", GameData._playerData.ghostBossKill);
            a = LoadTxt.GetAchievement(3);
            if (GameData._playerData.Achievements[3] == 0)
            {
                if (GameData._playerData.ghostBossKill >= a.req)
                {
                    StoreAchievement(3);
					_floating.CallInFloating("Achievement:" + a.name, 0);
					_log.AddLog("Achievement:" + a.name, true);
                }
            }
        }
        else if (monsterId == 3)
        {
            //GhostKing
            GameData._playerData.ghostKingKill++;
            _gameData.StoreData("GhostKingKill", GameData._playerData.ghostKingKill);
            a = LoadTxt.GetAchievement(4);
            if (GameData._playerData.Achievements[4] == 0)
            {
                if (GameData._playerData.ghostKingKill >= a.req)
                {
                    StoreAchievement(4);
                    _floating.CallInFloating("Achievement:" + a.name, 0);
                    _log.AddLog("Achievement:" + a.name, true);
                }
            }
        }
        else if (monsterId == 1801 || monsterId == 1802 || monsterId == 1803)
        {
            //Dragon
            GameData._playerData.dragonKilled++;
            _gameData.StoreData("DragonKilled", GameData._playerData.dragonKilled);
            a = LoadTxt.GetAchievement(32);
            if (GameData._playerData.Achievements[32] == 0)
            {
                if (GameData._playerData.dragonKilled >= a.req)
                {
                    StoreAchievement(32);
                    _floating.CallInFloating("Achievement:" + a.name, 0);
                    _log.AddLog("Achievement:" + a.name, true);
                }
            }
        }
        else if (monsterId == 3008 || monsterId == 3108 || monsterId == 3208 || monsterId == 3308 || monsterId == 3408)
        {
            if (GameData._playerData.Achievements[39] == 1)
                return;
            
            if (monsterId == 3008)
            {
                GameData._playerData.orderCamp = 1;
                _gameData.StoreData("OrderCamp", 1);
            }
            else if (monsterId == 3108)
            {
                GameData._playerData.truthCamp = 1;
                _gameData.StoreData("TruthCamp", 1);
            }
            else if (monsterId == 3208)
            {
                GameData._playerData.lifeCamp = 1;
                _gameData.StoreData("LifeCamp", 1);
            }
            else if (monsterId == 3308)
            {
                GameData._playerData.chaosCamp = 1;
                _gameData.StoreData("ChaosCamp", 1);
            }
            else
            {
                GameData._playerData.deathCamp = 1;
                _gameData.StoreData("DeathCamp", 1);
            }

            a = LoadTxt.GetAchievement(39);
            if (ReconizeCamp() >=a.req)
            {
                StoreAchievement(39);
                _floating.CallInFloating ("Achievement:" + a.name, 0);
                _log.AddLog ("Achievement:" + a.name,true);
				PlaceActions p = this.GetComponentInChildren<PlaceActions> ();
				p.CallInComplete(2);
            }
        }

		GameData._playerData.monsterKilled++;
		_gameData.StoreData ("MonsterKilled", GameData._playerData.monsterKilled);
		a = LoadTxt.GetAchievement (16);
		if (GameData._playerData.Achievements [16] == 0) {
			if (GameData._playerData.monsterKilled >= a.req) {
				StoreAchievement (16);
				_floating.CallInFloating ("Achievement:" + a.name, 0);
				_log.AddLog ("Achievement:" + a.name,true);
			}
		}
	}

	/// <summary>
	/// Tastes the wine,4 bit id.
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	public void TasteWine(int itemId){
		Achievement a = new Achievement ();

		GameData._playerData.wineDrinked++;
		_gameData.StoreData ("WineDrinked", GameData._playerData.wineDrinked);
		if (GameData._playerData.Achievements [22] == 0) {
			a = LoadTxt.GetAchievement (22);
			if (GameData._playerData.wineDrinked >= a.req) {
				StoreAchievement (22);
				_floating.CallInFloating ("Achievement:" + a.name,0);
				_log.AddLog ("Achievement:" + a.name,true);
			}
		}

		int[] w = GameData._playerData.wineTasted;
		int[] wNew = new int[w.Length + 1];
		for (int i = 0; i < wNew.Length-1; i++) {
			if (w [i] == itemId)
				return;
			wNew [i] = w [i];
		}
		wNew [wNew.Length - 1] = itemId;
		GameData._playerData.wineTasted = wNew;
		_gameData.StoreData ("WineTasted", _gameData.GetStrFromInt (wNew));
		a = LoadTxt.GetAchievement (5);
		if (GameData._playerData.Achievements [5] == 0) {
			if (wNew.Length >= a.req) {
				StoreAchievement (5);
				_floating.CallInFloating ("Achievement:" + a.name,0);
				_log.AddLog ("Achievement:" + a.name,true);
			}
		}
	}

	public void LoadMemmory(){
		if (GameData._playerData.Achievements [23] == 0) {
			Achievement a = LoadTxt.GetAchievement (23);
			StoreAchievement (23);
			_floating.CallInFloating ("Achievement:" + a.name,0);
			_log.AddLog ("Achievement:" + a.name,true);
		}
	}

	/// <summary>
	/// Cooks the food,4 bit id.
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	public void CookFood(int itemId){
		
		int[] f = GameData._playerData.foodCooked;
		int[] fNew = new int[f.Length + 1];

		for (int i = 0; i < fNew.Length-1; i++) {
			if (f [i] == itemId)
				return;
			fNew [i] = f [i];
		}
		fNew [fNew.Length - 1] = itemId;
		GameData._playerData.foodCooked = fNew;
		_gameData.StoreData ("FoodCooked", _gameData.GetStrFromInt (fNew));

		if (GameData._playerData.Achievements [6] == 0) {
			Achievement a = LoadTxt.GetAchievement (6);
			if (fNew.Length >= a.req) {
				StoreAchievement (6);
				_floating.CallInFloating ("Achievement:" + a.name,0);
				_log.AddLog ("Achievement:" + a.name,true);
			}
		}
	}

	/// <summary>
	/// Collects the melee weapon,4 bit id.
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	public void CollectMeleeWeapon(int itemId){

		int[] m = GameData._playerData.meleeCollected;
		int[] mNew = new int[m.Length + 1];


		for (int i = 0; i < m.Length; i++) {
			if (m [i] == itemId)
				return;
			else
				mNew [i] = m [i];		
		}
		mNew [mNew.Length - 1] = itemId;

		GameData._playerData.meleeCollected = mNew;
		_gameData.StoreData ("MeleeCollected", _gameData.GetStrFromInt (mNew));

		if (GameData._playerData.Achievements [11] == 0) {
			Achievement a = LoadTxt.GetAchievement (11);
			if (mNew.Length >= a.req) {
				StoreAchievement (11);
				_floating.CallInFloating ("Achievement:" + a.name,0);
				_log.AddLog ("Achievement:" + a.name,true);
			}
		}
	}

	/// <summary>
	/// Collects the ranged weapon,4 bit id.
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	public void CollectRangedWeapon(int itemId){

		int[] r = GameData._playerData.rangedCollected;
		int[] rNew = new int[r.Length + 1];

		for (int i = 0; i < rNew.Length-1; i++) {
			if (r [i] == itemId)
				return;
			rNew [i] = r [i];
		}
		rNew [rNew.Length-1] = itemId;
		GameData._playerData.rangedCollected = rNew;
		_gameData.StoreData ("RangedCollected", _gameData.GetStrFromInt (rNew));
		if (GameData._playerData.Achievements [13] == 0) {
			Achievement a = LoadTxt.GetAchievement (13);
			if (rNew.Length >= a.req) {
				StoreAchievement (13);
				_floating.CallInFloating ("Achievement:" + a.name,0);
				_log.AddLog ("Achievement:" + a.name,true);
			}
		}
	}

	/// <summary>
	/// Collects the magic weapon,4 bit id.
	/// </summary>
	/// <param name="itemId">Item identifier.</param>
	public void CollectMagicWeapon(int itemId){

		int[] m = GameData._playerData.magicCollected;
		int[] mNew = new int[m.Length + 1];

		for (int i = 0; i < mNew.Length-1; i++) {
			if (m [i] == itemId)
				return;
			mNew [i] = m [i];
		}
		mNew [mNew.Length - 1] = itemId;
		GameData._playerData.magicCollected = mNew;
		_gameData.StoreData ("MagicCollected", _gameData.GetStrFromInt (mNew));

		if (GameData._playerData.Achievements [12] == 0) {
			Achievement a = LoadTxt.GetAchievement (12);
			if (mNew.Length >= a.req) {
				StoreAchievement (12);
				_floating.CallInFloating ("Achievement:" + a.name,0);
				_log.AddLog ("Achievement:" + a.name,true);
			}
		}
	}

	public void CapturePet(){

		GameData._playerData.petsCaptured++;
		_gameData.StoreData ("PetsCaptured", GameData._playerData.petsCaptured);
		if (GameData._playerData.Achievements [14] == 0) {
			Achievement a = LoadTxt.GetAchievement (14);
			if (GameData._playerData.petsCaptured >= a.req) {
				StoreAchievement (14);
				_floating.CallInFloating ("Achievement:" + a.name, 0);
				_log.AddLog ("Achievement:" + a.name,true);
			}
		}
	}

	public void MountPet(int monsterId){
		Achievement a = new Achievement ();
		if (GameData._playerData.Achievements [15] == 0) {
			a = LoadTxt.GetAchievement (15);
			StoreAchievement (15);
			_floating.CallInFloating ("Achievement:" + a.name,0);
			_log.AddLog ("Achievement:" + a.name,true);
		}

		//Dragon
		if (GameData._playerData.Achievements [36] == 0) {
            if(monsterId==1801 || monsterId==1802 || monsterId==1803 || monsterId==2448 || monsterId==2449 || monsterId==2450) {
				a = LoadTxt.GetAchievement (36);
				StoreAchievement (36);
				_floating.CallInFloating ("Achievement:" + a.name,0);
				_log.AddLog ("Achievement:" + a.name,true);
			}
		}
	}

	public void NewPlaceFind(){

		int openCount = 0;
		foreach (int key in GameData._playerData.MapOpenState.Keys) {
			openCount += GameData._playerData.MapOpenState [key];
		}
		Achievement a = new Achievement ();
		if (openCount >= LoadTxt.GetAchievement(19).req && (GameData._playerData.Achievements [19] == 0)) {
			a = LoadTxt.GetAchievement (19);
			StoreAchievement (19);
			_floating.CallInFloating ("Achievement:" + a.name,0);
			_log.AddLog ("Achievement:" + a.name,true);
		}
		if (openCount >= LoadTxt.GetAchievement(31).req && (GameData._playerData.Achievements [31] == 0)) {
			StoreAchievement (31);
			a = LoadTxt.GetAchievement (31);
			_floating.CallInFloating ("Achievement:" + a.name,0);
			_log.AddLog ("Achievement:" + a.name,true);
		}
	}

	public void TimeChange(){
		Achievement a = new Achievement ();
		if (GameData._playerData.Achievements [20] == 0) {
			a = LoadTxt.GetAchievement (20);
			if (GameData._playerData.dayNow >= a.req) {
				StoreAchievement (20);
				_floating.CallInFloating ("Achievement:" + a.name,0);
				_log.AddLog ("Achievement:" + a.name,true);
			}
		}
		if (GameData._playerData.Achievements [34] == 0) {
			a = LoadTxt.GetAchievement (34);
			if (GameData._playerData.dayNow >= a.req) {
				StoreAchievement (34);
				_floating.CallInFloating ("Achievement:" + a.name,0);
				_log.AddLog ("Achievement:" + a.name,true);
			}
		}
	}

	public void TotalSearch(){
		if (GameData._playerData.Achievements [18] == 0) {
			Achievement a = LoadTxt.GetAchievement (18);
			StoreAchievement (18);
			_floating.CallInFloating ("Achievement:" + a.name, 0);
			_log.AddLog ("Achievement:" + a.name,true);
		}

	}

	public void Sleep(int t){

		GameData._playerData.sleepTime += t;
		_gameData.StoreData ("SleepTime", GameData._playerData.sleepTime);

		if (GameData._playerData.Achievements [21] == 0) {
			Achievement a = LoadTxt.GetAchievement (21);
			if (GameData._playerData.sleepTime >= a.req) {
				StoreAchievement (21);
				_floating.CallInFloating ("Achievement:" + a.name,0);
				_log.AddLog ("Achievement:" + a.name,true);
			}
		}
	}

	public void TechUpgrade(){
		foreach (int key in GameData._playerData.techLevels.Keys) {
			Technique[] techs = LoadTxt.GetTechList ();
			for (int k = 0; k < techs.Length; k++) {
				if ((key == techs[k].type) && (GameData._playerData.techLevels [key] < techs[k].maxLv)) {
					return;
				}
			}
		}
		Achievement a = LoadTxt.GetAchievement (35);
		StoreAchievement (35);
		_floating.CallInFloating ("Achievement:" + a.name,0);
		_log.AddLog ("Achievement:" + a.name,true);
	}

	public void ConstructBulding(){
		Achievement a = new Achievement ();
		if (GameData._playerData.Achievements [17] == 0) {
			if(GameData._playerData.BedRoomOpen == 0)
				return;
			if(GameData._playerData.WarehouseOpen ==0)
				return;
			if (GameData._playerData.KitchenOpen==0)
				return;
			if (GameData._playerData.WorkshopOpen == 0)
				return;
			if (GameData._playerData.StudyOpen == 0)
				return;
			if (GameData._playerData.FarmOpen == 0)
				return;
			if (GameData._playerData.PetsOpen == 0)
				return;
			if (GameData._playerData.WellOpen == 0)
				return;
			if (GameData._playerData.AchievementOpen == 0)
				return;
			if (GameData._playerData.AltarOpen == 0)
				return;
			StoreAchievement (17);
			a = LoadTxt.GetAchievement (17);
			_floating.CallInFloating ("Achievement:" + a.name,0);
			_log.AddLog ("Achievement:" + a.name, true);
		}

		if (GameData._playerData.Achievements [33] == 0) {
			if(GameData._playerData.BedRoomOpen <GameConfigs.MaxLv_BedRoom)
				return;
			if(GameData._playerData.WarehouseOpen <GameConfigs.MaxLv_Warehouse)
				return;
			if (GameData._playerData.KitchenOpen<GameConfigs.MaxLv_Kitchen)
				return;
			if (GameData._playerData.WorkshopOpen <GameConfigs.MaxLv_Workshop)
				return;
			if (GameData._playerData.StudyOpen<GameConfigs.MaxLv_Study)
				return;
			if (GameData._playerData.FarmOpen <GameConfigs.MaxLv_Farm)
				return;
			if (GameData._playerData.PetsOpen <GameConfigs.MaxLv_Pets)
				return;
			if (GameData._playerData.WellOpen <GameConfigs.MaxLv_Well)
				return;
			if (GameData._playerData.AchievementOpen <GameConfigs.MaxLv_Achievement)
				return;
			if (GameData._playerData.AltarOpen <GameConfigs.MaxLv_Altar)
				return;
			StoreAchievement (33);
			a = LoadTxt.GetAchievement (33);
			_floating.CallInFloating ("Achievement:" + a.name,0);
			_log.AddLog ("Achievement:" + a.name,true);
		}
	}

	public void Fight(string kind){
		Achievement a = new Achievement ();
		if (kind == "Melee") {
			GameData._playerData.meleeAttackCount++;
			_gameData.StoreData ("MeleeAttackCount", GameData._playerData.meleeAttackCount);
			if (GameData._playerData.Achievements [25] == 0) {
				a = LoadTxt.GetAchievement (25);
				if (GameData._playerData.meleeAttackCount >= a.req) {
					StoreAchievement (25);
					_floating.CallInFloating ("Achievement:" + a.name,0);
					_log.AddLog ("Achievement:" + a.name,true);
				}
			}
		}
		if (kind == "Ranged") {
			GameData._playerData.rangedAttackCount++;
			_gameData.StoreData ("RangedAttackCount", GameData._playerData.rangedAttackCount);
			if (GameData._playerData.Achievements [26] == 0) {
				a = LoadTxt.GetAchievement (26);
				if (GameData._playerData.rangedAttackCount >=a.req) {
					StoreAchievement (26);
					_floating.CallInFloating ("Achievement:" + a.name,0);
					_log.AddLog ("Achievement:" + a.name,true);
				}
			}
		}
		if (kind == "Magic" ) {
			GameData._playerData.magicAttackCount++;
			_gameData.StoreData ("MagicAttackCount", GameData._playerData.magicAttackCount);
			if (GameData._playerData.Achievements [27] == 0) {
				a = LoadTxt.GetAchievement (27);
				if (GameData._playerData.magicAttackCount >= a.req) {
					StoreAchievement (27);
					_floating.CallInFloating ("Achievement:" + a.name,0);
					_log.AddLog ("Achievement:" + a.name,true);
				}
			}
		}
	}

	public void CatchThief(int thiefId){
		Achievement a = new Achievement ();
		GameData._playerData.thiefCaught++;
		_gameData.StoreData ("ThiefCaught", GameData._playerData.thiefCaught);

		if (GameData._playerData.Achievements [1] == 0) {
			a = LoadTxt.GetAchievement (1);
			if (GameData._playerData.thiefCaught >= a.req) {
				StoreAchievement (1);
				_floating.CallInFloating ("Achievement:" + a.name, 0);
				_log.AddLog ("Achievement:" + a.name, true);
			}
		}

		if (thiefId >= 200) {
			GameData._playerData.legendThiefCaught++;
			_gameData.StoreData ("LegendThiefCaught", GameData._playerData.legendThiefCaught);
			if (GameData._playerData.Achievements [38] == 0) {
				a = LoadTxt.GetAchievement (38);
				if (GameData._playerData.legendThiefCaught >= a.req) {
					StoreAchievement (38);
					_floating.CallInFloating ("Achievement:" + a.name,0);
					_log.AddLog ("Achievement:" + a.name,true);
				}
			}
		}
	}

	public void RenownChange(){
		if (GameData._playerData.Achievements [10] == 1)
			return;
		Achievement a = LoadTxt.GetAchievement (10);
		if (GameData._playerData.Renown >= a.req) {
			StoreAchievement (10);
			_floating.CallInFloating ("Achievement:" + a.name,0);
			_log.AddLog ("Achievement:" + a.name,true);
		}

		if (GameData._playerData.Achievements [9] == 1)
			return;
		a = LoadTxt.GetAchievement (9);
		if (GameData._playerData.Renown >= a.req) {
			StoreAchievement (9);
			_floating.CallInFloating ("Achievement:" + a.name,0);
			_log.AddLog ("Achievement:" + a.name,true);
		}

		if (GameData._playerData.Achievements [8] == 1)
			return;
		a = LoadTxt.GetAchievement (8);
		if (GameData._playerData.Renown >= a.req) {
			StoreAchievement (8);
			_floating.CallInFloating ("Achievement:" + a.name,0);
			_log.AddLog ("Achievement:" + a.name,true);
		}
		
		if (GameData._playerData.Achievements [7] == 1)
			return;
		a = LoadTxt.GetAchievement (7);
		if (GameData._playerData.Renown >= a.req) {
			StoreAchievement (7);
			_floating.CallInFloating ("Achievement:" +a.name,0);
			_log.AddLog ("Achievement:" + a.name,true);
		}
		
	}

	public void GetToNewDungeon(int lv){
        if (GameData._playerData.Achievements[37] == 1)
            return;
		Achievement a = LoadTxt.GetAchievement (37);
		if (GameData._playerData.dungeonLevelMax >= a.req)
        {
            StoreAchievement (37);
			_floating.CallInFloating ("Achievement:" + a.name,0);
			_log.AddLog ("Achievement:" + a.name,true);
        }

        if (GameData._playerData.Achievements[24] == 1)
            return;
		a = LoadTxt.GetAchievement (24);
		if (GameData._playerData.dungeonLevelMax >= a.req)
        {
            StoreAchievement (24);
			_floating.CallInFloating ("Achievement:" + a.name,0);
			_log.AddLog ("Achievement:" + a.name,true);
        }
	}

	void StoreAchievement(int index){
		GameData._playerData.Achievements [index] = 1;
		_gameData.StoreData ("Achievements", _gameData.GetStrFromTechList (GameData._playerData.Achievements));
	}

}
