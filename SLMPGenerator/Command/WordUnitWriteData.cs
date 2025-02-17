using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Command
{
    internal class WordUnitWriteData
    {
        public ushort StartAddress { get; private set; }
        public DeviceCode DeviceCode { get; private set; }
        public ushort NumberOfDevicePoints { get; private set; }
        public IReadOnlyList<ushort> WriteDataList { get; private set; }


        public WordUnitWriteData(DeviceCode deviceCode, ushort startAddress, ushort writeData)
        {
            DeviceCode = deviceCode ?? throw new ArgumentNullException(nameof(deviceCode));
            StartAddress = startAddress;
            WriteDataList = new List<ushort> { writeData };
            NumberOfDevicePoints = (ushort)WriteDataList.Count;
        }

        public WordUnitWriteData(DeviceCode deviceCode, ushort startAddress,  IReadOnlyList<ushort> writeDataList)
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
            return obj is WordUnitWriteData other && DeviceCode.Equals(other.DeviceCode) && StartAddress.Equals(other.StartAddress) && NumberOfDevicePoints.Equals(other.NumberOfDevicePoints) && WriteDataList.SequenceEqual(other.WriteDataList);
        }
    }
}
