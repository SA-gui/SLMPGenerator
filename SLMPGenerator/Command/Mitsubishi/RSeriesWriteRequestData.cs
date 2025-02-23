using SLMPGenerator.Command.Write;
using SLMPGenerator.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Command.Mitsubishi
{
    internal class RSeriesWriteRequestData : IRequestData
    {
        private byte[] _command = new byte[] { 0x14, 0x01 };
        private byte[] _bitSubCommand = new byte[] { 0x00, 0x03 };
        private byte[] _wordSubCommand = new byte[] { 0x00, 0x02 };
        private const int _padding = 8;

        public byte[] BinaryCode { get; private set; } = Array.Empty<byte>();
        public string ASCIICode { get; private set; } = string.Empty;

        public DeviceType DeviceType { get; private set; }

        public int StartAddress { get; private set; }

        public ushort NumberOfDevicePoints { get; private set; }

        internal RSeriesWriteRequestData(DeviceCode deviceCode, WordUnitWriteData wordUnitWriteData)
        {
            DeviceType = deviceCode.DeviceType;
            StartAddress = wordUnitWriteData.StartAddress;
            NumberOfDevicePoints = wordUnitWriteData.NumberOfDevicePoints;
            byte[] commnad = _command.Reverse().ToArray();
            byte[] subCommand = _wordSubCommand.Reverse().ToArray();
            byte[] binaryDevicePoints = BitHelper.ToBytesLittleEndian(wordUnitWriteData.NumberOfDevicePoints);
            byte[] binaryAddress = ConvertToBinaryAddress(deviceCode.DeviceNoRange, StartAddress);
            byte[] binaryWriteData=new byte[] { };

            foreach (short data in wordUnitWriteData.WriteDataList)
            {
                binaryWriteData = new byte[] { }
                .Concat(binaryWriteData)
                .Concat(BitHelper.ToBytesLittleEndian((ushort)data)).ToArray();
            }

            SetBinaryCode(commnad, subCommand, binaryAddress, deviceCode.BinaryCode, binaryDevicePoints, binaryWriteData);
            SetASCIICode(commnad, subCommand, wordUnitWriteData.StartAddress, deviceCode.ASCIICode, binaryDevicePoints, binaryWriteData);
        }

        internal RSeriesWriteRequestData(DeviceCode deviceCode, BitUnitWriteData bitUnitWriteData)
        {
            DeviceType = deviceCode.DeviceType;
            StartAddress = bitUnitWriteData.StartAddress;
            NumberOfDevicePoints = bitUnitWriteData.NumberOfDevicePoints;
            byte[] commnad = _command.Reverse().ToArray();
            byte[] subCommand = _bitSubCommand.Reverse().ToArray();
            byte[] binaryDevicePoints = BitHelper.ToBytesLittleEndian(bitUnitWriteData.NumberOfDevicePoints);
            byte[] binaryAddress = ConvertToBinaryAddress(deviceCode.DeviceNoRange, StartAddress);
            byte[] binaryWriteData = new byte[] { };

            foreach (bool data in bitUnitWriteData.WriteDataList)
            {
                binaryWriteData = new byte[] { }
                .Concat(binaryWriteData)
                .Concat(BitHelper.ToBytesLittleEndian(data ? (ushort)1 : (ushort)0)).ToArray();
            }
            SetBinaryCode(commnad, subCommand, binaryAddress, deviceCode.BinaryCode, binaryDevicePoints, binaryWriteData);
            SetASCIICode(commnad, subCommand, bitUnitWriteData.StartAddress, deviceCode.ASCIICode, binaryDevicePoints, binaryWriteData);
        }

        private void SetBinaryCode(byte[] command, byte[] subCommand, byte[] binaryAddress, byte[] devCode, byte[] binaryDevPoints, byte[] binaryWriteData)
        {
            BinaryCode = new byte[] { }
                .Concat(command)
                .Concat(subCommand)
                .Concat(binaryAddress)
                .Concat(devCode)
                .Concat(binaryDevPoints)
                .Concat(binaryWriteData)
                .ToArray();
        }

        private void SetASCIICode(byte[] command, byte[] subCommand, ushort startAddress, string devCode, byte[] binaryDevPoints, byte[] binaryWriteData)
        {
            string asciiCommand = BitHelper.ToReverseString(command);
            string asciiSubCommand = BitHelper.ToReverseString(subCommand);
            string asciiAddress = startAddress.ToString().PadLeft(_padding, '0');
            string asciiDevicePoints = BitHelper.ToReverseString(binaryDevPoints);
            string asciiWriteData = BitHelper.ToReverseString(binaryWriteData);

            ASCIICode = asciiCommand
                    + asciiSubCommand
                    + devCode
                    + asciiAddress
                    + asciiDevicePoints
                    + asciiWriteData;
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

        public override int GetHashCode()
        {
            return ASCIICode.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is RSeriesWriteRequestData other && ASCIICode.Equals(other.ASCIICode);
        }
    }
}
