using SLMPGenerator.Command;
using SLMPGenerator.Common;
using Xunit;

namespace SLMPGenerator.Tests.Common
{
    public class UnitTest_RequestDataLength
    {
        /// <summary>
        /// 有効なパラメータでコンストラクタを呼び出した場合、BinaryCodeとASCIICodeが正しく設定されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(2, 4, new byte[] { 0x06, 0x00 }, "0006")]
        [InlineData(4, 8, new byte[] { 0x0C, 0x00 }, "000C")]
        public void Constructor_ValidParameters_SetsBinaryCodeAndASCIICode(int monitoringTimerLength, int requestDataLength, byte[] expectedBinaryCode, string expectedASCIICode)
        {
            // Arrange
            var monitoringTimer = new MonitoringTimerStub(monitoringTimerLength);
            var requestData = new RequestDataStub(requestDataLength);

            // Act
            var requestDataLengthObj = new RequestDataLength(monitoringTimer, requestData);

            // Assert
            Assert.Equal(expectedBinaryCode, requestDataLengthObj.BinaryCode);
            Assert.Equal(expectedASCIICode, requestDataLengthObj.ASCIICode);
        }

        /// <summary>
        /// 同じASCIICodeを持つRequestDataLengthオブジェクトが等しいと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_SameASCIICode_ReturnsTrue()
        {
            // Arrange
            var monitoringTimer = new MonitoringTimerStub(2);
            var requestData = new RequestDataStub(4);
            var obj1 = new RequestDataLength(monitoringTimer, requestData);
            var obj2 = new RequestDataLength(monitoringTimer, requestData);

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
            var monitoringTimer1 = new MonitoringTimerStub(2);
            var requestData1 = new RequestDataStub(4);
            var obj1 = new RequestDataLength(monitoringTimer1, requestData1);

            var monitoringTimer2 = new MonitoringTimerStub(4);
            var requestData2 = new RequestDataStub(8);
            var obj2 = new RequestDataLength(monitoringTimer2, requestData2);

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
            var monitoringTimer = new MonitoringTimerStub(2);
            var requestData = new RequestDataStub(4);
            var obj1 = new RequestDataLength(monitoringTimer, requestData);
            var obj2 = new RequestDataLength(monitoringTimer, requestData);

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
            var monitoringTimer1 = new MonitoringTimerStub(2);
            var requestData1 = new RequestDataStub(4);
            var obj1 = new RequestDataLength(monitoringTimer1, requestData1);

            var monitoringTimer2 = new MonitoringTimerStub(4);
            var requestData2 = new RequestDataStub(8);
            var obj2 = new RequestDataLength(monitoringTimer2, requestData2);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }

    // スタブクラス
    internal class MonitoringTimerStub : MonitoringTimer
    {
        public MonitoringTimerStub(int sec) : base(RequestDestModuleIOType.OwnStationCPU, sec)
        {

        }


    }

    internal class RequestDataStub : IRequestData
    {
        public RequestDataStub(int length)
        {
            BinaryCode = new byte[length];
            ASCIICode = new string('0', length);
        }

        public byte[] BinaryCode { get; }
        public string ASCIICode { get; }
        public DeviceType DeviceType => DeviceType.Word;
        public int Address => 0;
        public ushort NumberOfDevicePoints => 0;
    }
}

