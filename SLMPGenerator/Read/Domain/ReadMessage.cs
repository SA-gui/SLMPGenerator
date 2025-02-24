using SLMPGenerator.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SLMPGenerator.Read.Domain
{
    internal class ReadMessage// : ISLMPMessage
    {
        public MonitoringTimer MonitoringTimer { get; private set; }
        public SubHeader SubHeader { get; private set; }
        public RequestDestNetworkNo RequestDestNetworkNo { get; private set; }
        public RequestDestStationNo RequestDestStationNo { get; private set; }
        public RequestDestModuleIONo RequestDestModuleIONo { get; private set; }
        public RequestDestMultiDropStationNo RequestDestMultiDropStationNo { get; private set; }
        public RequestLength RequestDataLength { get; private set; }
        public ReadRequest RequestData { get; private set; }

        public byte[] BinaryCode { get; private set; }
        public byte[] ASCIIBinaryCode { get; private set; }

        public ReadMessage(
            ReadRequest requestData, 
            MonitoringTimer monitoringTimer, 
            SubHeader subHeader, 
            RequestDestNetworkNo requestDestNetworkNo, RequestDestStationNo requestDestStationNo, 
            RequestDestModuleIONo requestDestModuleIONo, RequestDestMultiDropStationNo requestDestMultiDropStationNo,
            RequestLength requestDataLength)
            
        {
            ValidateRequestStationNo(requestDestNetworkNo.Value, requestDestStationNo.Value);
            MonitoringTimer = monitoringTimer ?? throw new ArgumentNullException(nameof(monitoringTimer));
            SubHeader = subHeader ?? throw new ArgumentNullException(nameof(subHeader));
            RequestDestNetworkNo = requestDestNetworkNo ?? throw new ArgumentNullException(nameof(requestDestNetworkNo));
            RequestDestStationNo = requestDestStationNo ?? throw new ArgumentNullException(nameof(requestDestStationNo));
            RequestDestModuleIONo = requestDestModuleIONo ?? throw new ArgumentNullException(nameof(requestDestModuleIONo));
            RequestDestMultiDropStationNo = requestDestMultiDropStationNo ?? throw new ArgumentNullException(nameof(requestDestMultiDropStationNo));
            RequestData = requestData ?? throw new ArgumentNullException(nameof(requestData));

            RequestDataLength = requestDataLength ?? throw new ArgumentNullException(nameof(requestDataLength));

            BinaryCode = CreateBinaryMessage();
            ASCIIBinaryCode = Encoding.ASCII.GetBytes(CreateASCIIMessage());
        }

        private static void ValidateRequestStationNo(ushort reqNetWorkNo, ushort reqStationNo)
        {
            if (!(reqNetWorkNo == 0 && reqStationNo == 255))
            {
                throw new ArgumentException("Invalid combination of reqNetWorkNo and reqStationNo");
            }
        }


        private byte[] CreateBinaryMessage()
        {
            return new byte[] { }
                            .Concat(SubHeader.BinaryCode)//subheader
                            .Concat(RequestDestNetworkNo.BinaryCode)//networkNo
                            .Concat(RequestDestStationNo.BinaryCode)//stationNo
                            .Concat(RequestDestModuleIONo.BinaryCode)//IONo
                            .Concat(RequestDestMultiDropStationNo.BinaryCode)//multiDropStationNo
                            .Concat(RequestDataLength.BinaryCode)//dataLength
                            .Concat(MonitoringTimer.BinaryCode)
                            .Concat(RequestData.BinaryCode)
                            .ToArray();
        }

        private string CreateASCIIMessage()
        {
            return new StringBuilder()
                    .Append(SubHeader.ASCIICode)
                    .Append(RequestDestNetworkNo.ASCIICode)
                    .Append(RequestDestStationNo.ASCIICode)
                    .Append(RequestDestModuleIONo.ASCIICode)
                    .Append(RequestDestMultiDropStationNo.ASCIICode)
                    .Append(RequestDataLength.ASCIICode)
                    .Append(MonitoringTimer.ASCIICode)
                    .Append(RequestData.ASCIICode)
                    .ToString();
        }


        public override int GetHashCode()
        {
            return CreateASCIIMessage().GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is ReadMessage other &&
                   ASCIIBinaryCode.SequenceEqual(other.ASCIIBinaryCode);
        }
    }
}

