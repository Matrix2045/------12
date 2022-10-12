using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 新画框
/// </summary>
public class NewHuakuang : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 新画框设置位置
    /// </summary>
    /// <param name="target"></param>
    public void SetPose(Transform target)
    {
        transform.position =target.position;
    }
    /// <summary>
    /// 新画框设置长宽
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void SetHeightorWidth(float width, float height)
    {
        transform.GetChild(0).localScale = new Vector3(width + 0.4f, height + 0.4f, 0.05f);
        transform.GetChild(1).localScale = new Vector3(width + 0.35f, height + 0.35f, 0.05f);
        transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.SetTextureScale("_MainTex", new Vector2(transform.localScale.x, transform.localScale.y ));
    }
}
