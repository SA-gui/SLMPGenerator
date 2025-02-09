using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Common
{
    internal class SubHeader
    {
        private byte[] _subHeader = new byte[] { 0x00, 0x50 };
        internal byte[] BinaryCode { get; private set; }
        internal string ASCIICode { get; private set; }
        internal SubHeader()
        {
            BinaryCode = _subHeader.Reverse().ToArray();
            ASCIICode = BitHelper.ToString(BinaryCode);
        }

        public override int GetHashCode()
        {
            return ASCIICode.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is SubHeader other && ASCIICode.Equals(other.ASCIICode);
        }
    }
}
