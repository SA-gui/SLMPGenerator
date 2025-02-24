using SLMPGenerator.Read.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Common
{
    internal class RequestLength
    {
        internal byte[] BinaryCode { get; private set; }
        internal string ASCIICode { get; private set; }

        internal RequestLength(MonitoringTimer monitoringTimer,IRequest requestData)
        {
            ushort binarydataLength = (ushort)(monitoringTimer.BinaryCode.Length + requestData.BinaryCode.Length);
            BinaryCode = BitHelper.ToBytesLittleEndian(binarydataLength);
            ushort asciiDataLength = (ushort)(monitoringTimer.ASCIICode.Length + requestData.ASCIICode.Length);
            ASCIICode = BitHelper.ToString(BitHelper.ToBytesBigEndian(asciiDataLength));
        }

        public override int GetHashCode()
        {
            return ASCIICode.GetHashCode();
        }
        public override bool Equals(object? obj)
        {
            return obj is RequestLength other && ASCIICode.Equals(other.ASCIICode);
        }

    }
}
