using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGameLoading : MonoBehaviour {

	public Slider progressSlider;
	public Image fillImage;
	public Text loadingTxt;
	public Text loadingPercent;
	private float t = 0;
	bool isLoading = false;
	// Use this for initialization
	void Start () {

	}

	void Update(){
		t += Time.deltaTime;
		if (t > 0.1f && !isLoading) {
			isLoading = true;
			LoadGame ();
		}
	}

	public void LoadGame(){
		StartCoroutine (StartLoading ());
	}

	IEnumerator StartLoading(){
		int displayProgress = 0;
		int toProgress = 0;
		AsyncOperation op = SceneManager.LoadSceneAsync (1);
		op.allowSceneActivation = false;
		while (op.progress < 0.9f) {
			toProgress = (int)op.progress * 100;
			while (displayProgress < toProgress) {
				++displayProgress;
				SetLoadingPercentage (displayProgress);
				yield return new WaitForEndOfFrame ();
			}
		}

		toProgress = 100;
		while (displayProgress < toProgress) {
			++displayProgress;
			SetLoadingPercentage (displayProgress);
			yield return new WaitForEndOfFrame ();
		}
		op.allowSceneActivation = true;
	}

	public void SetLoadingPercentage(int value){
		float v = value / 100f;
		progressSlider.value = v;
		fillImage.color = new Color (0, Mathf.Max (0, (value + 0.5f) / 1f), 0f, 1f);

		if ((int)(v * 10) % 4 == 0) {
			loadingTxt.text = "Loading";
		}else if((int)(v * 10) % 4 == 1){
			loadingTxt.text = "Loading.";
		}else if((int)(v * 10) % 4 == 2){
			loadingTxt.text = "Loading..";
		}else{
			loadingTxt.text = "Loading...";
		}
		loadingPercent.text = value + "%";
	}
}
