using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace api_filesystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // 创建递归目录，相对路径目录
            Directory.CreateDirectory("c:\\1\\2\\3\\");
            Directory.CreateDirectory("1\\2\\3");


            Console.WriteLine("The End");
            return;
        }
    }
}
