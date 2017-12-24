
using System.Collections;
using System.Collections.Generic;

public class Plants {
	public int plantType;
	public Dictionary<int,int> plantReq;
	public int plantTime;
	public int plantGrowCycle;
	public Dictionary<int,int> plantObtain;
}


public class FarmState{
	public int open;
	public int plantType;
	public int plantTime;
}