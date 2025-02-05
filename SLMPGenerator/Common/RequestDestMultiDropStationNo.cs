using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Common
{
    internal class RequestDestMultiDropStationNo
    {
        internal byte[] BinaryCode { get; private set; }
        internal string ASCIICode { get; private set; }

        internal RequestDestMultiDropStationNo(ushort multiDropStationNo)
        {
            BinaryCode = new byte[] { BitHelper.ConvertToBytesLittleEndian(multiDropStationNo)[0] };
            ASCIICode = BitConverter.ToString(BinaryCode).Replace("-", "");
        }

        public override int GetHashCode()
        {
            return BinaryCode != null ? BitConverter.ToInt32(BinaryCode, 0) : 0;
        }

        public override bool Equals(object? obj)
        {
            return obj is RequestDestMultiDropStationNo other && BinaryCode.SequenceEqual(other.BinaryCode);
        }

        public static bool operator ==(RequestDestMultiDropStationNo left, RequestDestMultiDropStationNo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RequestDestMultiDropStationNo left, RequestDestMultiDropStationNo right)
        {
            return !(left == right);
        }

    }
}
