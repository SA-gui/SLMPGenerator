using SLMPGenerator.Command.Mitsubishi;
using SLMPGenerator.Common;
using SLMPGenerator.Read.Domain;
using SLMPGenerator.Read.Infrastructure.Mitsubishi;
using Xunit;

namespace UnitTests.Read.Infrastructure.Mitsubishi
{
    public class UnitTest_RSeriesReadRequestFactory
    {
        /// <summary>
        /// 有効なビットデバイスの読み取りリクエストデータを作成する場合、正しいIRequestDataオブジェクトが返されることをテストします。 
        /// </summary>
        [Theory]
        [InlineData(MessageType.Binary, "M100", 10)]
        [InlineData(MessageType.ASCII, "B200", 20)]
        public void Create_ValidBitDevice_ReturnsCorrectIRequestData(MessageType messageType, string rawAddress, ushort points)
        {
            // Arrange
            var factory = new RSeriesReadRequestFactory();

            // Act
            var result = factory.Create(DeviceAccessType.Bit, messageType, rawAddress, points);

            // Assert
            Assert.IsType<ReadRequest>(result);
        }

        /// <summary>
        /// 有効なワードデバイスの読み取りリクエストデータを作成する場合、正しいIRequestDataオブジェクトが返されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(MessageType.Binary, "D100", 10)]
        [InlineData(MessageType.ASCII, "D200", 20)]
        public void Create_ValidWordDevice_ReturnsCorrectIRequestData(MessageType messageType, string rawAddress, ushort points)
        {
            // Arrange
            var factory = new RSeriesReadRequestFactory();

            // Act
            var result = factory.Create(DeviceAccessType.Word, messageType, rawAddress, points);

            // Assert
            Assert.IsType<ReadRequest>(result);
        }

        /// <summary>
        /// 無効なデバイスアクセス型を渡した場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Theory]
        [InlineData((DeviceAccessType)999, MessageType.Binary, "D100", 10)]
        public void Create_InvalidDeviceAccessType_ThrowsArgumentException(DeviceAccessType devReadType, MessageType messageType, string rawAddress, ushort points)
        {
            // Arrange
            var factory = new RSeriesReadRequestFactory();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => factory.Create(devReadType, messageType, rawAddress, points));
        }

        /// <summary>
        /// 無効なデバイス名を渡した場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Theory]
        [InlineData(DeviceAccessType.Bit, MessageType.Binary, "XXX100", 10)]
        [InlineData(DeviceAccessType.Word, MessageType.ASCII, "YYY200", 20)]
        public void Create_InvalidDeviceName_ThrowsArgumentException(DeviceAccessType devReadType, MessageType messageType, string rawAddress, ushort points)
        {
            // Arrange
            var factory = new RSeriesReadRequestFactory();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => factory.Create(devReadType, messageType, rawAddress, points));
        }
    }
}

