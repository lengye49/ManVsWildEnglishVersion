using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HomeManager : MonoBehaviour {

	public Button BedRoom;
	public Button Warehouse;
	public Button Kitchen;
	public Button Workshop;
	public Button Study;
	public Button Farm;
	public Button Pets;
	public Button Well;
	public Button MailBox;
	public Button Altar;

	private Color openColor;

	void Start(){
		BedRoom.gameObject.SetActive (false);
		Warehouse.gameObject.SetActive (false);
		Kitchen.gameObject.SetActive (true);
		Workshop.gameObject.SetActive (false);
		Study.gameObject.SetActive (false);
		Farm.gameObject.SetActive (false);
		Pets.gameObject.SetActive (false);
		Well.gameObject.SetActive (false);
		MailBox.gameObject.SetActive (false);
		Altar.gameObject.SetActive (false);
		openColor = new Color (126f / 255f, 241f / 255f, 251f / 255f, 1f);
	}

	public void UpdateContent(){
//		Color c = new Color (126f / 255f, 241f / 255f, 251f / 255f);

		Kitchen.gameObject.GetComponent<Image> ().color = GameData._playerData.KitchenOpen > 0 ? openColor : Color.gray;
		Kitchen.gameObject.GetComponentInChildren<Text> ().text = "Kitchen";
		Kitchen.gameObject.GetComponentInChildren<Text> ().color = GameData._playerData.KitchenOpen > 0 ? openColor : Color.gray;

		if (GameData._playerData.KitchenOpen > 0) {
			BedRoom.gameObject.SetActive (true);
			BedRoom.gameObject.GetComponent<Image> ().color = GameData._playerData.BedRoomOpen > 0 ? openColor : Color.gray;
			BedRoom.gameObject.GetComponentInChildren<Text> ().text = "BedRoom";
			BedRoom.gameObject.GetComponentInChildren<Text> ().color=GameData._playerData.BedRoomOpen > 0 ? openColor : Color.gray;

			if (GameData._playerData.BedRoomOpen > 0) {
				Warehouse.gameObject.SetActive (true);
				Warehouse.gameObject.GetComponent<Image> ().color = GameData._playerData.WarehouseOpen > 0 ? openColor : Color.gray;
				Warehouse.gameObject.GetComponentInChildren<Text> ().text = "Warehouse";
				Warehouse.gameObject.GetComponentInChildren<Text> ().color = GameData._playerData.WarehouseOpen > 0 ? openColor : Color.gray;

				if (GameData._playerData.WarehouseOpen > 0) {
					Workshop.gameObject.SetActive (true);
					Workshop.gameObject.GetComponent<Image> ().color = GameData._playerData.WorkshopOpen > 0 ? openColor : Color.gray;
					Workshop.gameObject.GetComponentInChildren<Text> ().text = "Workshop";
					Workshop.gameObject.GetComponentInChildren<Text> ().color = GameData._playerData.WorkshopOpen > 0 ? openColor : Color.gray;

					if (GameData._playerData.WorkshopOpen > 0) {
						Study.gameObject.SetActive (true);
						Study.gameObject.GetComponent<Image> ().color = GameData._playerData.StudyOpen > 0 ? openColor : Color.gray;
						Study.gameObject.GetComponentInChildren<Text> ().text = "Study";
						Study.gameObject.GetComponentInChildren<Text> ().color = GameData._playerData.StudyOpen > 0 ? openColor : Color.gray;

						if (GameData._playerData.StudyOpen > 0) {
							Farm.gameObject.SetActive (true);
							Farm.gameObject.GetComponent<Image> ().color = GameData._playerData.FarmOpen > 0 ? openColor : Color.gray;
							Farm.gameObject.GetComponentInChildren<Text> ().text = "Farm";
							Farm.gameObject.GetComponentInChildren<Text> ().color = GameData._playerData.FarmOpen > 0 ? openColor : Color.gray;

							Pets.gameObject.SetActive (true);
							Pets.gameObject.GetComponent<Image> ().color = GameData._playerData.PetsOpen > 0 ? openColor : Color.gray;
							Pets.gameObject.GetComponentInChildren<Text> ().text = "Pets";
							Pets.gameObject.GetComponentInChildren<Text> ().color = GameData._playerData.PetsOpen > 0 ? openColor : Color.gray;

							Well.gameObject.SetActive (true);
							Well.gameObject.GetComponent<Image> ().color = GameData._playerData.WellOpen > 0 ? openColor : Color.gray;
							Well.gameObject.GetComponentInChildren<Text> ().text = "Well";
							Well.gameObject.GetComponentInChildren<Text> ().color = GameData._playerData.WellOpen > 0 ? openColor : Color.gray;

							if (GameData._playerData.FarmOpen > 0 && GameData._playerData.PetsOpen > 0 && GameData._playerData.WellOpen > 0) {
								Altar.gameObject.SetActive (true);
								Altar.gameObject.GetComponent<Image> ().color = GameData._playerData.AltarOpen > 0 ? openColor : Color.gray;
								Altar.gameObject.GetComponentInChildren<Text> ().text = "Altar";
								Altar.gameObject.GetComponentInChildren<Text> ().color = GameData._playerData.AltarOpen > 0 ? openColor : Color.gray;
								
								if (GameData._playerData.AltarOpen > 0) {
									MailBox.gameObject.SetActive (true);
									MailBox.gameObject.GetComponent<Image> ().color = openColor;
									MailBox.gameObject.GetComponentInChildren<Text> ().text = "Achievement";
								}
							}
						}
					}
				}
			}
		}
	}
}
