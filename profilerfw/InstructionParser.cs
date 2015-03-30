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
    public class InstructionParser
    {

        private readonly Dictionary<Type, IEmitter> emitters = new Dictionary<Type, IEmitter>();
        private class EmptyKey { }

        public InstructionParser()
        {
            emitters.Add(typeof(EmptyKey), new EmitterNoArgs());
            emitters.Add(typeof(ConstructorInfo), new EmitterCtor());
            emitters.Add(typeof(MethodInfo), new EmitterMethod());
            emitters.Add(typeof(byte), new EmitterByte());
            emitters.Add(typeof(double), new EmitterDouble());
            emitters.Add(typeof(short), new EmitterShort());
            emitters.Add(typeof(int), new EmitterInt());
            emitters.Add(typeof(long), new EmitterLong());
            emitters.Add(typeof(Label), new EmitterLabel());
            emitters.Add(typeof(Label[]), new EmitterLabelArray());
            emitters.Add(typeof(LocalBuilder), new EmitterLocalBuilder());
            emitters.Add(typeof(SignatureHelper), new EmitterSignHelper());
            emitters.Add(typeof(FieldInfo), new EmitterFieldInfo());
            emitters.Add(typeof(SByte), new EmitterSByte());
            emitters.Add(typeof(Single), new EmitterSingle());
            emitters.Add(typeof(String), new EmitterString());
            emitters.Add(typeof(Type), new EmitterType());
        }

        public void ReplaceEmmitter(Type operandType, IEmitter value)
        {
            emitters[operandType] = value;
        }

        internal void CopyTo(Instruction instr, ILGenerator destMethod)
        {
            Type operandType = instr.Operand == null ? typeof(EmptyKey) : instr.Operand.GetType();
            foreach (var item in emitters)
            {
                if (item.Key.IsAssignableFrom(operandType)) { 
                    item.Value.Emit(instr, destMethod);
                    break;
                }
            }
        }

        private class EmitterNoArgs : IEmitter
        {
            public void Emit(Instruction instr, ILGenerator destMethod)
            {
                destMethod.Emit(instr.OpCode);
            }
        }

        private class EmitterCtor : IEmitter
        {
            public void Emit(Instruction instr, ILGenerator destMethod)
            {
                destMethod.Emit(instr.OpCode, (ConstructorInfo)instr.Operand);
            }
        }

        private class EmitterMethod : IEmitter
        {
            public void Emit(Instruction instr, ILGenerator destMethod)
            {
                destMethod.Emit(instr.OpCode, (MethodInfo)instr.Operand);
            }
        }
        private class EmitterByte : IEmitter
        {
            public void Emit(Instruction instr, ILGenerator destMethod)
            {
                destMethod.Emit(instr.OpCode, (byte)instr.Operand);
            }
        }
        private class EmitterDouble : IEmitter
        {
            public void Emit(Instruction instr, ILGenerator destMethod)
            {
                destMethod.Emit(instr.OpCode, (double)instr.Operand);
            }
        }
        private class EmitterShort : IEmitter
        {
            public void Emit(Instruction instr, ILGenerator destMethod)
            {
                destMethod.Emit(instr.OpCode, (short)instr.Operand);
            }
        }
        private class EmitterInt : IEmitter
        {
            public void Emit(Instruction instr, ILGenerator destMethod)
            {
                destMethod.Emit(instr.OpCode, (int)instr.Operand);
            }
        }
        private class EmitterLong : IEmitter
        {
            public void Emit(Instruction instr, ILGenerator destMethod)
            {
                destMethod.Emit(instr.OpCode, (long)instr.Operand);
            }
        }

        private class EmitterLabel : IEmitter
        {
            public void Emit(Instruction instr, ILGenerator destMethod)
            {
                destMethod.Emit(instr.OpCode, (Label)instr.Operand);
            }
        }

        private class EmitterLabelArray : IEmitter
        {
            public void Emit(Instruction instr, ILGenerator destMethod)
            {
                destMethod.Emit(instr.OpCode, (Label[])instr.Operand);
            }
        }

        private class EmitterLocalBuilder : IEmitter
        {
            public void Emit(Instruction instr, ILGenerator destMethod)
            {
                destMethod.Emit(instr.OpCode, (LocalBuilder)instr.Operand);
            }
        }

        private class EmitterSignHelper : IEmitter
        {
            public void Emit(Instruction instr, ILGenerator destMethod)
            {
                destMethod.Emit(instr.OpCode, (SignatureHelper)instr.Operand);
            }
        }

        private class EmitterFieldInfo : IEmitter
        {
            public void Emit(Instruction instr, ILGenerator destMethod)
            {
                destMethod.Emit(instr.OpCode, (FieldInfo)instr.Operand);
            }
        }

        private class EmitterSByte : IEmitter
        {
            public void Emit(Instruction instr, ILGenerator destMethod)
            {
                destMethod.Emit(instr.OpCode, (sbyte)instr.Operand);
            }
        }

        private class EmitterSingle : IEmitter
        {
            public void Emit(Instruction instr, ILGenerator destMethod)
            {
                destMethod.Emit(instr.OpCode, (Single)instr.Operand);
            }
        }

        private class EmitterString : IEmitter
        {
            public void Emit(Instruction instr, ILGenerator destMethod)
            {
                destMethod.Emit(instr.OpCode, (String)instr.Operand);
            }
        }

        private class EmitterType : IEmitter
        {
            public void Emit(Instruction instr, ILGenerator destMethod)
            {
                destMethod.Emit(instr.OpCode, (Type)instr.Operand);
            }
        }



    }
}
