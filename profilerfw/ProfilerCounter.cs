using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace profilerfw
{
    public class ProfilerCounter
    {
        private const string ASSEMBLY_PREFIX = "Instr";
        private const string TYPE_PREFIX = "Instr";

        readonly Profiler prof;

        public ProfilerCounter(Profiler prof)
        {
            this.prof = prof;
        }

        public Type ToEntryPointCounter(Type src, string methodName)
        {
            string asmName = ASSEMBLY_PREFIX + src.Name;
            string typeName = TYPE_PREFIX + src.Name;
            //
            // Create Assembly Builder
            //
            ModuleBuilder mb;
            AssemblyBuilder asmBuilder = Profiler.CreateAsmBuilderExe(asmName, out mb);
            // 
            // Create Type Builder
            //
            TypeBuilder tb = mb.DefineType(typeName, src.Attributes);
            //
            // Emit Entry Method 
            //
            MethodBuilder destMethod = prof.EmitEntryMethod(src.GetMethod(methodName), tb);
            asmBuilder.SetEntryPoint(destMethod);
            //
            // Finish the type.
            //
            Type t = tb.CreateType();
            asmBuilder.Save(asmName + ".exe");
            return t;
        }

        public static void PrintReport()
        {
            throw new NotImplementedException();
        }

        public static Dictionary<MethodInfo, int> Report()
        {
            throw new NotImplementedException();
        }

    }
}
