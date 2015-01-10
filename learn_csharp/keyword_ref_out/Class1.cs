using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace keyword_ref_out
{
    public class Class1
    {
        int a = 0;
        Class1 func(Class1 c1)
        {
            c1 = new Class1();
            c1.a = 2;
            return c1;
        }
        Class1 fun1(ref Class1 c1)
        {
            //c1 = new Class1();
            c1.a = 2;
            return c1;
        }
        Class1 func2(out Class1 c1)
        {
            c1 = new Class1();
            c1.a = 2;
            return c1;
        }
        Class1 fun3(ref Class1 c1)
        {
            c1 = new Class1();
            c1.a = 2;
            return c1;
        }
        void Show()
        {
            Console.WriteLine("------------- a=" + a);
        }

        static void ASSERT(bool exp)
        {
            if (exp)
            {
                Console.WriteLine("----------- assert true!");
            }
            else
            {
                Console.WriteLine("----------- assert false!");
            }
        }

        static void Main()
        {
            Class1 c1 = new Class1();
            Class1 c2 = null;

            c1.a = 1;
            ASSERT(c1 == c1.func(c1));
            c1.Show();

            c1.a = 1;
            c2 = c1;
            ASSERT(c1 == c1.fun1(ref c1));
            c1.Show();

            c1.a = 1;
            ASSERT(c1 == c1.func2(out c1));
            c1.Show();

            c1.a = 1;
            ASSERT(c1 == c1.func2(out c1));
            c1.Show();
            c2.Show();

        }


    }
}
