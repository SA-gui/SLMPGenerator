using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLMPGenerator.Command.Write
{
    internal class BitUnitWriteData
    {
        public ushort StartAddress { get; private set; }
        public DeviceCode DeviceCode { get; private set; }
        public ushort NumberOfDevicePoints { get; private set; }
        public IReadOnlyList<bool> WriteDataList { get; private set; }


        public BitUnitWriteData(DeviceCode deviceCode, ushort startAddress, bool writeData)
            : this(deviceCode, startAddress, new List<bool> { writeData })
        {
        }

        public BitUnitWriteData(DeviceCode deviceCode, ushort startAddress, IReadOnlyList<bool> writeDataList)
        {
            DeviceCode = deviceCode ?? throw new ArgumentNullException(nameof(deviceCode));
            StartAddress = startAddress;
            WriteDataList = writeDataList ?? throw new ArgumentNullException(nameof(writeDataList));
            NumberOfDevicePoints = (ushort)writeDataList.Count;
        }

        public override int GetHashCode()
        {
            int hashCode = DeviceCode.GetHashCode() ^ StartAddress.GetHashCode() ^ NumberOfDevicePoints.GetHashCode();
            foreach (var data in WriteDataList)
            {
                hashCode = hashCode ^ data.GetHashCode();
            }
            return hashCode;
        }

        public override bool Equals(object? obj)
        {
            
            return obj is BitUnitWriteData other && StartAddress.Equals(other.StartAddress) && DeviceCode.Equals(other.DeviceCode) && NumberOfDevicePoints.Equals(other.NumberOfDevicePoints) && WriteDataList.SequenceEqual(other.WriteDataList);
        }
    }
}
