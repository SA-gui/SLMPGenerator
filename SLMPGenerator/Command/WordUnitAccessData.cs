using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Command
{
    internal class WordUnitAccessData
    {
        public ushort Address { get; private set; }
        public DeviceCode DeviceCode { get; private set; }
        public ushort NumberOfDevicePoints { get; private set; }


        public WordUnitAccessData(DeviceCode deviceCode, ushort address, ushort numberOfPoints)
        {
            DeviceCode = deviceCode ?? throw new ArgumentNullException(nameof(deviceCode));
            Address = address;
            NumberOfDevicePoints = numberOfPoints;
        }

        public override int GetHashCode()
        {
            return DeviceCode.GetHashCode() ^ Address.GetHashCode() ^ NumberOfDevicePoints.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is WordUnitAccessData other && DeviceCode.Equals(other.DeviceCode) && Address.Equals(other.Address) && NumberOfDevicePoints.Equals(other.NumberOfDevicePoints);
        }
    }
}
