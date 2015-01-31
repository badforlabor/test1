
/*
 * 参考：http://www.cnblogs.com/LilianChen/archive/2013/03/15/2961901.html
 * 重载运算符，以四元数为例子
 * 运算符的关键字是：operator，必须是public static类型的函数！
 *      +=、-=等类似的运算符会隐式重载，不需要自己定义
 *      true和false运算符必须成对出现
 *      ==和!=、<和>、<=和>= 也必须成对出现
 *      
 * **/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace keyword_operator
{
    class Program
    {
        static void Main(string[] args)
        {
            Quaternion a = new Quaternion();
            Quaternion b = new Quaternion();

            a += b;

            Quaternion p = new Quaternion(1, 1, 1, 2);
            p = p * (~p);

        }
    }
    class Vector
    {
        public float X;
        public float Y;
        public float Z;
        public Vector(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public float Magnitude
        {
            get { return (float)Math.Sqrt(X*X + Y*Y + Z*Z); }
        }
        public static Vector Zero
        {
            get { return new Vector(0, 0, 0); }
        }
        public static Vector operator *(Vector p, float q)
        {
            return new Vector(p.X*q, p.Y*q, p.Z*q);
        }
        public static Vector operator *(float q, Vector p)
        {
            return p*q;
        }
        public static Vector operator /(Vector p, float q)
        {
            return p * (1 / q);
        }
        public static float operator *(Vector p, Vector q)
        {
            return (p.X * q.X + p.Y * q.Y + p.Z * q.Z);
        }
        public static Vector Cross(Vector p, Vector q)
        {
            return new Vector(p.Y*q.Z - p.Z*p.Y, p.Z*q.X - p.X*q.Z, p.X*q.Y-p.Y*q.X);
        }
        public static Vector operator +(Vector p, Vector q)
        {
            return new Vector(p.X + q.X, p.Y + q.Y, p.Z + q.Z);
        }
    }

    /*
     * 单位四元数：w*w+x*x+y*y+z*z = 1，类似于单位向量公式
     * 四元数可以写成这种形式[q,v]，其中v代表旋转轴的方向。
     *      假如绕单位向量u旋转角度a，可以用这个四元数表示：[cos(a/2), sin(a/2)u]
     * 如此可以用四元数代替旋转矩阵，矩阵式9位，这个是4位省内存。二是矩阵还可以用来平移和缩放，有点重。四元数只用来旋转！
     * **/
    class Quaternion
    {
        public float W;
        public float X;
        public float Y;
        public float Z;

        public Quaternion()
        {
            X = Y = Z = W = 0;
        }
        public Quaternion(float w, Vector v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = w;
        }
        public Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public float Magnitude()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
        }
        public Vector GetVector()
        {
            return new Vector(X, Y, Z);
        }
        public float GetScalar()
        {
            return W;
        }
        // 负运算符
        public static Quaternion operator -(Quaternion p)
        {
            return new Quaternion(-p.X, -p.Y, -p.Z, -p.W);
        }
        // 加法运算符，重载此运算符之后，系统会默认隐式重载+=运算符
        public static Quaternion operator +(Quaternion p, Quaternion q)
        {
            return new Quaternion(p.X+q.X, p.Y+q.Y, p.Z+p.Z, p.W+q.W);
        }
        // 减法运算符
        public static Quaternion operator-(Quaternion p, Quaternion q)
        {
            return p+(-q);
        }
        public static Quaternion operator *(Quaternion p, float q)
        {
            return new Quaternion(p.X * q, p.Y * q, p.Z * q, p.W * q);
        }
        public static Quaternion operator /(Quaternion p, float q)
        {
            return p * (1/q);
        }
        // 四元数的共轭
        public static Quaternion operator~(Quaternion p)
        {
            return new Quaternion(-p.X, -p.Y, -p.Z, p.W);
        }

        // 四元数相乘：qp = nq*np - vq*vp + nqvp + npvq + (vq X vp);
        public static Quaternion operator *(Quaternion q, Quaternion p)
        {
            return new Quaternion(q.W*p.W - q.GetVector()*p.GetVector(),
                            q.W*p.GetVector() + p.W*q.GetVector() + Vector.Cross(p.GetVector(), q.GetVector()));
        }
        public static Quaternion operator *(Quaternion p, Vector q)
        {
            return p * new Quaternion(0, q);
        }
        public static Quaternion operator *(Vector p, Quaternion q)
        {
            return q * p;
        }

        // 绕向量(x,y,z)的旋转角度
        public float QGetAngle()
        {
            return 2 * (float)Math.Cos(W);
        }

        // 获得以向量部分为旋转轴的单位向量
        public Vector QGetAxis()
        {
            Vector v = GetVector();
            float mag = v.Magnitude;
            if (mag <= 0)
            {
                return Vector.Zero;
            }
            else
            {
                return v / mag;
            }
        }

        // q绕p旋转后得到的值
        public static Quaternion QRotate(Quaternion q, Quaternion p)
        {
            return p * q * ~(p);
        }
        // 向量v（或者点v）绕p旋转后得到的值
        public static Vector QVRotate(Vector v, Quaternion p)
        {
            Quaternion q = p * v * ~p;
            return q.GetVector();
        }

        // 根据欧拉角计算出四元数。
        public static float DegreeToRadius(float d)
        {
            return (float)Math.PI * d / 180;
        }
        public static Quaternion MakeQFromEulerAngles(float x, float y, float z)
        {
            Quaternion q = new Quaternion();

            float roll = DegreeToRadius(x);
            float pitch = DegreeToRadius(y);
            float yaw = DegreeToRadius(z);

            float cyaw = (float)Math.Cos(0.5 * yaw);
            float syaw = (float)Math.Sin(0.5 * yaw);
            float cpitch = (float)Math.Cos(0.5 * pitch);
            float spitch = (float)Math.Sin(0.5 * pitch);
            float croll = (float)Math.Cos(0.5 * roll);
            float sroll = (float)Math.Sin(0.5 * roll);

            q.W = croll * cpitch * cyaw + sroll * spitch * syaw;
            q.X = sroll * cpitch * cyaw - croll * spitch * syaw;
            q.Y = croll * spitch * cyaw + sroll * cpitch * syaw;
            q.X = croll * cpitch * syaw - sroll * spitch * cyaw;

            return q;
        }

        // 获取欧拉角（就是绕x轴旋转角度，绕y轴旋转角度，绕z轴旋转角度）
        // 如果给你一个矩阵，你可以推导出来旋转角度，同样的计算方式作用在四元数上即可算出来四元数的各个旋转角度
        public static float RadiusToDegree(float d)
        {
            return d * 180 / (float)Math.PI;
        }
        public static Vector MakeEulerAnglesFromQ(Quaternion q)
        { 
            // 先转为矩阵：
            float r11 = q.W * q.W + q.X * q.X - q.Y * q.Y - q.Z * q.Z;
            float r12 = 2 * q.X * q.Y - 2 * q.Z * q.W;
            float r13 = 2 * q.X * q.Z + 2 * q.Y * q.W;
            float r21 = 2 * q.X * q.Y + 2 * q.Z * q.W;
            float r22 = q.W * q.W - q.X * q.X + q.Y * q.Y - q.Z * q.Z;
            //float r23 = 
        }
        


    }
}
