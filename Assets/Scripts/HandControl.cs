using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandControl : MonoBehaviour,IPointerClickHandler
{
    public Canvas canvas;
    public ScrollRect scroll;
    /// <summary>
    /// 上一帧两指间距离
    /// </summary>
    private float lastDistance = 0;
    /// <summary>
    /// 当前两个手指之间的距离
    /// </summary>
    private float twoTouchDistance = 0;
    /// <summary>
    /// 第一根手指按下的坐标
    /// </summary>
    Vector2 firstTouch = Vector2.zero;

    /// <summary>
    /// 第二根手指按下的坐标
    /// </summary>
    Vector2 secondTouch = Vector2.zero;

    /// <summary>
    /// 是否有两只手指按下
    /// </summary>
    private bool isTwoTouch = false;
    //默认x
    public float NormalX = 0;
    ///默认y
    public float NormalY = 0;

    private uint clicknum = 0;
    private float clivktimer = 0;
    private bool doublecheck = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {

        if (transform.localScale.x < NormalX || transform.localScale.y < NormalY)
        {
            transform.localScale = new Vector2(NormalX, NormalY);
        }
        else
        {
            float value = Input.GetAxis("Mouse ScrollWheel");
            transform.localScale = new Vector2(transform.localScale.x * (1 + value * 0.5f), transform.localScale.y * (1 + value * 0.5f));
            GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x * (1 + value * 0.5f), GetComponent<RectTransform>().anchoredPosition.y * (1 + value * 0.5f));
        }
        NewHand();
        ////如果有两个及以上的手指按下
        //if (Input.touchCount > 1)
        //{
        //    scroll.enabled = false;
        //    //当第二根手指按下的时候
        //    if (Input.GetTouch(1).phase == TouchPhase.Began)
        //    {
        //        //获取第一根手指的位置
        //        firstTouch = Input.touches[0].position;
        //        //获取第二根手指的位置
        //        secondTouch = Input.touches[1].position;

        //        lastDistance = Vector2.Distance(firstTouch, secondTouch);

        //        isTwoTouch = true;
        //    }

        //    //如果有两根手指按下
        //    if (isTwoTouch)
        //    {
        //        //每一帧都得到两个手指的坐标以及距离
        //        firstTouch = Input.touches[0].position;
        //        secondTouch = Input.touches[1].position;
        //        twoTouchDistance = Vector2.Distance(firstTouch, secondTouch);
        //        //当前图片的缩放
        //        Vector3 curImageScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
        //        //两根手指上一帧和这帧之间的距离差
        //        //因为100个像素代表单位1，把距离差除以100看缩放几倍
        //        float changeScaleDistance = (twoTouchDistance - lastDistance) / 300;
        //        //图片的缩放等于当前的缩放加上 修改的缩放
        //        if (transform.localScale.x < NormalX || transform.localScale.y < NormalY)
        //        {
        //            transform.localScale = new Vector2(NormalX, NormalY);
        //        }
        //        else
        //        {
        //            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x * (1 + changeScaleDistance)), Mathf.Abs(transform.localScale.y * (1 + changeScaleDistance)));
        //        }
        //        //这一帧结束后，当前的距离就会变成上一帧的距离了
        //        lastDistance = twoTouchDistance;
        //    }
        //    if (Input.GetTouch(1).phase == TouchPhase.Ended)
        //    {
        //        scroll.enabled = true;
        //        isTwoTouch = false;
        //        lastDistance = 0;
        //        firstTouch = Vector3.zero;
        //        secondTouch = Vector3.zero;
        //    }
        //}
    }
    private void NewHand()
    {
        if (Input.touchCount <= 1)
        {
            scroll.enabled = true;
            firstTouch = Vector3.zero;
            secondTouch = Vector3.zero;
            lastDistance = 0;
            doublecheck = false;
        }
        else if(Input.touchCount == 2)
        {
            doublecheck = true;
            clicknum = 0;
            clivktimer = 0;
            scroll.enabled = false;
            if ((firstTouch == Vector2.zero) && (secondTouch == Vector2.zero))
            {
                //获取第一根手指的位置
                firstTouch = Input.touches[0].position;
                //获取第二根手指的位置
                secondTouch = Input.touches[1].position;

                lastDistance = Vector2.Distance(firstTouch, secondTouch);
            }
            else {
                //每一帧都得到两个手指的坐标以及距离
                firstTouch = Input.touches[0].position;
                secondTouch = Input.touches[1].position;
                twoTouchDistance = Vector2.Distance(firstTouch, secondTouch);
                //当前图片的缩放
                Vector3 curImageScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
                //两根手指上一帧和这帧之间的距离差
                //因为100个像素代表单位1，把距离差除以100看缩放几倍
                float changeScaleDistance = (twoTouchDistance - lastDistance) / 300;
                //图片的缩放等于当前的缩放加上 修改的缩放
                if (transform.localScale.x < NormalX || transform.localScale.y < NormalY)
                {
                    transform.localScale = new Vector2(NormalX, NormalY);
                }
                else
                {
                    transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x * (1 + changeScaleDistance)), Mathf.Abs(transform.localScale.y * (1 + changeScaleDistance)));
                    GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x * (1 + changeScaleDistance), GetComponent<RectTransform>().anchoredPosition.y * (1 + changeScaleDistance));
                }
                //这一帧结束后，当前的距离就会变成上一帧的距离了
                lastDistance = twoTouchDistance;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (doublecheck)
            return;
        if (Time.time - clivktimer > 0.3f)
        {
            clicknum = 1;
            clivktimer = Time.time;
        }else if(Time.time - clivktimer <= 0.3f)
        {
            clicknum++;
            clivktimer = Time.time;
        }
        if (clicknum == 2)
        {
            Vector2 _pos = Vector2.one;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                Input.mousePosition, canvas.worldCamera, out _pos);
            GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x - _pos.x, GetComponent<RectTransform>().anchoredPosition.y - _pos.y);
            GetComponent<RectTransform>().localScale = new Vector2(GetComponent<RectTransform>().localScale.x * 2, GetComponent<RectTransform>().localScale.y * 2);
            GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x * 2, GetComponent<RectTransform>().anchoredPosition.y * 2);
        }
    }
}
