using SLMPGenerator.Command;
using SLMPGenerator.Command.Mitsubishi;
using SLMPGenerator.UseCase;
using Xunit;

namespace SLMPGenerator.Tests.Command.Mitsubishi
{
    public class UnitTest_RSeriesRequestDataFactory
    {
        /// <summary>
        /// 有効なビットデバイスの読み取りリクエストデータを作成する場合、正しいIRequestDataオブジェクトが返されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(MessageType.Binary, "M100", 10)]
        [InlineData(MessageType.ASCII, "B200", 20)]
        public void CreateReadRequestData_ValidBitDevice_ReturnsCorrectIRequestData(MessageType messageType, string rawAddress, ushort points)
        {
            // Arrange & Act
            var result = RSeriesRequestDataFactory.CreateReadRequestData(DeviceAccessType.Bit, messageType, rawAddress, points);

            // Assert
            Assert.IsType<RSeriesReadRequestData>(result);
        }

        /// <summary>
        /// 有効なワードデバイスの読み取りリクエストデータを作成する場合、正しいIRequestDataオブジェクトが返されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(MessageType.Binary, "D100", 10)]
        [InlineData(MessageType.ASCII, "D200", 20)]
        public void CreateReadRequestData_ValidWordDevice_ReturnsCorrectIRequestData(MessageType messageType, string rawAddress, ushort points)
        {
            // Arrange & Act
            var result = RSeriesRequestDataFactory.CreateReadRequestData(DeviceAccessType.Word, messageType, rawAddress, points);

            // Assert
            Assert.IsType<RSeriesReadRequestData>(result);
        }

        /// <summary>
        /// 無効なデバイスアクセス型を渡した場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Theory]
        [InlineData((DeviceAccessType)999, MessageType.Binary, "D100", 10)]
        public void CreateReadRequestData_InvalidDeviceAccessType_ThrowsArgumentException(DeviceAccessType devReadType, MessageType messageType, string rawAddress, ushort points)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => RSeriesRequestDataFactory.CreateReadRequestData(devReadType, messageType, rawAddress, points));
        }

        /// <summary>
        /// 無効なデバイス名を渡した場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Theory]
        [InlineData(MessageType.Binary, "X100", 10)]
        [InlineData(MessageType.ASCII, "Y200", 20)]
        public void CreateReadRequestData_InvalidDeviceName_ThrowsArgumentException(MessageType messageType, string rawAddress, ushort points)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => RSeriesRequestDataFactory.CreateReadRequestData(DeviceAccessType.Word, messageType, rawAddress, points));
        }

        /// <summary>
        /// 有効なビットデバイスの読み取りリクエストデータを作成する場合、正しいIRequestDataオブジェクトが返されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(MessageType.Binary, "M100", 10)]
        [InlineData(MessageType.ASCII, "B200", 20)]
        public void CreateBitUnitReadRequestData_ValidBitDevice_ReturnsCorrectIRequestData(MessageType messageType, string rawAddress, ushort points)
        {
            // Arrange & Act
            var result = RSeriesRequestDataFactory.CreateReadRequestData(DeviceAccessType.Bit, messageType, rawAddress, points);

            // Assert
            Assert.IsType<RSeriesReadRequestData>(result);
        }

        /// <summary>
        /// 有効なワードデバイスの読み取りリクエストデータを作成する場合、正しいIRequestDataオブジェクトが返されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(MessageType.Binary, "D100", 10)]
        [InlineData(MessageType.ASCII, "D200", 20)]
        public void CreateWordUnitReadRequestData_ValidWordDevice_ReturnsCorrectIRequestData(MessageType messageType, string rawAddress, ushort points)
        {
            // Arrange & Act
            var result = RSeriesRequestDataFactory.CreateReadRequestData(DeviceAccessType.Word, messageType, rawAddress, points);

            // Assert
            Assert.IsType<RSeriesReadRequestData>(result);
        }
    }
}




