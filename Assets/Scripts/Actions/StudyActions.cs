using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StudyActions : MonoBehaviour {

	public GameObject contentS;
	private GameObject studyCell;
	private ArrayList studyCells = new ArrayList ();


	public void UpdateStudy(){
		Destroy (studyCell);
		studyCell= Instantiate (Resources.Load ("studyCell")) as GameObject;
		studyCell.SetActive (false);

		ClearContents ();
		int i = 0;
		Technique[] tList = LoadTxt.GetTechList ();
		for (int key=0;key<tList.Length;key++) {
			int lv = tList [key].lv;
//			int maxlv = tList [key].maxLv;
			int learntLv = GameData._playerData.techLevels [tList[key].type];
			if (lv != (learntLv + 1))
				continue;
			GameObject o;
			if (i >= studyCells.Count) {
				o = Instantiate (studyCell) as GameObject;
				o.SetActive (true);
				o.transform.SetParent (contentS.transform);
				o.transform.localPosition = Vector3.zero;
				o.transform.localScale = Vector3.one;
				studyCells.Add (o);
			} else {
				o = studyCells [i] as GameObject;
				o.SetActive (true);
			}
			o.name = tList[key].id.ToString();
			Text[] t = o.GetComponentsInChildren<Text> ();
			t [0].text = tList[key].name;
			i++;
		}

		if (i < studyCells.Count) {
			for (int j = i; j < studyCells.Count; j++) {
				GameObject o = studyCells [j] as GameObject;
				o.SetActive (false);
			}
		}
		contentS.GetComponent<RectTransform> ().sizeDelta = new Vector2(800,120 * i);
	}


	void ClearContents(){
		for (int i = 0; i < studyCells.Count; i++) {
			GameObject o = studyCells [i] as GameObject;
			Text[] t = o.GetComponentsInChildren<Text> ();
			for (int j = 0; j < t.Length; j++)
				t [j].text = "";
		}
	}

	public void OnLeave(){
		Destroy (studyCell);
		if (studyCells == null)
			return;
		foreach (GameObject o in studyCells)
			Destroy (o);
		studyCells.Clear ();
	}
}
