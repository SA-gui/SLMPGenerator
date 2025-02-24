using SLMPGenerator.Common;
using SLMPGenerator.Common.Command;
using SLMPGenerator.Read.Domain.ReadData;
using SLMPGenerator.Read.Infrastructure.Mitsubishi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Read.Domain
{
    internal class ReadRequest : IRequest
    {
        private byte[] _command = new byte[] { 0x04, 0x01 };
        
        private byte[] _oneByteWordSubCommand = new byte[] { 0x00, 0x00 };
        private byte[] _oneByteBitSubCommand = new byte[] { 0x00, 0x01 };
        private byte[] _twoBytesWordSubCommand = new byte[] { 0x00, 0x02 };
        private byte[] _twoBytesBitSubCommand = new byte[] { 0x00, 0x03 };

        private const int _oneBytePadding = 6;
        private const int _twoBytesPadding = 8;
        private const int _cutArrayLength = 3; // 先頭３の配列を切り取る為の変数

        public byte[] Command { get; private set; }
        public byte[] SubCommand { get; private set; }
        public byte[] BinaryAddress { get; private set; }
        public DeviceCode DeviceCode { get; private set; }

        public byte[] BinaryDevicePoints { get; private set; }
        public int StartAddress { get; private set; }
        public string ASCIIStartAddress { get; private set; }
        public ushort NumberOfDevicePoints { get; private set; }

        public byte[] BinaryCode { get; private set; }
        public string ASCIICode { get; private set; }

        internal ReadRequest(DeviceCode deviceCode, BitUnitReadData bitUnitReadData)
        {
            Command = _command.Reverse().ToArray();
            DeviceCode = deviceCode ?? throw new ArgumentNullException(nameof(deviceCode));
            StartAddress = bitUnitReadData.StartAddress;
            NumberOfDevicePoints = bitUnitReadData.NumberOfDevicePoints;
            BinaryDevicePoints = BitHelper.ToBytesLittleEndian(bitUnitReadData.NumberOfDevicePoints);

            if (IsOneByteDataSize(deviceCode))
            {
                SubCommand = _oneByteBitSubCommand.Reverse().ToArray();
                ASCIIStartAddress = StartAddress.ToString().PadLeft(_oneBytePadding, '0');
                BinaryAddress = ConvertToBinaryAddress(deviceCode.DeviceNoRange, StartAddress).Take(_cutArrayLength).ToArray();
            }
            else
            {
                SubCommand = _twoBytesBitSubCommand.Reverse().ToArray();
                ASCIIStartAddress = StartAddress.ToString().PadLeft(_twoBytesPadding, '0');
                BinaryAddress = ConvertToBinaryAddress(deviceCode.DeviceNoRange, StartAddress);
            }

            BinaryCode = CreateRequestBinaryCode();
            ASCIICode = CreateRequestASCIICode();
        }

        internal ReadRequest(DeviceCode deviceCode, WordUnitReadData wordUnitReadData)
        {
            Command = _command.Reverse().ToArray();
            DeviceCode = deviceCode ?? throw new ArgumentNullException(nameof(deviceCode));
            StartAddress = wordUnitReadData.StartAddress;
            NumberOfDevicePoints = wordUnitReadData.NumberOfDevicePoints;
            BinaryDevicePoints = BitHelper.ToBytesLittleEndian(wordUnitReadData.NumberOfDevicePoints);

            if (IsOneByteDataSize(deviceCode))
            {
                SubCommand = _oneByteWordSubCommand.Reverse().ToArray();
                ASCIIStartAddress = StartAddress.ToString().PadLeft(_oneBytePadding, '0');
                BinaryAddress = ConvertToBinaryAddress(deviceCode.DeviceNoRange, StartAddress).Take(_cutArrayLength).ToArray();
            }
            else
            {
                SubCommand = _twoBytesWordSubCommand.Reverse().ToArray();
                ASCIIStartAddress = StartAddress.ToString().PadLeft(_twoBytesPadding, '0');
                BinaryAddress = ConvertToBinaryAddress(deviceCode.DeviceNoRange, StartAddress);
            }

            BinaryCode = CreateRequestBinaryCode();
            ASCIICode = CreateRequestASCIICode();
        }



        /// <summary>
        /// 1byteデータサイズかどうかを判定する
        /// サブコマンドが0000,0001の場合は1byteデータサイズ
        /// 0002,0003の場合は2byteデータサイズ
        /// </summary>
        /// <param name="subCommand"></param>
        /// <returns></returns>
        private bool IsOneByteDataSize(byte[] subCommand)
        {
            int subCommandValue = BitConverter.ToInt16(subCommand, 0);
            if (subCommandValue <= 1)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// デバイスコードが2桁の場合は1byteデータサイズ
        /// </summary>
        /// <param name="deviceCode"></param>
        /// <returns></returns>
        private bool IsOneByteDataSize(DeviceCode deviceCode)
        {
 
            if (deviceCode.ASCIICode.Length == 2)
            {
                return true;
            }
            return false;
        }



        private byte[] ConvertToBinaryAddress(DeviceNoRange deviceNoRange, int address)
        {
            if (DeviceNoRange.Hex == deviceNoRange)
            {
                string hexAddress = address.ToString();
                int decimalAddress = int.Parse(hexAddress, System.Globalization.NumberStyles.HexNumber);
                return BitHelper.ToBytesLittleEndian(decimalAddress);
            }
            return BitHelper.ToBytesLittleEndian(address);
        }


        private byte[] CreateRequestBinaryCode()
        {
            return new byte[] { }
                .Concat(Command)
                .Concat(SubCommand)
                .Concat(BinaryAddress)
                .Concat(DeviceCode.BinaryCode)
                .Concat(BinaryDevicePoints)
                .ToArray();
        }

        private string CreateRequestASCIICode()
        {
            string asciiCommand = BitHelper.ToReverseString(Command);
            string asciiSubCommand = BitHelper.ToReverseString(SubCommand);
            string asciiAddress = ASCIIStartAddress;
            string asciiDevicePoints = BitHelper.ToReverseString(BinaryDevicePoints);

            string aSCIICode = asciiCommand
                    + asciiSubCommand
                    + DeviceCode.ASCIICode
                    + asciiAddress
                    + asciiDevicePoints;
            return aSCIICode;
        }

        public override int GetHashCode()
        {
            return ASCIICode.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is ReadRequest other && ASCIICode.Equals(other.ASCIICode);
        }

    }
}
