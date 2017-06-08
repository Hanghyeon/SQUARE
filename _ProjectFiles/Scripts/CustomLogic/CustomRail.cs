using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public enum PlayMode
{
    Linear,
    Catmull,
}

public class CustomRail : MonoBehaviour {

    public static CustomRail Singleton;

    public Transform target = null;

    public float nodeFowardLineSize = 5f;
    public float screenSpaceSize = 3f;
    [Header("Node")]
    public List<Node> nodes = new List<Node>();       // Catmull을 사용하고자 한다면, 4개 이상을 유지해야함

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        //타겟 바라보게 하는건데 나중에 필요 없어짐
        if (target != null)
        {
            foreach (Node item in nodes)
            {
                Quaternion id = Quaternion.identity;
                Vector3 temp = (target.position - item.nodeTrans.position).normalized;
                id.SetLookRotation(temp);
                item.nodeTrans.rotation = id;
            }
        }
        //---------------------------------
    }

    //private void OnDrawGizmos()
    //{

    //    Gizmos.color = Color.red;

    //    foreach (Node item in nodes)
    //    {
    //        Gizmos.DrawRay(item.nodeTrans.position, item.nodeTrans.forward * nodeFowardLineSize);
    //    }

    //    for (int index = 0; index < nodes.Count - 1; index++)
    //    {
    //        Handles.color = Color.green;
    //        Handles.DrawDottedLine(nodes[index].nodeTrans.position, nodes[index + 1].nodeTrans.position, screenSpaceSize);
    //    }
    //}


    public Vector3 PositionOnRail(int seg, float ratio, PlayMode mode)
    {
        switch (mode)
        {
            default:
            case PlayMode.Linear:
                return LinearPosition(seg, ratio);
            case PlayMode.Catmull:
                return CatmullPosition(seg, ratio);
        }
    }


    public Vector3 LinearPosition(int seg, float ratio)
    {
        Vector3 p1 = nodes[seg].nodeTrans.position;
        Vector3 p2 = nodes[seg + 1].nodeTrans.position;

        return Vector3.Lerp(p1, p2, ratio);
    }

    public Vector3 CatmullPosition(int seg, float ratio)
    {
        Vector3 p1, p2, p3, p4;

        if (seg == 0)
        {
            p1 = nodes[seg].nodeTrans.position;
            p2 = p1;
            p3 = nodes[seg + 1].nodeTrans.position;
            p4 = nodes[seg + 2].nodeTrans.position;
        }
        else if (seg == nodes.Count - 2)
        {
            p1 = nodes[seg - 1].nodeTrans.position;
            p2 = nodes[seg].nodeTrans.position;
            p3 = nodes[seg + 1].nodeTrans.position;
            p4 = p3;
        }
        else
        {
            p1 = nodes[seg - 1].nodeTrans.position;
            p2 = nodes[seg].nodeTrans.position;
            p3 = nodes[seg + 1].nodeTrans.position;
            p4 = nodes[seg + 2].nodeTrans.position;
        }

        float t2 = ratio * ratio;
        float t3 = t2 * ratio;

        float x =
            0.5f * ((2.0f * p2.x) +
            (-p1.x + p3.x) * ratio +
            (2.0f * p1.x - 5.0f * p2.x + 4 * p3.x - p4.x) * t2 +
            (-p1.x + 3.0f * p2.x - 3.0f * p3.x + p4.x) * t3);

        float y =
            0.5f * ((2.0f * p2.y) +
            (-p1.y + p3.y) * ratio +
            (2.0f * p1.y - 5.0f * p2.y + 4 * p3.y - p4.y) * t2 +
            (-p1.y + 3.0f * p2.y - 3.0f * p3.y + p4.y) * t3);

        float z =
            0.5f * ((2.0f * p2.z) +
            (-p1.z + p3.z) * ratio +
            (2.0f * p1.z - 5.0f * p2.z + 4 * p3.z - p4.z) * t2 +
            (-p1.z + 3.0f * p2.z - 3.0f * p3.z + p4.z) * t3);

        return new Vector3(x, y, z);
    }

    public Quaternion Orientation(int seg, float ratio)
    {
        Quaternion q1 = nodes[seg].nodeTrans.rotation;
        Quaternion q2 = nodes[seg + 1].nodeTrans.rotation;

        return Quaternion.Lerp(q1, q2, ratio);
    }
}
