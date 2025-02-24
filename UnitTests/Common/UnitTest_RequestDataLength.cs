using SLMPGenerator.Common;
using SLMPGenerator.Common.Command;
using SLMPGenerator.Read.Domain;
using Xunit;

namespace UnitTests.Common
{
    public class UnitTest_RequestDataLength
    {
        private readonly MonitoringTimer _mockMonitoringTimer;

        public UnitTest_RequestDataLength()
        {
            _mockMonitoringTimer = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 1.0);
        }

        /// <summary>
        /// コンストラクタに有効なパラメータを渡した場合、BinaryCodeとASCIICodeが正しく設定されることをテストします。
        /// </summary>
        [Fact]
        public void Constructor_ValidParameters_SetsBinaryCodeAndASCIICode()
        {
            // Arrange
            var requestData = new MockRequestData();

            // Act
            var requestDataLength = new RequestLength(_mockMonitoringTimer, requestData);

            // Assert
            Assert.Equal(new byte[] { 0x04, 0x00 }, requestDataLength.BinaryCode);
            Assert.Equal("0008", requestDataLength.ASCIICode);
        }

        /// <summary>
        /// 同じASCIICodeを持つRequestDataLengthオブジェクトが等しいと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_SameASCIICode_ReturnsTrue()
        {
            // Arrange
            var requestData = new MockRequestData();
            var obj1 = new RequestLength(_mockMonitoringTimer, requestData);
            var obj2 = new RequestLength(_mockMonitoringTimer, requestData);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// 異なるASCIICodeを持つRequestDataLengthオブジェクトが等しくないと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_DifferentASCIICode_ReturnsFalse()
        {
            // Arrange
            var requestData1 = new MockRequestData();
            var requestData2 = new MockRequestData { ASCIICode = "001" };
            var obj1 = new RequestLength(_mockMonitoringTimer, requestData1);
            var obj2 = new RequestLength(_mockMonitoringTimer, requestData2);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// 同じASCIICodeを持つRequestDataLengthオブジェクトが同じハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_SameASCIICode_ReturnsSameHashCode()
        {
            // Arrange
            var requestData = new MockRequestData();
            var obj1 = new RequestLength(_mockMonitoringTimer, requestData);
            var obj2 = new RequestLength(_mockMonitoringTimer, requestData);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        /// <summary>
        /// 異なるASCIICodeを持つRequestDataLengthオブジェクトが異なるハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_DifferentASCIICode_ReturnsDifferentHashCode()
        {
            // Arrange
            var requestData1 = new MockRequestData();
            var requestData2 = new MockRequestData { ASCIICode = "001" };
            var obj1 = new RequestLength(_mockMonitoringTimer, requestData1);
            var obj2 = new RequestLength(_mockMonitoringTimer, requestData2);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }



    // Mock class for IRequestData
    internal class MockRequestData : IRequest
    {
        public byte[] Command => new byte[] { 0x00, 0x01, 0x02, 0x03 };
        public byte[] SubCommand => new byte[] { 0x00, 0x01 };
        public byte[] BinaryAddress => new byte[] { 0x00, 0x01, 0x02 };
        public DeviceCode DeviceCode => new DeviceCode(new byte[] { 0xA8 }, "D*", DeviceType.Word, DeviceNoRange.Dec);
        public byte[] BinaryDevicePoints => new byte[] { 0x00, 0x0A };
        public int StartAddress => 0;
        public string ASCIIStartAddress => "000000";
        public ushort NumberOfDevicePoints => 10;
        public byte[] BinaryCode => new byte[] { 0x0A, 0x00 };
        public string ASCIICode { get; set; } = "000A";
    }
}


