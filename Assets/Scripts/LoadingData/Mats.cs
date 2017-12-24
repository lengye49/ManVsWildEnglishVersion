using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Mats Type:
/// 0 Normal material
/// 1 Blue print or recipe
/// 2 Food
/// 3 Melee
/// 4 Ranged
/// 5 Magic
/// 6 Head
/// 7 Body
/// 8 Shoe
/// 9 Accessories
/// 10 Ammo
/// </summary>
public class Mats  {
	public int id;
	public int type;
	public string name;
	public int desc;//工作台和厨房等级限制
	public int price;
	public Dictionary<int,float> property;
	public int combGet;
	public Dictionary<int,int> combReq;
	public int needBlueprint;
	public string makingType;
	public int makingTime;
	public int castSpirit;
	public string tags;
	public int skillId;
	public int quality;
    public string description;//物品描述
}
	