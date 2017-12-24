using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class AlterActions : MonoBehaviour {

	public Text ssNum;
	public Text memoryPoolState;
	public Button storeButton;
	public Button recoverButton;
    public FloatingActions _floating;
	public LoadingBar _loadingBar;
	public Transform altar;
	public Transform sacrifice;
	public GameObject[] Items;
	public Text changeText;

	private GameData _gameData;
	private bool isAltar = true;

	void Start(){
		_gameData = this.gameObject.GetComponentInParent<GameData> ();
	}

	public void UpdateAltar(){
        int num = 0;
        if (GameData._playerData.bp.ContainsKey(22020000))
            num = GameData._playerData.bp[22020000];

		ssNum.text = GameConfigs.SoulStoneForStoreMem + "/" + num;
        ssNum.color = (num >= GameConfigs.SoulStoneForStoreMem) ? Color.green : Color.red;
        storeButton.interactable = (num >= GameConfigs.SoulStoneForStoreMem);

		bool s = GameData._playerData.HasMemmory > 0;
		memoryPoolState.text = s ? "已存档" : "无存档";
		memoryPoolState.color = s ? Color.green : Color.red;
		recoverButton.interactable = s;
	}

	public void StoreMemory(){
        int num = 0;
        if (GameData._playerData.bp.ContainsKey(22020000))
            num = GameData._playerData.bp[22020000];

        if (num < GameConfigs.SoulStoneForStoreMem)
			return;
		_gameData.ConsumeItemInHome(2202, GameConfigs.SoulStoneForStoreMem);
		_gameData.StoreMemmory ();
		UpdateAltar ();
        _floating.CallInFloating("存档成功", 0);
	}

	public void RecoverMemory(){
		PlayerPrefs.SetInt ("IsDead", 0);
		this.gameObject.GetComponentInParent<AchieveActions> ().LoadMemmory (); //Achievement
		_gameData.RebirthLoad ();
		StartCoroutine (StartRebirth ());
	}

	IEnumerator StartRebirth(){
		int t = _loadingBar.CallInLoadingBar (60);
		yield return new WaitForSeconds (t);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void RestartGame(){
		PlayerPrefs.SetInt ("IsDead", 0);
		_gameData.ReStartLoad ();

		GetComponentInParent<PanelManager>().GoToPanel("Home");
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
        GameObject head = GameObject.FindGameObjectWithTag("HeadUI");
        head.GetComponent<HeadUiManager>().UpdateHeadUI();
	}

	public void Change(){
		if (isAltar) {
			ChangeToSacrifice ();
			changeText.text="祭坛";
			isAltar = false;
		} else {
			ChangeToAltar ();
			changeText.text="祭祀";
			isAltar = true;
		}
	}

	void ChangeToSacrifice(){
		altar.localPosition=new Vector3 (-2000f, 0, 0);
		sacrifice.localPosition = Vector3.zero;
		UpdateSacrifice ();
	}

	void ChangeToAltar(){
		sacrifice.localPosition=new Vector3 (-2000f, 0, 0);
		altar.localPosition = Vector3.zero;
		UpdateAltar ();
	}
		
	public void UpdateSacrifice(){
		for (int i = 0; i < Items.Length; i++) {
			Text[] ts = Items [i].GetComponentsInChildren<Text> ();
			Button b = Items [i].GetComponentInChildren<Button> ();

			ShopItem s = LoadTxt.GetShopItem (GameData._playerData.sacrificeList [i]);
			Mats m = LoadTxt.MatDic [s.itemId];
			int price = s.bundleNum * m.price;

			ts [0].text = m.name + "×" + s.bundleNum;
			ts [0].color = GameConfigs.MatColor [m.quality];
			ts [1].text = m.description;

			int j = 2;
			if (m.property != null) {
				foreach (int key in m.property.Keys) {
					ts [j].text = PlayerData.GetPropName (key) + " " + (m.property [key] > 0 ? "+" : "-") + m.property [key];
					ts [j].color = Color.white;
					j++;
				}
			} else {
				ts [j].text = "无法直接使用。";
				ts [j].color = Color.white;
				j++;
			}

			ts [4].text = "献祭(" + price + ")";
			bool isEnough = _gameData.CountInHome (GameConfigs.AltarMarkId / 10000) >= price;
			b.interactable = isEnough;
			ts [4].color = isEnough ? Color.green : Color.gray;
		}
	}

	public void DoSacrifice(int index){
		ShopItem s = LoadTxt.GetShopItem (GameData._playerData.sacrificeList [index]);
		Mats m = LoadTxt.MatDic [s.itemId];
		int price = s.bundleNum * m.price;
		if (_gameData.CountInHome (GameConfigs.AltarMarkId/10000) < price)
			return;
		_gameData.ConsumeItemInHome (GameConfigs.AltarMarkId / 10000, price);
		_gameData.AddItem (m.id * 10000, s.bundleNum);
		_floating.CallInFloating (m.name + " +" + s.bundleNum, 0);
		UpdateSacrifice ();
	}

	public void TestUpdate(){
		GameData._playerData.sacrificeList = LoadTxt.GetAltarShopList (4);
		PlayerPrefs.SetString ("SacrificeList", _gameData.GetStrFromInt (GameData._playerData.sacrificeList));
		UpdateSacrifice ();
	}
		
}
