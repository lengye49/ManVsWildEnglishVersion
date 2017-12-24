using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingBar : MonoBehaviour {

	private float value;
	private Slider loadingBar;
	private Text loadingText;
	private string loadingTxt;
	public Image fillImage;
	private int totalTime;
	// Use this for initialization
	void Start () {
		loadingBar = this.gameObject.GetComponent<Slider> ();
		loadingTxt = "In Progress";
		loadingText = this.gameObject.GetComponentInChildren<Text> ();
	}

	public int CallInLoadingBar(int costMin){
		totalTime = 1;
		value = 0;
		this.gameObject.SetActive (true);
		this.gameObject.transform.localPosition = new Vector3 (0f, -666f, 0f);
		StartLoading ();
		return 1;
	}

	void StartLoading(){
		value = value + 0.03f;
		if ((int)(value * 10) % 4 == 0) {
			loadingTxt = "In Progress";
		}else if((int)(value * 10) % 4 == 1){
			loadingTxt = "In Progress.";
		}else if((int)(value * 10) % 4 == 2){
			loadingTxt = "In Progress..";
		}else{
			loadingTxt = "In Progress...";
		}
        loadingBar.value = value * 2f;
		loadingText.text = loadingTxt;
		fillImage.color = new Color (0, Mathf.Max (0, (value + 0.5f) / 1f), 0f, 1f);
		if (value < 0.5)
			StartCoroutine (LoadingInProgress ());
		else {
			this.gameObject.SetActive (false);
		}

	}


	IEnumerator LoadingInProgress(){
		float f = 0.001f*totalTime;
		yield return new WaitForSeconds (f);
		StartLoading ();
	}

}
