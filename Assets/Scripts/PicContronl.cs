using UnityEngine;
using LitJson;
using System.Collections.Generic;

public class PicContronl : MonoBehaviour
{
    /// <summary>
    /// 单例
    /// </summary>
    private static PicContronl Instance;
    private static UnityEngine.Object lockobj = new UnityEngine.Object();
    public static PicContronl GetInstance
    {
        get
        {
            lock (lockobj)
            {
                if (Instance == null)
                {
                    Instance = FindObjectOfType<PicContronl>() as PicContronl;
                }
            }
            return Instance;
        }
    }
    public PicItem[] picItems;
    private string DataUrl = "api/Index/getHallWork";
    private int LoadCount = 0;
    private int currentPic = 0;
    private int LoadPicDateCount = 0;
    public int MaxCount;
    private List<string> Work_IDs = new List<string>();
    private List<string> BackPlanes = new List<string>();
    private List<string> ImageUrls = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public  void Init()
    {
        picItems= GetComponentsInChildren<PicItem>();
        LoadPicData();
    }
    /// <summary>
    /// 加载场馆图片数据
    /// </summary>
    void LoadPicData()
    {
        //UIContronl.GetInstance.TipStr("数据加载中", true);
        WWWForm form = new WWWForm();
        form.AddField("hall_id", Fengmian.HallID);
        HttpHelper.GetInstance.Post(DataUrl, form, LoadPicDataSuccess, LoadPicDataFail);
        form = null;
    }
    /// <summary>
    /// 加载数据成功
    /// </summary>
    void LoadPicDataSuccess(string result)
    {
        JsonData Pics = JsonMapper.ToObject(result);
        if (Pics["code"].ToString() == "1")
        {
            JsonData PicDatas = Pics["data"];
            
            foreach (JsonData item in PicDatas)
            {
                if (item["work_id"].ToString() == "0")
                    continue;
                if (LoadCount >= MaxCount)
                    return;
                Work_IDs.Add(item["work_id"].ToString());
                BackPlanes.Add(item["type"].ToString());
                ImageUrls.Add(item["image"].ToString());
                transform.GetChild(LoadCount).GetComponent<PicItem>().ID = LoadCount;
                //transform.GetChild(LoadCount).GetComponent<PicItem>().Work_ID = item["work_id"].ToString();
                //transform.GetChild(LoadCount).GetComponent<PicItem>().BackPlane = item["type"].ToString();
                //transform.GetChild(LoadCount).GetComponent<PicItem>().url = item["image"].ToString();
                //transform.GetChild(LoadCount).GetComponent<PicItem>().PicColor= item["color"].ToString();
                transform.GetChild(LoadCount).gameObject.SetActive(true);
                transform.GetChild(LoadCount).GetComponent<PicItem>().LoadingObj();
                //transform.GetChild(LoadCount).GetComponent<PicItem>().LoadPicData();
                LoadCount++;
            }       
        }
        for (int i = 0; i < LoadCount; i++)
        {
            transform.GetChild(i).GetComponent<PicItem>().Work_ID = Work_IDs[transform.GetChild(i).GetComponent<PicItem>().IndexNum];
            transform.GetChild(i).GetComponent<PicItem>().BackPlane = BackPlanes[transform.GetChild(i).GetComponent<PicItem>().IndexNum];
            transform.GetChild(i).GetComponent<PicItem>().url = ImageUrls[transform.GetChild(i).GetComponent<PicItem>().IndexNum];
            //transform.GetChild(LoadCount).GetComponent<PicItem>().PicColor= item["color"].ToString();
            transform.GetChild(i).GetComponent<PicItem>().LoadPicData();
        }

        if (Pics["code"].ToString() == "0")
        {
            UIContronl.GetInstance.TipStr("数据错误!",true);
        }
    }
    /// <summary>
    /// 加载图片信息
    /// </summary>
    public void loadPicDate()
    {
        LoadPicDateCount++;
        if (LoadPicDateCount == LoadCount)
        {
            if (Fengmian.pos != "")
            {
                FirstPersonContronl.GetInstance.MovePic(int.Parse(Fengmian.pos), false);
                transform.GetChild(int.Parse(Fengmian.pos)).GetComponent<PicItem>().LoadPic();
            }
            else
            {
                loadPicOnce();
            }
        }
    }
    /// <summary>
    /// 单次加载图片
    /// </summary>
    public void loadPicOnce()
    {
        if (currentPic >= LoadCount||currentPic>=MaxCount)
        {
            UIContronl.GetInstance.TipStr("",false);
            FirstPersonContronl.GetInstance.isCan = true;
            return;
        }
        if (Fengmian.pos != "")
        {
            if (currentPic == int.Parse(Fengmian.pos))
            {
                currentPic++;
            }
        }
        transform.GetChild(currentPic++).GetComponent<PicItem>().LoadPic();
        //currentPic++;
    }

    /// <summary>
    /// 加载数据失败
    /// </summary>
    void LoadPicDataFail(string result)
    {
        UIContronl.GetInstance.TipStr(result,true);
    }
}
