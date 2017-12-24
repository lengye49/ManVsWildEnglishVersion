using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementActions : MonoBehaviour {

	public GameObject ContentA;
	private GameObject achievementCell;
	private ArrayList achievementCells;


	public void UpdateAchievement(){
        OnLeave();
		achievementCells = new ArrayList ();
		achievementCell = Instantiate (Resources.Load ("achievementCell")) as GameObject;
		achievementCell.SetActive (false);

		Achievement[] a = LoadTxt.GetAllAchievement ();
		for (int i = 0; i < a.Length; i++) {
			GameObject o = Instantiate (achievementCell) as GameObject;
			o.SetActive (true);
			o.transform.SetParent (ContentA.transform);
			o.transform.localPosition = Vector3.zero;
			o.transform.localScale = Vector3.one;
			achievementCells.Add (o);
			SetAchievement (o, a [i]);
		}
		ContentA.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(900,110 * achievementCells.Count);
	}

	void SetAchievement(GameObject o,Achievement a){
		Text[] t = o.gameObject.GetComponentsInChildren<Text> ();
		t [0].text = a.name;
		t [1].text = a.desc + "(" + AchieveActions.GetProgress (a.id) + ")";
        if (GameData._playerData.Achievements[a.id] == 1)
        {
            t[0].color = Color.green;
            t[1].color = Color.green;
        }
        else
        {
            t[0].color = Color.white;
            t[1].color = Color.white;
        }
	}

	public void OnLeave(){
		Destroy (achievementCell);
		if (achievementCells == null)
			return;
		foreach (GameObject o in achievementCells) {
			Destroy (o);
		}
		achievementCells.Clear ();
	}
}
