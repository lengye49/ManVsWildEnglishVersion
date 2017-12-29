using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class SerendipityActions : MonoBehaviour {

    public Transform SDetail;
    public Transform NewMessage;

    private Text headText;
    private Text detailText;
    private Text noticeText;
    private Text studyText;
    private Button studyButton;
    private Vector3 restPoint;
	private bool isAd = true;

    private Dictionary<int,string> nameList;
    private Dictionary<int,string> messageList;
    private GameData _gameData;

    void Start(){
        Vungle.init("5a4448d7d49ac43a0c00c018", "Test_iOS", "vungleTest");
        Vungle.onAdFinishedEvent += (args) =>{
            AdFinished(args);
        } ;

        _gameData = this.gameObject.GetComponent<GameData>();
        ResetNameList();
        ResetMessageList();
        restPoint = new Vector3(5000f, 0f, 0f);
        SDetail.localPosition = restPoint;
        NewMessage.localPosition = restPoint;
        studyButton = SDetail.GetComponentInChildren<Button>();
        Text[] ts = SDetail.GetComponentsInChildren<Text>();
        headText = ts[0];
        detailText = ts[1];
        noticeText = ts[2];
        studyText = ts[3];
    }

    void Update(){
		if (isAd) {
			studyButton.interactable = Vungle.isAdvertAvailable ();
			studyText.text = Vungle.isAdvertAvailable () ? "Check" : "Loading";
		}
    }


    /// <summary>
    /// 检查是否触发了意外事件。如果在室外，触发概率高一些。
    /// </summary>
    /// <returns><c>true</c>, if serendipity was checked, <c>false</c> otherwise.</returns>
    /// <param name="outdoor">If set to <c>true</c> outdoor.</param>
    public void CheckSerendipity(){
        
        if (GameData._playerData.minutesPassed < 1440)
            return;

        int r = Random.Range(0, 9999);

        //不触发意外事件
        if (r > 750)
            return;

        //0信息 1广告
        //如果广告没有准备好，则显示信息
        if (r > 500 || !Vungle.isAdvertAvailable())
            ShowSerendipity(0);
        else
            ShowSerendipity(1);
        
    }
        
    /// <summary>
    /// 根据类型显示意外事件，0是叙事，1是广告
    /// </summary>
    /// <param name="sType">S type.</param>
    void ShowSerendipity(int sType){
        SDetail.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        SDetail.localPosition = Vector3.zero;
        SDetail.DOBlendableScaleBy(Vector3.one, 0.5f);

        if (sType == 0)
        {
            int index = Algorithms.GetIndexByRange(0, nameList.Count);
            headText.text = nameList[index];
            detailText.text = "You found a " + nameList[index] + ", there is something wrote on it.";
            noticeText.text = "";
			studyText.text = "Check";
            studyButton.interactable = true;
            isAd = false;
        }
        else
        {
            headText.text = "Magic Stone";
            detailText.text = "You found a magic stone, check it and you may get some prize unexpected.";
            noticeText.text = "(It's an Ad, your will get a reward after watching it.)";
            studyButton.interactable = Vungle.isAdvertAvailable();
            isAd = true;
        }
        studyText.name = sType.ToString();
    }


    /// <summary>
    /// 根据类型确定是显示信息还是播放广告
    /// </summary>
    public void OnStudySerendipity(){
        
        OnCancelSerendipity();

        int sType = int.Parse(studyText.name);
        if (sType == 0)
        {
            NewMessage.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            NewMessage.localPosition = Vector3.zero;
            NewMessage.DOBlendableScaleBy(Vector3.one, 0.5f);

            Text t = NewMessage.GetComponentInChildren<Text>();
			int index = 0;

			if (GameData._playerData.placeNowId < 26)
				index = Random.Range (0, 44);
			else
				index = Random.Range (44, messageList.Count-1);
			
            t.text = messageList[index];
            _gameData.AddItem(22020000,2);
            GetComponentInChildren<LogManager>().AddLog("Soulstone +2.",true);
        }
        else
        {
//            int i = GetComponentInChildren<LoadingBar>().CallInLoadingBar(60);
            PlayRewardedAd();
        }
    }
		

    void PlayRewardedAd(){
//        Debug.Log("播放广告");
        Vungle.playAd(true, "ThisUser");
    }

    /// <summary>
    /// 根据播放完成的事件来发送奖励
    /// </summary>
    /// <param name="args">Arguments.</param>
    void AdFinished(AdFinishedEventArgs args){
        if (args.WasCallToActionClicked)
        {
            //点击了下载按钮，奖励20灵魂石
            _gameData.AddItem(22020000,30);
			GetComponentInChildren<LogManager>().AddLog("You get 30 soulstones from the Magic Stone.",true);
        }
        else if (args.IsCompletedView)
        {
            //完成了播放，奖励10灵魂石
            _gameData.AddItem(22020000,15);
			GetComponentInChildren<LogManager>().AddLog("You get 15 soulstone from the Magic Stone.",true);
        }
        else
        {
            //未完成播放，没有奖励
            _gameData.AddItem(22020000,2);
			GetComponentInChildren<LogManager>().AddLog("You get 2 soulstone from the Magic Stone.",true);
        }

    }


    public void OnCancelSerendipity(){
        SDetail.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        SDetail.localPosition = restPoint;
    }

    public void OnCloseMessage(){
        NewMessage.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        NewMessage.localPosition = restPoint;
    }

    void ResetNameList(){
        nameList = new Dictionary<int, string>();
        nameList.Add(0, "piece of paper");
        nameList.Add(1, "piece of cloth");
        nameList.Add(2, "piece of shell");
        nameList.Add(3, "broken diary");
        nameList.Add(4, "sailor's diary");
        nameList.Add(5, "broken shield"); 
    }

    void ResetMessageList(){
        messageList = new Dictionary<int, string>();
		messageList.Add(0,"The residents on the land are complex, human, elves, dragons,etc.");
		messageList.Add(1,"There are many space rifts around the dungeon.");
		messageList.Add(2,"There are powerful creatures in the dungeon.");
        messageList.Add(3,"During the night, be careful of the ghosts.");
        messageList.Add(4,"You can capture some pets, they are very helpful.");
        messageList.Add(5,"Lock your warehouse while you are leaving.");
        messageList.Add(6,"The mayor will deliver supplies to your warehouse every month.");
        messageList.Add(7,"There are a lot of treasures in the dungeon.");
        messageList.Add(8,"If you want to go somewhere far away, try the space vortex.");
		messageList.Add(9,"Rumors say an evil master is in control of the dungeon.");
        messageList.Add(10,"Be careful of the monsters on the hill.");
        messageList.Add(11,"The creatures are growing more powerful.");
		messageList.Add(12,"The residents on the land are complex, human, elves, dragons,etc.");
		messageList.Add(13,"There are many space rifts around the dungeon.");
		messageList.Add(14,"There are powerful creatures in the dungeon.");
		messageList.Add(15,"During the night, be careful of the ghosts.");
		messageList.Add(16,"You can capture some pets, they are very helpful.");
		messageList.Add(17,"Lock your warehouse while you are leaving.");
		messageList.Add(18,"The mayor will deliver supplies to your warehouse every month.");
		messageList.Add(19,"There are a lot of treasures in the dungeon.");
		messageList.Add(20,"If you want to go somewhere far away, try the space vortex.");
		messageList.Add(21,"Rumors say an evil master is in control of the dungeon.");
		messageList.Add(22,"Be careful of the monsters on the hill.");
		messageList.Add(23,"The creatures are growing more powerful.");
		messageList.Add(24,"The residents on the land are complex, human, elves, dragons,etc.");
		messageList.Add(25,"There are many space rifts around the dungeon.");
		messageList.Add(26,"There are powerful creatures in the dungeon.");
		messageList.Add(27,"During the night, be careful of the ghosts.");
		messageList.Add(28,"You can capture some pets, they are very helpful.");
		messageList.Add(29,"Lock your warehouse while you are leaving.");
		messageList.Add(30,"The mayor will deliver supplies to your warehouse every month.");
		messageList.Add(31,"There are a lot of treasures in the dungeon.");
		messageList.Add(32,"If you want to go somewhere far away, try the space vortex.");
		messageList.Add(33,"Rumors say an evil master is in control of the dungeon.");
		messageList.Add(34,"Be careful of the monsters on the hill.");
		messageList.Add(35,"The creatures are growing more powerful.");
        messageList.Add(36,"The mayor is well learnt and has been to many places.");
        messageList.Add(37,"Kill monsters to get renown and get more supplies.");
        messageList.Add(38,"Make good use of the mines, they are limited.");
        messageList.Add(39,"You will not die of low energy, but you can not dig mines or cut trees without energy.");
		messageList.Add(40,"The mayor is well learnt and has been to many places.");
		messageList.Add(41,"Kill monsters to get renown and get more supplies.");
		messageList.Add(42,"Make good use of the mines, they are limited.");
		messageList.Add(43,"You will not die of low energy, but you can not dig mines or cut trees without energy.");
		messageList.Add(44,"...You will not die of low energy, but you can not dig mines or cut trees without energy.");
		messageList.Add(45,"Human of Order is not origin residents here, they are from the west.");
		messageList.Add(46,"The creatures are growing more powerful as you do.");
		messageList.Add(47,"If you want to go somewhere far away, try the space vortex.");
		messageList.Add(48,"Rumors say an evil master is in control of the dungeon.");
		messageList.Add(49,"Be careful of the monsters on the hill.");
		messageList.Add(50,"The creatures are growing more powerful.");
		messageList.Add(51,"The mayor is well learnt and has been to many places.");
		messageList.Add(52,"Kill monsters to get renown and get more supplies.");
		messageList.Add(53,"Make good use of the mines, they are limited.");
		messageList.Add(54,"You will not die of low energy, but you can not dig mines or cut trees without energy.");
		messageList.Add(55,"The mayor is well learnt and has been to many places.");
		messageList.Add(56,"Kill monsters to get renown and get more supplies.");
		messageList.Add(57,"Make good use of the mines, they are limited.");
		messageList.Add(58,"You will not die of low energy, but you can not dig mines or cut trees without energy.");
		messageList.Add(59,"...You will not die of low energy, but you can not dig mines or cut trees without energy.");
		messageList.Add(60,"Human of Order is not origin residents here, they are from the west.");
		messageList.Add(61,"The creatures are growing more powerful as you do.");
    }

}
