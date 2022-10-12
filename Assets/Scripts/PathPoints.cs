using UnityEngine;

public class PathPoints : MonoBehaviour
{
    public Transform NextPoint;
    public float Dis=1;
    public void SetNext()
    {
        FirstPersonContronl.GetInstance.AiTartget(NextPoint);
    }
    private void OnDrawGizmos()
    {
        if (NextPoint == null)
            return;
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, NextPoint.position);
    }
    public void ResetPose(float width,float height)
    {
        float halfFOV = (Camera.main.fieldOfView * 0.5f) * Mathf.Deg2Rad;
        float aspect = Camera.main.aspect;
        float dis1 = height*0.7f / Mathf.Tan(halfFOV);
        float dis2 = (width*0.7f / aspect) / Mathf.Tan(halfFOV);
        float dis= ((dis1> dis2 ? dis1: dis2))*1.2f;
        if (dis < 1)
            return;
        Dis = dis;
        Transform pose =PicContronl.GetInstance.transform.GetChild(transform.GetSiblingIndex());
        Vector3 pos = new Vector3(pose.position.x + pose.forward.x * dis, 0, pose.position.z + pose.forward.z * dis);
        transform.position = pos;
    }

}
