﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Command.Read
{
    internal class BitUnitReadData
    {

        public ushort StartAddress { get; private set; }
        public DeviceCode DeviceCode { get; private set; }
        public ushort NumberOfDevicePoints { get; private set; }


        public BitUnitReadData(DeviceCode deviceCode, ushort startAddress, ushort numberOfDevPoints)
        {
            DeviceCode = deviceCode ?? throw new ArgumentNullException(nameof(deviceCode));
            StartAddress = startAddress;
            NumberOfDevicePoints = numberOfDevPoints;
        }

        public override int GetHashCode()
        {
            return DeviceCode.GetHashCode() ^ StartAddress.GetHashCode() ^ NumberOfDevicePoints.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is BitUnitReadData other && DeviceCode.Equals(other.DeviceCode) && StartAddress.Equals(other.StartAddress) && NumberOfDevicePoints.Equals(other.NumberOfDevicePoints);
        }
    }
}
