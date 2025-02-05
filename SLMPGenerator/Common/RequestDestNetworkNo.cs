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
        private ushort _networkNo;

        internal byte[] BinaryCode { get; private set; }
        internal string ASCIICode { get; private set; }

        internal RequestDestNetworkNo(ushort networkNo)
        {
            Validate(networkNo);
            _networkNo = networkNo;
            
            BinaryCode = BitHelper.ConvertToBytesLittleEndian(networkNo).Take(1).ToArray();
            ASCIICode = BitConverter.ToString(BinaryCode).Replace("-", "");
        }


        private void Validate(int networkNo)
        {

            if (networkNo < MIN_VALUE || networkNo > MAX_VALUE)
            {
                throw new ArgumentException($"Network No must be between {MIN_VALUE} and {MAX_VALUE}", nameof(networkNo));
            }

        }
        private string ToAscii()
        {
            byte[] bytes = BitConverter.GetBytes(_networkNo);
            return bytes[1].ToString("X2") + bytes[0].ToString("X2");
        }



        public override int GetHashCode()
        {
            return HashCode.Combine(_networkNo);
        }

        public override bool Equals(object? obj)
        {
            return obj is RequestDestNetworkNo No &&
                   _networkNo == No._networkNo;
        }

        public static bool operator ==(RequestDestNetworkNo left, RequestDestNetworkNo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RequestDestNetworkNo left, RequestDestNetworkNo right)
        {
            return !(left == right);
        }





    }
}
