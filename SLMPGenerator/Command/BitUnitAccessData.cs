using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Command
{
    internal class BitUnitAccessData
    {

        public ushort Address { get; private set; }
        public DeviceCode DeviceCode { get; private set; }
        public ushort NumberOfDevicePoints { get; private set; }


        public BitUnitAccessData(DeviceCode deviceCode, ushort address, ushort numberOfDevPoints)
        {
            DeviceCode = deviceCode ?? throw new ArgumentNullException(nameof(deviceCode));
            Address = address;
            NumberOfDevicePoints = numberOfDevPoints;
        }

        public override int GetHashCode()
        {
            return DeviceCode.GetHashCode() ^ Address.GetHashCode() ^ NumberOfDevicePoints.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is BitUnitAccessData other && DeviceCode.Equals(other.DeviceCode) && Address.Equals(other.Address) && NumberOfDevicePoints.Equals(other.NumberOfDevicePoints);
        }
    }
}
