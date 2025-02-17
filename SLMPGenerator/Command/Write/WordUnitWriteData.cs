using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Command.Write
{
    internal class WordUnitWriteData
    {
        public ushort StartAddress { get; private set; }
        public DeviceCode DeviceCode { get; private set; }
        public ushort NumberOfDevicePoints { get; private set; }
        public IReadOnlyList<short> WriteDataList { get; private set; }

        public WordUnitWriteData(DeviceCode deviceCode, ushort startAddress, short writeData)
            : this(deviceCode, startAddress, new List<short> { writeData })
        {
        }

        public WordUnitWriteData(DeviceCode deviceCode, ushort startAddress, IReadOnlyList<short> writeDataList)
        {
            DeviceCode = deviceCode ?? throw new ArgumentNullException(nameof(deviceCode));
            StartAddress = startAddress;
            WriteDataList = writeDataList ?? throw new ArgumentNullException(nameof(writeDataList));
            NumberOfDevicePoints = (ushort)writeDataList.Count;
        }

        public override int GetHashCode()
        {
            return DeviceCode.GetHashCode() ^ StartAddress.GetHashCode() ^ NumberOfDevicePoints.GetHashCode() ^ WriteDataList.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is WordUnitWriteData other && StartAddress.Equals(other.StartAddress) && DeviceCode.Equals(other.DeviceCode) && NumberOfDevicePoints.Equals(other.NumberOfDevicePoints) && WriteDataList.SequenceEqual(other.WriteDataList);
        }
    }
}
