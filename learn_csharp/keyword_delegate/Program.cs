using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace keyword_delegate
{
    class Program
    {
        // 相当于c++中的 typedef int (*Func)(int a, int b);
        // 现在Func相当于一个类（跟class A本质上是相同的），只不过他是一个函数指针
        delegate int Func(int a, int b);
        void ShowResult(int a, int b, Func func)
        {
            Console.WriteLine("show result:" + func(a, b));
        }
        int Add(int a, int b)
        {
            return a + b;
        }
        static int Multiply(int a, int b)
        {
            return a * b;
        }

        // 泛型模板
        delegate T2 Func2<T1, T2>(T1 a, T1 b);
        static void ShowResult_Float(float a)
        {
            Console.WriteLine("show float result:" + a);
        }
        static void ShowResult2<T>(T a)
        {
            Console.WriteLine("show float result:" + a);
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            p.ShowResult(1, 2, p.Add);
            p.ShowResult(1, 2, Multiply);

            // 匿名的delegate函数
            p.ShowResult(1, 2, 
                delegate(int a, int b)
                {
                    return (a - b);
                }
            );

            /*
             * lambda表达式，语法规则
             *      一个参数： param => exp
             *      多个参数：(param-list) => exp
             *      exp 必须是一个表达式（运算符表达式），不能包含“,”。exp得用"{}"或者"()"包裹起来。
             */
            Func subtract = (a, b) => (a - b);
            p.ShowResult(1, 2, subtract);
            p.ShowResult(1, 2, (a, b) => (a - b));
            int xx = 3, yy = 4;
            int cc = xx + yy + new Random().Next(0);
            p.ShowResult(1, 2, (a, b) => (a - b + cc));

            Func2<int, float> exp = (a, b) => (a + b);
            ShowResult_Float(exp(1,2));

            Func2<int, string> exp2 = (a,b)=>(a+b+"--返回结果是string！");
            ShowResult2(exp2(2,2));

            Func2<bool, int> exp3 = (a, b) => { if (a) { return 1; } else { return 2; } };
            ShowResult2(exp3(true, false));
            

            Console.WriteLine("The End");
        }
    }
}
