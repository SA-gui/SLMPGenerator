using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Mitsubishi
{
    internal class DataRegister:IRegister
    {
        private byte[] _binaryCode16bit;
        private byte[] _binaryCode32bit;
        private ushort _address;
        internal DataRegister(ushort address)
        {
            _address = address;
            _binaryCode16bit = new byte[] { 0xA8 };
            _binaryCode32bit = new byte[] { 0x00,0xA8 };
        }

        public override bool Equals(object obj)
        {
            if (obj is DataRegister other)
            {
                return _address == other._address && _binaryCode16bit.SequenceEqual(other._binaryCode16bit);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_address, _binaryCode16bit[0]);
        }

        public byte[] GetBinaryCode16bit()
        {
            return _binaryCode16bit;
        }

        public byte[] GetBinaryCode32bit()
        {
            return _binaryCode32bit;
        }

        public ushort GetAddress()
        {
            return _address;
        }

        public static bool operator ==(DataRegister left, DataRegister right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }
            return left.Equals(right);
        }

        public static bool operator !=(DataRegister left, DataRegister right)
        {
            return !(left == right);
        }
    }
}
