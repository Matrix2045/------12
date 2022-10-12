using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UTLTool : MonoBehaviour
{
    /// <summary>
    /// 单例
    /// </summary>
    private static UTLTool Instance;
    private static UnityEngine.Object lockobj = new UnityEngine.Object();
    public static UTLTool GetInstance
    {
        get
        {
            lock (lockobj)
            {
                if (Instance == null)
                {
                    Instance = FindObjectOfType<UTLTool>() as UTLTool;
                }
            }
            return Instance;
        }
    }
    public Transform Parent;
}
