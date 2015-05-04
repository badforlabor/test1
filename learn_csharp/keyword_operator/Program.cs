
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
    class Vector3
    {
        public float X;
        public float Y;
        public float Z;
        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public float Magnitude
        {
            get { return (float)Math.Sqrt(X*X + Y*Y + Z*Z); }
        }
        public static Vector3 Zero
        {
            get { return new Vector3(0, 0, 0); }
        }
        public static Vector3 operator *(Vector3 p, float q)
        {
            return new Vector3(p.X*q, p.Y*q, p.Z*q);
        }
        public static Vector3 operator *(float q, Vector3 p)
        {
            return p*q;
        }
        public static Vector3 operator /(Vector3 p, float q)
        {
            return p * (1 / q);
        }
        public static float operator *(Vector3 p, Vector3 q)
        {
            return (p.X * q.X + p.Y * q.Y + p.Z * q.Z);
        }
        public static Vector3 Cross(Vector3 p, Vector3 q)
        {
            return new Vector3(p.Y*q.Z - p.Z*p.Y, p.Z*q.X - p.X*q.Z, p.X*q.Y-p.Y*q.X);
        }
        public static Vector3 operator +(Vector3 p, Vector3 q)
        {
            return new Vector3(p.X + q.X, p.Y + q.Y, p.Z + q.Z);
        }
    }

    /*
     * 参考：3D数学基础：图形与游戏开发，
     * 单位四元数：w*w+x*x+y*y+z*z = 1，类似于单位向量公式
     * 四元数可以写成这种形式[q,v]，其中v代表旋转轴的方向。
     *      假如绕单位向量u旋转角度a，可以用这个四元数表示：[cos(a/2), sin(a/2)u]
     * 如此可以用四元数代替旋转矩阵，矩阵式9位，这个是4位省内存。二是矩阵还可以用来平移和缩放，有点重。四元数只用来旋转！
     * **/
    class Quaternion
    {
        // w的取值范围一定是[-1, 1]，因为他表示的是角度
        public float W;
        public float X;
        public float Y;
        public float Z;

        public Quaternion()
        {
            X = Y = Z = W = 0;
        }
        public Quaternion(Vector3 v, float w)
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


        /*
         * 基础数学
         * **/
        public static float Sin(float a)
        {
            return (float)Math.Sin(a);
        }
        public static float Cos(float a)
        {
            return (float)Math.Cos(a);
        }
        public static float Sqrt(float a)
        {
            return Sqrt(a);
        }
        public static float Acos(float a)
        {
            return (float)Math.Acos(a);
        }
        public static float Asin(float a)
        {
            return (float)Math.Asin(a);
        }
        public static float Atan2(float y, float x)
        {
            return (float)Math.Atan2(y, x);
        }
        public static float RadiusToDegree(float d)
        {
            return d * 180 / (float)Math.PI;
        }
        public static float DegreeToRadius(float d)
        {
            return (float)Math.PI * d / 180;
        }


        // 模
        public float Magnitude()
        {
            return Sqrt(X * X + Y * Y + Z * Z + W * W);
        }
        // 单位四元数
        public static Quaternion Identity
        { 
            get{return new Quaternion(Vector3.Zero, 1);}
        }
        // 正规化
        public void Normalize()
        {
            float mag = Magnitude();
            if (mag == 0)
            {
                W = 1;
                X = 0;
                Y = 0;
                Z = 0;
            }
            else
            {
                W /= mag;
                X /= mag;
                Y /= mag;
                Z /= mag;
            }
        }

        // 获得旋转轴
        public Vector3 GetRotateAxis()
        {
            // 参见QRotateFromAxis, X = Axis.x * sin(theta/2)，所以 Axis.x = X / sqrt(1-Cos(theta/2)*Cos(theta)) = X / sqrt(1-W*W)
            return new Vector3(X, Y, Z) * (1 / Sqrt(1-W*W));
        }
        // 获得角度值，比如30度
        public float GetRotateAngle()
        {
            // w = Cos(theta/2)
            return Acos(W) * 2;
        }

        // 生成四元数：
        // 这个四元数的意义是：绕x轴旋转theta角度。举个粒子，如果V * Q=将V绕x轴旋转theta角度
        public static Quaternion QFromRotateX(float theta)
        {
            theta *= 0.5f;
            return new Quaternion(Sin(theta), 0, 0, Cos(theta));
        }
        public static Quaternion QFromRotateY(float theta)
        {
            theta *= 0.5f;
            return new Quaternion(0, Sin(theta), 0, Cos(theta));
        }
        public static Quaternion QFromRotateZ(float theta)
        {
            theta *= 0.5f;
            return new Quaternion(0, 0, Sin(theta), Cos(theta));
        }
        // 举个粒子：QFromRotateX = QFromRotateAxis(new Vector3(1,0,0), theta)，明白否！
        public static Quaternion QFromRotateAxis(Vector3 axis, float theta)
        {
            // axis是格式化之后的（normalize）
            theta *= 0.5f;
            return new Quaternion(axis.X * Sin(theta), axis.Y * Sin(theta), axis.Z * Sin(theta), Cos(theta));
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
            return p.Conjugate();
        }
        // 共轭
        public Quaternion Conjugate()
        {
            return new Quaternion(-X, -Y, -Z, W);
        }
        // 逆 = 共轭 / 模
        public Quaternion Inverse()
        {
            return Conjugate() / Magnitude();
        }
        // 幂，加入q绕x轴转了30度，p绕x轴转了60度，那么p = q2（2次幂），同样p1/3（三分之一次幂）表示绕x轴转了20度
        //      四元数的幂表示的意义和自然数的幂一样，都是倍数的意思。
        public Quaternion Power(float exponent)
        {
            Quaternion ret = new Quaternion(X, Y, Z, W);
            if (Math.Abs(W) < 1)
            {
                float alpha = Acos(ret.W);
                float newalpha = alpha * exponent;
                ret.W = Cos(newalpha);
                float mult = Sin(newalpha) / Sin(alpha);
                ret.X *= mult;
                ret.Y *= mult;
                ret.Z *= mult;
            }
            else
            {
                // 单位四元数的N次幂依然是单位的
                return Identity;
            }
            return ret;
        }
        // 差值lerp
        // 这个可以分为三步：计算p和q的差，在取差上的一部分delta，在p上加上delta。
        public static Quaternion Lerp(Quaternion p, Quaternion q, float t)
        {
            return p * ((p.Inverse() * q).Power(t));
        }



        // 四元数相乘：qp = nq*np - vq*vp + nqvp + npvq + (vq X vp);
        // 类似于矩阵相乘，表示的意义是：连续旋转
        public static Quaternion operator *(Quaternion q, Quaternion p)
        {
            return new Quaternion(q.W*p.GetRotateAxis() + p.W*q.GetRotateAxis() + Vector3.Cross(p.GetRotateAxis(), q.GetRotateAxis()),
                    q.W*p.W - q.GetRotateAxis()*p.GetRotateAxis());
        }
        public static Quaternion operator *(Quaternion p, Vector3 q)
        {
            return p * new Quaternion(q, 0);
        }
        public static Quaternion operator *(Vector3 p, Quaternion q)
        {
            return q * p;
        }

        // 绕向量(x,y,z)的旋转角度
        public float QGetAngle()
        {
            return 2 * Cos(W);
        }

        // 获得以向量部分为旋转轴的单位向量
        public Vector3 QGetAxis()
        {
            Vector3 v = GetRotateAxis();
            float mag = v.Magnitude;
            if (mag <= 0)
            {
                return Vector3.Zero;
            }
            else
            {
                return v / mag;
            }
        }

        // q绕p旋转后得到的值：q' = 逆p * q * p = 共轭p * q * p
        public static Quaternion QRotate(Quaternion q, Quaternion p)
        {
            return p.Inverse() * q * p;
        }
        // 向量v（或者点v）绕p旋转后得到的值
        public static Vector3 QVRotate(Vector3 v, Quaternion p)
        {
            Quaternion q = p * v * ~p;
            return q.GetRotateAxis();
        }

        // 根据欧拉角计算出四元数。
        // 这个构建方法和矩阵类似，如果是旋转矩阵，那么可以拆成绕x轴的，绕y轴的，绕z轴的，然后三个矩阵相乘即可
        //      同样，四元数也一样：q = Px * Py * Pz
        public static Quaternion MakeQFromEulerAngles(float x, float y, float z)
        {
            Quaternion q = new Quaternion();

            float roll = DegreeToRadius(x);
            float pitch = DegreeToRadius(y);
            float yaw = DegreeToRadius(z);

            float cyaw = Cos(0.5f * yaw);
            float syaw = Sin(0.5f * yaw);
            float cpitch = Cos(0.5f * pitch);
            float spitch = Sin(0.5f * pitch);
            float croll = Cos(0.5f * roll);
            float sroll = Sin(0.5f * roll);

            q.W = croll * cpitch * cyaw + sroll * spitch * syaw;
            q.X = sroll * cpitch * cyaw - croll * spitch * syaw;
            q.Y = croll * spitch * cyaw + sroll * cpitch * syaw;
            q.X = croll * cpitch * syaw - sroll * spitch * cyaw;

            return q;
        }

        // 获取欧拉角（就是绕x轴旋转角度，绕y轴旋转角度，绕z轴旋转角度）
        // 如果给你一个矩阵，你可以推导出来旋转角度，同样的计算方式作用在四元数上即可算出来四元数的各个旋转角度
        public static Vector3 MakeEulerAnglesFromQ(Quaternion q)
        { 
            // 先转为矩阵：
            float r11 = q.W * q.W + q.X * q.X - q.Y * q.Y - q.Z * q.Z;
            float r12 = 2 * q.X * q.Y - 2 * q.Z * q.W;
            float r13 = 2 * q.X * q.Z + 2 * q.Y * q.W;
            float r21 = 2 * q.X * q.Y + 2 * q.Z * q.W;
            float r22 = q.W * q.W - q.X * q.X + q.Y * q.Y - q.Z * q.Z;
            float r23 = 2 * q.Z * q.Y - 2 * q.X * q.W;
            float r31 = 2 * q.Z * q.X - 2 * q.Y * q.W;
            float r32 = 2 * q.Z * q.Y + 2 * q.X * q.W;
            float r33 = q.W * q.W - q.X * q.X - q.Y * q.Y + q.Z * q.Z;

            /*
             * 从旋转矩阵中，我们知道tanX = r21/r11, tanY = r32/r33, sinZ = -r31
             * */
            float tmp = Math.Abs(r31);
            Vector3 ret = Vector3.Zero;
            if (tmp > 0.999999)
            {
                ret.X = RadiusToDegree(0);
                ret.Y = RadiusToDegree(-((float)Math.PI / 2) * r31 / tmp);
                ret.Z = RadiusToDegree(Atan2(-r12, -r31 * r13));
            }
            else
            {
                ret.X = RadiusToDegree(Atan2(r32, r33));
                ret.Y = RadiusToDegree(Asin(-r31));
                ret.Z = RadiusToDegree(Atan2(r21, r11));
            }
            return ret;
        }
        
        // 实现unity的Quaternion
        public static float Angle(Quaternion a, Quaternion b)
        {
            return 0;
        }
        public static Quaternion AngleAxis(float angle, Vector3 axis)
        {
            return new Quaternion(axis, angle);
        }
        public static float Dot(Quaternion a, Quaternion b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;
        }
        // 通过欧拉角构建四元数
        public static Quaternion Euler(float x, float y, float z)
        {
            return MakeQFromEulerAngles(x, y, z);
        }
        // 角位移：deltaQ = 逆Q * P（从q到p的角位移）
        public static Quaternion FromToRotation(Vector3 from, Vector3 to)
        {
            return new Quaternion(from, 0).Inverse() * new Quaternion(to, 0); 
        }
        // 四元数的逆
        public static Quaternion Inverse(Quaternion p)
        {
            return p.Conjugate() / p.Magnitude();
        }
        //public static Quaternion Lerp(Quaternion from, Quaternion to, float time)
        //{
        //    return from;
        //}
        public static Quaternion LookRotation(Vector3 forward, Vector3 upward)
        {
            return new Quaternion();
        }
        public static Quaternion RotateTowards(Quaternion from, Quaternion to, float maxDegreesDelta)
        {
            return from;
        }
        public static Quaternion Slerp(Quaternion from, Quaternion to, float t)
        {
            return from;
        }
        
    }
}
