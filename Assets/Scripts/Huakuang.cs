using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 画框
/// </summary>
public class Huakuang : MonoBehaviour
{
    public Transform BianShang;
    public Transform BianXia;
    public Transform BianLeft;
    public Transform BianRight;
    public Transform Jiao1;
    public Transform Jiao2;
    public Transform Jiao3;
    public Transform Jiao4;
    public Transform HuaBan;
    /// <summary>
    /// 设置画框位置
    /// </summary>
    /// <param name="target"></param>
    public void SetPose(Transform target)
    {
        transform.position = target.position;
    }
    /// <summary>
    /// 设置画框长宽
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void SetHeightorWidth(float width, float height)
    {        
        HuaBan.localScale = new Vector3(width + 0.1f, height+0.1f, 0.1f);
        BianLeft.localScale = new Vector3(BianLeft.localScale.x, height+0.07f, BianLeft.localScale.z);
        BianLeft.localPosition = new Vector3(-(width*0.5f+0.05f), BianLeft.localPosition.y, BianLeft.localPosition.z);
        BianRight.localScale = new Vector3(BianRight.localScale.x, height + 0.07f, BianRight.localScale.z);
        BianRight.localPosition = new Vector3((width* 0.5f + 0.05f), BianRight.localPosition.y, BianRight.localPosition.z);
        BianShang.localScale = new Vector3(width + 0.07f, BianShang.localScale.y, BianShang.localScale.z);
        BianShang.localPosition = new Vector3(BianShang.localPosition.x , (height* 0.5f + 0.05f),  BianShang.localPosition.z);
        BianXia.localScale = new Vector3(width + 0.07f, BianXia.localScale.y, BianXia.localScale.z);
        BianXia.localPosition = new Vector3(BianXia.localPosition.x, -(height* 0.5f + 0.05f), BianXia.localPosition.z);
        Jiao1.localPosition = new Vector3(-(width*0.5f + 0.05f), -(height*0.5f + 0.05f), Jiao1.localPosition.z);
        Jiao2.localPosition = new Vector3((width * 0.5f + 0.05f), -(height * 0.5f + 0.05f), Jiao2.localPosition.z);
        Jiao3.localPosition = new Vector3(width * 0.5f + 0.05f, height * 0.5f + 0.05f, Jiao3.localPosition.z);
        Jiao4.localPosition = new Vector3(-(width * 0.5f + 0.05f), (height * 0.5f + 0.05f), Jiao4.localPosition.z);
    }
    /// <summary>
    /// 材质适配
    /// </summary>
    public void Material(string str)
    {
        Material mt = Resources.Load<Material>("huakuang_jiao");
        mt.mainTexture = Resources.Load<Texture>(str);
        Jiao1.GetComponent<MeshRenderer>().material = mt;
        Jiao1.GetComponent<MeshRenderer>().material.SetTextureScale("_MainTex", new Vector2(Jiao1.localScale.x * 10, Jiao1.localScale.y * 10));
        Jiao2.GetComponent<MeshRenderer>().material = mt;
        Jiao2.GetComponent<MeshRenderer>().material.SetTextureScale("_MainTex", new Vector2(Jiao2.localScale.x * 10, Jiao2.localScale.y * 10));
        Jiao3.GetComponent<MeshRenderer>().material = mt;
        Jiao3.GetComponent<MeshRenderer>().material.SetTextureScale("_MainTex", new Vector2(Jiao3.localScale.x * 10, Jiao3.localScale.y * 10));
        Jiao4.GetComponent<MeshRenderer>().material = mt;
        Jiao4.GetComponent<MeshRenderer>().material.SetTextureScale("_MainTex", new Vector2(Jiao4.localScale.x * 10, Jiao4.localScale.y * 10));
        mt = Resources.Load<Material>("huakuang_sx");
        mt.mainTexture = Resources.Load<Texture>(str);
        BianShang.GetComponent<MeshRenderer>().material = mt;
        BianXia.GetComponent<MeshRenderer>().material = mt;
        BianShang.GetComponent<MeshRenderer>().material.SetTextureScale("_MainTex", new Vector2(BianShang.localScale.x * 10, BianShang.localScale.y * 10));
        BianXia.GetComponent<MeshRenderer>().material.SetTextureScale("_MainTex", new Vector2(BianXia.localScale.x * 10, BianXia.localScale.y * 10));
        mt = Resources.Load<Material>("huakuang_zy");
        mt.mainTexture = Resources.Load<Texture>(str);
        BianLeft.GetComponent<MeshRenderer>().material = mt;
        BianRight.GetComponent<MeshRenderer>().material = mt;
        BianLeft.GetComponent<MeshRenderer>().material.SetTextureScale("_MainTex", new Vector2(BianLeft.localScale.x * 10, BianLeft.localScale.y * 10));
        BianRight.GetComponent<MeshRenderer>().material.SetTextureScale("_MainTex", new Vector2(BianRight.localScale.x * 10, BianRight.localScale.y * 10));
    }
}
