using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Common
{
    internal class RequestedStationNo
    {
        private const ushort MIN_VALUE = 1;
        private const ushort MAX_VALUE = 120;
        private const ushort VALUE1 = 125;
        private const ushort VALUE2 = 126;
        private const ushort VALUE3 = 255;
        private ushort _stationNo;

        internal byte[] BinaryCode { get; private set; }

        internal RequestedStationNo(ushort stationNo)
        {
            Validate(stationNo);
            _stationNo = stationNo;
            BinaryCode = new byte[] { BitConverter.GetBytes(stationNo)[0] };
        }


        private void Validate(int stationNo)
        {

            if ((stationNo < MIN_VALUE || stationNo > MAX_VALUE) && stationNo != VALUE1 && stationNo != VALUE2 && stationNo != VALUE3)
            {
                throw new ArgumentException($"Station No must be between {MIN_VALUE}-{MAX_VALUE}, {VALUE1}, or {VALUE2}or {VALUE3}", nameof(stationNo));
            }

        }
        private string ToAscii()
        {
            byte[] bytes = BitConverter.GetBytes(_stationNo);
            return bytes[1].ToString("X2") + bytes[0].ToString("X2");
        }

        private byte[] ToAsciiBinary()
        {
            byte[] bytes = BitConverter.GetBytes(_stationNo);
            // 上位バイト　下位バイトの順にする
            byte highByte = bytes[1];
            byte lowByte = bytes[0];
            return new byte[] { (byte)(highByte >> 4), (byte)(highByte & 0x0F), (byte)(lowByte >> 4), (byte)(lowByte & 0x0F) };
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(_stationNo);
        }

        public override bool Equals(object? obj)
        {
            return obj is RequestedStationNo No &&
                   _stationNo == No._stationNo;
        }

        public static bool operator ==(RequestedStationNo left, RequestedStationNo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RequestedStationNo left, RequestedStationNo right)
        {
            return !(left == right);
        }

    }
}
