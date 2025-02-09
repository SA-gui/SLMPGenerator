using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Common
{
    internal class RequestDestNetworkNo
    {
        private const ushort MIN_VALUE = 0;
        private const ushort MAX_VALUE = 239;
        internal byte[] BinaryCode { get; private set; }
        internal string ASCIICode { get; private set; }

        internal RequestDestNetworkNo(ushort networkNo)
        {
            Validate(networkNo);
            BinaryCode = BitHelper.ToBytesLittleEndian(networkNo).Take(1).ToArray();
            ASCIICode = BitHelper.ToString(BinaryCode);
        }


        private void Validate(int networkNo)
        {

            if (networkNo < MIN_VALUE || networkNo > MAX_VALUE)
            {
                throw new ArgumentException($"Network No must be between {MIN_VALUE} and {MAX_VALUE}", nameof(networkNo));
            }

        }

        public override int GetHashCode()
        {
            return ASCIICode.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is RequestDestNetworkNo other && ASCIICode.Equals(other.ASCIICode);
        }

    }
}
