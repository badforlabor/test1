using UnityEngine;
using UnityEditor;
using System.Collections;



public class ThreeBody : MonoBehaviour
{
    public GameObject body1;
    public GameObject body2;
    public GameObject body3;
}

[CustomEditor(typeof(ThreeBody))]
public class ThreeBodyEditor : Editor
{
    public void OnSceneGUI()
    {
        ThreeBody me = (ThreeBody)target;
        if (me == null || me.body1 == null || me.body2 == null || me.body3 == null)
        {
            return;
        }

        // 显示一个label
        Handles.Label(me.body1.transform.position, " " + me.body1.name);
        Handles.Label(me.body2.transform.position, " " + me.body2.name);
        Handles.Label(me.body3.transform.position, " " + me.body3.name);

        // 显示一个可更改位置的绳结
        me.body1.transform.position = Handles.PositionHandle(me.body1.transform.position, me.body1.transform.rotation);
        //me.body2.transform.position = Handles.PositionHandle(me.body2.transform.position, me.body2.transform.rotation);
        me.body2.transform.rotation = Handles.RotationHandle(me.body2.transform.rotation, me.body2.transform.position);
        me.body3.transform.position = Handles.PositionHandle(me.body3.transform.position, me.body3.transform.rotation);

        // 显示连接线
        Handles.color = Color.red;
        Handles.DrawLine(me.body1.transform.position, me.body2.transform.position);
        Handles.color = Color.white;

        // 显示曲线，能直接连接所有的物体（1连接2,2连接3,3连接4）
        Handles.DrawPolyLine(me.transform.position, me.body2.transform.position, me.body3.transform.position, me.transform.position);

        // 显示贝塞尔曲线
        float w = HandleUtility.GetHandleSize(Vector3.zero) * 0.1f;
        //Camera.current.transform.position
        Handles.DrawBezier(me.body1.transform.position, me.transform.position, Vector3.up, Vector3.up, Color.green, null, 1);
    }
}