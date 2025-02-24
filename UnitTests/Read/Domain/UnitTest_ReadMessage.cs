using SLMPGenerator.Common.Command;
using SLMPGenerator.Common;

using SLMPGenerator.Read.Domain;
using Xunit;
using SLMPGenerator.Read.Infrastructure.Mitsubishi;
using SLMPGenerator.Read.Domain.ReadData;

namespace UnitTests.Read.Domain
{
    public class UnitTest_ReadMessage
    {
        /// <summary>
        /// コンストラクタに有効なパラメータを渡した場合、プロパティが正しく設定されることをテストします。
        /// </summary>
        [Fact]
        public void Constructor_ValidParameters_SetsProperties()
        {
            // Arrange
            var requestData = new MockReadRequestData();
            var monitoringTimer = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 1.0);
            var subHeader = new SubHeader();
            var requestDestNetworkNo = new RequestDestNetworkNo(0);
            var requestDestStationNo = new RequestDestStationNo(255);
            var requestDestModuleIONo = new RequestDestModuleIONo(RequestDestModuleIOType.OwnStationCPU);
            var requestDestMultiDropStationNo = new RequestDestMultiDropStationNo(0);
            var requestDataLength = new RequestLength(monitoringTimer, requestData);

            // Act
            var readMessage = new ReadMessage(requestData, monitoringTimer, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength);

            // Assert
            Assert.Equal(requestData, readMessage.RequestData);
            Assert.Equal(monitoringTimer, readMessage.MonitoringTimer);
            Assert.Equal(subHeader, readMessage.SubHeader);
            Assert.Equal(requestDestNetworkNo, readMessage.RequestDestNetworkNo);
            Assert.Equal(requestDestStationNo, readMessage.RequestDestStationNo);
            Assert.Equal(requestDestModuleIONo, readMessage.RequestDestModuleIONo);
            Assert.Equal(requestDestMultiDropStationNo, readMessage.RequestDestMultiDropStationNo);
        }

        /// <summary>
        /// 無効なネットワーク番号とステーション番号の組み合わせを渡した場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Theory]
        [InlineData(1, 118)]
        [InlineData(0, 118)]
        public void Constructor_InvalidNetworkAndStationNo_ThrowsArgumentException(ushort reqNetWorkNo, ushort reqStationNo)
        {
            // Arrange
            var requestData = new MockReadRequestData();
            var monitoringTimer = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 1.0);
            var subHeader = new SubHeader();
            var requestDestNetworkNo = new RequestDestNetworkNo(reqNetWorkNo);
            var requestDestStationNo = new RequestDestStationNo(reqStationNo);
            var requestDestModuleIONo = new RequestDestModuleIONo(RequestDestModuleIOType.OwnStationCPU);
            var requestDestMultiDropStationNo = new RequestDestMultiDropStationNo(0);
            var requestDataLength = new RequestLength(monitoringTimer, requestData);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new ReadMessage(requestData, monitoringTimer, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength));
        }

        /// <summary>
        /// バイナリメッセージが正しく生成されることをテストします。
        /// </summary>
        [Fact]
        public void CreateBinaryMessage_ValidParameters_ReturnsCorrectBinaryMessage()
        {
            // Arrange
            var requestData = new MockReadRequestData();
            var monitoringTimer = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 1.0);
            var subHeader = new SubHeader();
            var requestDestNetworkNo = new RequestDestNetworkNo(0);
            var requestDestStationNo = new RequestDestStationNo(255);
            var requestDestModuleIONo = new RequestDestModuleIONo(RequestDestModuleIOType.OwnStationCPU);
            var requestDestMultiDropStationNo = new RequestDestMultiDropStationNo(0);
            var requestDataLength = new RequestLength(monitoringTimer, requestData);
            var readMessage = new ReadMessage(requestData, monitoringTimer, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength);

            // Act
            var result = readMessage.BinaryCode;

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
            var requestData = new MockReadRequestData();
            var monitoringTimer = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 1.0);
            var subHeader = new SubHeader();
            var requestDestNetworkNo = new RequestDestNetworkNo(0);
            var requestDestStationNo = new RequestDestStationNo(255);
            var requestDestModuleIONo = new RequestDestModuleIONo(RequestDestModuleIOType.OwnStationCPU);
            var requestDestMultiDropStationNo = new RequestDestMultiDropStationNo(0);
            var requestDataLength = new RequestLength(monitoringTimer, requestData);
            var readMessage = new ReadMessage(requestData, monitoringTimer, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength);

            // Act
            var result = readMessage.ASCIIBinaryCode;

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        /// <summary>
        /// 同じプロパティ値を持つReadMessageオブジェクトが等しいと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_SameProperties_ReturnsTrue()
        {
            // Arrange
            var requestData = new MockReadRequestData();
            var monitoringTimer = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 1.0);
            var subHeader = new SubHeader();
            var requestDestNetworkNo = new RequestDestNetworkNo(0);
            var requestDestStationNo = new RequestDestStationNo(255);
            var requestDestModuleIONo = new RequestDestModuleIONo(RequestDestModuleIOType.OwnStationCPU);
            var requestDestMultiDropStationNo = new RequestDestMultiDropStationNo(0);
            var requestDataLength = new RequestLength(monitoringTimer, requestData);
            var obj1 = new ReadMessage(requestData, monitoringTimer, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength);
            var obj2 = new ReadMessage(requestData, monitoringTimer, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// 異なるプロパティ値を持つReadMessageオブジェクトが等しくないと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_DifferentProperties_ReturnsFalse()
        {
            // Arrange
            var requestData = new MockReadRequestData();
            var monitoringTimer1 = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 1.0);
            var monitoringTimer2 = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 2.0);
            var subHeader = new SubHeader();
            var requestDestNetworkNo = new RequestDestNetworkNo(0);
            var requestDestStationNo = new RequestDestStationNo(255);
            var requestDestModuleIONo = new RequestDestModuleIONo(RequestDestModuleIOType.OwnStationCPU);
            var requestDestMultiDropStationNo = new RequestDestMultiDropStationNo(0);
            var requestDataLength1 = new RequestLength(monitoringTimer1, requestData);
            var requestDataLength2 = new RequestLength(monitoringTimer2, requestData);
            var obj1 = new ReadMessage(requestData, monitoringTimer1, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength1);
            var obj2 = new ReadMessage(requestData, monitoringTimer2, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength2);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// 同じプロパティ値を持つReadMessageオブジェクトが同じハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_SameProperties_ReturnsSameHashCode()
        {
            // Arrange
            var requestData = new MockReadRequestData();
            var monitoringTimer = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 1.0);
            var subHeader = new SubHeader();
            var requestDestNetworkNo = new RequestDestNetworkNo(0);
            var requestDestStationNo = new RequestDestStationNo(255);
            var requestDestModuleIONo = new RequestDestModuleIONo(RequestDestModuleIOType.OwnStationCPU);
            var requestDestMultiDropStationNo = new RequestDestMultiDropStationNo(0);
            var requestDataLength = new RequestLength(monitoringTimer, requestData);
            var obj1 = new ReadMessage(requestData, monitoringTimer, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength);
            var obj2 = new ReadMessage(requestData, monitoringTimer, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        /// <summary>
        /// 異なるプロパティ値を持つReadMessageオブジェクトが異なるハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_DifferentProperties_ReturnsDifferentHashCode()
        {
            // Arrange
            var requestData1 = new MockReadRequestData();
            var requestData2 = new MockReadRequestData();
            var monitoringTimer = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 1.0);
            var subHeader = new SubHeader();
            var requestDestNetworkNo = new RequestDestNetworkNo(0);
            var requestDestStationNo = new RequestDestStationNo(255);
            var requestDestModuleIONo = new RequestDestModuleIONo(RequestDestModuleIOType.OwnStationCPU);
            var requestDestMultiDropStationNo = new RequestDestMultiDropStationNo(0);
            var requestDataLength1 = new RequestLength(monitoringTimer, requestData1);
            var requestDataLength2 = new RequestLength(monitoringTimer, requestData2);
            var obj1 = new ReadMessage(requestData1, monitoringTimer, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength1);
            var obj2 = new ReadMessage(requestData2, monitoringTimer, subHeader, requestDestNetworkNo, requestDestStationNo, requestDestModuleIONo, requestDestMultiDropStationNo, requestDataLength2);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }

    // Mock class for ReadRequestData
    internal class MockReadRequestData : ReadRequest
    {
        public MockReadRequestData() : base(new DeviceCode(new byte[] { 0xA8 }, "D*", DeviceType.Word, DeviceNoRange.Dec), new WordUnitReadData(new DeviceCode(new byte[] { 0xA8 }, "D*", DeviceType.Word, DeviceNoRange.Dec), 0, 10))
        {
        }
    }
}

