using SLMPGenerator.Common;
using SLMPGenerator.Common.Command;
using SLMPGenerator.Read;
using SLMPGenerator.Read.Domain;
using SLMPGenerator.Read.Domain.ReadData;
using Xunit;

namespace UnitTests.Read
{
    public class UnitTest_ReadService
    {
        private readonly ReadRequest _mockReadRequestData;

        public UnitTest_ReadService()
        {
            var deviceCode = new DeviceCode(new byte[] { 0xA8 }, "D*", DeviceType.Word, DeviceNoRange.Dec);
            var wordReadData = new WordUnitReadData(deviceCode, 0, 10);
            _mockReadRequestData = new ReadRequest(deviceCode, wordReadData);
        }

        /// <summary>
        /// コンストラクタに有効なパラメータを渡した場合、プロパティが正しく設定されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(MessageType.Binary, DeviceAccessType.Bit, 0, 255, RequestDestModuleIOType.OwnStationCPU, 0, 1.0)]
        [InlineData(MessageType.ASCII,  DeviceAccessType.Word, 0, 255, RequestDestModuleIOType.ControlCPU, 1, 2.0)]
        public void Constructor_ValidParameters_SetsProperties(MessageType messageType,  DeviceAccessType devAccessType, ushort reqNetWorkNo, ushort reqStationNo, RequestDestModuleIOType reqIOType, ushort multiDropStationNo, double timerSec)
        {
            // Arrange
            var factory = new MockReadRequestDataFactory(_mockReadRequestData);

            // Act
            var readService = new ReadService(factory, messageType, devAccessType, reqNetWorkNo, reqStationNo, reqIOType, multiDropStationNo, timerSec);

            // Assert
            Assert.Equal(messageType, readService.MessageType);
            Assert.Equal(devAccessType, readService.DevAccessType);
            Assert.Equal(reqNetWorkNo, readService.ReqNetWorkNo);
            Assert.Equal(reqStationNo, readService.ReqStationNo);
            Assert.Equal(reqIOType, readService.ReqIOType);
            Assert.Equal(multiDropStationNo, readService.MultiDropStationNo);
            Assert.Equal(timerSec, readService.TimerSec);
        }

        /// <summary>
        /// 無効なMessageTypeを渡した場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Fact]
        public void Constructor_InvalidMessageType_ThrowsArgumentException()
        {
            // Arrange
            var factory = new MockReadRequestDataFactory(_mockReadRequestData);
            MessageType messageType = (MessageType)999;
            DeviceAccessType devAccessType = DeviceAccessType.Bit;
            ushort reqNetWorkNo = 0;
            ushort reqStationNo = 255;
            RequestDestModuleIOType reqIOType = RequestDestModuleIOType.OwnStationCPU;
            ushort multiDropStationNo = 0;
            double timerSec = 1.0;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new ReadService(factory, messageType, devAccessType, reqNetWorkNo, reqStationNo, reqIOType, multiDropStationNo, timerSec));
        }

        /// <summary>
        /// 無効なPLCTypeを渡した場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Fact]
        public void Constructor_InvalidPLCType_ThrowsArgumentException()
        {
            // Arrange
            var factory = new MockReadRequestDataFactory(_mockReadRequestData);
            MessageType messageType = MessageType.Binary;
            DeviceAccessType devAccessType = DeviceAccessType.Bit;
            ushort reqNetWorkNo = 0;
            ushort reqStationNo = 255;
            RequestDestModuleIOType reqIOType = RequestDestModuleIOType.OwnStationCPU;
            ushort multiDropStationNo = 0;
            double timerSec = 1.0;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new ReadService(factory, messageType,devAccessType, reqNetWorkNo, reqStationNo, reqIOType, multiDropStationNo, timerSec));
        }

        /// <summary>
        /// 無効なDeviceAccessTypeを渡した場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Fact]
        public void Constructor_InvalidDeviceAccessType_ThrowsArgumentException()
        {
            // Arrange
            var factory = new MockReadRequestDataFactory(_mockReadRequestData);
            MessageType messageType = MessageType.Binary;
            DeviceAccessType devAccessType = (DeviceAccessType)999;
            ushort reqNetWorkNo = 0;
            ushort reqStationNo = 255;
            RequestDestModuleIOType reqIOType = RequestDestModuleIOType.OwnStationCPU;
            ushort multiDropStationNo = 0;
            double timerSec = 1.0;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new ReadService(factory, messageType, devAccessType, reqNetWorkNo, reqStationNo, reqIOType, multiDropStationNo, timerSec));
        }

        /// <summary>
        /// 無効なRequestDestModuleIOTypeを渡した場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Fact]
        public void Constructor_InvalidRequestDestModuleIOType_ThrowsArgumentException()
        {
            // Arrange
            var factory = new MockReadRequestDataFactory(_mockReadRequestData);
            MessageType messageType = MessageType.Binary;
            DeviceAccessType devAccessType = DeviceAccessType.Bit;
            ushort reqNetWorkNo = 0;
            ushort reqStationNo = 255;
            RequestDestModuleIOType reqIOType = (RequestDestModuleIOType)999;
            ushort multiDropStationNo = 0;
            double timerSec = 1.0;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new ReadService(factory, messageType,devAccessType, reqNetWorkNo, reqStationNo, reqIOType, multiDropStationNo, timerSec));
        }

        /// <summary>
        /// 有効なパラメータでメッセージを作成する場合、正しいReadMessageオブジェクトが返されることをテストします。
        /// </summary>
        [Theory]
        [InlineData("D100", 10)]
        [InlineData("M200", 20)]
        public void Create_ValidParameters_ReturnsCorrectReadMessage(string rawAddress, ushort points)
        {
            // Arrange
            var factory = new MockReadRequestDataFactory(_mockReadRequestData);
            var readService = new ReadService(factory, MessageType.Binary,  DeviceAccessType.Word, 0, 255, RequestDestModuleIOType.OwnStationCPU, 0, 1.0);

            // Act
            var result = readService.Create(rawAddress, points);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ReadMessage>(result);
        }

        /// <summary>
        /// 無効なパラメータでメッセージを作成する場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Fact]
        public void Create_InvalidParameters_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ReadService(null, MessageType.Binary, DeviceAccessType.Word, 0, 255, RequestDestModuleIOType.OwnStationCPU, 0, 1.0));
        }
    }

    // Mock class for IReadRequestDataFactory
    internal class MockReadRequestDataFactory : IReadRequestFactory
    {
        private readonly ReadRequest _mockReadRequestData;

        public MockReadRequestDataFactory(ReadRequest mockReadRequestData)
        {
            _mockReadRequestData = mockReadRequestData;
        }

        public ReadRequest Create(DeviceAccessType devAccessType, MessageType messageType, string rawAddress, ushort points)
        {
            return _mockReadRequestData;
        }
    }
}

