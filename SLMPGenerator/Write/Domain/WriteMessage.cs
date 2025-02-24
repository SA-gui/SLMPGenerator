using SLMPGenerator.Command.Mitsubishi;
using SLMPGenerator.Common;

using System.Text;

using SLMPGenerator.Write.Infrastructure.Mitsubishi;
using SLMPGenerator.Common.Command;
using SLMPGenerator.Read.Infrastructure.Mitsubishi;
using SLMPGenerator.Read.Domain;


namespace SLMPGenerator.Write.Domain
{
    internal class WriteMessage //: ISLMPMessage
    {
        public MonitoringTimer MonitoringTimer { get; private set; }
        public SubHeader SubHeader { get; private set; }
        public RequestDestNetworkNo RequestDestNetworkNo { get; private set; }
        public RequestDestStationNo RequestDestStationNo { get; private set; }
        public RequestDestModuleIONo RequestDestModuleIONo { get; private set; }
        public RequestDestMultiDropStationNo RequestDestMultiDropStationNo { get; private set; }
        public RequestLength RequestDataLength { get; private set; }
        public WriteRequest RequestData { get; private set; }

        public byte[] BinaryCode { get; private set; }
        public byte[] ASCIIBinaryCode { get; private set; }

        public WriteMessage(WriteRequest requestData,
            MonitoringTimer monitoringTimer, SubHeader subHeader,
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

        private void ValidateRequestStationNo(ushort reqNetWorkNo, ushort reqStationNo)
        {
            // ネットワーク番号が0かつステーション番号が255でない場合は例外をスローする
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
            return obj is WriteMessage other &&
                   ASCIIBinaryCode.SequenceEqual(other.ASCIIBinaryCode);
        }
    }
}
