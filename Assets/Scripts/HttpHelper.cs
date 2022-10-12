using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Profiling;
using UnityEngine.UI;


public class HttpHelper : MonoBehaviour
{ 
    /// <summary>
  /// 单例
  /// </summary>
    private static HttpHelper Instance;
    private static UnityEngine.Object lockobj = new UnityEngine.Object();
    public static HttpHelper GetInstance
    {
        get
        {
            lock (lockobj)
            {
                if (Instance == null)
                {
                    Instance = FindObjectOfType<HttpHelper>() as HttpHelper;
                }
            }
            return Instance;
        }
    }
    private float downTimeOut = 15;
    private string MainUrl = "http://show.artoforiental.com/";
    private UnityAction<Texture2D> Picsuccess=null;
    private UnityAction<string> Picfail=null;
    private string content;
    //public Dictionary<string, byte[]> picbytes = new Dictionary<string, byte[]>();
    /// <summary>
    /// 请求数据
    /// </summary>
    /// <param name="url"></param>
    /// <param name="form"></param>
    /// <param name="Success"></param>
    /// <param name="Fail"></param>
    //public void Post(string url, WWWForm form,   Action<string> Success=null, Action<string> Fail=null)
    //{
    //    StartCoroutine(SendPost(url, form, Success, Fail));
    //}
    public void Post(string url, WWWForm form, UnityAction<string> Success=null,UnityAction<string> Fail=null )
    {
            StartCoroutine(SendPost(url, form, Success, Fail));
    }
    IEnumerator SendPost(string url, WWWForm form, UnityAction<string> Success = null, UnityAction<string> Fail = null)
    {
        if (GameManager.GetInstance.platform == Platform.Web)
        {
            WWW pose;
            if (form != null)
            {
                pose = new WWW(MainUrl + url, form);
            }
            else
            {
                pose = new WWW(MainUrl + url);
            }
            float process = pose.progress;
            float timeout = Time.time;
            while (pose != null && pose.isDone == false)
            {
                if (process < pose.progress)
                {
                    timeout = Time.time;
                    process = pose.progress;
                }
                if (Time.time - timeout > downTimeOut)
                {
                    if (Fail != null)
                    {
                        Fail("连接超时");
                        pose.Dispose();
                        yield break;
                    }
                }
                yield return null;
            }
            yield return pose;
            if (pose.error != null)
            {
                if (Fail != null)
                {
                    Fail("网络异常：" + pose.error);
                    pose.Dispose();
                    yield break;
                }
            }
            else
            {
                if (Success != null)
                {
                    Success(pose.text);
                    pose.Dispose();
                    yield break;
                }
            }
        }
        if (GameManager.GetInstance.platform == Platform.None) {
            string s = url.Replace("/", "");
            content = Resources.Load<TextAsset>(s).text;
            JsonData Pics = JsonMapper.ToObject(content);
            Success(Pics.ToJson());
        }
    }
    /// <summary>
    /// 下载图片
    /// </summary>
    /// <param name="url"></param>
    /// <param name="Success"></param>
    /// <param name="Fail"></param>
    public void Pic(string url, int MaxLength,UnityAction<Texture2D> Success = null, UnityAction<string> Fail = null)
    {
        StartCoroutine(DownPic(url, MaxLength,Success, Fail));
    }
    /// <summary>
    /// 下载图片
    /// </summary>
    /// <param name="url"></param>
    /// <param name="Success"></param>
    /// <param name="Fail"></param>
    IEnumerator DownPic(string url,int MaxLength,UnityAction<Texture2D> Success=null, UnityAction<string> Fail=null)
    {
        //这里的地址可以填本地文件地址  file://[文件路径]
        using (UnityWebRequest www =
            new UnityWebRequest(url+ "&x-oss-process=image/resize,m_lfit,h_"+MaxLength+",w_"+MaxLength))
        {
            DownloadHandlerTexture texhandler = new DownloadHandlerTexture(true);
            www.downloadHandler = texhandler;
            www.SendWebRequest();
            float process = www.downloadProgress;
            float timeout = Time.time;
            while (www != null && www.isDone == false)
            {
                if (process < www.downloadProgress)
                {
                    timeout = Time.time;
                    process = www.downloadProgress;
                }
                if (Time.time - timeout > downTimeOut)
                {
                    if (Fail != null)
                    {
                        Fail("连接超时");
                        www.Dispose();
                        yield break;
                    }
                }
                yield return null;
            }
            yield return www.isDone;
            if (www.isNetworkError)
            {
                Fail(www.error);
            }
            else
            {
                Texture2D tex = texhandler.texture;
                tex.Compress(false);
                tex.Apply();
                Success(tex);
                texhandler.Dispose();
                www.Dispose();
                //yield return Resources.UnloadUnusedAssets();
                //System.GC.Collect();
               yield return new WaitForEndOfFrame();
            }
        }
    }
    //下载成功
    public void PicSuccess(String str)
    {
        //过滤特殊字符   
        String dummyData = str.Trim().Replace("data:image/jpeg;base64,", "").Replace("%", "").Replace(",", "").Replace(" ", "+");
        str = null;
        if (dummyData.Length % 4 > 0)
        {
            dummyData = dummyData.PadRight(dummyData.Length + 4 - dummyData.Length % 4, '=');
        }
        byte[] byteArray = Convert.FromBase64String(dummyData);
        dummyData = null;
        Texture2D pic = new Texture2D(10,10, TextureFormat.RGB24, false);
        pic.LoadImage(byteArray);
        pic.Compress(false);
        pic.Apply();
        Picsuccess?.Invoke(pic);
    }
}
