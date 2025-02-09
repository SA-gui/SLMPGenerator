using SLMPGenerator.Command;
using SLMPGenerator.UseCase;
using Xunit;

namespace SLMPGenerator.Tests.Command
{
    public class UnitTest_AddressHelper
    {
        /// <summary>
        /// 有効なアドレスを分割する場合、デバイス名とアドレスが正しく返されることをテストします。
        /// </summary>
        [Theory]
        [InlineData("D100", "D", (ushort)100)]
        [InlineData("M200", "M", (ushort)200)]
        [InlineData("X300", "X", (ushort)300)]
        public void SplitAddress_ValidAddress_ReturnsDeviceAndAddress(string rawAddress, string expectedDevice, ushort expectedAddress)
        {
            // Arrange & Act
            var result = AddressHelper.SplitAddress(rawAddress);

            // Assert
            Assert.Equal((expectedDevice, expectedAddress), result);
        }

        /// <summary>
        /// 無効なアドレスを分割する場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Theory]
        [InlineData("D")]
        [InlineData("100")]
        [InlineData("D100X")]
        public void SplitAddress_InvalidAddress_ThrowsArgumentException(string rawAddress)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => AddressHelper.SplitAddress(rawAddress));
        }

        /// <summary>
        /// 有効なデバイスポイントを検証する場合、例外がスローされないことをテストします。
        /// </summary>
        [Theory]
        [InlineData(MessageType.Binary, DeviceType.Bit, 7168)]
        [InlineData(MessageType.Binary, DeviceType.Word, 960)]
        [InlineData(MessageType.Binary, DeviceType.DoubleWord, 960)]
        [InlineData(MessageType.ASCII, DeviceType.Bit, 3584)]
        [InlineData(MessageType.ASCII, DeviceType.Word, 960)]
        [InlineData(MessageType.ASCII, DeviceType.DoubleWord, 960)]
        internal void ValidateDevPoints_ValidPoints_DoesNotThrowException(MessageType messageType, DeviceType deviceType, ushort points)
        {
            // Arrange & Act & Assert
            AddressHelper.ValidateDevPoints(messageType, deviceType, points);
        }

        /// <summary>
        /// 無効なデバイスポイントを検証する場合、ArgumentOutOfRangeExceptionがスローされることをテストします。
        /// </summary>
        [Theory]
        [InlineData(MessageType.Binary, DeviceType.Bit, 7169)]
        [InlineData(MessageType.Binary, DeviceType.Word, 961)]
        [InlineData(MessageType.Binary, DeviceType.DoubleWord, 961)]
        [InlineData(MessageType.ASCII, DeviceType.Bit, 3585)]
        [InlineData(MessageType.ASCII, DeviceType.Word, 961)]
        [InlineData(MessageType.ASCII, DeviceType.DoubleWord, 961)]
        internal void ValidateDevPoints_InvalidPoints_ThrowsArgumentOutOfRangeException(MessageType messageType, DeviceType deviceType, ushort points)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => AddressHelper.ValidateDevPoints(messageType, deviceType, points));
        }
    }
}


