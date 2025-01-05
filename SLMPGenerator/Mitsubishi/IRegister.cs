using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Mitsubishi
{
    internal interface IRegister
    {
        byte[] GetBinaryCode16bit();
        byte[] GetBinaryCode32bit();
        ushort GetAddress();
    }
}
