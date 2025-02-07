using SLMPGenerator.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Common
{
    internal class RequestDataLength
    {
        internal byte[] BinaryCode { get; private set; }
        internal string ASCIICode { get; private set; }

        internal RequestDataLength(MonitoringTimer monitoringTimer,IRequestData requestData)
        {
            ushort binarydataLength = (ushort)(monitoringTimer.BinaryCode.Length + requestData.BinaryCode.Length);
            BinaryCode = BitHelper.ConvertToBytesLittleEndian(binarydataLength);
            ushort asciiDataLength = (ushort)(monitoringTimer.ASCIICode.Length + requestData.ASCIICode.Length);
            ASCIICode = BitConverter.ToString(BitHelper.ConvertToBytesLittleEndian(asciiDataLength).Reverse().ToArray()).Replace("-", "");
        }

        public override int GetHashCode()
        {
            return BinaryCode != null ? BitConverter.ToInt32(BinaryCode, 0) : 0;
        }
        public override bool Equals(object? obj)
        {
            return obj is RequestDataLength other && BinaryCode.SequenceEqual(other.BinaryCode);
        }

        public static bool operator ==(RequestDataLength left, RequestDataLength right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(RequestDataLength left, RequestDataLength right)
        {
            return !(left == right);
        }


    }
}
