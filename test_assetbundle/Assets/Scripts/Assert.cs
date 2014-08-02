using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    class DebugTools
    {
        public static void Assert(bool exp)
        {
            if (!exp)
            {
                throw new Exception();
            }
        }

    }
