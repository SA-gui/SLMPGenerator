using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Common
{
    internal class RequestDestModuleIONo
    {
        internal byte[] BinaryCode { get; private set; }
        internal string ASCIICode { get; private set; }
        internal RequestDestModuleIONo(RequestDestModuleIOType requestedModuleIOType)
        {
            Validate((ushort)requestedModuleIOType);
            BinaryCode = BitHelper.ConvertToBytesLittleEndian((ushort)requestedModuleIOType);
            ASCIICode = BitConverter.ToString(BitHelper.ConvertToBytesBigEndian((ushort)requestedModuleIOType)).Replace("-", "");
        }

        private void Validate(ushort moduleIONo)
        {
            if (!Enum.IsDefined(typeof(RequestDestModuleIOType), moduleIONo))
            {
                throw new ArgumentException("Invalid RequestedIOType value.");
            }
        }

        public override int GetHashCode()
        {
            return BinaryCode != null ? BitConverter.ToInt32(BinaryCode, 0) : 0;
        }

        public override bool Equals(object? obj)
        {
            return obj is RequestDestModuleIONo other && BinaryCode.SequenceEqual(other.BinaryCode);
        }

        public static bool operator ==(RequestDestModuleIONo left, RequestDestModuleIONo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RequestDestModuleIONo left, RequestDestModuleIONo right)
        {
            return !(left == right);
        }
    }
}
