using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profilerfw
{
    class Program
    {
        static void Main(string[] args)
        {
            new Profiler().ToEntryPoint(typeof(TestInheritance.Program), "DoIt");
            // new Profiler().ToEntryPoint(typeof(TestRestSharp.App), "DoRequest");
        }
    }
}
