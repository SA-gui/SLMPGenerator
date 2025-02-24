using SLMPGenerator.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Command
{
    internal class AddressHelper
    {
        private const string REGEX = @"^(?<device>[A-Za-z]+)(?<address>\d+)$";

        internal static (string,ushort) SplitAddress(string rawAddress)
        {
            var match = System.Text.RegularExpressions.Regex.Match(rawAddress, REGEX);
            if (!match.Success)
            {
                throw new ArgumentException("Invalid address format");
            }
            var device = match.Groups["device"].Value;
            var address = ushort.Parse(match.Groups["address"].Value);
            return (device, address);
        }

        internal static void ValidateDevPoints(MessageType messageType, DeviceType deviceType,ushort points)
        {
            //slmp reference manual
            var maxDevicePoints = (messageType, deviceType) switch
            {
                (MessageType.Binary, DeviceType.Bit) => 7168,
                (MessageType.Binary, DeviceType.Word) => 960,
                (MessageType.Binary, DeviceType.DoubleWord) => 960,
                (MessageType.ASCII, DeviceType.Bit) => 3584,
                (MessageType.ASCII, DeviceType.Word) => 960,
                (MessageType.ASCII, DeviceType.DoubleWord) => 960,
                _ => throw new ArgumentOutOfRangeException($"Invalid Combination. MessageType:{messageType} DeviceType:{deviceType}")
            };
            if (points > maxDevicePoints)
            {
                throw new ArgumentOutOfRangeException($"Number of device points exceeds the limit. MessageType:{messageType} DeviceType:{deviceType} Max:{maxDevicePoints} Points:{points}");
            }
        }


    }
}
