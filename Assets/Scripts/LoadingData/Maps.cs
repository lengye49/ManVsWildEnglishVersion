using System.Collections;
using System.Collections.Generic;

public class Maps {
	public int id;
	public string name;
	public string desc;
	public Dictionary<int,int> distances;
	public ArrayList mapNext;//探索本地图可以打开的地图列表
}
