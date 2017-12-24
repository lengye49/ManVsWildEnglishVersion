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
			TypeWritter ("    很遗憾，你在探险过程中死于[精神失常]。 \n战斗中释放魔法会降低精神，可以通过休息，饮酒等提高精神。");
			break;
		case "Food":
			TypeWritter ("    很遗憾，你在探险过程中死于[过度饥饿]。 \n饱腹度会随着时间的积累而降低。可以通过进食提高饱腹度。");
			break;
		case "Water":
			TypeWritter ("    很遗憾，你在探险过程中死于[身体脱水]。 \n身体水分会随着时间的积累而降低。可以通过喝水、饮料等提高身体的水分。");
			break;
		case "Cold":
			TypeWritter ("    很遗憾，你在探险过程中死于[炎热]。 \n体温会随着时间的变动而降低，夏季会降低的更快。可以通过洗澡、喝冰水等降低体温。");
			break;
		case "Hot":
			TypeWritter ("    很遗憾，你在探险过程中死于[寒冷]。 \n体温会随着时间的变动而降低，冬季会降低的更快。可以通过点火把、饮酒等提高体温。");
			break;
        case "Hp":
			TypeWritter ("    很遗憾，你在探险过程中死于[生命值过低]。 \n不要尝试挑战过于强大的对手，进入战斗前先调整到最佳状态。战斗结束后可以通过进食提高生命值。");
            break;
		default:
			TypeWritter ("    很遗憾，你在探险过程中死于[生命值过低]。 \n不要尝试挑战过于强大的对手，进入战斗前先调整到最佳状态。战斗结束后可以通过进食提高生命值。");
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
