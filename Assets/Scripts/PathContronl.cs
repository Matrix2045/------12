using UnityEngine;

public class PathContronl : MonoBehaviour
{   /// <summary>
    /// 单例
    /// </summary>
    private static PathContronl Instance;
    private static UnityEngine.Object lockobj = new UnityEngine.Object();
    public static PathContronl GetInstance
    {
        get
        {
            lock (lockobj)
            {
                if (Instance == null)
                {
                    Instance = FindObjectOfType<PathContronl>() as PathContronl;
                }
            }
            return Instance;
        }
    }
    private PathPoints[] points;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
    }
    /// <summary>
    /// 初始化
    /// </summary>
    void Init()
    {
        points = GetComponentsInChildren<PathPoints>();
    }
    /// <summary>
    /// 获取最近点
    /// </summary>
    public  void Mindis()
    {
        Vector3 targetpose = FirstPersonContronl.GetInstance.transform.position;
        PathPoints minpoint=null;
        float dis=500;
        foreach (var item in points)
        {
            float value = Vector3.Distance(item.transform.position, targetpose);
            if (dis > value)
            {
                dis = value;
                minpoint = item;
            }
        }
        if (FirstPersonContronl.GetInstance.isRoom)
        {
            FirstPersonContronl.GetInstance.AiTartget(minpoint.transform);
        }
        else 
        {
            FirstPersonContronl.GetInstance.AiTartget(points[0].transform);
        }
        FirstPersonContronl.GetInstance.SetAI(true);
    }
    public  void MoveToNext(PathPoints current)
    {
        current.SetNext();
    }
    /// <summary>
    /// 获取位置
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public Transform Pose(string str)
    {
        foreach (var item in points)
        {
            if (item.name != str)
                 continue;
            return item.transform;
        }
        return null;
    }
}
