using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathActions : MonoBehaviour {

	public Text deathMsg;
	public Button rebirthButton;
    private string txt;
	private bool isPrinting=false;


	public void UpdateDeath(string cause){
        PlayerPrefs.SetInt("IsDead", 1);
		PlayerPrefs.SetString ("DeathCause", cause);
		if (isPrinting)
			return;

		switch (cause) {
		case "Spirit":
			TypeWritter ("You died of low spirit.");
			break;
		case "Food":
			TypeWritter ("Your died of hunger.");
			break;
		case "Water":
			TypeWritter ("You died of dehydration.");
			break;
		case "Cold":
			TypeWritter ("You died of cold.");
			break;
		case "Hot":
			TypeWritter ("You died of hot.");
			break;
        case "Hp":
			TypeWritter ("You died of low hp.");
            break;
		default:
			TypeWritter ("You died of low hp.");
			break;
		}
			
		rebirthButton.interactable = (GameData._playerData.HasMemmory > 0);
	}

    void TypeWritter(string s){
		isPrinting = true;
        txt = "";
        float t = 0f;
        float pop = 0.1f;
        string ss;

        for (int i = 0; i < s.Length; i++)
        {
            t += pop;
            ss = s.Substring(i, 1);
            StartCoroutine(WriteLetter(t, ss));
        }

		StartCoroutine (ResetPrint());
    }

    IEnumerator WriteLetter(float t, string s){
        yield return new WaitForSeconds(t);
        txt += s;
        deathMsg.text = txt;
    }

	IEnumerator ResetPrint(){
		yield return new WaitForSeconds (3f);
		isPrinting = false;
	}
}
