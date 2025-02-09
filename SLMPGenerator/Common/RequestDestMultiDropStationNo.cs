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
            BinaryCode = new byte[] { BitHelper.ToBytesLittleEndian(multiDropStationNo)[0] };
            ASCIICode = BitHelper.ToString(BinaryCode);
        }

        public override int GetHashCode()
        {
            return ASCIICode.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is RequestDestMultiDropStationNo other && ASCIICode.Equals(other.ASCIICode);
        }

    }
}
