using System.Collections;
using System.Collections.Generic;

public class Monster {
	public int id;
	public string name;
	public int level;
	public int model;
	public int spirit;
	public float speed;
	public float range;
	public int vitalSensibility;
	public Skill[] skillList;
	public string[] bodyPart;
	public Dictionary<int,float> drop;
	public int canCapture;
	public int groupNum;
	public int mapOpen;
	public int renown;
    public int livePlace;
}

public class MonsterModel{
	public int modelType;
	public float hp;
	public float hp_inc;
	public float atk;
	public float atk_inc;
	public float def;
	public float def_inc;
	public float hit;
	public float dodge;
}


public class Unit{
	public string name;
	public int monsterId;
	public int level;
	public float hp;
	public int spirit;
	public float atk;
	public float def;
	public float hit;
	public float dodge;
	public float speed;
	public float range;
	public Skill[] skillList;
	public Dictionary<int,float> drop;
	public int vitalSensibility;
	public string hit_Body;
	public string hit_Move;
	public string hit_Vital;
	public int canCapture;
	public float castSpeedBonus;
	public int mapOpen;
	public int renown;
}

/// <summary>
/// Monster title, prop = prop * (1+Bonus) //id必须按顺序排列
/// </summary>
public class MonsterTitle{
	public int id; 
	public string title;
	public float hpBonus;
	public float atkBonus;
	public float defBonus;
	public float attSpeedBonus;
	public float speedBonus;
	public float dodgeBonus;
}

/// <summary>
/// Skills, power,effectProp是万分比
/// </summary>
public class Skill{
	public int id;
	public string name;
	public int target;
	public int power;
	public int effectId;
	public int effectProp;
	public float castSpeed;
}

/// <summary>
/// Effect type 0吸血|比例 1吸精|比例 2眩晕|时间 3致死 4减甲|值 5减速|值 6...(概率放在技能上)
/// </summary>
public class SkillEffect{
	public int id;
	public int type;
	public int param;
}

