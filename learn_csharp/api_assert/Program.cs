using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace api_assert
{

    class AssertManager
    {
        public static void Assert(bool exp)
        {
            Debug.Assert(exp);
        }
        public static void Assert2(bool exp)
        {
            if (exp)
            {

            }
            else
            {
                Console.WriteLine(Environment.StackTrace);
            }
        }

        public static void Failed(string exp)
        {
            Debug.Fail(exp);
        }
    }

    class Program
    {

        static void test1()
        {
            //AssertManager.Assert(false);
            AssertManager.Assert2(false);
        }

        static void Main(string[] args)
        {
            //Debug.Fail("wo cao!");

            //Debug.Assert(false, "wo cao!");

            test1();

            Console.WriteLine("The End");
        }
    }
}
