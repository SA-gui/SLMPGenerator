using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Command
{
    internal interface IRequestData
    {

        public byte[] BinaryCode { get; }
        public string ASCIICode { get; }

        public DeviceType DeviceType { get; }
        public int StartAddress { get; }
        public ushort NumberOfDevicePoints { get; }


    }
}
