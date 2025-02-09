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
            BinaryCode = BitHelper.ToBytesLittleEndian((ushort)requestedModuleIOType);
            ASCIICode = BitHelper.ToReverseString(BinaryCode);
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
            return ASCIICode.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is RequestDestModuleIONo other && ASCIICode.Equals(other.ASCIICode);
        }
    }
}
