using System.Collections;
using UnityEngine;

public class PlaySound : MonoBehaviour {

	public AudioClip[] soundClips;
	private AudioSource _thisAudio;
	
	public AudioSource click;

	void Start(){
		_thisAudio = this.gameObject.GetComponent<AudioSource> ();
	}

	public void PlayClickSound(){
		click.Play ();
	}

	public void PlayEnvironmentSound(string soundName){
		_thisAudio.Stop ();
		switch (soundName) {
		case "cave":
			if (soundClips.Length > 0) {
				_thisAudio.clip = soundClips [0];
				_thisAudio.Play ();
			}
			break;
		case "death":
			if (soundClips.Length > 1) {
				_thisAudio.clip = soundClips [1];
				_thisAudio.Play ();
			}
			break;
		case "door_close":
			if (soundClips.Length > 2) {
				_thisAudio.clip = soundClips [2];
				_thisAudio.Play ();
			}
			break;
		case "door_open":
			if (soundClips.Length > 3) {
				_thisAudio.clip = soundClips [3];
				_thisAudio.Play ();
			}
			break;
		case "dungeon":
			if (soundClips.Length > 4) {
				_thisAudio.clip = soundClips [4];
				_thisAudio.Play ();
			}
			break;
		case "ghost":
			if (soundClips.Length > 5) {
				_thisAudio.clip = soundClips [5];
				_thisAudio.Play ();
			}
			break;
		case "hawk":
			if (soundClips.Length > 6) {
				_thisAudio.clip = soundClips [6];
				_thisAudio.Play ();
			}
			break;
		case "island":
			if (soundClips.Length > 7) {
				_thisAudio.clip = soundClips [7];
				_thisAudio.Play ();
			}
			break;
		case "mountain":
			if (soundClips.Length > 8) {
				_thisAudio.clip = soundClips [8];
				_thisAudio.Play ();
			}
			break;
		case "rain_heavy":
			if (soundClips.Length > 9) {
				_thisAudio.clip = soundClips [9];
				_thisAudio.Play ();
			}
			break;
		case "rain_small":
			if (soundClips.Length > 10) {
				_thisAudio.clip = soundClips [10];
				_thisAudio.Play ();
			}
			break;
		case "river":
			if (soundClips.Length > 11) {
				_thisAudio.clip = soundClips [11];
				_thisAudio.Play ();
			}
			break;
		case "sea":
			if (soundClips.Length > 12) {
				_thisAudio.clip = soundClips [12];
				_thisAudio.Play ();
			}
			break;
		case "wild":
			if (soundClips.Length > 13) {
				_thisAudio.clip = soundClips [13];
				_thisAudio.Play ();
			}
			break;
		case "woods":
			if (soundClips.Length > 14) {
				_thisAudio.clip = soundClips [14];
				_thisAudio.Play ();
			}
			break;
		default:
//			Debug.Log ("Wrong soundName = " + soundName);
			break;
		}
	}
}
