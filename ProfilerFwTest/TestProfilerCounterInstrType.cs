using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection.Emit;
using profilerfw;

namespace ProfilerFwTest
{
    public class A
    {
        public virtual void Foo()
        {
            Console.WriteLine("Foo");
            for (int i = 0; i < 3; i++)
            {
                this.Bar();
            }
        }
        public virtual void Bar()
        {
            Console.WriteLine("Bar");
        }
    }

    [TestClass]
    public class TestProfilerCounterInstrType
    {
        [TestMethod]
        public void test_type_instrumentation_with_counting_calls()
        {
            //
            // Create Assembly Builder
            //
            ModuleBuilder mb;
            AssemblyBuilder asmBuilder = Profiler.CreateAsmBuilderDll("InstrA", out mb);
            // 
            // Create Type Builder
            //
            TypeBuilder tb = mb.DefineType("InstrA", typeof(A).Attributes);
            // 
            // Act
            //
            Type instrType = ProfilerCounterInstrType.BuildInstrType(typeof(A), tb);
            asmBuilder.Save("InstrA.dll");
            A a = (A) Activator.CreateInstance(instrType);
            a.Foo();
            // 
            // Assert
            //
            var res = ProfilerCounter.Report();
            Assert.AreEqual(1, res[typeof(A).GetMethod("Foo")]);
            Assert.AreEqual(3, res[typeof(A).GetMethod("Bar")]);
        }
    }
}
