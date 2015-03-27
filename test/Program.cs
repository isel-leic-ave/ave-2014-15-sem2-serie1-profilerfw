using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestInheritance
{
    public class Program
    {
        public static void DoIt()
        {
            C c = new C();
            new A().M1(null, c);
            c.M3();
        }
    }
    public class A
    {
        public void M1(B b, C c)
        {
            Console.WriteLine("M1");
            c.M3();
        }
    }
    public class B { public void M2() { Console.WriteLine("M2"); } }
    public class C { 
        public void M3() {
            Console.WriteLine("M3");
            D.M4(); 
        } 
    }
    public class D
    {
        public static void M4() {
            Console.WriteLine("M4");
        }
        public static void M5(E e) {
            Console.WriteLine("M5");
        }
    }
    public class E { 
        public static void M6() {
            Console.WriteLine("M6");
        } 
    }
}
