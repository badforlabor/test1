
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
    }
    class Quaternion
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public Quaternion()
        {
            X = Y = Z = W = 0;
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
            return new Quaternion();
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


        


    }
}
