using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fengmian : MonoBehaviour
{
    /// <summary>
    /// 单例
    /// </summary>
    private static Fengmian Instance;
    private static UnityEngine.Object lockobj = new UnityEngine.Object();
    public static Fengmian GetInstance
    {
        get
        {
            lock (lockobj)
            {
                if (Instance == null)
                {
                    Instance = FindObjectOfType<Fengmian>() as Fengmian;
                }
            }
            return Instance;
        }
    }

    public float MaxLength_x;
    public float MaxLength_y;
    private float size_x;
    private float size_y;
    public string HuaKuangColor;
    private string url = "api/Index/getHallInfo";
    //private string goodurl = "/api/Index/getLike";
    private GameObject Fail;
    private GameObject Loading;
    private float rate;
    public static string HallID = "";
    public  static string token = "";
    public static string SenceId="";
    public static string pos = "";
    public TextMesh Name;
    public TextMesh Cehuaren;
    public TextMesh Danwei;
    public TextMesh Add;
    public TextMesh StartTime;
    public TextMesh EndTime;
    public TextMesh Liulanliang;
    public TextMesh Tianjiashijian;
    public Texture tex;
    public string picurl;
    public bool isCanGood = false;
    Color color;
    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = false;
        if (GameManager.GetInstance.platform == Platform.Web)
        {
            Application.ExternalCall("gettoken");
            Application.ExternalCall("gethallid");
            Application.ExternalCall("getsence");
            Application.ExternalCall("getpos");
            Sethallid("12");
            Settoken("8dbea006-fbce-4a72-935c-4636f507d9ba");
        }
        else 
        {
            Sethallid("12");
            Settoken("8dbea006-fbce-4a72-935c-4636f507d9ba");
        }
    }
    /// <summary>
    /// 设置展馆id
    /// </summary>
    /// <param name="str"></param>
    public void Sethallid(string str)
    {
        if (str == "")
            str = "1";
        HallID = str;
        WWWForm form = new WWWForm();
        form.AddField("hall_id", HallID);
        //HttpHelper.GetInstance.Post(goodurl, form, GoodSuccess, GoodFail);
        Loading = GameObject.Instantiate(Resources.Load("Loading") as GameObject);
        Loading.transform.SetParent(transform.GetChild(0));
        Loading.transform.localPosition = Vector3.zero;
        Loading.transform.localEulerAngles = new Vector3(0, 0, 180);
        Loading.transform.localScale = new Vector3((1f/transform.localScale.x), (1f/ transform.localScale.y), 0.2f);
        Debug.Log(url);
        HttpHelper.GetInstance.Post(url, form, LoadPic, LoadFail);
        form = null;
        
    }
    /// <summary>
    /// 获取是否可以点赞
    /// </summary>
    /// <param name="str"></param>
    public void GoodSuccess(string str)
    {
        JsonData Pics = JsonMapper.ToObject(str);
        if (Pics["code"].ToString() == "1")
        {
            
            if (Pics["data"]["open_like"].ToString() == "1")
            {
                isCanGood = true;
            }
            if (Pics["data"]["open_like"].ToString() == "0")
            {
                isCanGood = false;
            }
        }
        else {
            isCanGood = false;
        }
        WWWForm form = new WWWForm();
        form.AddField("hall_id", HallID);
        HttpHelper.GetInstance.Post(url, form, LoadPic, LoadFail);
        form = null;
    }
    /// <summary>
    /// 获取点赞失败
    /// </summary>
    /// <param name="str"></param>
    public void GoodFail(string str)
    {
        isCanGood = false;
        WWWForm form = new WWWForm();
        form.AddField("hall_id", HallID);
        HttpHelper.GetInstance.Post(url, form, LoadPic, LoadFail);
        form = null;
    }
    /// <summary>
    /// 设置token
    /// </summary>
    /// <param name="str"></param>
    public void Settoken(string str)
    {
        token = str;
    }
    /// <summary>
    /// 设置场景
    /// </summary>
    /// <param name="str"></param>
    public void Setsence(string str)
    {
        SenceId = str;
    }
    /// <summary>
    /// 设置起始位置
    /// </summary>
    /// <param name="str"></param>
    public void Setpos(string str)
    {
        pos = str;
    }
    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// 加载封面信息失败
    /// </summary>
    /// <param name="str"></param>
    private void LoadFail(string str)
    {
        UIContronl.GetInstance.TipStr(str,true);
    }
    /// <summary>
    /// 加载封面信息成功
    /// </summary>
    /// <param name="str"></param>
    public void LoadPic(string str)
    {
        Debug.Log(str);
        JsonData Pics = JsonMapper.ToObject(str);
        if (Pics["code"].ToString() == "1")
        {
            //Name.text = Pics["data"]["name"].ToString();
            //Cehuaren.text ="策划人："+ Pics["data"]["curator"].ToString();
            //Danwei.text = "主办单位："+Pics["data"]["organizer"].ToString();
            //Add.text ="线下观展地址："+ Pics["data"]["exhibition_address"].ToString();
            //StartTime.text ="线下观展开始时间："+ Pics["data"]["exhibition_start_time"].ToString();
            //EndTime.text = "线下观展结束时间："+Pics["data"]["exhibition_end_time"].ToString();
            //Liulanliang.text = "浏览量："+Pics["data"]["pageviews"].ToString();
            //Tianjiashijian.text ="添加时间："+ Pics["data"]["createtime"].ToString();
            picurl = Pics["data"]["image"].ToString();
            if (float.Parse(Pics["data"]["width"].ToString()) != 0)
            {
                size_x = float.Parse(Pics["data"]["width"].ToString()) / 100;
            }
            if (float.Parse(Pics["data"]["height"].ToString()) != 0)
            {
                size_y = float.Parse(Pics["data"]["height"].ToString()) / 100;
            }
            //HuaKuangColor = Pics["data"]["color"].ToString();
            HttpHelper.GetInstance.Pic(picurl, 500, PicSuccess, PicFail);
        }
        if (Pics["code"].ToString() == "0")
        {
            PicFail("");
        }
    }
    /// <summary>
    /// 下载封面失败
    /// </summary>
    /// <param name="str"></param>
    public void PicFail(string str)
    {
        if (Loading != null)
            Destroy(Loading);
        Fail = GameObject.Instantiate(Resources.Load("Fail") as GameObject);
        Fail.transform.parent = transform;
        Fail.transform.localPosition = new Vector3(0, 0, 0.07f);
        Fail.transform.localEulerAngles = new Vector3(0, 0, 180);
    }
    /// <summary>
    /// 加载封面成功
    /// </summary>
    /// <param name="tex"></param>
    void PicSuccess(Texture2D Tex)
    {
        if (Loading != null)
            Destroy(Loading);
        tex = Tex;
        transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = tex;
        float min = 1;
        if (size_x > MaxLength_x || size_y > MaxLength_y)
        {
            min = (((MaxLength_x / size_x) < (MaxLength_y / size_y) ? (MaxLength_x / size_x) : (MaxLength_y / size_y)));
        }
        transform.localScale = new Vector3(size_x * min, size_y * min, 0.1f);
        PicContronl.GetInstance.Init();
    }
    private void OnDestroy()
    {

    }
}
