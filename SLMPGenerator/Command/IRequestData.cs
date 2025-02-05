using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Command
{
    internal interface IRequestData
    {
        byte[] Command { get; }
        byte[] SubCommand { get; }
        byte[] Data { get; }
        byte[] BinaryCode { get; }
        string ASCIICode { get; }
    }
}
