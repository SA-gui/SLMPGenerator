using SLMPGenerator.Common;
using SLMPGenerator.Write.Domain;
using SLMPGenerator.Write.Infrastructure.Mitsubishi;
using Xunit;

namespace UnitTests.Write.Infrastructure.Mitsubishi
{
    public class UnitTest_RSeriesWriteRequestFactory
    {
        /// <summary>
        /// 有効なビットデバイスの書き込みリクエストデータを作成する場合、正しいIWriteRequestDataオブジェクトが返されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(MessageType.Binary, "M100", new bool[] { true, false, true })]
        [InlineData(MessageType.ASCII, "B200", new bool[] { false, true, false, true })]
        public void Create_ValidBitDevice_ReturnsCorrectIWriteRequestData(MessageType messageType, string rawAddress, bool[] writeData)
        {
            // Arrange
            var factory = new RSeriesWriteRequestFactory();

            // Act
            var result = factory.Create(messageType, rawAddress, writeData.ToList());

            // Assert
            Assert.IsType<WriteRequest>(result);
        }

        /// <summary>
        /// 有効なワードデバイスの書き込みリクエストデータを作成する場合、正しいIWriteRequestDataオブジェクトが返されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(MessageType.Binary, "D100", new short[] { 1, 2, 3 })]
        [InlineData(MessageType.ASCII, "D200", new short[] { 4, 5, 6 })]
        public void Create_ValidWordDevice_ReturnsCorrectIWriteRequestData(MessageType messageType, string rawAddress, short[] writeData)
        {
            // Arrange
            var factory = new RSeriesWriteRequestFactory();

            // Act
            var result = factory.Create(messageType, rawAddress, writeData.ToList());

            // Assert
            Assert.IsType<WriteRequest>(result);
        }

        /// <summary>
        /// 無効なデバイス名を渡した場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Theory]
        [InlineData(MessageType.Binary, "XXX100", new short[] { 1, 2, 3 })]
        [InlineData(MessageType.ASCII, "YYY200", new bool[] { true, false, true })]
        public void Create_InvalidDeviceName_ThrowsArgumentException(MessageType messageType, string rawAddress, object writeData)
        {
            // Arrange
            var factory = new RSeriesWriteRequestFactory();

            // Act & Assert
            if (writeData is short[] shortData)
            {
                Assert.Throws<ArgumentException>(() => factory.Create(messageType, rawAddress, shortData.ToList()));
            }
            else if (writeData is bool[] boolData)
            {
                Assert.Throws<ArgumentException>(() => factory.Create(messageType, rawAddress, boolData.ToList()));
            }
        }


    }
}


