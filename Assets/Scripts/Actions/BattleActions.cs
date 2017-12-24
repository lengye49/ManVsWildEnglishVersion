using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class BattleActions : MonoBehaviour {

	public Text enemyName;
	public Text enemyDistance;
	public GameObject battleLogContainer;
	public Slider enemyHpSlider;
	public Image enemyHpFill;
	public Slider myHpSlider;
	public Image myHpFill;
	public Text timeBarEnd;
	public GameObject myPoint;
	public GameObject enemyPoint;

	public Button bp;
	public Button map;
	public Button goHome;
	public Button meleeAttack;
	public Button rangedAttack;
	public Button magicAttack;
	public Button jumpForward;
	public Button jumpBackward;
	public Button capture;
	public Button autoButton;
	public Button returnButton;
	public Text[] battleLogs = new Text[9];

	public LogManager _logManager;

	public Image autoOn;
	public Image autoOff;

	private Unit enemy;
	private float myNextTurn;
	private float enemyNextTurn;
	private float distance;
	private GameData _gameData;
	private PanelManager _panelManager;
	private FloatingActions _floating;
	private float enemyMaxHp;
	private bool isAuto;
	private int captureFailTime;
	private int thisEnemyIndex;
	private Monster[] thisMonsters;

	private AchieveActions _achieveActions;

	void Start(){
		_gameData = this.gameObject.GetComponentInParent<GameData> ();
		this.gameObject.transform.localPosition = new Vector3 (-2000, 0, 0);
		_panelManager = this.gameObject.GetComponentInParent<PanelManager> ();
		_floating = GameObject.Find ("FloatingSystem").GetComponent<FloatingActions> ();
		_achieveActions = this.gameObject.GetComponentInParent<AchieveActions> ();
	}

	public void InitializeBattleField(Monster[] monsters,bool isAttacked){
		thisMonsters = monsters;
		bp.interactable = false;
		map.interactable = false;
		goHome.interactable = false;
		thisEnemyIndex = 0;
		autoButton.gameObject.SetActive (true);
		returnButton.gameObject.SetActive (false);
		if (isAuto)
			OnAuto ();
		ClearLog ();

		if (isAttacked) {
			if (monsters.Length > 1)
				AddLog ("注意，你被" + monsters.Length + "名敌人围攻了!", 0);
			else
				AddLog ("注意，你被" + monsters [0].name + "偷袭了！", 0);
		} else {
			if (monsters.Length > 1)
				AddLog ("你发现了" + monsters.Length + "名敌人!", 0);
			else
				AddLog ("你发现了" + monsters [0].name + "!", 0);
		}

		CheckNewPlayerGuide ();

		SetEnemy ();

        if (isAttacked)
        {
            if (distance > enemy.range)
            {
                Move(true, enemy.speed, true);
            }
            else
            {
                EnemyCastSkill();
            }
        }
	}

	void SetEnemy(){
		SetState ();
		int r = Random.Range(0, 999);
		if (r > 900)
		{
			int titleIndex = Algorithms.GetIndexByRange(0, GameConfigs.MonsterTitleCount - 1);
			GetEnemyProperty(thisMonsters[thisEnemyIndex - 1], titleIndex);
		}
		else
		{
			GetEnemyProperty(thisMonsters[thisEnemyIndex - 1]);
		}

		ResetPanel ();
	}

	void SetState(){
		thisEnemyIndex++;
		myNextTurn = 0;
		enemyNextTurn = 0;
		captureFailTime = 0;
	}

	void CallOutBattlePanel(){
		bp.interactable = true;
		map.interactable = true;
		goHome.interactable = true;
		this.gameObject.transform.localPosition = new Vector3 (-2000, 0, 0);
	}
		
	void ClearLog(){
		for (int i = 0; i < battleLogs.Length; i++) {
			battleLogs [i].text = "";
		}
	}

	void SetActions(){
		if (isAuto) {
			meleeAttack.interactable = false;
			rangedAttack.interactable = false;
			magicAttack.interactable = false;
			jumpForward.interactable = false;
			jumpBackward.interactable = false;
			capture.interactable = false;
		} else {
			meleeAttack.interactable = (GameData._playerData.property [19] >= distance);

			rangedAttack.interactable = (GameData._playerData.RangedId > 0 
									&& GameData._playerData.property [20] >= distance 
									&& GameData._playerData.AmmoId > 0 
									&& GameData._playerData.AmmoNum > 0);
			magicAttack.interactable = (GameData._playerData.MagicId > 0);
			jumpForward.interactable = true;
			jumpBackward.interactable = true;
			capture.interactable = ((enemy.hp <= (enemyMaxHp * 0.5f)) && enemy.canCapture > 0 && captureFailTime < 3);
		}
	}

	void GetEnemyProperty(Monster m){
//		Debug.Log("thisEnemyId = " + m.id);
		enemy = new Unit ();
		enemy.monsterId = m.id;
		enemy.name = m.name;
		//怪物实力随等级增强
		enemy.level = m.level + GameData._playerData.dayNow / GameConfigs.MonsterUpgradeTime;
//		Debug.Log (enemy.level);

		MonsterModel md = LoadTxt.GetMonsterModel (m.model);

		enemy.hp = md.hp + md.hp_inc * (enemy.level - 1);
		enemyMaxHp = enemy.hp;
		enemy.spirit = m.spirit;
		enemy.atk = md.atk + md.atk_inc * (enemy.level - 1);
//		Debug.Log("ThisEnemyInitAtk = " + enemy.atk);
		enemy.def = md.def + md.def_inc * (enemy.level - 1);
		enemy.hit = md.hit;
		enemy.dodge = md.dodge;
		enemy.speed = m.speed;
		enemy.range = m.range;
//        print(m.range);
		enemy.castSpeedBonus = 0;
		enemy.skillList = m.skillList;
		enemy.drop = m.drop;
		enemy.vitalSensibility = m.vitalSensibility;
		enemy.hit_Body = m.bodyPart[0];
		enemy.hit_Vital = m.bodyPart[1];
		enemy.hit_Move = m.bodyPart [2];
		enemy.canCapture = m.canCapture;
		enemy.mapOpen = m.mapOpen;
		enemy.renown = m.renown;
	}

	void GetEnemyProperty(Monster m,int titleIndex){
		GetEnemyProperty(m);
		MonsterTitle mt = LoadTxt.GetMonsterTitle (titleIndex);
		enemy.name +=  "[" + mt.title + "]";
		enemy.hp *= 1.0f + mt.hpBonus ;
		enemyMaxHp = enemy.hp;
		enemy.atk *= 1.0f + mt.atkBonus ;
		enemy.def *= 1.0f + mt.defBonus ;
		enemy.dodge *= 1.0f + mt.dodgeBonus ;
		enemy.speed *= (1.0f + mt.speedBonus );
		enemy.castSpeedBonus = mt.attSpeedBonus;
	}

	void ResetPanel(){
		int maxdistance = Mathf.Max ((int)(enemy.range), (int)GameData._playerData.property [19], (int)GameData._playerData.property [20]);
		int r = Random.Range (0, 100);
		if (r < 75)
			distance = Random.Range (0, (int)GameData._playerData.property [19]);
		else
			distance = Random.Range(0, maxdistance);

		enemyName.text = enemy.name;
		enemyDistance.text = distance.ToString() +"m";
		SetMyHpSlider ();
		SetEnemyHpSlider ();
		SetPoint ();
		SetActions ();

		if (isAuto) {
			isAuto = false;
			OnAuto ();
		}
	}

	private float maxTime = 0f;
	void SetPoint(){
		if (maxTime == 0f)
			maxTime = 20f;
		else if (Mathf.Max (myNextTurn, enemyNextTurn) > maxTime * 0.9f)
			maxTime = maxTime * 3f;
		timeBarEnd.text = maxTime.ToString ();
		float myPointX = 800f * myNextTurn / (float)maxTime - 400f;
		float enemyPointX = 800f * enemyNextTurn / (float)maxTime - 400f;
		myPoint.transform.DOLocalMoveX (myPointX, 0.5f);
		enemyPoint.transform.DOLocalMoveX (enemyPointX, 0.5f);
	}

	/// <summary>
	/// Add log.需要处理新加一行，显示被遮挡的问题
	/// </summary>
	/// <param name="s">S.</param>
	/// <param name="isGood">Is good:0normal 1good 2bad.</param>
	void AddLog(string s,int isGood){
		for (int i = 0; i < 8; i++) {
			battleLogs [i].text = battleLogs [i + 1].text;
			battleLogs [i].color = new Color (battleLogs [i + 1].color.r, battleLogs [i + 1].color.g, battleLogs [i + 1].color.b, battleLogs [i + 1].color.a - 0.1f);
		}

		battleLogs[8].text =  "→" + s;
		if (isGood == 1)
			battleLogs[8].color = new Color(0f,1f,0f,1f);
		else if (isGood == 2)
			battleLogs[8].color = new Color(1f,0f,0f,1f);
		else
			battleLogs[8].color = new Color(1f,1f,1f,1f);
	}

	void AutoFight(){
		while (GameData._playerData.hpNow > 0 && enemy.hp > 0) {
			if (myNextTurn < enemyNextTurn) {
				//如果近战攻击高，则向前移动后再攻击；如果远程攻击高，直接用远程攻击
				if (GameData._playerData.property [13] > GameData._playerData.property [14]) {
					if (distance > GameData._playerData.property [19]) {
						Move (true, GameData._playerData.property [23], false);
					} else {
						MeleeFight ();
					}
				} else{
					if (distance > GameData._playerData.property [20]) {
						Move (true, GameData._playerData.property [23],false);
					} else {
						RangedFight ();
					}
				}
			} else {
				if (distance > enemy.range) {
					Move (true, enemy.speed,true);
				} else {
					EnemyCastSkill ();
				}
			}
		}
	}

	void Move(bool forward,float speed,bool isEnemyMove){

		int f = forward ? -1 : 1;
		float d = Mathf.Max (0, distance + speed * f);
		float dis = distance - d;
		distance = d;
		string s = forward ? "前进" : "后退";
		if (isEnemyMove) {
			enemyNextTurn += 1;
			AddLog (enemy.name + s + "了" + dis + "米。", 0);
		} else {
			myNextTurn += 1;
			AddLog ("你" + s + "了" + dis + "米。", 0);
		}

		enemyDistance.text = distance + "米";
		SetPoint ();
		SetActions ();
	}

	void Fight(Skill s){
		AddEffect (s, true,0);
	}

	void Fight(float hit,float dodge,float vitalSensibility,float spirit,float atk,float def,Skill s,bool isMyAtk){
		int hitRate = Algorithms.IsDodgeOrCrit (hit, dodge, vitalSensibility, spirit);
		string hitPart = "";
		if (hitRate == 0) {
//			Debug.Log ("Missed!");
			AddLog ((isMyAtk ? "你" : enemy.name) + "发起攻击，但是" + (isMyAtk ? enemy.name : "你") + "灵巧地躲开了!",0);
			return;
		} else if (hitRate == 1) {
			hitPart = isMyAtk ? GetHitPart (enemy.hit_Body) : "身体";
		} else {
			hitPart = isMyAtk ? GetHitPart (enemy.hit_Vital) : "头部";
		}

		int dam = Algorithms.CalculateDamage (atk, def, s, hitRate,isMyAtk);
//		Debug.Log ("Atk = " + atk + ", Dam = " + dam);

		if (isMyAtk) {
			enemy.hp -= dam;
			SetEnemyHpSlider ();
//			if(s>0)
//				AddEffect (LoadTxt.skillDic [skillId], isMyAtk, dam);
			AddLog ("你击中了" + enemy.name + "的" + hitPart + "，造成"+dam+"点伤害。", 0);
		} else {
			_gameData.ChangeProperty (0, -dam);
			SetMyHpSlider ();
			AddEffect (s, isMyAtk, dam);
			AddLog (enemy.name + "击中了你的" + hitPart + "，造成"+dam+"点伤害。", 0);
		}

		CheckBattleEnd ();
	}

	string GetHitPart(string s){
		string[] ss = s.Split ('|');
		return ss [Algorithms.GetIndexByRange (0, ss.Length)];
	}

	void AddEffect(Skill s,bool isMyAtk,int dam){
		if (s.effectId == 0)
			return;
		if (s.effectId > 0) {
			int r = Algorithms.GetIndexByRange (0, 100);
			if (r < s.effectProp) {
				switch (s.effectId) {
				case 100:
					if (isMyAtk) {
						enemyNextTurn += 2;
						AddLog (enemy.name + "受到[减速]效果，2秒内无法行动。", 0);
					} else {
						myNextTurn += 2;
						AddLog ("你受到[减速]效果，2秒内无法行动。", 0);
					}
					break;
				case 101:
					if (isMyAtk) {
						enemyNextTurn += 3;
						AddLog (enemy.name + "受到[减速]效果，3秒内无法行动。", 0);
					} else {
						myNextTurn += 3;
						AddLog ("你受到[混乱]效果，3秒内无法行动。", 0);
					}
					break;
				case 102:
					if (isMyAtk) {
						enemyNextTurn += 5;
						AddLog (enemy.name + "受到[魅惑]效果，5秒内无法行动。", 0);
					} else {
						myNextTurn += 5;
						AddLog ("你受到[魅惑]效果，5秒内无法行动。", 0);
					}
					break;
				case 103:
					if (isMyAtk) {
						enemyNextTurn += 4;
						AddLog (enemy.name + "受到[眩晕]效果，7秒内无法行动。", 0);
					} else {
						myNextTurn += 4;
						AddLog ("你受到[眩晕]效果，7秒内无法行动", 0);
					}
					break;
				case 104:
					if (isMyAtk) {
						enemyNextTurn += 4;
						AddLog (enemy.name + "受到[冰冻]效果，7秒内无法行动。", 0);
					} else {
						myNextTurn += 4;
						_gameData.ChangeProperty (10, -5);
						AddLog ("你受到[冰冻]效果，7秒内无法行动，温度-5℃。", 0);
					}
					break;
				case 105:
					if (isMyAtk) {
						enemyNextTurn += 5;
						AddLog (enemy.name + "被石化了5秒。", 0);
					} else {
						myNextTurn += 5;
						AddLog ("你被石化了5秒。", 0);
					}
					break;
				case 106:
					if (isMyAtk) {
						_gameData.ChangeProperty (2, 5);
						enemy.spirit -= 5;
						AddLog ("你吸取了" + enemy.name + "5点精神。", 0);
					} else {
						_gameData.ChangeProperty (2, -5);
						enemy.spirit += 5;
						AddLog (enemy.name + "吸取了你5点精神。", 0);
					}
					break;
				case 107:
					if (isMyAtk) {
						enemyNextTurn += 3;
						AddLog (enemy.name + "受到[束缚]效果，3秒内无法行动。", 0);
					} else {
						myNextTurn += 3;
						AddLog ("你受到[束缚]效果，3秒内无法行动。", 0);
					}
					break;
				case 109:
					int hpPlus = (int)(dam * 0.3f);
					if (isMyAtk) {
						_gameData.ChangeProperty (0, hpPlus);
						AddLog ("并吸取了" + hpPlus + "点生命。", 0);
					} else {
						enemy.hp += hpPlus;
						AddLog ("并吸取了" + hpPlus + "点生命。", 0);
					}
					break;
				case 110:
					int hpPlus1 = (int)(dam * 0.5f);
					if (isMyAtk) {
						_gameData.ChangeProperty (0, hpPlus1);
						AddLog ("并吸取了" + hpPlus1 + "点生命。", 0);
					} else {
						enemy.hp += hpPlus1;
						AddLog ("并吸取了" + hpPlus1 + "点生命。", 0);
					}
					break;
				case 111:
					if (!isMyAtk) {
						_gameData.ChangeProperty (10, 5);
						AddLog ("温度+5℃。", 0);
					}
					break;
				default:
					Debug.Log ("Incorrect effectId: " + s.effectId);
					break;
				}
			}
			SetMyHpSlider ();
			SetEnemyHpSlider ();
		}
	}

	void CheckBattleEnd(){
		if (enemy.hp > 0) {
			CheckEnemyAction ();
			SetActions ();
			return;
		}

		string s = "你击败了" + enemy.name + "。";
		AddLog (s, 1);

		if (enemy.mapOpen > 0) {
			if (GameData._playerData.MapOpenState [enemy.mapOpen] == 0) {
				GameData._playerData.MapOpenState [enemy.mapOpen] = 1;
				_gameData.StoreData ("MapOpenState", _gameData.GetStrFromMapOpenState (GameData._playerData.MapOpenState));
				s = "他告诉你去往" + LoadTxt.MapDic [enemy.mapOpen].name + "的方向。";
				AddLog (s, 1);
				_logManager.AddLog ("你发现了去" + LoadTxt.MapDic [enemy.mapOpen].name + "的路。", true);
			} else {
				s = "他告诉你去往" + LoadTxt.MapDic [enemy.mapOpen].name + "的方向。";
				AddLog (s, 1);
				s = "但是你已经知道了。";
				AddLog (s, 1);
			}
		}

		Dictionary<int,int> drop = Algorithms.GetReward (enemy.drop);

		if (drop.Count > 0) {
            s = "获得了";
			foreach (int key in drop.Keys) {
				int itemId = GenerateItemId (key);
				_gameData.AddItem (itemId, drop [key]);
				s += LoadTxt.MatDic [key].name + " ×" + drop [key] + ",";

				//Achievement
				switch (LoadTxt.MatDic [key].type) {
				case 3:
					this.gameObject.GetComponentInParent<AchieveActions> ().CollectMeleeWeapon (key);
					break;
				case 4:
					this.gameObject.GetComponentInParent<AchieveActions> ().CollectRangedWeapon (key);
					break;
				case 5:
					this.gameObject.GetComponentInParent<AchieveActions> ().CollectMagicWeapon (key);
					break;
				default:
					break;
				}
			}
			s = s.Substring (0, s.Length - 1) ;
			if (enemy.renown > 0) {
				s += "和" + enemy.renown + "点声望。";
			} else {
				s += "。";
			}
			AddLog (s,1);
		} else {			
			if (enemy.renown > 0) {
				s = "获得了" + enemy.renown + "点声望。";
				AddLog (s,1);
			} 
		}

		if (enemy.renown > 0)
			_gameData.AddRenown (enemy.renown);


        if (enemy.monsterId == 3008 && GameData._playerData.orderCamp == 0)
        {
            AddLog("你获得了秩序阵营的认可，秩序阵营决定退出战争。", 1);
        }
        else if (enemy.monsterId == 3108 && GameData._playerData.truthCamp == 0)
        {
            AddLog("你获得了真理阵营的认可，真理阵营决定不再挑起战争。", 1);
        }
        else if (enemy.monsterId == 3208 && GameData._playerData.lifeCamp == 0)
        {
            AddLog("你获得了生命阵营的认可，生命阵营决定回归森林。", 1);
        }
        else if (enemy.monsterId == 3308 && GameData._playerData.chaosCamp == 0)
        {
            AddLog("你获得了混乱阵营的认可，混乱阵营决定退回深渊。", 1);
        }
        else if (enemy.monsterId == 3408 && GameData._playerData.deathCamp == 0)
        {
            AddLog("你获得了死亡阵营的认可，死亡阵营决定保持沉寂。", 1);
        }
        
		//添加成就
		_achieveActions.DefeatEnemy (enemy.monsterId);

		StartCoroutine (WaitAndCheck ());
	}

	IEnumerator WaitAndCheck(){
		meleeAttack.interactable = false;
		rangedAttack.interactable = false;
		magicAttack.interactable = false;
		jumpForward.interactable = false;
		jumpBackward.interactable = false;
		capture.interactable = false;
		yield return new WaitForSeconds (1f);
		CheckNextEnemy ();

	}

	void CheckNextEnemy(){
		if (thisEnemyIndex < thisMonsters.Length) {
			SetEnemy ();
		} else {
			autoButton.gameObject.SetActive (false);
			returnButton.gameObject.SetActive (true);
		}
	}

	int GenerateItemId(int orgId){
		int i = 0;
		int j = 0;
		if (LoadTxt.MatDic [orgId].type == 3) {
			i = Algorithms.GetIndexByRange (0, 10);
			j = _gameData.meleeIdUsedData;
			PlayerPrefs.SetInt ("MeleeIdUsed", j++);
		} else if (LoadTxt.MatDic [orgId].type == 4) {
			i = Algorithms.GetIndexByRange (0, 10);
			j = _gameData.rangedIdUsedData;
			PlayerPrefs.SetInt ("RangedIdUsed", j++);
		} 

		return orgId * 10000 + i * 1000 + j;
	}


	void SetEnemyHpSlider(){
		enemyHpSlider.value = enemy.hp / enemyMaxHp;
		enemyHpFill.color = GetColor (enemy.hp / enemyMaxHp);
	}

	void SetMyHpSlider(){
		myHpSlider.value = GameData._playerData.property [0] / GameData._playerData.property [1];
		myHpFill.color = GetColor (GameData._playerData.property [0] / GameData._playerData.property [1]);
	}

	Color GetColor(float value){
		Color c = new Color ();

		if (value > 0.5)
			c = new Color ((1f - value) * 300f / 255f, 150f / 255F, 0F,1f);
		else if (value <= 0)
			c = new Color (1f, 1f, 1f, 0f);
		else
			c = new Color (150f / 255f, value * 300f / 255f, 0f,1f);



		return c;
	}

	public void MeleeFight(){
//		int skillId = LoadTxt.MatDic [(int)(GameData._playerData.MeleeId/10000)].skillId;
		myNextTurn += GameData._playerData.property [21];
		SetPoint ();
		Fight (GameData._playerData.property [16], enemy.dodge, enemy.vitalSensibility, GameData._playerData.property [2], GameData._playerData.property [13], enemy.def, new Skill(), true);
		//Achievement
		_achieveActions.Fight ("Melee");
	}
	public void RangedFight(){
//		int skillId = LoadTxt.MatDic [(int)(GameData._playerData.RangedId/10000)].skillId;
		myNextTurn += GameData._playerData.property [22];

		float att = GameData._playerData.property [14];

		int ammoId = GameData._playerData.AmmoId / 10000;
		if (LoadTxt.MatDic [ammoId].property.ContainsKey (26))
			att *= (LoadTxt.MatDic [ammoId].property [26] / 100f + 1f);

		if (LoadTxt.MatDic [ammoId].property.ContainsKey (14))
			att += LoadTxt.MatDic [ammoId].property [14];
		
		SetPoint ();
		Fight (GameData._playerData.property [17], enemy.dodge, enemy.vitalSensibility, GameData._playerData.property [2], att, enemy.def, new Skill(), true);

		GameData._playerData.AmmoNum--;
		_gameData.StoreData ("AmmoNum", GameData._playerData.AmmoNum);

		if (GameData._playerData.AmmoNum == 0) {
			GameData._playerData.AmmoId = 0;
			_gameData.StoreData ("AmmoId", 0);
		}

		//Achievement
		_achieveActions.Fight ("Ranged");
	}
	public void MagicFight(){
		myNextTurn += 1;
		SetPoint ();
		_gameData.ChangeProperty (2, -(int)(LoadTxt.MatDic [GameData._playerData.MagicId/10000].castSpirit * GameData._playerData.MagicCostRate));

//		print (GameData._playerData.MagicId);
		if (GameData._playerData.MagicId / 10000 == 303 || GameData._playerData.MagicId / 10000 == 305) {
			int heal = (int)(GameData._playerData.property [24] * GameData._playerData.MagicPower / 10);
			_gameData.ChangeProperty (0, heal);
			AddLog ("你使用" + LoadTxt.MatDic [GameData._playerData.MagicId / 10000].name + ",回复了" + heal + "点生命。", 0);
		} else {
			int dam = (int)(GameData._playerData.property [24] * GameData._playerData.MagicPower );
			enemy.hp -= dam;
			enemy.hp = Mathf.Max (0, enemy.hp);
			AddLog ("你使用" + LoadTxt.MatDic [GameData._playerData.MagicId / 10000].name + ",对" + enemy.name + "造成" + dam + "点伤害。", 0);
		}

		SetEnemyHpSlider ();
		SetMyHpSlider ();
		CheckBattleEnd ();

		//Achievement
		_achieveActions.Fight ("Magic");
	}
	public void JumpForward(){
		Move (true, GameData._playerData.property [23], false);
		CheckEnemyAction ();
	}
	public void JumpBackward(){
		Move (false, GameData._playerData.property [23], false);
		CheckEnemyAction ();
	}
	public void Capture(){
		if (enemy.canCapture == 0 || (enemy.hp / enemyMaxHp > 0.5f) || captureFailTime >= 3)
			return;
		if (_gameData.GetUsedPetSpace () + enemy.canCapture > GameData._playerData.PetsOpen * 10) {
			_floating.CallInFloating ("宠物笼空间不足!", 1);
			return;
		}

		myNextTurn++;
		SetPoint ();
		float rate = 0.2f - enemy.level / 100 * 0.5f;

		rate *= GameData._playerData.CaptureRate;
		int i = Algorithms.GetIndexByRange (0, 10000);
		if (i < (int)(rate * 10000)) {
			Pet p = new Pet ();
			p.monsterId = enemy.monsterId;
			p.state = 0;
			p.alertness = enemy.level;
			p.speed = (int)enemy.speed;
			p.name = enemy.name;
			_gameData.AddPet (p);
			AddLog ("你捕获了新宠物: " + enemy.name, 0);

			//Achievement
			this.gameObject.GetComponentInParent<AchieveActions> ().CapturePet ();
			//战斗结束。。
			StartCoroutine (WaitAndCheck ());
		}else{
			captureFailTime++;
			AddLog ("你试图抓捕" + enemy.name + "，但是失败了。", 0);
            if(captureFailTime>=3)
                AddLog ("已经超过捕捉上限，该猎物不能被捕捉了。", 0);
			CheckEnemyAction ();
		}
	}

	void CheckEnemyAction(){
		while (enemyNextTurn < myNextTurn) {
            if (distance > enemy.range) {
                Move (true, enemy.speed,true);
            } else {
                EnemyCastSkill ();
            }
		}
	}

	void EnemyCastSkill(){
		int index = Algorithms.GetIndexByRange (0, enemy.skillList.Length);
		enemyNextTurn += enemy.skillList[index].castSpeed * (1 - enemy.castSpeedBonus);
		SetPoint ();
		Fight (enemy.hit, GameData._playerData.property [18], 90, enemy.spirit, enemy.atk, GameData._playerData.property [15], enemy.skillList [index], false);
	}

	public void OnAuto(){
		isAuto = !isAuto;
		autoOn.gameObject.SetActive (isAuto);
		autoOff.gameObject.SetActive (!isAuto);
		if (isAuto)
			AutoFight ();
	}

	public void OnReturn(){
		CallOutBattlePanel ();
		_panelManager.GoToPanel ("Father");
	}

	void CheckNewPlayerGuide(){
		if (_gameData.battleCount >= 10)
			return;
		_gameData.battleCount++;
		_gameData.StoreData ("BattleCount", _gameData.battleCount);
		AddLog ("点击[前跳]靠近对手，[后跃]拉开距离;", 0);
		AddLog ("点击[近战]、[远程]、[魔法]使用对应武器攻击;", 0);
		AddLog ("某些对手在生命值较低时可以被[捕获]为宠物;", 0);
		AddLog ("在你优势较大时可以点击[自动]开启自动战斗。", 0);
	}
}
