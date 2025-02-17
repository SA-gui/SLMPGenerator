using SLMPGenerator.Command.Read;
using SLMPGenerator.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Command.Mitsubishi
{
    internal class QSeriesReadRequestData : IRequestData
    {
        private byte[] _command = new byte[] { 0x04, 0x01 };
        private byte[] _bitSubCommand = new byte[] { 0x00, 0x01 };
        private byte[] _wordSubCommand = new byte[] { 0x00, 0x00 };
        private int _padding = 6;

        public byte[] BinaryCode { get; private set; } = Array.Empty<byte>();
        public string ASCIICode { get; private set; } = string.Empty;

        public DeviceType DeviceType { get; private set; }

        public int StartAddress { get; private set; }

        public ushort NumberOfDevicePoints { get; private set; }

        internal QSeriesReadRequestData(DeviceCode deviceCode, WordUnitReadData wordAccess)
        {
            DeviceType = deviceCode.DeviceType;
            StartAddress = wordAccess.StartAddress;
            NumberOfDevicePoints = wordAccess.NumberOfDevicePoints;
            byte[] commnad = _command.Reverse().ToArray();
            byte[] subCommand = _wordSubCommand.Reverse().ToArray();
            byte[] binaryDevicePoints = BitHelper.ToBytesLittleEndian(wordAccess.NumberOfDevicePoints);
            byte[] binaryAddress = ConvertToBinaryAddress(deviceCode.DeviceNoRange, StartAddress);

            SetBinaryCode(commnad, subCommand, binaryAddress, deviceCode.BinaryCode, binaryDevicePoints);
            SetASCIICode(commnad, subCommand, binaryAddress, deviceCode.ASCIICode, binaryDevicePoints);
        }

        internal QSeriesReadRequestData(DeviceCode deviceCode, BitUnitReadData bitAccess)
        {
            DeviceType = deviceCode.DeviceType;
            StartAddress = bitAccess.StartAddress;
            NumberOfDevicePoints = bitAccess.NumberOfDevicePoints;
            byte[] commnad = _command.Reverse().ToArray();
            byte[] subCommand = _bitSubCommand.Reverse().ToArray();
            byte[] binaryDevicePoints = BitHelper.ToBytesLittleEndian(bitAccess.NumberOfDevicePoints);
            byte[] binaryAddress = ConvertToBinaryAddress(deviceCode.DeviceNoRange, StartAddress);

            SetBinaryCode(commnad, subCommand, binaryAddress, deviceCode.BinaryCode, binaryDevicePoints);
            SetASCIICode(commnad, subCommand, binaryAddress, deviceCode.ASCIICode, binaryDevicePoints);
        }

        private void SetBinaryCode(byte[] command, byte[] subCommand, byte[] binaryAddress, byte[] devCode, byte[] binaryDevPoints)
        {
            BinaryCode = new byte[] { }
                .Concat(command)
                .Concat(subCommand)
                .Concat(binaryAddress)
                .Concat(devCode)
                .Concat(binaryDevPoints)
                .ToArray();
        }

        private void SetASCIICode(byte[] command, byte[] subCommand, byte[] binaryAddress, string devCode, byte[] binaryDevPoints)
        {
            string asciiCommand = BitHelper.ToReverseString(command);
            string asciiSubCommand = BitHelper.ToReverseString(subCommand);
            string asciiAddress = BitHelper.ToReverseString(binaryAddress).PadLeft(_padding, '0');
            string asciiDevicePoints = BitHelper.ToReverseString(binaryDevPoints);

            ASCIICode = asciiCommand
                    + asciiSubCommand
                    + devCode
                    + asciiAddress
                    + asciiDevicePoints;
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
            return obj is QSeriesReadRequestData other && ASCIICode.Equals(other.ASCIICode);
        }
    }
}
