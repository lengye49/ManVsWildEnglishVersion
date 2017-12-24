﻿using UnityEngine;
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
		openColor = new Color (150f / 255f, 150f / 255f, 150f / 255f, 1f);
	}

	public void UpdateContent(){

		Kitchen.gameObject.GetComponent<Image> ().color = GameData._playerData.KitchenOpen > 0 ? openColor : Color.gray;
		Kitchen.gameObject.GetComponentInChildren<Text> ().text = "厨房";
		Kitchen.gameObject.GetComponentInChildren<Text> ().color = GameData._playerData.KitchenOpen > 0 ? Color.white : Color.gray;

		if (GameData._playerData.KitchenOpen > 0) {
			BedRoom.gameObject.SetActive (true);
			BedRoom.gameObject.GetComponent<Image> ().color = GameData._playerData.BedRoomOpen > 0 ? openColor : Color.gray;
			BedRoom.gameObject.GetComponentInChildren<Text> ().text = "休息室";
			BedRoom.gameObject.GetComponentInChildren<Text> ().color=GameData._playerData.BedRoomOpen > 0 ? Color.white : Color.gray;

			if (GameData._playerData.BedRoomOpen > 0) {
				Warehouse.gameObject.SetActive (true);
				Warehouse.gameObject.GetComponent<Image> ().color = GameData._playerData.WarehouseOpen > 0 ? openColor : Color.gray;
				Warehouse.gameObject.GetComponentInChildren<Text> ().text = "仓库";
				Warehouse.gameObject.GetComponentInChildren<Text> ().color = GameData._playerData.WarehouseOpen > 0 ? Color.white : Color.gray;

				if (GameData._playerData.WarehouseOpen > 0) {
					Workshop.gameObject.SetActive (true);
					Workshop.gameObject.GetComponent<Image> ().color = GameData._playerData.WorkshopOpen > 0 ? openColor : Color.gray;
					Workshop.gameObject.GetComponentInChildren<Text> ().text = "工作台";
					Workshop.gameObject.GetComponentInChildren<Text> ().color = GameData._playerData.WorkshopOpen > 0 ? Color.white : Color.gray;

					if (GameData._playerData.WorkshopOpen > 0) {
						Study.gameObject.SetActive (true);
						Study.gameObject.GetComponent<Image> ().color = GameData._playerData.StudyOpen > 0 ? openColor : Color.gray;
						Study.gameObject.GetComponentInChildren<Text> ().text = "研究室";
						Study.gameObject.GetComponentInChildren<Text> ().color = GameData._playerData.StudyOpen > 0 ? Color.white : Color.gray;

						if (GameData._playerData.StudyOpen > 0) {
							Farm.gameObject.SetActive (true);
							Farm.gameObject.GetComponent<Image> ().color = GameData._playerData.FarmOpen > 0 ? openColor : Color.gray;
							Farm.gameObject.GetComponentInChildren<Text> ().text = "农田";
							Farm.gameObject.GetComponentInChildren<Text> ().color = GameData._playerData.FarmOpen > 0 ? Color.white : Color.gray;

							Pets.gameObject.SetActive (true);
							Pets.gameObject.GetComponent<Image> ().color = GameData._playerData.PetsOpen > 0 ? openColor : Color.gray;
							Pets.gameObject.GetComponentInChildren<Text> ().text = "宠物笼";
							Pets.gameObject.GetComponentInChildren<Text> ().color = GameData._playerData.PetsOpen > 0 ? Color.white : Color.gray;

							Well.gameObject.SetActive (true);
							Well.gameObject.GetComponent<Image> ().color = GameData._playerData.WellOpen > 0 ? openColor : Color.gray;
							Well.gameObject.GetComponentInChildren<Text> ().text = "水井";
							Well.gameObject.GetComponentInChildren<Text> ().color = GameData._playerData.WellOpen > 0 ? Color.white : Color.gray;

							if (GameData._playerData.FarmOpen > 0 && GameData._playerData.PetsOpen > 0 && GameData._playerData.WellOpen > 0) {
								Altar.gameObject.SetActive (true);
								Altar.gameObject.GetComponent<Image> ().color = GameData._playerData.AltarOpen > 0 ? openColor : Color.gray;
								Altar.gameObject.GetComponentInChildren<Text> ().text = "祭坛";
								Altar.gameObject.GetComponentInChildren<Text> ().color = GameData._playerData.AltarOpen > 0 ? Color.white : Color.gray;
								
								if (GameData._playerData.AltarOpen > 0) {
									MailBox.gameObject.SetActive (true);
									MailBox.gameObject.GetComponent<Image> ().color = openColor;
									MailBox.gameObject.GetComponentInChildren<Text> ().text = "成就";
								}
							}
						}
					}
				}
			}
		}
	}
}
