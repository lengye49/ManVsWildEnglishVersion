using UnityEngine;
using System.Collections;

public class ClickMapCell : MonoBehaviour {

	public void GoToPlace(){		
		int i = int.Parse (this.gameObject.name);
		Maps m = LoadTxt.MapDic [i];
		ExploreActions e = this.gameObject.GetComponentInParent<ExploreActions> ();
        e.GoToPlace (m.id);
	}
}
