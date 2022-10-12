using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
/// <summary>
/// 功能文件，设置点位
/// </summary>
public class PoseSet : MonoBehaviour
{
    [MenuItem("MyTool/SetPose")]
    private static void Set()
    {
        for (int i = 0; i < PicContronl.GetInstance.transform.childCount;i++)
        {
            Transform pose = PicContronl.GetInstance.transform.GetChild(i);
            Vector3  pos= new Vector3(pose.position.x + pose.forward.x * 1f, 0, pose.position.z + pose.forward.z * 1f);
            PathContronl.GetInstance.transform.GetChild(i).position = pos;
            Vector3 dir = PathContronl.GetInstance.transform.GetChild(i).position - new Vector3(pose.position.x, 0, pose.position.z);
            PathContronl.GetInstance.transform.GetChild(i).right = -dir;
            //PathContronl.GetInstance.transform.GetChild(i).position = new Vector3(
            //    -PathContronl.GetInstance.transform.GetChild(i).forward.x*0.2f + PathContronl.GetInstance.transform.GetChild(i).position.x,
            //     -PathContronl.GetInstance.transform.GetChild(i).forward.y * 0.2f + PathContronl.GetInstance.transform.GetChild(i).position.y,
            //      -PathContronl.GetInstance.transform.GetChild(i).forward.z * 0.2f + PathContronl.GetInstance.transform.GetChild(i).position.z);
        }
    }
}
#endif
