
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PicStatus
{
    NoPic,
    Loading,
    Error,
    Success
}
public class PicItem : MonoBehaviour
{
    public int IndexNum;
    public float MaxLength_x;
    public float MaxLength_y;
    public string PicColor;
    public string Work_ID = "0";
    public string PicName;
    public string Author;
    public string Creattime;
    public string AuthPicUrl;
    public Texture2D AuthPic;
    public string BackPlane;
    public float Size_x = 0;
    public float Size_y = 0;
    public string Size;
    public string url;
    public string AudioUrl;
    public string VideoUrl;
    public PicStatus status = PicStatus.NoPic;
    public int ID = 0;
    //public GameObject Huakuang;
    public Texture2D tex;
    public float rate;
    public float width;
    public float height;
    private string DataUrl = "api/Index/getHallWorkInfo";
    private string goodurl = "/api/Index/worksLike_2";
    Color color;
    private GameObject Fail;
    private GameObject Loading;
    public List<string> xiaoguourls;
    public string like_num;
    public string desc;
    public string is_like;
    public GameObject good;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// 加载图片数据
    /// </summary>
    public void LoadPicData()
    {
        if (url == "")
        {
            LoadPicDataFail("空");
            return;
        }
        WWWForm form = new WWWForm();
        form.AddField("work_id", Work_ID);
        form.AddField("token", Fengmian.token);
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
            if (GameManager.GetInstance.platform == Platform.None)
            {
                JsonData PicDatas = Pics["data"];
                foreach (JsonData item in PicDatas) {
                    if (item["work_id"].ToString() == Work_ID) {
                        PicName = item["name"].ToString();
                        Author = item["author"].ToString();
                        Creattime = item["creativetime"].ToString();
                        Size_x = float.Parse(item["width"].ToString()) / 100;
                        Size_y = float.Parse(item["height"].ToString()) / 100;
                        Size = item["width"].ToString() + "cm*" + item["height"].ToString() + "cm";
                        //like_num = PicDatas["like_num"].ToString();
                        desc = item["desc"].ToString();
                        VideoUrl = item["video"].ToString();
                        AudioUrl = item["music"].ToString();
                        AuthPicUrl = item["authorlogo"].ToString();
                    }
                }
            }
            else
            {
                JsonData PicDatas = Pics["data"];
                PicName = PicDatas["name"].ToString();
                Author = PicDatas["author"].ToString();
                Creattime = PicDatas["creativetime"].ToString();
                Size_x = float.Parse(PicDatas["width"].ToString()) / 100;
                Size_y = float.Parse(PicDatas["height"].ToString()) / 100;
                Size = PicDatas["width"].ToString() + "cm*" + PicDatas["height"].ToString() + "cm";
                //like_num = PicDatas["like_num"].ToString();
                desc = PicDatas["desc"].ToString();
                VideoUrl = PicDatas["video"].ToString();
                AudioUrl = PicDatas["music"].ToString();
                AuthPicUrl = PicDatas["authorlogo"].ToString();
            }
            //is_like = PicDatas["is_like"].ToString();
            //foreach (var item in PicDatas["renderings"])
            //{
            //    xiaoguourls.Add(item.ToString());
            //}
            gameObject.SetActive(true);
            float min = 1;
            if (Size_x > MaxLength_x || Size_y > MaxLength_y)
            {
                min = (((MaxLength_x / Size_x) < (MaxLength_y / Size_y) ? (MaxLength_x / Size_x) : (MaxLength_y / Size_y)));
            }
            PathContronl.GetInstance.transform.GetChild(transform.GetSiblingIndex()).GetComponent<PathPoints>().ResetPose(Size_x * min, Size_y * min);
        }
        if (Pics["code"].ToString() == "0")
        {
            PicName = "数据丢失";
            Author = "数据丢失";
            Creattime = "数据丢失";
            Size = "数据丢失";
            UIContronl.GetInstance.TipStr("数据丢失", true);
        }
        LoadAuthPic();
    }
    /// <summary>
    /// 加载数据失败
    /// </summary>
    void LoadPicDataFail(string result)
    {
        PicName = result;
        PicContronl.GetInstance.loadPicDate();
    }
    /// <summary>
    /// 加载头像
    /// </summary>
    public void LoadAuthPic()
    {
        if (AuthPicUrl == "")
        {
            PicContronl.GetInstance.loadPicDate();
            return;
        }
        HttpHelper.GetInstance.Pic(AuthPicUrl, 100, AuthPicSuccess, AuthPicFail);
    }
    /// <summary>
    /// 加载头像成功
    /// </summary>
    /// <param name="arg0"></param>
    private void AuthPicSuccess(Texture2D arg0)
    {

        AuthPic = arg0;
        PicContronl.GetInstance.loadPicDate();
    }
    /// <summary>
    /// 加载头像失败
    /// </summary>
    /// <param name="arg0"></param>
    private void AuthPicFail(string arg0)
    {
        PicContronl.GetInstance.loadPicDate();
    }
    /// <summary>
    /// 加载图片
    /// </summary>
    public void LoadPic()
    {
        if (url == "")
        {
            gameObject.SetActive(false);
            UIContronl.GetInstance.NoPic(ID);
            PicContronl.GetInstance.loadPicOnce();
            return;
        }
        status = PicStatus.Loading;
        HttpHelper.GetInstance.Pic(url,550,PicSuccess, PicFail);
        //UIContronl.GetInstance.Loading(ID);
    }
    /// <summary>
    /// 加载
    /// </summary>
    public void LoadingObj()
    {
        Loading = GameObject.Instantiate(Resources.Load("Loading") as GameObject);
        Loading.transform.SetParent(this.transform.GetChild(0));
        Loading.transform.localPosition = Vector3.zero;
        Loading.transform.localEulerAngles = new Vector3(0, 0, 180);
        Loading.transform.localScale = new Vector3(0.2f/transform.localScale.x, 0.2f/ transform.localScale.y, 0.2f);
        UIContronl.GetInstance.AddPic(ID);
    }
    /// <summary>
    /// 加载失败
    /// </summary>
    /// <param name="str"></param>
    public void PicFail(string str)
    {
        Destroy(Loading);
        Fail = GameObject.Instantiate(Resources.Load("Fail") as GameObject);
        Fail.transform.SetParent(this.transform.GetChild(0));
        Fail.transform.localPosition = new Vector3(0, 0, 0.07f);
        Fail.transform.localEulerAngles = new Vector3(0, 0, 180);
        Fail.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        UIContronl.GetInstance.LoadFail(ID);
        PicContronl.GetInstance.loadPicOnce();
    }

    /// <summary>
    /// 加载成功
    /// </summary>
    /// <param name="tex"></param>
    void PicSuccess(Texture2D Tex)
    {
        StartCoroutine("creat", Tex);
    }
    /// <summary>
    /// 生成
    /// </summary>
    /// <param name="Tex"></param>
    /// <returns></returns>
    IEnumerator creat(Texture2D Tex)
    {
        Destroy(Loading);
        status = PicStatus.Success;
        GetComponent<BoxCollider>().enabled = true;
        tex = Tex;
        width = tex.width;
        height = tex.height;
        rate = ((float)tex.width / tex.height);
        float min = 1;
        if (Size_x > MaxLength_x || Size_y > MaxLength_y)
        {
            min = (((MaxLength_x / Size_x) < (MaxLength_y / Size_y) ? (MaxLength_x / Size_x) : (MaxLength_y / Size_y)));
        }
        transform.localScale = new Vector3(Size_x * min, Size_y * min, 0.1f);
        GameObject obj = Resources.Load("newhuakuang") as GameObject;
        GameObject huakuang = GameObject.Instantiate(obj);
        huakuang.transform.position = transform.position;
        huakuang.transform.localEulerAngles = transform.localEulerAngles;
        huakuang.GetComponent<NewHuakuang>().SetHeightorWidth(Size_x * min, Size_y * min);
        //huakuang.GetComponent<Huakuang>().Material(PicColor);
        huakuang.SetActive(true);
        //if (BackPlane == "one")
        //{
        //    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1f);
        //    //GameObject huakuang = GameObject.Instantiate(Huakuang);
        //    GameObject obj = Resources.Load("huakuang" + PicColor) as GameObject;
        //    GameObject huakuang = GameObject.Instantiate(obj);
        //    huakuang.transform.position = transform.position;
        //    huakuang.transform.localEulerAngles = transform.localEulerAngles;
        //    huakuang.GetComponent<NewHuakuang>().SetHeightorWidth(Size_x * min, Size_y * min);
        //    //huakuang.GetComponent<Huakuang>().Material(PicColor);
        //    huakuang.SetActive(true);
        //}
        //else 
        //{
        //    GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //    obj.transform.parent = this.transform;
        //    obj.transform.localScale=Vector3.one;
        //    obj.transform.localPosition = new Vector3(0,0,-0.53f);
        //    Destroy(obj.GetComponent<BoxCollider>());
        //}
        //transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = tex;
        transform.GetChild(0).gameObject.SetActive(false);
        GameObject plane = Resources.Load("Plane")as GameObject;
        GameObject.Instantiate(plane, transform);
        transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.mainTexture = tex;
        //yield return new WaitForSeconds(0.1f);
        //Resources.UnloadUnusedAssets();
        //System.GC.Collect();
        UIContronl.GetInstance.LoadSuccess(ID, tex);
        PicContronl.GetInstance.loadPicOnce();
        //if (Fengmian.GetInstance.isCanGood)
        //{
            good = GameObject.Instantiate(Resources.Load("good1") as GameObject);
            good.transform.SetParent(transform);
            good.GetComponent<Transform>().localPosition = new Vector3( -0.4f, 0.5f, 0.1f);
            good.GetComponent<Transform>().localEulerAngles = new Vector3(-270, 0, 0);
            good.GetComponent<Transform>().localScale = new Vector3(0.025f / transform.localScale.x, 0.025f / transform.localScale.y, 0.025f / transform.localScale.y);
            //good.transform.GetChild(1).GetComponent<TextMesh>().text = like_num;
            //good.transform.GetChild(1).gameObject.SetActive(false);
            //good.transform.GetChild(0).gameObject.SetActive(true);
            //good.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate
            //{
            //    WWWForm form = new WWWForm();
            //    form.AddField("token", Fengmian.token);
            //    form.AddField("work_id", Work_ID);
            //    HttpHelper.GetInstance.Post(goodurl, form, good1Success, good1Fail);
            //    form = null;
            //});
            good.transform.GetComponent<good>().go += delegate
            {
                //WWWForm form = new WWWForm();
                //form.AddField("token", Fengmian.token);
                //form.AddField("work_id", Work_ID);
                //HttpHelper.GetInstance.Post(goodurl, form, good1Success, good1Fail);
                //form = null;
                UIContronl.GetInstance.ShowPic2();
            };
        good.SetActive(false);
        //}
        yield return null;
    }
    //点赞成功
    public void good1Success(string str)
    {
        JsonData Good = JsonMapper.ToObject(str);
        if (Good["code"].ToString() == "1")
        {
            like_num = (int.Parse(like_num) + 1).ToString();
            //good.transform.GetChild(2).GetComponent<Text>().text = like_num;
            good.transform.GetChild(1).GetComponent<TextMesh>().text = like_num;
            is_like = "1";
        }
        else if (Good["code"].ToString() == "0")
        {
            UIContronl.GetInstance.SetGoodResultText("今天已点赞，请明天再来。");
        }
    }
    //点赞失败
    public void good1Fail(string str)
    {
        UIContronl.GetInstance.SetGoodResultText("点赞失败！");
    }
}
