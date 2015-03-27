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
    interface IEmitter
    {
        void Emit(Instruction instr, ILGenerator destMethod);
    }
}
