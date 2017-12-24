using UnityEngine;
using System.Collections;

public class ClickPetCell : MonoBehaviour {

	public void OnClick(){
		this.gameObject.GetComponentInParent<PlaySound> ().PlayClickSound ();
		int i = int.Parse (this.gameObject.name);
		Pet p = new Pet ();
		foreach (int key in GameData._playerData.Pets.Keys) {
			if (key == i) {
				p = GameData._playerData.Pets [key];
				break;
			}
		}

		this.gameObject.GetComponentInParent<PetsActions> ().CallInDetail (p, i);
	}
}
