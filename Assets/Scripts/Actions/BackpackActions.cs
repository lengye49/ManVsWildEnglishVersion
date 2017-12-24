using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BackpackActions : MonoBehaviour {

	public Text Melee;
	public Text Ranged;
	public Text Magic;
	public Text Head;
	public Text Body;
	public Text Shoe;
	public Text Ammo;
	public Text AmmoNum;
	public Text Mount;
    public Text BpNum;
    public Text Renown;
    public Text RenownName;

	public GameObject contentB;
	private GameObject bpCell;
	private ArrayList bpCells = new ArrayList ();
	
	public void UpdataPanel(){

		Destroy (bpCell);
		bpCell = Instantiate (Resources.Load ("bpCellNormal")) as GameObject;
		bpCell.SetActive (false);
		BpNum.text = "(" + GameData._playerData.bp.Count + "/" + GameData._playerData.bpNum + ")";
		UpdateCharacter();
		UpdateBpContent ();
        SetRenown();
	}

    void SetRenown(){
        int r = GameData._playerData.Renown;
        Renown.text = r.ToString();
        if (r < 50)
        {
            RenownName.text = "籍籍无名";
            RenownName.color = GameConfigs.MatColor[0];
        }
        else if (r < 200)
        {
            RenownName.text = "小有名气";
            RenownName.color = GameConfigs.MatColor[1];
        }
        else if (r < 400)
        {
            RenownName.text = "声名远扬";
            RenownName.color = GameConfigs.MatColor[2];
        }
        else if (r < 800)
        {
            RenownName.text = "威名赫赫";
            RenownName.color = GameConfigs.MatColor[3];
        }
        else
        {
            RenownName.text = "威震天下";
            RenownName.color = GameConfigs.MatColor[4];
        }
    }

	void UpdateCharacter(){
        
        UpdateEquip(Melee, GameData._playerData.MeleeId);
        UpdateEquip(Ranged, GameData._playerData.RangedId);
        UpdateEquip(Magic, GameData._playerData.MagicId);
        UpdateEquip(Head, GameData._playerData.HeadId);
        UpdateEquip(Body, GameData._playerData.BodyId);
        UpdateEquip(Shoe, GameData._playerData.ShoeId);


		if (GameData._playerData.AmmoId > 0 && GameData._playerData.AmmoNum > 0) {
			Ammo.text = LoadTxt.MatDic [(int)(GameData._playerData.AmmoId / 10000)].name;
			AmmoNum.text = "×" + GameData._playerData.AmmoNum;
            Color c = new Color();
            c = GameConfigs.MatColor[LoadTxt.MatDic[(int)(GameData._playerData.AmmoId / 10000)].quality];
            Ammo.color = c;
            AmmoNum.color = c;
		} else {
			Ammo.text = "";
			AmmoNum.text = "";
		}
		Mount.text = (GameData._playerData.Mount.monsterId > 0) ? (GameData._playerData.Mount.name) : "";
	}

    void UpdateEquip(Text t,int itemId){
        if (itemId > 0)
        {
            Mats mat = new Mats();
            mat = LoadTxt.MatDic[(int)(itemId / 10000)];
            t.text = mat.name;
            t.color = GameConfigs.MatColor[mat.quality];
        }else{
            t.text = "";
        }
    }

	void UpdateBpContent (){
		
		for (int i = 0; i < bpCells.Count; i++) {
			ClearContent (bpCells [i] as GameObject);
		}

		if (bpCells.Count<GameData._playerData.bp.Count) {
			int n = bpCells.Count;
			for (int i = n; i < GameData._playerData.bp.Count; i++) {
				GameObject o = Instantiate (bpCell) as GameObject;
				o.SetActive (true);
				o.transform.SetParent (contentB.transform);
				o.transform.localPosition = Vector3.zero;
				o.transform.localScale = Vector3.one;
				bpCells.Add (o);
				ClearContent (o);
			}
			contentB.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(375,80 * GameData._playerData.bp.Count);
		}

		int j = 0;
		foreach (int key in GameData._playerData.bp.Keys) {
            if (!LoadTxt.MatDic.ContainsKey((int)(key / 10000)))
            {
                GetComponentInParent<GameData>().DeleteItemInBp(key);
                continue;
            }

			if (j >= GameData._playerData.bpNum)
				return;
			GameObject o = bpCells [j] as GameObject;
			o.SetActive (true);
			o.gameObject.name = key.ToString ();
			o.GetComponent<Button> ().interactable = true;
			Text[] t = o.GetComponentsInChildren<Text> ();
			t [0].text = LoadTxt.MatDic [(int)(key / 10000)].name;
			t [1].text = GameData._playerData.bp [key].ToString();
			j++;
		}
	}

	void ClearContent(GameObject o){
		Text[] ts = o.gameObject.GetComponentsInChildren<Text> ();
		for (int i = 0; i < ts.Length; i++)
			ts [i].text = "";
		o.GetComponent<Button> ().interactable = false;
	}

	public void OnLeave(){
		Destroy (bpCell);
		if (bpCells == null)
			return;
		foreach (GameObject o in bpCells)
			Destroy (o);
		bpCells.Clear ();
	}
}
