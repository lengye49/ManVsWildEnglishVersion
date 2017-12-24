using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClickFarmButton : MonoBehaviour {

	private FarmActions _farmAction;
	private Button b;
	void Start(){
		_farmAction = this.gameObject.GetComponentInParent<FarmActions> ();
		b = this.gameObject.GetComponentInChildren<Button> ();
	}

	public void OnChargeOrPrepare(){
		this.gameObject.GetComponentInParent<PlaySound> ().PlayClickSound ();
		string[] s = b.name.Split ('|');

		int i = int.Parse (s [0]);
		if (s[1] == "Prepare") {
			_farmAction.CallInPlantingTip (i);
		} else if (s[1] == "Charge") {
			_farmAction.ChargeCrop (i);
		} else {
			Debug.Log ("No crop found.");
		}
	}
}
