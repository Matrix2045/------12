using LitJson;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIContronl : MonoBehaviour
{   /// <summary>
    /// 单例
    /// </summary>
    private static UIContronl Instance;
    private static UnityEngine.Object lockobj = new UnityEngine.Object();
    public static UIContronl GetInstance
    {
        get
        {
            lock (lockobj)
            {
                if (Instance == null)
                {
                    Instance = FindObjectOfType<UIContronl>() as UIContronl;
                }
            }
            return Instance;
        }
    }
    public Text tip;

    public Button AIOpen;
    public Button AIPause;
    public GameObject rawimage;
    public GameObject PicScroll;
    public GameObject PicOpen;
    public GameObject PicClose;
    public Transform PicContent;
    private int ChooseID;
    private Vector2 Targetxy;
    private bool isCanScro = false;
    public GameObject ChoosePanel;
    public GameObject AudioOpen;
    public GameObject AudioPause;
    public Text speed;
    private float speednum = 1;

    /// <summary>
    /// 详情
    /// </summary>
    private PicItem targetpic;
    public GameObject Details;
    public Text DetailsText;
    public bool isDetail = false;
    /// <summary>
    /// 展示1
    /// </summary>
    public GameObject PicShow1;
    public GameObject Pic1Content;
    public GameObject Pic1Load;
    public GameObject Pic1Fail;
    public Button Pic1Close;
    private bool LoadPic1 = false;
    private bool UnLoadPic1 = false;
    private WaitForSeconds wait;
    /// <summary>
    /// 展示2
    /// </summary>
    public GameObject PicShow2;
    public RawImage Pic2;
    public GameObject Pic2Panel;
    public Text Pic2name;
    public Text Pic2username;
    public Text Xinxi;
    public GameObject Xinxipanel;
    public GameObject Pic2Button;
    public GameObject XiaoguoButton;
    public GameObject Desc;
    public GameObject DescContent;
    public GameObject picaudioOpen;
    public GameObject picaudioClose;
    public GameObject picVideoOpen;
    public GameObject LoginPanel;
    private string loginurl = "api/Index/worksInquiry";
    private string goodurl = "/api/Index/worksLike_2";
    public GameObject xunjia;
    Texture2D t2d;
    public GameObject use1Pic;
    public GameObject use2Pic;

    public GameObject FirstMusic;
    public GameObject Good1;
    public GameObject Good2;
    public GameObject GoodText;

    public GameObject Pic3Panel;
    public GameObject Pic3Load;
    public GameObject Pic3Error;
    public GameObject Pic3Up;
    public GameObject Pic3Down;
    public GameObject Pic3pic;
    private int pic3Target = 0;
    private bool pic3Load = false;
    private bool pic3close = false;

    public GameObject GoodResult;
    public Text GoodResultText;
    public Button GoodResultButton;

    private enum ScreenState
    {
        None,
        Vertical,
        Horizontal
    }
    private ScreenState Sstate = ScreenState.None;
    /// <summary>
    /// 图片列表
    /// </summary>
    private void SetPicScrollState()
    {
        if (GetComponent<RectTransform>().rect.width > GetComponent<RectTransform>().rect.height)
        {
            PicScroll.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            PicScroll.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            PicScroll.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 90);
            PicScroll.GetComponent<RectTransform>().sizeDelta = new Vector2(220, GetComponent<RectTransform>().rect.width * 0.6f);
            PicScroll.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -GetComponent<RectTransform>().rect.height * 0.4f);
            if (Sstate == ScreenState.Horizontal)
                return;
            for (int i = 0; i < PicContent.childCount; i++)
            {
                PicContent.GetChild(i).GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, -90);
            }
            Sstate = ScreenState.Horizontal;
        }
        if (GetComponent<RectTransform>().rect.width < GetComponent<RectTransform>().rect.height && Sstate != ScreenState.Vertical)
        {

            PicScroll.GetComponent<RectTransform>().localEulerAngles = Vector3.zero;
            PicScroll.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            PicScroll.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
            PicScroll.GetComponent<RectTransform>().sizeDelta = new Vector2(240, PicScroll.GetComponent<RectTransform>().sizeDelta.y);
            PicScroll.GetComponent<RectTransform>().offsetMin = new Vector2(PicScroll.GetComponent<RectTransform>().offsetMin.x, 500);
            PicScroll.GetComponent<RectTransform>().offsetMax = new Vector2(PicScroll.GetComponent<RectTransform>().offsetMax.x, -500);
            PicScroll.GetComponent<RectTransform>().anchoredPosition = new Vector2(-150, 0);
            if (Sstate == ScreenState.Vertical)
                return;
            for (int i = 0; i < PicContent.childCount; i++)
            {
                PicContent.GetChild(i).GetComponent<RectTransform>().localEulerAngles = Vector3.zero;
            }
            Sstate = ScreenState.Vertical;
        }

    }
    /// <summary>
    /// 关闭点赞
    /// </summary>
    public void GoodResultClose()
    {
        GoodResult.SetActive(false);
    }
    /// <summary>
    /// 设置点赞结果
    /// </summary>
    /// <param name="str"></param>
    public void SetGoodResultText(string str)
    {
        GoodResult.SetActive(true);
        GoodResultText.text = str;
    }
    /// <summary>
    /// 寻路打开
    /// </summary>
    // Start is called before the first frame update
    public void AIopen()
    {
        AIOpen.gameObject.SetActive(false);
        AIPause.gameObject.SetActive(true);
        PathContronl.GetInstance.Mindis();
        CloseDetails();
    }
    /// <summary>
    /// 寻路关闭
    /// </summary>
    public void AIpause()
    {
        AIOpen.gameObject.SetActive(true);
        AIPause.gameObject.SetActive(false);
        FirstPersonContronl.GetInstance.SetAI(false);
    }
    /// <summary>
    /// 设置滑动栏
    /// </summary>
    public void SetPicScroll()
    {
        PicOpen.SetActive(!PicScroll.activeSelf);
        PicClose.SetActive(PicScroll.activeSelf);
        PicScroll.SetActive(!PicScroll.activeSelf);
    }
    /// <summary>
    /// 添加图片
    /// </summary>
    public void AddPic(int id)
    {
        GameObject pic = GameObject.Instantiate(rawimage) as GameObject;
        pic.transform.SetParent(PicContent);
        pic.name = id.ToString();
        pic.GetComponent<Button>().onClick.AddListener(delegate { ChoosePic(id); });
        pic.transform.localScale = Vector3.one;
        pic.SetActive(true);
        PicContent.GetChild(id).transform.Find("Load").gameObject.SetActive(true);
    }
    /// <summary>
    /// 加载中
    /// </summary>
    /// <param name="i"></param>
    public void Loading(int i)
    {
        PicContent.GetChild(i).transform.Find("Load").gameObject.SetActive(true);
    }
    /// <summary>
    /// 加载失败
    /// </summary>
    /// <param name="i"></param>
    public void LoadFail(int i)
    {
        PicContent.GetChild(i).transform.Find("Load").gameObject.SetActive(false);
        PicContent.GetChild(i).transform.Find("Fail").gameObject.SetActive(true);
    }
    /// <summary>
    /// 没图片
    /// </summary>
    /// <param name="i"></param>
    public void NoPic(int i)
    {
        PicContent.GetChild(i).gameObject.SetActive(false);
    }
    /// <summary>
    /// 加载成功
    /// </summary>
    /// <param name="i"></param>
    /// <param name="tex"></param>
    public void LoadSuccess(int i, Texture2D tex)
    {
        PicContent.GetChild(i).transform.Find("Load").gameObject.SetActive(false);
        PicContent.GetChild(i).transform.Find("Fail").gameObject.SetActive(false);
        PicContent.GetChild(i).transform.Find("Pic").gameObject.SetActive(true);
        PicContent.GetChild(i).transform.Find("Pic").GetComponent<RawImage>().texture = tex;
    }
    /// <summary>
    /// 选择图片
    /// </summary>
    public void ChoosePic(int ID)
    {
        if (targetpic != null)
        {
            targetpic.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        }
        FirstPersonContronl.GetInstance.MovePic(ID, true);
        PicContent.GetChild(ChooseID).transform.Find("Panel").gameObject.SetActive(false);
        ChooseID = ID;
        PicContent.GetChild(ChooseID).transform.Find("Panel").gameObject.SetActive(true);
        float rawheight = rawimage.GetComponent<RectTransform>().rect.height;
        float PicScrollheight = PicScroll.GetComponent<RectTransform>().rect.height;
        float PicContentheight = PicContent.GetComponent<RectTransform>().rect.height;
        float imagepos = PicContent.GetComponent<VerticalLayoutGroup>().padding.top + (rawheight + PicContent.GetComponent<VerticalLayoutGroup>().spacing) * ID + rawheight / 2;
        if (imagepos < PicScrollheight / 2)
        {
            Targetxy = new Vector2(0, 0);
        }
        else if (PicContentheight - imagepos < PicScrollheight / 2)
        {
            Targetxy = new Vector2(0, PicContentheight - PicScrollheight + 20);
        }
        else
        {
            Targetxy = new Vector2(0, imagepos - PicScrollheight / 2);
        }
        isCanScro = true;

    }
    private void Start()
    {
        wait = new WaitForSeconds(0.1f);
    }
    private void Update()
    {
        SetPicScrollState();
        if (isCanScro)
        {
            PicContent.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(PicContent.GetComponent<RectTransform>().anchoredPosition, Targetxy, Time.deltaTime * 10);
        }
    }
    /// <summary>
    /// 滑动事件
    /// </summary>
    public void ScrollDrag()
    {
        isCanScro = false;
    }
    /// <summary>
    /// 显隐
    /// </summary>
    public void Choose()
    {
        ChoosePanel.SetActive(!ChoosePanel.activeSelf);
    }
    /// <summary>
    /// 音乐
    /// </summary>
    public void Audio()
    {
        AudioManager.GetInstance.PlayOrPause();
        AudioOpen.SetActive(!AudioOpen.activeSelf);
        AudioPause.SetActive(!AudioPause.activeSelf);
    }
    /// <summary>
    /// 速度
    /// </summary>
    public void Speed()
    {
        if (speed.text == "X2.0")
        {
            speednum = 0.5f;
        }
        else
        {
            speednum *= 2f;
        }
        FirstPersonContronl.GetInstance.SetSpeed = speednum;
        speed.text = "X" + speednum.ToString("#0.0");
    }
    /// <summary>
    /// 打开详情
    /// </summary>
    /// <param name="str"></param>
    public void ShowDetails(PicItem pic)
    {
        targetpic = pic;
        Details.SetActive(true);
        DetailsText.text = pic.PicName;
        isDetail = true;
        targetpic.gameObject.transform.GetChild(2).gameObject.SetActive(true);
    }
    /// <summary>
    /// 关闭详情
    /// </summary>
    public void CloseDetails()
    {  
        if(targetpic!=null)
        {
        targetpic.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        }
        Details.SetActive(false);
        isDetail = false;
    }
    ///// <summary>
    ///// 打开大图
    ///// </summary>
    public void ShowPic1(int value)
    {
        if (!isDetail || ChooseID != value)
            return;
          StartCoroutine("showPic1");
    }
    /// <summary>
    /// 大图按钮
    /// </summary>
    public void ShowPic1Btn()
    {
        StartCoroutine("showPic1");
    }
    IEnumerator showPic1()
    {
        PicShow1.SetActive(true);
        Pic1Load.SetActive(true);
        Pic1Content.gameObject.SetActive(false);
        Pic1Fail.SetActive(false);
        FirstPersonContronl.GetInstance.isCanrote = false;
        while (UnLoadPic1)
        {
            yield return wait;
        }
        LoadPic1 = true;
        HttpHelper.GetInstance.Pic(targetpic.url, 2000, SuccessPic1, FailPic1);
    }
    /// <summary>
    /// 下载大图成功
    /// </summary>
    /// <param name="Tex"></param>
    private void SuccessPic1(Texture2D Tex)
    {
        StartCoroutine("showpic1", Tex);
    }
    IEnumerator showpic1(Texture2D Tex)
    {
        yield return new WaitForSeconds(1);
        float width = GetComponent<RectTransform>().rect.width;
        float height = GetComponent<RectTransform>().rect.height;
        float min = (((width / Tex.width) < (height / Tex.height) ? (width / Tex.width) : (height / Tex.height)));
        Pic1Content.GetComponent<RectTransform>().localScale = new Vector2(Tex.width * min * 0.9f, Tex.height * min * 0.9f);
        Pic1Content.GetComponent<HandControl>().NormalX = Tex.width * min * 0.8f;
        Pic1Content.GetComponent<HandControl>().NormalY = Tex.height * min * 0.8f;
        Pic1Load.SetActive(false);
        Pic1Content.GetComponent<RawImage>().texture = Tex;
        Pic1Content.gameObject.SetActive(true);
        LoadPic1 = false;
    }
    /// <summary>
    /// 下载失败
    /// </summary>
    /// <param name="str"></param>
    private void FailPic1(string str)
    {
        Pic1Fail.SetActive(true);
        LoadPic1 = false;
    }
    /// <summary>
    /// 展示封面
    /// </summary>
    /// <param name="url"></param>
    public void ShowFengmian(string url)
    {
        PicShow1.SetActive(true);
        Pic1Load.SetActive(true);
        Pic1Content.gameObject.SetActive(false);
        Pic1Fail.SetActive(false);
        FirstPersonContronl.GetInstance.isCanrote = false;
        HttpHelper.GetInstance.Pic(url, 2000, SuccessPic1, FailPic1);
    }
    /// <summary>
    /// 关闭大图
    /// </summary>
    public void ClosePic1()
    {
        StartCoroutine("closepic1");
    }
    IEnumerator closepic1()
    {
        PicShow1.SetActive(false);
        while (LoadPic1)
        {
            yield return wait;
        }
        while (Pic1Content.GetComponent<RawImage>().texture == null)
        {
            yield return wait;
        }
        UnLoadPic1 = true;
        Texture2D Tex = Pic1Content.GetComponent<RawImage>().texture as Texture2D;
        float min1 = (((5 / (float)Tex.width) < (5 / (float)Tex.height) ? (5 / (float)Tex.width) : (5 / (float)Tex.height)));
        t2d = new Texture2D((int)(Tex.width * min1), (int)(Tex.height * min1), TextureFormat.RGB24, false);
        Color color;
        for (int i = 0; i < t2d.width; i++)
        {
            for (int j = 0; j < t2d.height; j++)
            {
                color = Tex.GetPixel((int)(i * (1 / min1)), (int)(j * (1 / min1)));
                t2d.SetPixel(i, j, color);
            }
        }
        t2d.Compress(false);
        t2d.Apply();
        Pic1Content.GetComponent<RawImage>().texture = t2d;
        Destroy(Tex);
        yield return wait;
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        FirstPersonContronl.GetInstance.isCanrote = true;
        UnLoadPic1 = false;
    }
    /// <summary>
    /// 打开详情
    /// </summary>
    public void ShowPic2()
    {
        FirstPersonContronl.GetInstance.isCanrote = false;
        PicShow2.SetActive(true);
        Pic2.texture = targetpic.tex;
        float width = Pic2Panel.GetComponent<RectTransform>().rect.width;
        float height = Pic2Panel.GetComponent<RectTransform>().rect.height;
        float min = (((width / targetpic.width) < (height / targetpic.height) ? (width / targetpic.width) : (height / targetpic.height)));
        Pic2.GetComponent<RectTransform>().sizeDelta = new Vector2(targetpic.width * min * 0.7f, targetpic.height * min * 0.7f);
        Pic2name.text =  targetpic.PicName;
        if (targetpic.Author != "")
        {
            Pic2username.text = targetpic.Author;
        }
        else {
            Pic2username.text = "未知";
        }
        if (targetpic.Creattime != "")
        {
            Xinxi.text = targetpic.Creattime;
        } else {
            Xinxi.text = "未知";
        }/*+ " " + targetpic.Size*/;
        //Xinxipanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(Pic2.GetComponent<RectTransform>().anchoredPosition.x,
        //    Pic2.GetComponent<RectTransform>().anchoredPosition.y - Pic2.GetComponent<RectTransform>().sizeDelta.y / 2 - Xinxipanel.GetComponent<RectTransform>().sizeDelta.y / 2);
        //Pic2Button.GetComponent<RectTransform>().anchoredPosition = new Vector2(Pic2.GetComponent<RectTransform>().anchoredPosition.x,
        //    Xinxipanel.GetComponent<RectTransform>().anchoredPosition.y - Xinxipanel.GetComponent<RectTransform>().sizeDelta.y / 2 - Pic2Button.GetComponent<RectTransform>().sizeDelta.y / 2-50);
        use1Pic.SetActive(true);
        use2Pic.SetActive(false);
        if (targetpic.AuthPic != null)
        {
            use1Pic.SetActive(false);
            use2Pic.SetActive(true);
            use2Pic.GetComponent<RawImage>().texture = targetpic.AuthPic;
        }
        Desc.GetComponent<Text>().text =targetpic.desc;        
        Desc.GetComponent<RectTransform>().sizeDelta = new Vector2(Desc.GetComponent<RectTransform>().sizeDelta.x, Desc.GetComponent<Text>().preferredHeight);
        DescContent.GetComponent<RectTransform>().sizeDelta = new Vector2(DescContent.GetComponent<RectTransform>().sizeDelta.x, Desc.GetComponent<Text>().preferredHeight);
        //if (Fengmian.GetInstance.isCanGood)
        //{
        //    GoodText.GetComponent<Text>().text = targetpic.like_num;
        //    Good2.SetActive(false);
        //    Good1.SetActive(true);
        //}
        //else
        //{
        //    Good2.SetActive(false);
        //    Good1.SetActive(false);
        //    GoodText.SetActive(false);
        //}
        //if (targetpic.xiaoguourls.Count != 0)
        //{
        //    XiaoguoButton.SetActive(true);
        //}
        //else
        //{
        //    XiaoguoButton.SetActive(false);
        //}
        picaudioClose.SetActive(false);
        picaudioOpen.SetActive(true);
        picVideoOpen.SetActive(true);
        if (targetpic.AudioUrl == "")
        {
            picaudioOpen.SetActive(false);
        }
        if (targetpic.VideoUrl == "")
        {
            picVideoOpen.SetActive(false);
        }
    }
    /// <summary>
    /// 关闭图片2
    /// </summary>
    public void ClosePic2()
    {
        PicAudioClose();
        PicShow2.SetActive(false);
        FirstPersonContronl.GetInstance.isCanrote = true;
    }
    /// <summary>
    /// 设置提示
    /// </summary>
    /// <param name="str"></param>
    public void TipStr(string str, bool bo)
    {
        tip.text = str;
        //tip.gameObject.SetActive(bo);
    }
    //询价
    public void Xunjia()
    {
        string str = PicContronl.GetInstance.transform.GetChild(ChooseID).GetComponent<PicItem>().Work_ID;
        WWWForm form = new WWWForm();
        form.AddField("work_id", str);
        form.AddField("token", Fengmian.token);
        HttpHelper.GetInstance.Post(loginurl, form, LoadLoginSuccess, LoadLoginFail);
        form = null;

    }
    public void XunjiaClose()
    {
        xunjia.SetActive(false);
    }
    private void LoadLoginSuccess(string arg0)
    {
        JsonData Pics = JsonMapper.ToObject(arg0);
        if (Pics["code"].ToString() == "101")
        {
            LoginPanel.SetActive(true);
        }
        else if (Pics["code"].ToString() == "0")
        {
            TipStr("数据异常！", true);
        }
        else if (Pics["code"].ToString() == "1")
        {
            xunjia.SetActive(true);
        }
        else if (Pics["code"].ToString() == "102")
        {
            Application.ExternalCall("OpenPhone");
        }
    }
    //登录按钮
    public void Login()
    {
        Application.ExternalCall("Login", "?hall_id=" + Fengmian.HallID + "/pos=" + targetpic.ID);
    }
    private void LoadLoginFail(string arg0)
    {
        TipStr("网络异常！", true);
    }
    public void CloseLogin()
    {
        LoginPanel.SetActive(false);
    }
    /// <summary>
    /// 返回主页
    /// </summary>
    public void ReturnHome()
    {
        SceneManager.LoadScene("Load");
    }
    /// <summary>
    /// 第一次播放音乐
    /// </summary>
    public void FirstPlayMusic()
    {
        AudioManager.GetInstance.PlayOrPause();
        FirstMusic.SetActive(false);
    }
    /// <summary>
    /// 分享
    /// </summary>
    public void Share()
    {
        //Application.ExternalCall("sharepath", "?sence=" + Fengmian.SenceId + "&hall_id=" + Fengmian.HallID + "&pos=" + targetpic.ID +
        //    "&作品:《" + targetpic.PicName + "》  作者:" + targetpic.Author + "  快来一起欣赏吧！");
        Application.ExternalCall("WXShare",targetpic.PicName,"?hall_id="+Fengmian.HallID+"/pos="+targetpic.ID,targetpic.url,targetpic.desc);
    }
    /// <summary>
    /// 点赞
    /// </summary>
    public void good1()
    {
        WWWForm form = new WWWForm();
        form.AddField("token", Fengmian.token);
        form.AddField("work_id", targetpic.Work_ID);
        HttpHelper.GetInstance.Post(goodurl, form, good1Success, good1Fail);
        form = null;       
    }
    //点赞成功
    public void good1Success(string str)
    {
        JsonData good = JsonMapper.ToObject(str);
        if (good["code"].ToString() == "1")
        {
            targetpic.like_num = (int.Parse(targetpic.like_num) + 1).ToString();
            //targetpic.good.transform.GetChild(2).GetComponent<Text>().text = targetpic.like_num;
            targetpic.good.transform.GetChild(1).GetComponent<TextMesh>().text = targetpic.like_num;
            targetpic.is_like = "1";
            GoodText.GetComponent<Text>().text = targetpic.like_num;
        }
        else if (good["code"].ToString() == "101")
        {
            LoginPanel.SetActive(true);
        }
        else if (good["code"].ToString() == "0")
        {
            SetGoodResultText("今天已点赞，请明天再来。");
        }
    }
    //点赞失败
    public void good1Fail(string str)
    {
        SetGoodResultText("点赞失败！");
    }
    ///// <summary>
    ///// 取消点赞
    ///// </summary>
    //public void good2()
    //{
    //    Good2.SetActive(false);
    //    Good1.SetActive(true);
    //    GoodText.GetComponent<Text>().text = (int.Parse(GoodText.GetComponent<Text>().text) - 1).ToString();
    //}
    /// <summary>
    /// 打开效果图
    /// </summary>
    public void ShowPic3()
    {
        Pic3Panel.SetActive(true);
        StartCoroutine("LoadXiaoguoPic");
    }
    /// <summary>
    /// 关闭效果图
    /// </summary>
    public void ClosePic3()
    {
        Pic3Panel.SetActive(false);
        pic3Target = 0;
        StartCoroutine("closepic3");
    }
    IEnumerator closepic3()
    {
        while (pic3Load)
        {
            yield return wait;
        }
        Pic3Load.SetActive(false);
        Pic3pic.SetActive(false);
        Pic3Up.SetActive(false);
        Pic3Down.SetActive(false);
        Pic3Error.SetActive(false);
        while (Pic3pic.GetComponent<RawImage>().texture == null)
        {
            yield return wait;
        }
        pic3close = true;
        Texture2D Tex = Pic3pic.GetComponent<RawImage>().texture as Texture2D;
        float min1 = (((5 / (float)Tex.width) < (5 / (float)Tex.height) ? (5 / (float)Tex.width) : (5 / (float)Tex.height)));
        t2d = new Texture2D((int)(Tex.width * min1), (int)(Tex.height * min1), TextureFormat.RGB24, false);
        Color color;
        for (int i = 0; i < t2d.width; i++)
        {
            for (int j = 0; j < t2d.height; j++)
            {
                color = Tex.GetPixel((int)(i * (1 / min1)), (int)(j * (1 / min1)));
                t2d.SetPixel(i, j, color);
            }
        }
        t2d.Compress(false);
        t2d.Apply();
        Pic3pic.GetComponent<RawImage>().texture = t2d;
        Destroy(Tex);
        yield return wait;
        yield return Resources.UnloadUnusedAssets();
        System.GC.Collect();
        yield return new WaitForEndOfFrame();
        pic3close = false;
    }
    /// <summary>
    /// 加载效果图
    /// </summary>
    IEnumerator LoadXiaoguoPic()
    {
        Pic3Load.SetActive(true);
        while (pic3close)
        {
            yield return wait;
        }
        pic3Load = true;
        HttpHelper.GetInstance.Pic(targetpic.xiaoguourls[pic3Target], 800, LoadXiaoguoSuccess, LoadXiaoguoFail);
    }
    /// <summary>
    /// 加载成功
    /// </summary>
    private void LoadXiaoguoSuccess(Texture2D tex)
    {
        StartCoroutine(LoadXiaoguosuccess(tex));
    }
    IEnumerator LoadXiaoguosuccess(Texture2D Tex)
    {
        if (Pic3pic.GetComponent<RawImage>().texture != null)
        {
            Texture2D tex = Pic3pic.GetComponent<RawImage>().texture as Texture2D;
            float min1 = (((5 / (float)tex.width) < (5 / (float)tex.height) ? (5 / (float)tex.width) : (5 / (float)tex.height)));
            t2d = new Texture2D((int)(tex.width * min1), (int)(tex.height * min1), TextureFormat.RGB24, false);
            Color color;
            for (int i = 0; i < t2d.width; i++)
            {
                for (int j = 0; j < t2d.height; j++)
                {
                    color = tex.GetPixel((int)(i * (1 / min1)), (int)(j * (1 / min1)));
                    t2d.SetPixel(i, j, color);
                }
            }
            t2d.Compress(false);
            t2d.Apply();
            Pic3pic.GetComponent<RawImage>().texture = t2d;
            Destroy(tex);
        }
        yield return wait;
        yield return Resources.UnloadUnusedAssets();
        System.GC.Collect();
        yield return new WaitForEndOfFrame();
        float width = GetComponent<RectTransform>().rect.width;
        float height = GetComponent<RectTransform>().rect.height;
        float min = (((width / Tex.width) < (height / Tex.height) ? (width / Tex.width) : (height / Tex.height)));
        Pic3pic.GetComponent<RectTransform>().localScale = new Vector2(Tex.width * min * 0.7f, Tex.height * min * 0.7f);
        Pic3pic.GetComponent<RawImage>().texture = Tex;
        Pic3Load.SetActive(false);
        Pic3pic.SetActive(true);
        if (pic3Target != 0)
        {
            Pic3Up.SetActive(true);
        }
        if (pic3Target != targetpic.xiaoguourls.Count - 1)
        {
            Pic3Down.SetActive(true);
        }
        pic3Load = false;
        yield return null;
    }
    /// <summary>
    /// 加载失败
    /// </summary>
    private void LoadXiaoguoFail(string str)
    {
        Pic3Load.SetActive(false);
        Pic3Error.SetActive(true);
        Pic3Up.SetActive(true);
        Pic3Down.SetActive(true);
        if (pic3Target == 0)
        {
            Pic3Up.SetActive(false);
        }
        else if (pic3Target == targetpic.xiaoguourls.Count - 1)
        {
            Pic3Down.SetActive(false);
        }
        pic3Load = false;
    }
    /// <summary>
    /// 上一张
    /// </summary>
    public void XiaoguoPicUp()
    {
        pic3Target--;
        Pic3pic.SetActive(false);
        Pic3Up.SetActive(false);
        Pic3Down.SetActive(false);
        Pic3Error.SetActive(false);
        StartCoroutine("LoadXiaoguoPic");
    }
    /// <summary>
    /// 下一张
    /// </summary>
    public void XiaoguoPicDown()
    {
        pic3Target++;
        Pic3pic.SetActive(false);
        Pic3Up.SetActive(false);
        Pic3Down.SetActive(false);
        Pic3Error.SetActive(false);
        StartCoroutine("LoadXiaoguoPic");
    }
    /// <summary>
    /// 图片音乐开
    /// </summary>
    public void PicAudioOpen()
    {
        Application.ExternalCall("PicAudioOpen", targetpic.AudioUrl);
        picaudioClose.SetActive(true);
        picaudioOpen.SetActive(false);
    }
    /// <summary>
    /// 图片音乐关
    /// </summary>
    public void PicAudioClose()
    {
        Application.ExternalCall("PicAudioClose");
        picaudioClose.SetActive(false);
        picaudioOpen.SetActive(true);
    }
    /// <summary>
    /// 图片视频
    /// </summary>
    public void PicVedio()
    {
        picaudioClose.SetActive(false);
        picaudioOpen.SetActive(true);
        Application.ExternalCall("PicVedio", targetpic.VideoUrl);
    }
}
