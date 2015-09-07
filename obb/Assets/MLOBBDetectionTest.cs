/****************************************************************************
Copyright (c) 2013-2014,Dalian-LingYOU tech.
 This is not a free-ware .DO NOT use it without any authorization.
 * 
 * date : 9/7/2015 3:41:38 PM
 * author : Labor
 * purpose : OBB检测。参考cocos2d-x代码CCOBB.CPP
****************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MLGame
{
    // obb检测，测试代码
    class MLOBBDetectionTest : MonoBehaviour
    {
        public GameObject g1 = null;
        public GameObject g2 = null;

        OBB obb1 = null;
        OBB obb2 = null;

        public void OnDrawGizmos()
        {
            if (g1 != null && g1.transform.hasChanged)
                obb1 = null;
            if (g2 != null && g2.transform.hasChanged)
                obb2 = null;

            if (obb1 == null)
            {
                obb1 = g1 == null ? null : OBB.SpawnOBB(g1.transform);
            }
            if (obb2 == null)
            {
                obb2 = g2 == null ? null : OBB.SpawnOBB(g2.transform);
            }

            Color old = Gizmos.color;
            if (obb1 != null && obb2 != null && obb1.Intersects(obb2))
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
            }

            OBB.DrawGizmos(obb1);
            OBB.DrawGizmos(obb2);

            Gizmos.color = old;
        }
    }
    public class OBBMono : MonoBehaviour
    {
        OBB obb = null;

        public void OnDrawGizmos()
        {
            if (obb == null || transform.hasChanged)
            {
                obb = OBB.SpawnOBB(this.transform);
            }
            OBB.DrawGizmos(obb);
        }
    }

    // 真正的obb处理类。
    class OBB
    {
        // 构建一个obb，只有渲染的物件才会有obb
        public static OBB SpawnOBB(Transform t)
        {
            Renderer r = t.GetComponent<Renderer>();
            if (r != null)
            {
                return new OBB(r);
            }
            return null;
        }

        // 判断是否相交
        public bool Intersects(OBB box)
        {
            float min1, max1, min2, max2;
            for (int i = 0; i < 3; i++)
            {
                getInterval(this, getFaceDirection(i), out min1, out max1);
                getInterval(box, getFaceDirection(i), out min2, out max2);
                if (max1 < min2 || max2 < min1) return false;
            }
    
            for (int i = 0; i < 3; i++)
            {
                getInterval(this, box.getFaceDirection(i), out min1, out max1);
                getInterval(box, box.getFaceDirection(i), out min2, out max2);
                if (max1 < min2 || max2 < min1) return false;
            }
    
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Vector3 axis;
                    axis = Vector3.Cross(getEdgeDirection(i), box.getEdgeDirection(j));
                    getInterval(this, axis, out min1, out max1);
                    getInterval(box, axis, out min2, out max2);
                    if (max1 < min2 || max2 < min1) return false;
                }
            }
            return true;
        }

        // 获取8个角的坐标
        public Vector3[] GetCorners()
        {
            Vector3[] verts = new Vector3[8];
            verts[0] = _center - _extentX + _extentY + _extentZ;     // left top front
            verts[1] = _center - _extentX - _extentY + _extentZ;     // left bottom front
            verts[2] = _center + _extentX - _extentY + _extentZ;     // right bottom front
            verts[3] = _center + _extentX + _extentY + _extentZ;     // right top front

            verts[4] = _center + _extentX + _extentY - _extentZ;     // right top back
            verts[5] = _center + _extentX - _extentY - _extentZ;     // right bottom back
            verts[6] = _center - _extentX - _extentY - _extentZ;     // left bottom back
            verts[7] = _center - _extentX + _extentY - _extentZ;     // left top back

            return verts;
        }

        Vector3 getFaceDirection(int index)
        {
            Vector3[] corners = GetCorners();            
    
            Vector3 faceDirection, v0, v1;
            switch(index)
            {
                case 0:// front and back
                    v0 = corners[2] - corners[1];
                    v1 = corners[0] - corners[1];
                    faceDirection = Vector3.Cross(v0, v1);
                    faceDirection.Normalize();
                    break;
                case 1:// left and right
                    v0 = corners[5] - corners[2];
                    v1 = corners[3] - corners[2];
                    faceDirection = Vector3.Cross(v0, v1);
                    faceDirection.Normalize();
                    break;
                case 2:// top and bottom
                    v0 = corners[1] - corners[2];
                    v1 = corners[5] - corners[2];
                    faceDirection = Vector3.Cross(v0, v1);
                    faceDirection.Normalize();
                    break;
                default:
                    throw new System.Exception("Invalid index!");
                    break;
            }
            return faceDirection;
        }
        Vector3 getEdgeDirection(int index)
        {
            Vector3[] corners = GetCorners();
    
            Vector3 tmpLine;
            switch(index)
            {
                case 0:// edge with x axis
                    tmpLine = corners[5] - corners[6];
                    tmpLine.Normalize();
                    break;
                case 1:// edge with y axis
                    tmpLine = corners[7] - corners[6];
                    tmpLine.Normalize();
                    break;
                case 2:// edge with z axis
                    tmpLine = corners[1] - corners[6];
                    tmpLine.Normalize();
                    break;
                default:
                    throw new Exception("Invalid index!");
                    break;
            }
            return tmpLine;
        }
        void computeExtAxis()
        {
            _extentX = _xAxis * _extents.x;
            _extentY = _yAxis * _extents.y;
            _extentZ = _zAxis * _extents.z;
        }
        static float projectPoint(Vector3 point, Vector3 axis)
        {
            float dot = Vector3.Dot(axis, point);
            float ret = dot * point.magnitude;
            return ret;
        }
        static void getInterval(OBB box, Vector3 axis, out float min, out float max)
        {
            Vector3[] corners = box.GetCorners();
            float value;
            min = max = projectPoint(axis, corners[0]);
            for (int i = 1; i < 8; i++)
            {
                value = projectPoint(axis, corners[i]);
                min = Math.Min(min, value);
                max = Math.Max(max, value);
            }
        }

        Vector3 _center;   // obb center
        Vector3 _xAxis;    // x axis of obb, unit vector
        Vector3 _yAxis;    // y axis of obb, unit vecotr
        Vector3 _zAxis;    // z axis of obb, unit vector
        Vector3 _extents;  // obb length along each axis

        // 根据上面的数值，调用computeExtAxis计算出来的
        Vector3 _extentX;  // _xAxis * _extents.x
        Vector3 _extentY;  // _yAxis * _extents.y
        Vector3 _extentZ;  // _zAxis * _extents.z

        private OBB(Renderer r)
        {
            _center = r.transform.position;
            _xAxis = r.transform.right;
            _yAxis = r.transform.up;
            _zAxis = r.transform.forward;
            _extents = r.bounds.extents;

            computeExtAxis();
        }
                
        public static void DrawGizmos(OBB obb)
        {
#if UNITY_EDITOR
            if (obb != null)
            {
                Vector3[] corners = obb.GetCorners();

                Gizmos.DrawLine(corners[0], corners[1]);
                Gizmos.DrawLine(corners[0], corners[3]);
                Gizmos.DrawLine(corners[0], corners[7]);

                Gizmos.DrawLine(corners[2], corners[1]);
                Gizmos.DrawLine(corners[2], corners[3]);
                Gizmos.DrawLine(corners[2], corners[5]);

                Gizmos.DrawLine(corners[4], corners[3]);
                Gizmos.DrawLine(corners[4], corners[5]);
                Gizmos.DrawLine(corners[4], corners[7]);

                Gizmos.DrawLine(corners[6], corners[1]);
                Gizmos.DrawLine(corners[6], corners[5]);
                Gizmos.DrawLine(corners[6], corners[7]);
            }
#endif
        }
    }
}
