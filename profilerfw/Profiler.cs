using Mono.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace profilerfw
{
    public class Profiler
    {

        private const string ASSEMBLY_PREFIX = "Instr";
        private const string TYPE_PREFIX = "Instr";

        private InstructionParser instrParser;

        public Profiler()
        {
            instrParser = new InstructionParser();
        }

        /// <summary>
        /// Considering that the method to instrument is not overloaded
        /// and has no arguments.
        /// </summary>
        public void ToEntryPoint(Type src, string methodName)
        {
            //
            // Create Assembly Builder
            //
            string asmName = ASSEMBLY_PREFIX + src.Name;
            ModuleBuilder mb;
            AssemblyBuilder asmBuilder = CreateAsmBuilderExe(asmName, out mb);
            // 
            // Create Type Builder
            //
            TypeBuilder tb = mb.DefineType(TYPE_PREFIX + src.Name, src.Attributes);
            //
            // Emit Ctor and Entry Method 
            //
            EmitParameterLessCtor(tb);
            MethodBuilder destMethod = EmitEntryMethod(src.GetMethod(methodName), tb);
            asmBuilder.SetEntryPoint(destMethod);
            //
            // Finish the type.
            //
            Type t = tb.CreateType();
            asmBuilder.Save(asmName + ".exe");
        }

        public static AssemblyBuilder CreateAsmBuilderExe(string name, out ModuleBuilder mb)
        {
            return CreateAsmBuilder(name, out mb, ".exe");
        }

        public static AssemblyBuilder CreateAsmBuilderDll(string name, out ModuleBuilder mb)
        {
            return CreateAsmBuilder(name, out mb, ".dll");
        }

        private static AssemblyBuilder CreateAsmBuilder(string name, out ModuleBuilder mb, string suffix)
        {
            AssemblyName asmName = new AssemblyName(name);
            AssemblyBuilder asmBuilder =
                AppDomain.CurrentDomain.DefineDynamicAssembly(
                    asmName,
                    AssemblyBuilderAccess.RunAndSave);
            //
            // For a single-module assembly, the module name is usually 
            // the assembly name plus an extension.
            //
            mb = asmBuilder.DefineDynamicModule(asmName.Name, asmName.Name + suffix);
            return asmBuilder;
        }

        public MethodBuilder EmitEntryMethod(MethodInfo srcMethod, TypeBuilder tb)
        {
            //
            // Emit Method
            //
            MethodBuilder mainBuilder = tb.DefineMethod(srcMethod.Name, srcMethod.Attributes, typeof(void), new Type[0]);
            ILGenerator mainMethod = mainBuilder.GetILGenerator();
            //
            // Declare Local Variables
            //
            foreach (LocalVariableInfo var in srcMethod.GetMethodBody().LocalVariables)
            {
                mainMethod.DeclareLocal(var.LocalType);
            }
            //
            // Copy every instruction from srcMethod to the destination mainMethod
            //
            foreach (Instruction instr in Disassembler.GetInstructions(srcMethod))
            {
                instrParser.CopyTo(instr, mainMethod);
            }
            return mainBuilder;
        }

        private void EmitParameterLessCtor(TypeBuilder tb)
        {
            //
            // Emit Constructor
            //
            ConstructorBuilder ctor1 = tb.DefineConstructor(
                MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
            ILGenerator ctor1IL = ctor1.GetILGenerator();
            //
            // For a constructor, argument zero is a reference to the new 
            // instance. Push it on the stack before calling the base 
            // class constructor. Specify the default constructor of the  
            // base class (System.Object) by passing an empty array of  
            // types (Type.EmptyTypes) to GetConstructor.
            //
            ctor1IL.Emit(OpCodes.Ldarg_0);
            ctor1IL.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
            ctor1IL.Emit(OpCodes.Ret);
        }

        internal void ReplaceEmmitter(Type type, IEmitter emitter)
        {
            instrParser.ReplaceEmmitter(type, emitter);
        }
    }
}
