using SLMPGenerator.Command;
using SLMPGenerator.Common;
using SLMPGenerator.Response;
using SLMPGenerator.UseCase;
using Xunit;

namespace SLMPGenerator.Tests.UseCase
{
    public class UnitTest_SLMPResponse
    {
        /// <summary>
        /// コンストラクタに有効なパラメータを渡した場合、プロパティが正しく設定されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(MessageType.Binary, DeviceAccessType.Bit)]
        [InlineData(MessageType.ASCII, DeviceAccessType.Word)]
        public void Constructor_ValidParameters_SetsProperties(MessageType messageType, DeviceAccessType devAccessType)
        {
            // Arrange & Act
            var slmpResponse = new SLMPResponse(messageType, devAccessType);

            // Assert
            Assert.Equal(messageType, slmpResponse.MessageType);
            Assert.Equal(devAccessType, slmpResponse.DevAccessType);
        }

        /// <summary>
        /// SLMPMessageオブジェクトを使用してコンストラクタを呼び出した場合、プロパティが正しく設定されることをテストします。
        /// </summary>
        [Fact]
        public void Constructor_WithSLMPMessage_SetsProperties()
        {
            // Arrange
            var slmpMessage = new SLMPMessage(MessageType.Binary, PLCType.Mitsubishi_R_Series, DeviceAccessType.Bit, 0, 255, RequestDestModuleIOType.OwnStationCPU, 0, 1.0);

            // Act
            var slmpResponse = new SLMPResponse(slmpMessage);

            // Assert
            Assert.Equal(slmpMessage.MessageType, slmpResponse.MessageType);
            Assert.Equal(slmpMessage.DevAccessType, slmpResponse.DevAccessType);
        }

        /// <summary>
        /// 有効なバイナリレスポンスを解析する場合、正しいデータが返されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x12, 0x34 }, 1, new short[] { 0x3412 })]
        [InlineData(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x12, 0x34, 0x56, 0x78 }, 2, new short[] { 0x3412, 0x7856 })]
        public void Resolve_ValidBinaryResponse_ReturnsCorrectData(byte[] response, ushort numberOfDevicePoints, short[] expectedData)
        {
            // Arrange
            var slmpResponse = new SLMPResponse(MessageType.Binary, DeviceAccessType.Word);

            // Act
            var result = slmpResponse.Resolve(response, numberOfDevicePoints);

            // Assert
            Assert.Equal(expectedData, result);
        }

        /// <summary>
        /// 有効なASCIIレスポンスを解析する場合、正しいデータが返されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x31, 0x32, 0x33, 0x34 }, 1, new short[] { 0x1234 })]
        [InlineData(new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38 }, 2, new short[] { 0x1234, 0x5678 })]
        public void Resolve_ValidASCIIResponse_ReturnsCorrectData(byte[] response, ushort numberOfDevicePoints, short[] expectedData)
        {
            // Arrange
            var slmpResponse = new SLMPResponse(MessageType.ASCII, DeviceAccessType.Word);

            // Act
            var result = slmpResponse.Resolve(response, numberOfDevicePoints);

            // Assert
            Assert.Equal(expectedData, result);
        }

        /// <summary>
        /// エラーコードを含むバイナリレスポンスを解析する場合、SLMPUnitErrorExceptionがスローされることをテストします。
        /// </summary>
        [Theory]
        [InlineData(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 })]
        public void Resolve_BinaryResponseWithError_ThrowsSLMPUnitErrorException(byte[] response)
        {
            // Arrange
            var slmpResponse = new SLMPResponse(MessageType.Binary, DeviceAccessType.Word);

            // Act & Assert
            Assert.Throws<SLMPUnitErrorException>(() => slmpResponse.Resolve(response, 1));
        }

        /// <summary>
        /// エラーコードを含むASCIIレスポンスを解析する場合、SLMPUnitErrorExceptionがスローされることをテストします。
        /// </summary>
        [Theory]
        [InlineData(new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x31, 0x30, 0x30, 0x30, 0x30, 0x31 })]
        public void Resolve_ASCIIResponseWithError_ThrowsSLMPUnitErrorException(byte[] response)
        {
            // Arrange
            var slmpResponse = new SLMPResponse(MessageType.ASCII, DeviceAccessType.Word);

            // Act & Assert
            Assert.Throws<SLMPUnitErrorException>(() => slmpResponse.Resolve(response, 1));
        }

        /// <summary>
        /// 同じプロパティ値を持つSLMPResponseオブジェクトが等しいと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_SameProperties_ReturnsTrue()
        {
            // Arrange
            var obj1 = new SLMPResponse(MessageType.Binary, DeviceAccessType.Word);
            var obj2 = new SLMPResponse(MessageType.Binary, DeviceAccessType.Word);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// 異なるプロパティ値を持つSLMPResponseオブジェクトが等しくないと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_DifferentProperties_ReturnsFalse()
        {
            // Arrange
            var obj1 = new SLMPResponse(MessageType.Binary, DeviceAccessType.Word);
            var obj2 = new SLMPResponse(MessageType.ASCII, DeviceAccessType.Bit);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// 同じプロパティ値を持つSLMPResponseオブジェクトが同じハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_SameProperties_ReturnsSameHashCode()
        {
            // Arrange
            var obj1 = new SLMPResponse(MessageType.Binary, DeviceAccessType.Word);
            var obj2 = new SLMPResponse(MessageType.Binary, DeviceAccessType.Word);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        /// <summary>
        /// 異なるプロパティ値を持つSLMPResponseオブジェクトが異なるハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_DifferentProperties_ReturnsDifferentHashCode()
        {
            // Arrange
            var obj1 = new SLMPResponse(MessageType.Binary, DeviceAccessType.Word);
            var obj2 = new SLMPResponse(MessageType.ASCII, DeviceAccessType.Bit);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}





