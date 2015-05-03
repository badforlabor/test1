using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api_interface
{
    class Base
    {
        virtual public void ShowMe()
        {
            Console.WriteLine("base");
        }
    }
    class A : Base
    {
        override public void ShowMe()
        {
            Console.WriteLine("A");
        }
    }
    class B : A
    {
        override public void ShowMe()
        {
            base.ShowMe();
            //((Base)this).ShowMe();
            //Base.ShowMe();
            Console.WriteLine("B");
        }
        //void ShowMe()
        //{
        //    Console.WriteLine("BB");
        //}
    }


    interface IBase
    {
        //static int ID { get; }
        int ID { get; }
    }
    class IA : IBase 
    {
        public int ID { get { return 1; } }
    }
    class IB : IBase
    {
        public int ID { get { return 2; } }
    }

    public class Class1
    {

        static void Main(string[] args)
        {
            B b = new B();
            b.ShowMe();

            //Console.WriteLine("" + ((IB)null).ID);

            IB ib = new IB();
            Console.WriteLine(""+ib.ID);

            IBase ibb = null;
            IB ibb2 = ibb as IB;

            Console.WriteLine("" + (ibb2 == null ? "" : "1"));
        }
    }
}
