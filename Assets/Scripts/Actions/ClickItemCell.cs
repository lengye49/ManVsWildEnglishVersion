using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickItemCell : MonoBehaviour {

	private TipManager _tipManager;

	public void OnWhCell(){
		this.gameObject.GetComponentInParent<PlaySound> ().PlayClickSound ();
		_tipManager = this.gameObject.GetComponentInParent<TipManager> ();
		int itemId = int.Parse (this.gameObject.name);
		_tipManager.ShowNormalTips (itemId,0);
	}

	public void OnBpCell(string place){
		this.gameObject.GetComponentInParent<PlaySound> ().PlayClickSound ();
		_tipManager = this.gameObject.GetComponentInParent<TipManager> ();
		int itemId = int.Parse (this.gameObject.name);
		if(place == "wh")
			_tipManager.ShowNormalTips (itemId,1);
		else if(place=="normal")
			_tipManager.ShowNormalTips (itemId,2);
	}

	/// <summary>
	/// Type:1Melee 2Ranged 3Magic 4Head 5Body 6Shoe 7Accessory 8Ammo 9Mount.
	/// </summary>
	/// <param name="type">Type.</param>
	public void OnEquipCell(int type){
		this.gameObject.GetComponentInParent<PlaySound> ().PlayClickSound ();
		_tipManager = this.gameObject.GetComponentInParent<TipManager> ();

		switch (type) {
		case 1:
			if (GameData._playerData.MeleeId > 0)
				_tipManager.ShowNormalTips (GameData._playerData.MeleeId, 3);
			else
				_tipManager.OnNormalTipCover ();
			break;
		case 2:
			if (GameData._playerData.RangedId > 0)
				_tipManager.ShowNormalTips (GameData._playerData.RangedId, 3);
			else
				_tipManager.OnNormalTipCover ();
			break;
		case 3:
			if (GameData._playerData.MagicId > 0)
				_tipManager.ShowNormalTips (GameData._playerData.MagicId, 3);
			else
				_tipManager.OnNormalTipCover ();
			break;
		case 4:
			if (GameData._playerData.HeadId > 0)
				_tipManager.ShowNormalTips (GameData._playerData.HeadId, 3);
			else
				_tipManager.OnNormalTipCover ();
			break;
		case 5:
			if (GameData._playerData.BodyId > 0)
				_tipManager.ShowNormalTips (GameData._playerData.BodyId, 3);
			else
				_tipManager.OnNormalTipCover ();
			break;
		case 6:
			if (GameData._playerData.ShoeId > 0)
				_tipManager.ShowNormalTips (GameData._playerData.ShoeId, 3);
			else
				_tipManager.OnNormalTipCover ();
			break;
		case 7:
//			if (GameData._playerData.AccessoryId > 0)
//				_tipManager.ShowNormalTips (GameData._playerData.AccessoryId, 3);
//			else
//				_tipManager.OnNormalTipCover ();
			break;
		case 8:
			if (GameData._playerData.AmmoId > 0)
				_tipManager.ShowNormalTips (GameData._playerData.AmmoId, 4);
			else
				_tipManager.OnNormalTipCover ();
			break;
		case 9:
			if (GameData._playerData.Mount.monsterId > 0)
				_tipManager.ShowMountTips (GameData._playerData.Mount);
			else
				_tipManager.OnNormalTipCover ();
			break;
		default:
			break;
		}
	}

	public void OnMakingCell(){
		this.gameObject.GetComponentInParent<PlaySound> ().PlayClickSound ();
		_tipManager = this.gameObject.GetComponentInParent<TipManager> ();
		int itemId = int.Parse (this.gameObject.name);
		_tipManager.ShowMakingTips (itemId);
	}

	public void OnStudyCell(){
		this.gameObject.GetComponentInParent<PlaySound> ().PlayClickSound ();
		_tipManager = this.gameObject.GetComponentInParent<TipManager> ();
		int studyId = int.Parse (this.gameObject.name);
		_tipManager.ShowTechTips (studyId);
	}
}
