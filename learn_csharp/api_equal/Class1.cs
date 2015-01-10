using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api_equal
{
    public class Class1
    {

        public class BundleBuildState
        {
            public string bundleName = "";
            public int version = -1;
            public long size = -1;
            public long changeTime = -1;
            public string[] lastBuildDependencies = null;
        }


        static void Main(string[] args)
        {
            string a = "1234";
            string b = "123" + "4";
            Console.WriteLine("a==b?" + (a == b));
            Console.WriteLine("a equal b?" + (a.Equals(b)));




            Dictionary<string, BundleBuildState> statesDict_Saved = new Dictionary<string, BundleBuildState>();
            statesDict_Saved.Add("1", new BundleBuildState());
            statesDict_Saved["1"].lastBuildDependencies = new string[1] { "xxxxxx" };
            
            Dictionary<string, BundleBuildState> statesDict_Saved2 = new Dictionary<string, BundleBuildState>();
            foreach (var dic in statesDict_Saved)
            {
                statesDict_Saved2.Add(dic.Key, dic.Value);
            }
            statesDict_Saved.Clear();
            Console.WriteLine("xxxxxxxx:" + statesDict_Saved2["1"].lastBuildDependencies[0]);







        }
    }
}
