using System.Collections;
using System.Collections.Generic;

public class Building  
{
	public int id;
	public string name;
	public int maxLv;
	public Dictionary<int,int> combReq = new Dictionary<int, int>();
	public string tips;
	public int timeCost;
}
