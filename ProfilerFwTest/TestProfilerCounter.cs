using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using profilerfw;
using System.Reflection;
using RestSharp;

namespace ProfilerFwTest
{
    [TestClass]
    public class TestProfilerCounter
    {
        [TestMethod]
        public void test_inheritance_program()
        {
            //
            // Arrange
            //
            ProfilerCounter prof = new ProfilerCounter(new Profiler());
            Type instrKlass = prof.ToEntryPointCounter(typeof(TestInheritance.Program), "DoIt");
            //
            // Act
            //
            MethodInfo instrMethod = instrKlass.GetMethod("DoIt");
            instrMethod.Invoke(null, null); // static mehod without arguments
            var results = ProfilerCounter.Report();
            //
            // Assert
            //
            Assert.AreEqual(2, results[typeof(TestInheritance.C).GetMethod("M3")]);
            Assert.AreEqual(1, results[typeof(TestInheritance.A).GetMethod("M1")]);
            Assert.AreEqual(1, results[typeof(object).GetMethod("ToString")]);
        }

        [TestMethod]
        public void test_restsharp_app()
        {
            //
            // Arrange
            //
            ProfilerCounter prof = new ProfilerCounter(new Profiler());
            Type instrKlass = prof.ToEntryPointCounter(typeof(TestRestSharp.App), "DoRequest");
            //
            // Act
            //
            MethodInfo instrMethod = instrKlass.GetMethod("DoRequest");
            instrMethod.Invoke(null, null); // static mehod without arguments
            var results = ProfilerCounter.Report();
            //
            // Assert
            //
            Assert.AreEqual(2, results[typeof(RestClient).GetMethod("set_BaseUrl", new Type[]{typeof(Uri)})]);
            Assert.AreEqual(5, results[typeof(RestClient).GetMethod("get_BaseUrl")]);
            Assert.AreEqual(1, results[typeof(RestClient).GetMethods().First(m => m.Name.Equals("Execute"))]);
        }
    }
}
