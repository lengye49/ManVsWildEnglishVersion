using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class FloatingActions : MonoBehaviour {

	private float upTime = 0.2f;
	private float waitTime = 1.3f;
	private float disappearTime = 0.5f;

	/// <summary>
	/// Calls in floating.
	/// </summary>
	/// <param name="str">Float text.</param>
	/// <param name="floatType">Float type:0Good,1Bad.</param>
	public void CallInFloating(string str,int floatType){
		GameObject f = Instantiate (Resources.Load ("floatingCell")) as GameObject;;

		f.transform.SetParent (this.gameObject.transform);
		f.SetActive (true);
		Text t = f.GetComponentInChildren<Text> ();
		t.text = str;

		Color c = new Color ();
		switch (floatType) {
		case 0:
			c = Color.green;
			break;
		case 1:
			c = Color.red;
			break;
		default:
			c = Color.green;
			break;
		}
		t.color = c;
		StartFloat (f);
	}
	void StartFloat(GameObject f){
		f.transform.localPosition = Vector3.zero;
		f.transform.localScale = new Vector3 (0.1f, 0.1f, 1);
		f.transform.DOLocalMoveY (100, upTime);
		f.transform.DOBlendableScaleBy (new Vector3 (1f, 1f, 1f),upTime);
		f.GetComponentInChildren<Text> ().DOFade (1, upTime);
		StartCoroutine (WaitAndNext (f));
	}

	IEnumerator WaitAndNext(GameObject f){
		yield return new WaitForSeconds (upTime + waitTime);
		EndFloat (f);
	}

	void EndFloat(GameObject f){
		f.transform.DOLocalMoveY (250, disappearTime);
		f.GetComponentInChildren<Text>().DOFade (0, disappearTime);
		StartCoroutine (WaitAndEnd (f));
	}

	IEnumerator WaitAndEnd(GameObject f){
		yield return new WaitForSeconds (disappearTime);
		Destroy (f);
	}
}
