using SLMPGenerator.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Device.Mitsubishi
{
    internal interface IDevice
    {

        ushort Address { get; }
        byte[] GetBinaryCode(DataSizeType dataSize);
        string GetASCIICode(DataSizeType dataSize);
        byte[] GetBinalyAddress(DataSizeType dataSize);
        string GetASCIIAddress(DataSizeType dataSize);

    }
}
