using SLMPGenerator.Common;
using SLMPGenerator.Common.Command;
using SLMPGenerator.Read.Domain;
using SLMPGenerator.Read.Domain.ReadData;

namespace SLMPGenerator.Read.Infrastructure.Mitsubishi
{
    internal class RSeriesReadRequestFactory : IReadRequestFactory
    {
        private static Dictionary<string, DeviceCode> _deviceCodes =
                new Dictionary<string, DeviceCode>(){
                    { "D", new DeviceCode(new byte[] { 0x00, 0xA8 }, "D***", DeviceType.Word, DeviceNoRange.Dec) },
                    { "M", new DeviceCode(new byte[] { 0x00, 0x90 }, "M***", DeviceType.Bit, DeviceNoRange.Dec) },
                    { "B", new DeviceCode(new byte[] { 0x00, 0xA0 }, "B***", DeviceType.Bit, DeviceNoRange.Hex) }
                };

        public ReadRequest Create(DeviceAccessType devAccessType, MessageType messageType, string rawAddress, ushort points)
        {
            if (string.IsNullOrEmpty(rawAddress))
            {
                throw new ArgumentException("rawAddress cannot be null or empty");
            }

            if (points <= 0)
            {
                throw new ArgumentException("points must be greater than zero");
            }

            switch (devAccessType)
            {
                case DeviceAccessType.Bit:
                    return CreateBitUnitReadRequestData(messageType, rawAddress, points);
                case DeviceAccessType.Word:
                    return CreateWordUnitReadRequestData(messageType, rawAddress, points);
                default:
                    throw new ArgumentException("Invalid DeviceAccessType");
            }
        }




        private ReadRequest CreateBitUnitReadRequestData(MessageType messageType, string rawAddress, ushort points)
        {
            var (device, address) = AddressHelper.SplitAddress(rawAddress);

            if (_deviceCodes.ContainsKey(device.ToString()))
            {

                DeviceCode deviceCode = _deviceCodes[device.ToString()];
                AddressHelper.ValidateDevPoints(messageType, deviceCode.DeviceType, points);

                return new ReadRequest(deviceCode, new BitUnitReadData(deviceCode, (ushort)address, points));
            }
            else
            {
                throw new ArgumentException("Invalid command");
            }
        }
        private ReadRequest CreateWordUnitReadRequestData(MessageType messageType, string rawAddress, ushort points)
        {
            var (device, address) = AddressHelper.SplitAddress(rawAddress);

            if (_deviceCodes.ContainsKey(device.ToString()))
            {

                DeviceCode deviceCode = _deviceCodes[device.ToString()];
                AddressHelper.ValidateDevPoints(messageType, deviceCode.DeviceType, points);

                return new ReadRequest(deviceCode, new WordUnitReadData(deviceCode, (ushort)address, points));
            }
            else
            {
                throw new ArgumentException("Invalid command");
            }
        }
    }
}
