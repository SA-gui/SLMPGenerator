using SLMPGenerator.Common;
using SLMPGenerator.Common.Command;
using SLMPGenerator.Write;
using SLMPGenerator.Write.Domain;
using SLMPGenerator.Write.Domain.WriteData;
using Xunit;

namespace UnitTests.Write
{
    public class UnitTest_WriteService
    {
        private readonly WriteRequest _mockWriteRequestData;

        public UnitTest_WriteService()
        {
            var deviceCode = new DeviceCode(new byte[] { 0xA8 }, "D*", DeviceType.Word, DeviceNoRange.Dec);
            var wordWriteData = new WordUnitWriteData(deviceCode, 0, new List<short> { 1, 2, 3 });
            _mockWriteRequestData = new WriteRequest(deviceCode, wordWriteData);
        }

        /// <summary>
        /// コンストラクタに有効なパラメータを渡した場合、プロパティが正しく設定されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(MessageType.Binary, 0, 255, RequestDestModuleIOType.OwnStationCPU, 0, 1.0)]
        [InlineData(MessageType.ASCII,  0, 255, RequestDestModuleIOType.ControlCPU, 1, 2.0)]
        public void Constructor_ValidParameters_SetsProperties(MessageType messageType,  ushort reqNetWorkNo, ushort reqStationNo, RequestDestModuleIOType reqIOType, ushort multiDropStationNo, double timerSec)
        {
            // Arrange
            var factory = new MockWriteRequestDataFactory(_mockWriteRequestData);

            // Act
            var writeService = new WriteService(factory, messageType,  reqNetWorkNo, reqStationNo, reqIOType, multiDropStationNo, timerSec);

            // Assert
            Assert.Equal(messageType, writeService.MessageType);
            Assert.Equal(reqNetWorkNo, writeService.ReqNetWorkNo);
            Assert.Equal(reqStationNo, writeService.ReqStationNo);
            Assert.Equal(reqIOType, writeService.ReqIOType);
            Assert.Equal(multiDropStationNo, writeService.MultiDropStationNo);
            Assert.Equal(timerSec, writeService.TimerSec);
        }

        /// <summary>
        /// 無効なMessageTypeを渡した場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Fact]
        public void Constructor_InvalidMessageType_ThrowsArgumentException()
        {
            // Arrange
            var factory = new MockWriteRequestDataFactory(_mockWriteRequestData);
            MessageType messageType = (MessageType)999;
            ushort reqNetWorkNo = 0;
            ushort reqStationNo = 255;
            RequestDestModuleIOType reqIOType = RequestDestModuleIOType.OwnStationCPU;
            ushort multiDropStationNo = 0;
            double timerSec = 1.0;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new WriteService(factory, messageType,  reqNetWorkNo, reqStationNo, reqIOType, multiDropStationNo, timerSec));
        }

        /// <summary>
        /// 無効なPLCTypeを渡した場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Fact]
        public void Constructor_InvalidPLCType_ThrowsArgumentException()
        {
            // Arrange
            var factory = new MockWriteRequestDataFactory(_mockWriteRequestData);
            MessageType messageType = MessageType.Binary;
            ushort reqNetWorkNo = 0;
            ushort reqStationNo = 255;
            RequestDestModuleIOType reqIOType = RequestDestModuleIOType.OwnStationCPU;
            ushort multiDropStationNo = 0;
            double timerSec = 1.0;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new WriteService(factory, messageType,  reqNetWorkNo, reqStationNo, reqIOType, multiDropStationNo, timerSec));
        }

        /// <summary>
        /// 無効なDeviceAccessTypeを渡した場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Fact]
        public void Constructor_InvalidDeviceAccessType_ThrowsArgumentException()
        {
            // Arrange
            var factory = new MockWriteRequestDataFactory(_mockWriteRequestData);
            MessageType messageType = MessageType.Binary;
            ushort reqNetWorkNo = 0;
            ushort reqStationNo = 255;
            RequestDestModuleIOType reqIOType = RequestDestModuleIOType.OwnStationCPU;
            ushort multiDropStationNo = 0;
            double timerSec = 1.0;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new WriteService(factory, messageType,  reqNetWorkNo, reqStationNo, reqIOType, multiDropStationNo, timerSec));
        }

        /// <summary>
        /// 無効なRequestDestModuleIOTypeを渡した場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Fact]
        public void Constructor_InvalidRequestDestModuleIOType_ThrowsArgumentException()
        {
            // Arrange
            var factory = new MockWriteRequestDataFactory(_mockWriteRequestData);
            MessageType messageType = MessageType.Binary;
            ushort reqNetWorkNo = 0;
            ushort reqStationNo = 255;
            RequestDestModuleIOType reqIOType = (RequestDestModuleIOType)999;
            ushort multiDropStationNo = 0;
            double timerSec = 1.0;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new WriteService(factory, messageType,  reqNetWorkNo, reqStationNo, reqIOType, multiDropStationNo, timerSec));
        }

        /// <summary>
        /// 有効なパラメータでメッセージを作成する場合、正しいWriteMessageオブジェクトが返されることをテストします。
        /// </summary>
        [Theory]
        [InlineData("D100", new short[] { 1, 2, 3 })]
        [InlineData("M200", new bool[] { true, false, true })]
        public void Create_ValidParameters_ReturnsCorrectWriteMessage(string rawAddress, object writeData)
        {
            // Arrange
            var factory = new MockWriteRequestDataFactory(_mockWriteRequestData);
            var writeService = new WriteService(factory, MessageType.Binary, 0, 255, RequestDestModuleIOType.OwnStationCPU, 0, 1.0);

            // Act
            WriteMessage result;
            if (writeData is short[] shortData)
            {
                result = writeService.Create(rawAddress, shortData.ToList());
            }
            else
            {
                result = writeService.Create(rawAddress, ((bool[])writeData).ToList());
            }

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WriteMessage>(result);
        }

        /// <summary>
        /// 無効なパラメータでメッセージを作成する場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Theory]
        [InlineData(100000,(ushort)RequestDestModuleIOType.OwnStationCPU)]
        [InlineData((ushort)MessageType.ASCII,100000)]
        [InlineData(10000, 100000)]
        public void Create_InvalidParameters_ThrowsArgumentException(ushort messageType,ushort reqModuleIOType)
        {
            // Arrange
            var factory = new MockWriteRequestDataFactory(_mockWriteRequestData);

            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new WriteService(factory, (MessageType)messageType, 0, 255, (RequestDestModuleIOType)reqModuleIOType, 0, 1.0));
        }
        
    }

    // Mock class for IWriteRequestDataFactory
    internal class MockWriteRequestDataFactory : IWriteRequestFactory
    {
        private readonly WriteRequest _mockWriteRequestData;

        public MockWriteRequestDataFactory(WriteRequest mockWriteRequestData)
        {
            _mockWriteRequestData = mockWriteRequestData;
        }

        public WriteRequest Create(MessageType messageType, string rawAddress, List<short> writeDataList)
        {
            return _mockWriteRequestData;
        }

        public WriteRequest Create(MessageType messageType, string rawAddress, List<bool> writeDataList)
        {
            return _mockWriteRequestData;
        }
    }
}


