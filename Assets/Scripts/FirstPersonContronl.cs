
using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 第一人称控制器
/// </summary>
public class FirstPersonContronl : MonoBehaviour
{
    /// <summary>
    /// 单例
    /// </summary>
    private static FirstPersonContronl Instance;
    private static UnityEngine.Object lockobj = new UnityEngine.Object();
    public static FirstPersonContronl GetInstance
    {
        get
        {
            lock (lockobj)
            {
                if (Instance == null)
                {
                    Instance = FindObjectOfType<FirstPersonContronl>() as FirstPersonContronl;
                }
            }
            return Instance;
        }
    }
    /// <summary>
    /// 移动速度
    /// </summary>
    private float MoveFSpeed = 0;
    private float MoveBSpeed = 0;
    private float MoveLSpeed = 0;
    private float MoveRSpeed = 0;
    private float MaxMoveSpeed = 1;
    private float MinMoveSpeed = 0;
    private float RateMoveSpeed = 0.1f;
    private float MaxRotaSpeed = 1;
    private float MinRotaSpeed = 0;
    private float RateRotaSpeed = 1f;
    private float RotateSpeed = 0;
    private float mousespeed = 0;
    /// <summary>
    /// 人物控制器
    /// </summary>
    private CharacterController controller;
    private bool isForward = false;
    private bool isBack = false;
    private bool isLeft = false;
    private bool isRight = false;
    private bool isRote = false;
    public bool isCanrote = true;
    /// <summary>
    /// AI
    /// </summary>
    public GameObject TargetEffect;
    private bool isSelf = false;
    private Vector3 SelfTarget;
    private bool isAI = false;
    private Transform AITarget;
    private float AIMoveSpeed = 0.3f;
    private float AIRotateSpeed = 90;
    public float SetSpeed = 1;
    private float SelfSpeed = 3;

    private bool isChoose = false;
    private Transform ChooseTarget;
    private Quaternion ChooseDir;
    /// <summary>
    /// 计时器
    /// </summary>
    private int StartTime = 0;
    private string ChooseTargetName="";
    private bool isStartTime = false;
    private string url = "api/Index/worksBrowse";
    public bool isCan = false;
    public bool isRoom = false;
    private bool isTouch = false;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    private void FixedUpdate()
    {
            WebContronl();
            Move();
            Rota();
            AI();
            Collider();
    }
    private void Update()
    {
        //if (!isCan)
        //    return;
        PointCheck();
        if (isForward)
            MoveFSpeed = Mathf.Lerp(MoveFSpeed, MaxMoveSpeed, RateMoveSpeed);
        if (isBack)
            MoveBSpeed = Mathf.Lerp(MoveBSpeed, MaxMoveSpeed, RateMoveSpeed);
        if (isLeft)
            MoveLSpeed = Mathf.Lerp(MoveLSpeed, MaxMoveSpeed, RateMoveSpeed);
        if (isRight)
            MoveRSpeed = Mathf.Lerp(MoveRSpeed, MaxMoveSpeed, RateMoveSpeed);
        if (!isRight && !isLeft && !isForward && !isBack)
        {
            MoveFSpeed = Mathf.Lerp(MoveFSpeed, MinMoveSpeed, RateMoveSpeed);
            MoveBSpeed = Mathf.Lerp(MoveBSpeed, MinMoveSpeed, RateMoveSpeed);
            MoveLSpeed = Mathf.Lerp(MoveLSpeed, MinMoveSpeed, RateMoveSpeed);
            MoveRSpeed = Mathf.Lerp(MoveRSpeed, MinMoveSpeed, RateMoveSpeed);
        }
        if (isRote)
        {
            RotateSpeed = Mathf.Lerp(RotateSpeed, MaxRotaSpeed, RateRotaSpeed);
        }
        if (!isRote)
        {
            RotateSpeed = Mathf.Lerp(RotateSpeed, MinRotaSpeed, RateRotaSpeed);
        }
        if (isAI)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, AITarget.localRotation, Time.deltaTime * AIRotateSpeed * SetSpeed*2);
        }
        if (isChoose)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, ChooseDir, Time.deltaTime * AIRotateSpeed * SetSpeed*2);

        }
    }
    /// <summary>
    /// 网页控制
    /// </summary>
    void WebContronl()
    {

        if (Input.touchCount == 1)
        {
            if (!IsPointerOverUIObject()&&isCanrote)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    if (Mathf.Abs(Input.GetAxis("Mouse X"))>0.1f)
                    {
                        isRote = true;
                        AIpause();
                        if (Input.GetAxis("Mouse X") > 0)
                            mousespeed = -1;
                        if (Input.GetAxis("Mouse X") < 0)
                            mousespeed = 1;
                    }
                }
            }
        }
        else if (Input.GetMouseButton(0) && isCanrote)
        {
            if (!IsPointerOverUIObject())
            {
                if (Input.GetAxis("Mouse X") != 0 && Mathf.Abs(Input.GetAxis("Mouse X")) > 0.1f)
                {

                    isRote = true;
                    AIpause();
                    if (Input.GetAxis("Mouse X") > 0)
                        mousespeed = -1;
                    if (Input.GetAxis("Mouse X") < 0)
                        mousespeed = 1;
                }
            }
        }
        else
        {
            isRote = false;
        }
    }
    /// <summary>
    /// 前移
    /// </summary>
    public void MoveForward()
    {
        isRight = true;
        AIpause();
    }
    /// <summary>
    /// 后移
    /// </summary>
    public void MoveBack()
    {
        isLeft = true;
        AIpause();
    }
    /// <summary>
    /// 左移
    /// </summary>
    public void MoveLeft()
    {
        isForward = true;
        AIpause();
    }
    /// <summary>
    /// 右移
    /// </summary>
    public void MoveRight()
    {
        isBack = true;
        AIpause();
    }
    /// <summary>
    /// 停止
    /// </summary>
    public void NoMove()
    {
        isRight = false;
        isLeft = false;
        isForward = false;
        isBack = false;
    }
    /// <summary>
    /// 移动
    /// </summary>
    void Move()
    {
        controller.Move(transform.forward * Time.deltaTime * MoveFSpeed * SetSpeed*2);
        controller.Move(-transform.forward * Time.deltaTime * MoveBSpeed * SetSpeed*2);
        controller.Move(-transform.right * Time.deltaTime * MoveLSpeed * SetSpeed*2);
        controller.Move(transform.right * Time.deltaTime * MoveRSpeed * SetSpeed*2);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
    /// <summary>
    /// 旋转
    /// </summary>
    void Rota()
    {
        if (!IsPointerOverUIObject())
        {
            transform.Rotate(mousespeed * RotateSpeed * Vector3.up * SetSpeed*2);
            if (RotateSpeed < 0.1f)
            {
                RotateSpeed = 0;
                mousespeed = 0;
            }
        }
    }
    /// <summary>
    /// 自动寻路
    /// </summary>
    /// <param name="bo"></param>
    public void SetAI(bool bo)
    {
        isAI = bo;
        if (bo)
        {
            isSelf = false;
            isChoose = false;
            TargetEffect.SetActive(false);
            TimeEnd();
        }
    }
    /// <summary>
    /// 设置自动寻路目标
    /// </summary>
    /// <param name="pos"></param>
    public void AiTartget(Transform pos)
    {
        AITarget = pos;
    }
    /// <summary>
    /// 自动浏览
    /// </summary>
    void AI()
    {
        if (isAI)
        {
            if (Vector3.Distance(transform.position, AITarget.position) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, AITarget.position, Time.deltaTime * AIMoveSpeed * SetSpeed*2);
            }
            else
            {
                PathContronl.GetInstance.MoveToNext(AITarget.GetComponent<PathPoints>());
            }

        }
        if (isSelf)
        {
            if (Vector3.Distance(transform.position, SelfTarget) > 1.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, SelfTarget, Time.deltaTime * SelfSpeed * SetSpeed * 2);
                //GetComponent<CharacterController>().Move(new Vector3(SelfTarget.x-transform.position.x, 
                //    0,
                //    SelfTarget.z - transform.position.z) * Time.deltaTime * SelfSpeed * SetSpeed * 0.2f);
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                TargetEffect.SetActive(true);
                TargetEffect.transform.position =new Vector3(SelfTarget.x,0.1f, SelfTarget.z);
            }
            else {
                TargetEffect.SetActive(false);
            }
        }
        if (isChoose)
        { 

            if (Vector3.Distance(transform.position, ChooseTarget.position) > 1f)
            {
                Camera.main.transform.position = new Vector3(ChooseTarget.position.x, Camera.main.transform.position.y, ChooseTarget.position.z);
            }
            if (Vector3.Distance(transform.position, ChooseTarget.position) > 0.01f)
            {
                //transform.position = Vector3.MoveTowards(transform.position, ChooseTarget.position, Time.deltaTime * SelfSpeed * SetSpeed);
                transform.position = ChooseTarget.position;
                transform.localEulerAngles = ChooseTarget.localEulerAngles;
                Camera.main.transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + 90, 0);
            }
        }
    }
    /// <summary>
    /// 自动浏览停止
    /// </summary>
    void AIpause()
    {
        isAI = false;
        isSelf = false;
        isChoose = false;
        TargetEffect.SetActive(false);
        UIContronl.GetInstance.AIpause();
        UIContronl.GetInstance.CloseDetails();
        //jiesuan
        TimeEnd();
    }
    /// <summary>
    /// 选择图片移动
    /// </summary>
    /// <param name="num"></param>
    /// <param name="isShowDetail"></param>
    public void MovePic(int num,bool isShowDetail)
    {
        if (isStartTime)
        {
            if (PicContronl.GetInstance.transform.GetChild(num).GetComponent<PicItem>().Work_ID != ChooseTargetName)
            {
                TimeEnd();
            }
        }
        if (!isStartTime)
        {
                StartTime = (int)Time.time;
                isStartTime = true;
        }
        Transform pic = PicContronl.GetInstance.transform.GetChild(num);
        ChooseTargetName = PicContronl.GetInstance.transform.GetChild(num).GetComponent<PicItem>().Work_ID;
        ChooseTarget = PathContronl.GetInstance.transform.GetChild(num);
        ChooseDir = PathContronl.GetInstance.transform.GetChild(num).rotation;
        isChoose = true;
        isSelf = false;
        isAI = false;
        UIContronl.GetInstance.AIpause();
        if(isShowDetail)
           UIContronl.GetInstance.ShowDetails(PicContronl.GetInstance.transform.GetChild(num).GetComponent<PicItem>());
    }
    /// <summary>
    /// 点击检测
    /// </summary>
    private void PointCheck()
    {
        if (Input.touchCount == 0)
        {
            isTouch = true;
        }
        if (Input.touchCount == 1 && !IsPointerOverUIObject()&&isTouch)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.name == "Flood" || hit.transform.name == "RoomOut")
                    {
                        SelfTarget = hit.point;
                        isSelf = true;
                        isChoose = false;
                        isAI = false;
                        UIContronl.GetInstance.AIpause();
                        UIContronl.GetInstance.CloseDetails();
                    }
                    if (hit.transform.GetComponent<PicItem>() != null)
                    {
                        int value = hit.transform.GetSiblingIndex();
                        if (Vector3.Distance(transform.position, new Vector3(hit.transform.position.x, 0, hit.transform.position.z)) <=
                        PathContronl.GetInstance.transform.GetChild(value).GetComponent<PathPoints>().Dis + 0.1f)
                        {
                            UIContronl.GetInstance.ShowPic1(value);
                        }
                        UIContronl.GetInstance.ChoosePic(value);
                    }
                    if (hit.transform.GetComponent<Fengmian>() != null)
                    {
                        UIContronl.GetInstance.ShowFengmian(Fengmian.GetInstance.picurl);
                    }
                }

            }
            isTouch = false;
        }
        else if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {              
                if (hit.transform.name == "Flood"|| hit.transform.name == "RoomOut")
                {
                    SelfTarget = hit.point;
                    isSelf = true;
                    isChoose = false;
                    isAI = false;
                    UIContronl.GetInstance.AIpause();
                    UIContronl.GetInstance.CloseDetails();
                }
                if (hit.transform.GetComponent<PicItem>() != null)
                {
                    int value = hit.transform.GetSiblingIndex();
                    if (Vector3.Distance(transform.position, new Vector3(hit.transform.position.x, 0, hit.transform.position.z)) <=
                    PathContronl.GetInstance.transform.GetChild(value).GetComponent<PathPoints>().Dis + 0.1f)
                    {
                        UIContronl.GetInstance.ShowPic1(value);
                    }
                    UIContronl.GetInstance.ChoosePic(value);
                }
                if (hit.transform.GetComponent<Fengmian>() != null)
                {
                    UIContronl.GetInstance.ShowFengmian(Fengmian.GetInstance.picurl);
                }
            }
        }
    }
    /// <summary>
    /// 浏览倒计时
    /// </summary>
    private void TimeEnd()
    {
        if (ChooseTargetName == "")
            return;
        if ((int)Time.time - (int)StartTime < 10)
        {
            StartTime = 0;
            ChooseTargetName = "";
            isStartTime = false;
            return;
        }
        //SendTime((int)Time.time - (int)StartTime);
        StartTime = 0;
        ChooseTargetName = "";
        isStartTime = false;
    }
    /// <summary>
    /// 记录浏览时间
    /// </summary>
    /// <param name="num"></param>
    private void SendTime(int num)
    {
        WWWForm form = new WWWForm();
        form.AddField("token", Fengmian.token);
        form.AddField("work_id", ChooseTargetName);
        form.AddField("browse_time", num);
        HttpHelper.GetInstance.Post(url, form, SendTimeSuccess, SendTimeFail);
    }
    /// <summary>
    /// 记录浏览时间成功
    /// </summary>
    /// <param name="str"></param>
    private void SendTimeSuccess(string str)
    {
        JsonData Pics = JsonMapper.ToObject(str);
        if (Pics["code"].ToString() == "1")
        {
            Debug.Log("记录成功！");
        }
        else if (Pics["code"].ToString() == "101")
        {
            UIContronl.GetInstance.LoginPanel.SetActive(true);
        }
        else if (Pics["code"].ToString() == "0")
        {
            Debug.Log("数据错误！");
        }
    }
    /// <summary>
    /// 记录时间失败
    /// </summary>
    /// <param name="str"></param>
    private void SendTimeFail(string str)
    {
        Debug.Log(str);
    }
    /// <summary>
    /// UI检测
    /// </summary>
    /// <returns></returns>
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List <RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }
    private void Collider()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.3f, -transform.up, out hit, 4f))
        {
            if (hit.transform.name == "RoomOut")
            {
                isRoom = false;
            }
            else 
            {
                isRoom = true;
            }
        }
    }

}