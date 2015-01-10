using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace keyword_grandpa
{
    class A
    {
        virtual public void Show()
        {
            Console.WriteLine("A");
        }
    }
    class B : A
    {
        override public void Show()
        {
            Console.WriteLine("B");
        }
        public void Show1()
        {
            base.Show();
        }
    }
    class C : B
    {
        override public void Show()
        {
            Show1();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            C c = new C();
            c.Show();
        }
    }
}
