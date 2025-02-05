using SLMPGenerator.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Device.Mitsubishi
{
    internal class DataRegister : IDevice
    {
        private byte[] _binaryCodeOnebyte = new byte[] { 0xA8 };
        private byte[] _binaryCodeTwobytes = new byte[] { 0x00, 0xA8 };


        public ushort Address { get; private set; }

        internal DataRegister(ushort address)
        {
            Address = address;
        }


        public byte[] GetBinaryCode(DataSizeType dataSize)
        {
            switch (dataSize)
            {
                case DataSizeType.OneByte:
                    return _binaryCodeOnebyte;
                case DataSizeType.TwoBytes:
                    return _binaryCodeTwobytes.Reverse().ToArray();
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataSize), dataSize, null);
            }
        }

        public string GetASCIICode(DataSizeType dataSize)
        {

            switch (dataSize)
            {
                case DataSizeType.OneByte:
                    return BitConverter.ToString(_binaryCodeOnebyte).Replace("-", "");
                case DataSizeType.TwoBytes:
                    return BitConverter.ToString(_binaryCodeTwobytes).Replace("-", "");
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataSize), dataSize, null);
            }
        }

        public byte[] GetBinalyAddress(DataSizeType dataSize)
        {
            switch (dataSize)
            {
                case DataSizeType.OneByte:
                    return BitHelper.ConvertToBytesLittleEndian((int)Address).Take(3).ToArray();//先頭3byte取得
                case DataSizeType.TwoBytes:
                    return BitHelper.ConvertToBytesLittleEndian((int)Address);
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataSize), dataSize, null);
            }
        }

        public string GetASCIIAddress(DataSizeType dataSize)
        {
            switch (dataSize)
            {
                case DataSizeType.OneByte:
                    return BitConverter.ToString(BitHelper.ConvertToBytesBigEndian((int)Address).Take(3).ToArray()).Replace("-", "").PadLeft(6, '0');
                case DataSizeType.TwoBytes:
                    return BitConverter.ToString(BitHelper.ConvertToBytesBigEndian((int)Address)).Replace("-", "").PadLeft(8, '0');
                default:
                    throw new ArgumentOutOfRangeException(nameof(dataSize), dataSize, null);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is DataRegister other)
            {
                return Address == other.Address;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Address.GetHashCode();
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
