using SLMPGenerator.Common;
using SLMPGenerator.Read.Domain;


namespace SLMPGenerator.Write.Domain
{
    internal interface IWriteRequestFactory
    {
        public WriteRequest Create(MessageType messageType, string rawAddress, List<short> writeDataList);
        public WriteRequest Create(MessageType messageType, string rawAddress, List<bool> writeDataList);

    }
}
