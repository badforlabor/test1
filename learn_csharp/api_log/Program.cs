using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api_log
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] a = new int[] { 1, 2, 3, 4 };
            Console.WriteLine("{0}", a.ToString());

            string abc = "xxxxxx" + "a";
            Console.WriteLine(abc);

            string d = "autoload/sys_lang_uicsv";
            //d = d.Substring(0, d.Length - (d.LastIndexOf(".") + 1));
            Console.WriteLine(d.Substring(0, (d.LastIndexOf("."))));


        }
    }
}
