using SLMPGenerator.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Command.Mitsubishi
{
    internal class QSeriesRequestDataFactory
    {
        private static Dictionary<string,  DeviceCode> _deviceCodes =
            new Dictionary<string, DeviceCode>(){
                    { "D", (new DeviceCode(new byte[] { 0xA8 }, "D*", DeviceType.Word, DeviceNoRange.Dec)) },
                    { "M", (new DeviceCode(new byte[] { 0x90 }, "M*", DeviceType.Bit, DeviceNoRange.Dec)) },
                    { "B", (new DeviceCode(new byte[] { 0xA0 }, "B*", DeviceType.Bit, DeviceNoRange.Hex)) }
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

        private static IRequestData CreateBitUnitReadRequestData(MessageType messageType, string rawAddress, ushort points)
        {
            var (device, address) = AddressHelper.SplitAddress(rawAddress);

            if (_deviceCodes.ContainsKey(device.ToString()))
            {

                DeviceCode deviceCode = _deviceCodes[device.ToString()];
                AddressHelper.ValidateDevPoints(messageType, deviceCode.DeviceType, points);

                return new QSeriesReadRequestData(deviceCode, new BitUnitAccessData(deviceCode, (ushort)address, points));
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

                return new QSeriesReadRequestData(deviceCode, new WordUnitAccessData(deviceCode, (ushort)address, points));
            }
            else
            {
                throw new ArgumentException("Invalid command");
            }
        }
    }
            

}
