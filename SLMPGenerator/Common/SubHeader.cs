using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Common
{
    internal class SubHeader
    {
        internal byte[] BinaryCode { get; private set; }

        internal SubHeader()
        {
            BinaryCode = new byte[] { 0x50, 0x00 };
        }

        public override int GetHashCode()
        {
            return BinaryCode != null ? BitConverter.ToInt32(BinaryCode, 0) : 0;
        }

        public override bool Equals(object? obj)
        {
            return obj is SubHeader other && BinaryCode.SequenceEqual(other.BinaryCode);
        }

        public static bool operator ==(SubHeader left, SubHeader right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SubHeader left, SubHeader right)
        {
            return !(left == right);
        }
    }
}
