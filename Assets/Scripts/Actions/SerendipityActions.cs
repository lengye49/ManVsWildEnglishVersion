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
        Vungle.init("59b4965c7e75cb8114000385", "Test_iOS", "vungleTest");
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
			studyText.text = Vungle.isAdvertAvailable () ? "查看" : "加载中";
		}
    }


    /// <summary>
    /// 检查是否触发了意外事件。如果在室外，触发概率高一些。
    /// </summary>
    /// <returns><c>true</c>, if serendipity was checked, <c>false</c> otherwise.</returns>
    /// <param name="outdoor">If set to <c>true</c> outdoor.</param>
    public void CheckSerendipity(){
        
        if (GameData._playerData.minutesPassed < 10800)
            return;

        int r = Random.Range(0, 9999);

        //不触发意外事件
        if (r > 500)
            return;

        //0信息 1广告
        if (r > 200)
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
            detailText.text = "你发现了" + nameList[index] + "，上面仿佛写着什么。";
            noticeText.text = "(手记，记录着失落之地的一些信息。)";
			studyText.text = "查看";
            studyButton.interactable = true;
            isAd = false;
        }
        else
        {
            headText.text = "留影石";
            detailText.text = "你发现了一个留影魔法石，不妨看看里面记录着什么，或许会有意想不到的收获。";
            noticeText.text = "(这是一条广告，观看完毕将获得一些灵魂石，下载相关内容将获得额外奖励。)";
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
            GetComponentInChildren<LogManager>().AddLog("你获得了2灵魂石。",true);
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
			GetComponentInChildren<LogManager>().AddLog("你从留影石中获得了30灵魂石。",true);
        }
        else if (args.IsCompletedView)
        {
            //完成了播放，奖励10灵魂石
            _gameData.AddItem(22020000,15);
			GetComponentInChildren<LogManager>().AddLog("你从留影石中获得了15灵魂石。",true);
        }
        else
        {
            //未完成播放，没有奖励
            _gameData.AddItem(22020000,2);
			GetComponentInChildren<LogManager>().AddLog("你从留影石中获得了2灵魂石。",true);
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
        nameList.Add(0, "破旧的纸片");
        nameList.Add(1, "一块破布");
        nameList.Add(2, "一个龟壳");
        nameList.Add(3, "残缺的日记");
        nameList.Add(4, "一页航海日志");
        nameList.Add(5, "刻字的盾牌"); 
    }

    void ResetMessageList(){
        messageList = new Dictionary<int, string>();
        messageList.Add(0,"...陆地上原住民种族复杂，还有一些强大的生灵，比如龙。");
        messageList.Add(1,"...奉命探查地牢周围的空间裂缝，约接近底层裂缝越多，实力所限无法到达80层以下。");
        messageList.Add(2,"...地牢中藏着强大而又邪恶的生灵，据说是一个邪恶的法师控制着他们。");
        messageList.Add(3,"...夜间最好不要在外面走动，否则会遭到幽灵的袭击。");
        messageList.Add(4,"...如有你有闲暇，可以驯养一些宠物。速度快的可以用来骑乘，实力强大的也可以看守家园。");
        messageList.Add(5,"...出门记得锁门，那些讨厌的盗贼总会在没人的时候光顾。");
        messageList.Add(6,"...镇长每隔一段时间就会给那个人运送一些补给...");
        messageList.Add(7,"...神秘的地牢里埋藏着大量的宝藏，常常有人去探险，但是大多有去无回。");
        messageList.Add(8,"...据说地牢中深处有一个巨大的空间旋涡，可以通往另一个世界。");
        messageList.Add(9,"...那个人的声望越来越高，小镇送去的补给也逐渐增多，我们可以多去偷几次，一定小心他的宠物。");
        messageList.Add(10,"...山上的矿产越来越少，又经常碰到野兽袭击，建议镇长大人开发新的矿脉。");
        messageList.Add(11,"...那个人可以打造强大的武器装备，实力成长的太快，去偷窃一定不能被他发现。");
        messageList.Add(12,"...虽然春夏秋冬降雨量不同，但是小溪的水量却没有什么变化，不知道溪水的源头是哪里。");
        messageList.Add(13,"...鲤鱼和很多食物一样，除了美味，还可以滋补气血。");
        messageList.Add(14,"...镇长年轻的时候去过很多地方，他知识广博，好像什么都知道。");
        messageList.Add(15,"...浆果一定要及早采摘，过季了就容易腐烂，被野兽叼走也是浪费。");
        messageList.Add(16,"...不管什么时候，矿产都是珍贵的资源。开发新的矿脉不仅需要实力，还需要极好的运气。");
        messageList.Add(17,"...挖矿和伐木都非常耗费体力，如果体力不足，只能回家睡觉。");
        messageList.Add(18,"...小镇上的人经常外出，一定知道去其他地方的路。那个酒保就经常去海边约会裁缝铺子的姑娘。");
        messageList.Add(19,"...“体力”值过低虽然不会危及生命，但是不能采集物资。");
        messageList.Add(20,"...击败土匪或者有魔力的怪物，会增加在小镇的声望。");
        messageList.Add(21,"...密切监视那个外来者，狼人才会是整个大陆的统治者...");
        messageList.Add(22,"...蛇人和熊人的领地...那个外来者...");
        messageList.Add(23,"...狼人和蛇人...外来者也杀了他们不少...看到外来者格杀勿论...");
        messageList.Add(24,"...那些兽人被三个势力在幕后统治...亡灵，矮人，精灵...");
        messageList.Add(25,"...那帮倒霉的矮人把自己埋在了矿坑中...");
        messageList.Add(26,"...亡灵的研究方向跟我族不同...永生...可恶的精灵...");
        messageList.Add(27,"...古堡中的势力已经被三族练手抹除，只留下几个苟延残喘的怪物...");
        messageList.Add(28,"...龙，真是强大而贪婪的种族，几乎没有人能控制它们。");
        messageList.Add(29,"...人类的后裔就集中在那个镇子上，他们实力弱小，不足为惧。");
        messageList.Add(30,"...三个兽人部落总是互相征战，这对我们人类来说是好事。发展强大了，再去收拾他们。");
        messageList.Add(31,"...那些窝在山里的盗贼十分可恶，要是我有强大的力量，一定去消灭他们。");
        messageList.Add(32,"...神秘的时空怪物经常通过空间旋涡侵入大陆，不知道空间旋涡的那一头又是哪里？");
        messageList.Add(33,"...广袤的荒原上十分危险，强大的雷鹰和泰坦守护在那里...");
        messageList.Add(34,"幽灵是灵魂的转化体，它们能生存的关键就是体内的灵魂石，通过吞噬灵魂不断成长，最高的可以成长为黑幽灵。");
        messageList.Add(35,"亲爱的喀秋莎，你在哪里？我飞遍了整个大陆，也没能看到你的身影...");
        messageList.Add(36,"杀死狼人部落的费尔曼，伟大的熊族怎么能跟那些瘦猴子通婚！");
        messageList.Add(37,"去探索寂静岭真是恐怖的经历，那里弥漫着浓雾，每走一步都踩在碎骨上，咔嚓咔嚓...");
        messageList.Add(38,"寂静岭的亡灵法师很少跟外界打交道，但是看那遍地尸骨就知道这是假象。据说他们在研究一种用兽人和幽灵合体的不死之法。");
        messageList.Add(39,"幽暗丛林的精灵大多喜好和平，除了那些嗜杀的暗精灵。");
        messageList.Add(40,"荒原上的食人魔不仅仅吃人，实际上人类的肉又少又难吃，他们反而更喜欢吃兽人和动物。");
		messageList.Add(41,"精神会影响人的命中率，同时也是释放魔法的必要条件。");
		messageList.Add(42,"挑战NPC收益较低，但却非常危险，一定要小心为上。");
		messageList.Add(43,"NPC实力强大，请谨慎挑战，挑战之前务必保存记忆。");
		messageList.Add(44,"NPC实力强大，请谨慎挑战，挑战之前务必保存记忆。");
		messageList.Add(45,"秩序阵营的人类不是这里的原住民，据说他们来自遥远的西方。");
		messageList.Add(46,"西方大陆发生了巨变，那些可怜人才跑来这里。");
		messageList.Add(47,"死亡阵营的正义之镰和灵魂女王原来是一对人类夫妻，他们的爱情并没有因为肉体由生变死而有丝毫的减少。");
		messageList.Add(48,"贪婪的矮人为了获得魔晶的能量，打开了深渊的传送门。");
		messageList.Add(49,"精灵比较友善，但是对敌人却从不手软。");
		messageList.Add(50,"高精灵和精灵虽然长得很像，但是前者迷恋真理，后者追从自由。");
		messageList.Add(51,"法师塔里的那些老顽固，往往偏执的不可理喻。");
		messageList.Add(52,"如果你想要永生，可以去死亡沼泽试试运气。但那种永生，肯定不是你想象的那么美好。");
		messageList.Add(53, "深渊里有很多领地，也就有很多领主。");
		messageList.Add(54, "军团长帕里斯和丛林守护者金鬃的友谊人兽皆知，但最早他们却是一对仇敌。");
		messageList.Add(55, "秩序执掌者艾莫斯为人苛刻，遇到他最好谨遵教条，免遭杀身之祸。");
		messageList.Add(56, "真理执掌者飓风高傲自大，从不会对凡人正眼相看，但他用于与之匹配的强大实力。");
		messageList.Add(57, "生命执掌者亚玟的微笑之下，蕴藏着强大的精灵力量，永远不要小看一直对你微笑的人。");
		messageList.Add(58, "深渊执掌者哈里森只是一个代号，谁吃了上一任执掌者，他就是新的哈里森。");
		messageList.Add(59, "死亡执掌者骨龙的来历无人可知，正义之镰把它唤醒之后，就把执掌者的位子让给了他。");
		messageList.Add(60, "现在大陆上的死亡沼泽以往并不存在，是深渊恶魔入侵之后，才逐渐显露出世。");
		messageList.Add(61, "这片大陆原名艾泽西，原住民信奉女神·飘。随着原住民被五大阵营的战乱侵蚀，女神也销声匿迹。");
    }

}
