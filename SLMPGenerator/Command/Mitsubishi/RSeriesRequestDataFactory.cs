using SLMPGenerator.UseCase;
using SLMPGenerator.Command.Read;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SLMPGenerator.Command.Write;

namespace SLMPGenerator.Command.Mitsubishi
{
    internal class RSeriesRequestDataFactory
    {

        private static Dictionary<string, DeviceCode> _deviceCodes =
                new Dictionary<string, DeviceCode>(){
                    { "D", (new DeviceCode(new byte[] { 0x00, 0xA8 }, "D***", DeviceType.Word, DeviceNoRange.Dec)) },
                    { "M", (new DeviceCode(new byte[] { 0x00, 0x90 }, "M***", DeviceType.Bit, DeviceNoRange.Dec)) },
                    { "B", (new DeviceCode(new byte[] { 0x00, 0xA0 }, "B***", DeviceType.Bit, DeviceNoRange.Hex)) }
                };

        internal static IRequestData CreateReadRequestData(DeviceAccessType devAccessType, MessageType messageType, string rawAddress, ushort points)
        {
            switch (devAccessType)
            {
                case DeviceAccessType.Bit:
                    return CreateBitUnitReadRequestData(messageType, rawAddress, points);
                case DeviceAccessType.Word:
                    return CreateWordUnitReadRequestData(messageType, rawAddress, points);
                default:
                    throw new ArgumentException("Invalid command");
            }
        }
        private static IRequestData CreateBitUnitReadRequestData(MessageType messageType,string rawAddress,ushort points)
        {
            var (device,address) = AddressHelper.SplitAddress(rawAddress);

            if (_deviceCodes.ContainsKey(device.ToString()))
            {

                DeviceCode deviceCode = _deviceCodes[device.ToString()];
                AddressHelper.ValidateDevPoints(messageType, deviceCode.DeviceType, points);

                return new RSeriesReadRequestData(deviceCode,new BitUnitReadData(deviceCode, (ushort)address,points) );
            }
            else
            {
                throw new ArgumentException("Invalid command");
            }
        }
        private static IRequestData CreateWordUnitReadRequestData(MessageType messageType, string rawAddress, ushort points)
        {
            var (device, address) = AddressHelper.SplitAddress(rawAddress);

            if (_deviceCodes.ContainsKey(device.ToString()))
            {

                DeviceCode deviceCode = _deviceCodes[device.ToString()];
                AddressHelper.ValidateDevPoints(messageType, deviceCode.DeviceType, points);

                return new RSeriesReadRequestData(deviceCode, new WordUnitReadData(deviceCode, (ushort)address, points));
            }
            else
            {
                throw new ArgumentException("Invalid command");
            }
        }



        internal static IRequestData CreateWriteRequestData(MessageType messageType, string rawAddress, List<short> writeDataList)
        {
            var (device, address) = AddressHelper.SplitAddress(rawAddress);

            if (_deviceCodes.ContainsKey(device.ToString()))
            {

                DeviceCode deviceCode = _deviceCodes[device.ToString()];
                AddressHelper.ValidateDevPoints(messageType, deviceCode.DeviceType, (ushort)writeDataList.Count);

                return new RSeriesWriteRequestData(deviceCode, new WordUnitWriteData(deviceCode, (ushort)address, writeDataList));
            }
            else
            {
                throw new ArgumentException("Invalid command");
            }
        }

        internal static IRequestData CreateWriteRequestData(MessageType messageType, string rawAddress, List<bool> writeDataList)
        {
            var (device, address) = AddressHelper.SplitAddress(rawAddress);

            if (_deviceCodes.ContainsKey(device.ToString()))
            {

                DeviceCode deviceCode = _deviceCodes[device.ToString()];
                AddressHelper.ValidateDevPoints(messageType, deviceCode.DeviceType, (ushort)writeDataList.Count);

                return new RSeriesWriteRequestData(deviceCode, new BitUnitWriteData(deviceCode, (ushort)address, writeDataList));
            }
            else
            {
                throw new ArgumentException("Invalid command");
            }
        }









    }
}
