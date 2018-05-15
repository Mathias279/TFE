using System;
using System.Collections.Generic;
using System.Text;

namespace TFE2017.Core.Service
{
    public class MyLogger
    {
        public static void WriteLine(string s)
        {

            Console.WriteLine("+++++++++++++++");
            Console.WriteLine("+" + s);
            Console.WriteLine("+++++++++++++++");
        }
    }
}
