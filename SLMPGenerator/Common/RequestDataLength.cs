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
            ushort dataLength = (ushort)(monitoringTimer.BinaryCode.Length + requestData.BinaryCode.Length);
            BinaryCode = BitHelper.ConvertToBytesLittleEndian(dataLength);
            ASCIICode = BitConverter.ToString(BitHelper.ConvertToBytesBigEndian(dataLength)).Replace("-", "");
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
