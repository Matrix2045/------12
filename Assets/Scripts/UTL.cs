using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
public class UTL : MonoBehaviour
{
    private static List<Transform> objs;
    private static List<Transform> targets;
    private static Transform Parent;
    
    [MenuItem("MyTool/UTLSet")]
    private static void Set()
    {
        Parent = UTLTool.GetInstance.Parent;
        objs = new List<Transform>();
        targets = new List<Transform>();
        for (int i = 0; i < Parent.childCount; i++)
        {
            GameObject obj = new GameObject("obj");
            obj.transform.position = Parent.GetChild(i).position;
            obj.transform.eulerAngles = Parent.GetChild(i).eulerAngles;
            obj.transform.localScale = Parent.GetChild(i).localScale;
            objs.Add(obj.transform);
            targets.Add(Parent.GetChild(i));
        }
        for (int i = 0; i < objs.Count; i++)
        {
            objs[i].SetParent(Parent);
            targets[i].SetParent(objs[i]);
            targets[i].localPosition = Vector3.zero;
            targets[i].localEulerAngles = Vector3.zero;
            targets[i].localScale = Vector3.one;
        }
    }
    [MenuItem("MyTool/UTLReset")]
    private static void RSet()
    {
        Parent = UTLTool.GetInstance.Parent;
        for (int i = 0; i < Parent.childCount; i++)
        {
            Parent.GetChild(i).GetChild(0).localPosition = Vector3.zero;
            Parent.GetChild(i).GetChild(0).localEulerAngles = Vector3.zero;
            Parent.GetChild(i).GetChild(0).localScale = Vector3.one;
        }
    }
}
#endif

