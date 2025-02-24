using SLMPGenerator.Common.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Common
{
    internal interface IRequest
    {
        public byte[] Command { get; }
        public byte[] SubCommand { get; }
        public byte[] BinaryAddress { get; }
        public DeviceCode DeviceCode { get; }

        public byte[] BinaryDevicePoints { get; }
        public int StartAddress { get; }
        public string ASCIIStartAddress { get; }
        public ushort NumberOfDevicePoints { get; }

        public byte[] BinaryCode { get;  }
        public string ASCIICode { get; }
    }
}
