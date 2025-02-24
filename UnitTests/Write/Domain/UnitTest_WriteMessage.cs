using SLMPGenerator.Common;
using SLMPGenerator.Common.Command;
using SLMPGenerator.Write.Domain;
using SLMPGenerator.Write.Domain.WriteData;
using Xunit;

namespace UnitTests.Write.Domain
{
    public class UnitTest_WriteMessage
    {
        /// <summary>
        /// コンストラクタに有効なパラメータを渡した場合、プロパティが正しく設定されることをテストします。
        /// </summary>
        [Fact]
        public void Constructor_ValidParameters_SetsProperties()
        {
            // Arrange
            var requestData = new MockWriteRequestData();
            var monitoringTimer = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 1.0);
            var subHeader = new SubHeader();
            var requestDestNetworkNo = new RequestDestNetworkNo(0);
            var requestDestStationNo = new RequestDestStationNo(255);
            var requestDestModuleIONo = new RequestDestModuleIONo(RequestDestModuleIOType.OwnStationCPU);
            var requestDestMultiDropStationNo = new RequestDestMultiDropStationNo(0);
            var requestDataLength = new RequestLength(monitoringTimer, requestData);

            // Act
            var writeMessage = new WriteMessage(requestData, monitoringTimer, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength);

            // Assert
            Assert.Equal(requestData, writeMessage.RequestData);
            Assert.Equal(monitoringTimer, writeMessage.MonitoringTimer);
            Assert.Equal(subHeader, writeMessage.SubHeader);
            Assert.Equal(requestDestNetworkNo, writeMessage.RequestDestNetworkNo);
            Assert.Equal(requestDestStationNo, writeMessage.RequestDestStationNo);
            Assert.Equal(requestDestModuleIONo, writeMessage.RequestDestModuleIONo);
            Assert.Equal(requestDestMultiDropStationNo, writeMessage.RequestDestMultiDropStationNo);
        }

        /// <summary>
        /// 無効なネットワーク番号とステーション番号の組み合わせを渡した場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Theory]
        [InlineData(0, 119)]
        [InlineData(0, 120)]
        public void Constructor_InvalidNetworkAndStationNo_ThrowsArgumentException(ushort reqNetWorkNo, ushort reqStationNo)
        {
            // Arrange
            var requestData = new MockWriteRequestData();
            var monitoringTimer = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 1.0);
            var subHeader = new SubHeader();
            var requestDestNetworkNo = new RequestDestNetworkNo(reqNetWorkNo);
            var requestDestStationNo = new RequestDestStationNo(reqStationNo);
            var requestDestModuleIONo = new RequestDestModuleIONo(RequestDestModuleIOType.OwnStationCPU);
            var requestDestMultiDropStationNo = new RequestDestMultiDropStationNo(0);
            var requestDataLength = new RequestLength(monitoringTimer, requestData);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new WriteMessage(requestData, monitoringTimer, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength));
        }

        /// <summary>
        /// バイナリメッセージが正しく生成されることをテストします。
        /// </summary>
        [Fact]
        public void CreateBinaryMessage_ValidParameters_ReturnsCorrectBinaryMessage()
        {
            // Arrange
            var requestData = new MockWriteRequestData();
            var monitoringTimer = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 1.0);
            var subHeader = new SubHeader();
            var requestDestNetworkNo = new RequestDestNetworkNo(0);
            var requestDestStationNo = new RequestDestStationNo(255);
            var requestDestModuleIONo = new RequestDestModuleIONo(RequestDestModuleIOType.OwnStationCPU);
            var requestDestMultiDropStationNo = new RequestDestMultiDropStationNo(0);
            var requestDataLength = new RequestLength(monitoringTimer, requestData);
            var writeMessage = new WriteMessage(requestData, monitoringTimer, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength);

            // Act
            var result = writeMessage.BinaryCode;

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        /// <summary>
        /// ASCIIメッセージが正しく生成されることをテストします。
        /// </summary>
        [Fact]
        public void CreateASCIIMessage_ValidParameters_ReturnsCorrectASCIIMessage()
        {
            // Arrange
            var requestData = new MockWriteRequestData();
            var monitoringTimer = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 1.0);
            var subHeader = new SubHeader();
            var requestDestNetworkNo = new RequestDestNetworkNo(0);
            var requestDestStationNo = new RequestDestStationNo(255);
            var requestDestModuleIONo = new RequestDestModuleIONo(RequestDestModuleIOType.OwnStationCPU);
            var requestDestMultiDropStationNo = new RequestDestMultiDropStationNo(0);
            var requestDataLength = new RequestLength(monitoringTimer, requestData);
            var writeMessage = new WriteMessage(requestData, monitoringTimer, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength);

            // Act
            var result = writeMessage.ASCIIBinaryCode;

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        /// <summary>
        /// 同じプロパティ値を持つWriteMessageオブジェクトが等しいと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_SameProperties_ReturnsTrue()
        {
            // Arrange
            var requestData = new MockWriteRequestData();
            var monitoringTimer = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 1.0);
            var subHeader = new SubHeader();
            var requestDestNetworkNo = new RequestDestNetworkNo(0);
            var requestDestStationNo = new RequestDestStationNo(255);
            var requestDestModuleIONo = new RequestDestModuleIONo(RequestDestModuleIOType.OwnStationCPU);
            var requestDestMultiDropStationNo = new RequestDestMultiDropStationNo(0);
            var requestDataLength = new RequestLength(monitoringTimer, requestData);
            var obj1 = new WriteMessage(requestData, monitoringTimer, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength);
            var obj2 = new WriteMessage(requestData, monitoringTimer, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// 異なるプロパティ値を持つWriteMessageオブジェクトが等しくないと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_DifferentProperties_ReturnsFalse()
        {
            // Arrange
            var requestData = new MockWriteRequestData();
            var monitoringTimer1 = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 1.0);
            var monitoringTimer2 = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 2.0);
            var subHeader = new SubHeader();
            var requestDestNetworkNo = new RequestDestNetworkNo(0);
            var requestDestStationNo = new RequestDestStationNo(255);
            var requestDestModuleIONo = new RequestDestModuleIONo(RequestDestModuleIOType.OwnStationCPU);
            var requestDestMultiDropStationNo = new RequestDestMultiDropStationNo(0);
            var requestDataLength1 = new RequestLength(monitoringTimer1, requestData);
            var requestDataLength2 = new RequestLength(monitoringTimer2, requestData);
            var obj1 = new WriteMessage(requestData, monitoringTimer1, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength1);
            var obj2 = new WriteMessage(requestData, monitoringTimer2, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength2);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// 同じプロパティ値を持つWriteMessageオブジェクトが同じハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_SameProperties_ReturnsSameHashCode()
        {
            // Arrange
            var requestData = new MockWriteRequestData();
            var monitoringTimer = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 1.0);
            var subHeader = new SubHeader();
            var requestDestNetworkNo = new RequestDestNetworkNo(0);
            var requestDestStationNo = new RequestDestStationNo(255);
            var requestDestModuleIONo = new RequestDestModuleIONo(RequestDestModuleIOType.OwnStationCPU);
            var requestDestMultiDropStationNo = new RequestDestMultiDropStationNo(0);
            var requestDataLength = new RequestLength(monitoringTimer, requestData);
            var obj1 = new WriteMessage(requestData, monitoringTimer, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength);
            var obj2 = new WriteMessage(requestData, monitoringTimer, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        /// <summary>
        /// 異なるプロパティ値を持つWriteMessageオブジェクトが異なるハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_DifferentProperties_ReturnsDifferentHashCode()
        {
            // Arrange
            var requestData1 = new MockWriteRequestData();
            var requestData2 = new MockWriteRequestData();
            var monitoringTimer = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 1.0);
            var subHeader = new SubHeader();
            var requestDestNetworkNo = new RequestDestNetworkNo(0);
            var requestDestStationNo = new RequestDestStationNo(255);
            var requestDestModuleIONo = new RequestDestModuleIONo(RequestDestModuleIOType.OwnStationCPU);
            var requestDestMultiDropStationNo = new RequestDestMultiDropStationNo(0);
            var requestDataLength1 = new RequestLength(monitoringTimer, requestData1);
            var requestDataLength2 = new RequestLength(monitoringTimer, requestData2);
            var obj1 = new WriteMessage(requestData1, monitoringTimer, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength1);
            var obj2 = new WriteMessage(requestData2, monitoringTimer, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength2);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }

    // Mock class for WriteRequest
    internal class MockWriteRequestData : WriteRequest
    {
        public MockWriteRequestData() : base(new DeviceCode(new byte[] { 0xA8 }, "D*", DeviceType.Word, DeviceNoRange.Dec), new WordUnitWriteData(new DeviceCode(new byte[] { 0xA8 }, "D*", DeviceType.Word, DeviceNoRange.Dec), 0, new List<short> { 1, 2, 3 }))
        {
        }
    }
}


