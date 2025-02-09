using SLMPGenerator.Common;
using Xunit;

namespace SLMPGenerator.Tests.Common
{
    public class UnitTest_MonitoringTimer
    {
        /// <summary>
        /// 有効なパラメータでコンストラクタを呼び出した場合、BinaryCodeとASCIICodeが正しく設定されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(RequestDestModuleIOType.OwnStationCPU, 1.0, new byte[] { 0x04, 0x00 }, "0004")]
        [InlineData(RequestDestModuleIOType.OwnStationCPU, 0.2, new byte[] { 0x01, 0x00 }, "0001")]
        [InlineData(RequestDestModuleIOType.OwnStationCPU, 10.0, new byte[] { 0x28, 0x00 }, "0028")]
        [InlineData(RequestDestModuleIOType.MultidropConnectionCPU, 0.5, new byte[] { 0x02, 0x00 }, "0002")]
        [InlineData(RequestDestModuleIOType.MultidropConnectionCPU, 60.0, new byte[] { 0xF0, 0x00 }, "00F0")]
        public void Constructor_ValidParameters_SetsBinaryCodeAndASCIICode(RequestDestModuleIOType destinationIOType, double timerSec, byte[] expectedBinaryCode, string expectedASCIICode)
        {
            // Arrange & Act
            var timer = new MonitoringTimer(destinationIOType, timerSec);

            // Assert
            Assert.Equal(expectedBinaryCode, timer.BinaryCode);
            Assert.Equal(expectedASCIICode, timer.ASCIICode);
        }

        /// <summary>
        /// 無効なRequestDestModuleIOTypeを渡した場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Theory]
        [InlineData((RequestDestModuleIOType)9999, 1.0)]
        public void Constructor_InvalidRequestIOType_ThrowsArgumentException(RequestDestModuleIOType invalidIOType, double timerSec)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new MonitoringTimer(invalidIOType, timerSec));
        }

        /// <summary>
        /// 無効なタイマー値を渡した場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Theory]
        [InlineData(RequestDestModuleIOType.OwnStationCPU, 0.3)]
        [InlineData(RequestDestModuleIOType.OwnStationCPU, -0.1)]
        public void Constructor_InvalidTimerValue_ThrowsArgumentException(RequestDestModuleIOType destinationIOType, double invalidTimerSec)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new MonitoringTimer(destinationIOType, invalidTimerSec));
        }

        /// <summary>
        /// 範囲外のタイマー値を渡した場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Theory]
        [InlineData(RequestDestModuleIOType.OwnStationCPU, 0.1)]
        [InlineData(RequestDestModuleIOType.OwnStationCPU, 10.1)]
        [InlineData(RequestDestModuleIOType.MultidropConnectionCPU, 0.4)]
        [InlineData(RequestDestModuleIOType.MultidropConnectionCPU, 60.1)]
        public void Constructor_TimerValueOutOfRange_ThrowsArgumentException(RequestDestModuleIOType destinationIOType, double outOfRangeTimerSec)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new MonitoringTimer(destinationIOType, outOfRangeTimerSec));
        }

        /// <summary>
        /// 同じASCIICodeを持つMonitoringTimerオブジェクトが等しいと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_SameASCIICode_ReturnsTrue()
        {
            // Arrange
            var timer1 = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 1.0);
            var timer2 = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 1.0);

            // Act
            bool result = timer1.Equals(timer2);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// 異なるASCIICodeを持つMonitoringTimerオブジェクトが等しくないと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_DifferentASCIICode_ReturnsFalse()
        {
            // Arrange
            var timer1 = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 1.0);
            var timer2 = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 2.0);

            // Act
            bool result = timer1.Equals(timer2);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// 同じASCIICodeを持つMonitoringTimerオブジェクトが同じハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_SameASCIICode_ReturnsSameHashCode()
        {
            // Arrange
            var timer1 = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 1.0);
            var timer2 = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 1.0);

            // Act
            int hashCode1 = timer1.GetHashCode();
            int hashCode2 = timer2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        /// <summary>
        /// 異なるASCIICodeを持つMonitoringTimerオブジェクトが異なるハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_DifferentASCIICode_ReturnsDifferentHashCode()
        {
            // Arrange
            var timer1 = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 1.0);
            var timer2 = new MonitoringTimer(RequestDestModuleIOType.OwnStationCPU, 2.0);

            // Act
            int hashCode1 = timer1.GetHashCode();
            int hashCode2 = timer2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}
