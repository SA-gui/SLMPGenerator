using SLMPGenerator.Common;
using SLMPGenerator.Common.Response;
using SLMPGenerator.Write.Domain;
using System.Text;
using Xunit;

namespace UnitTests.Write.Domain
{
    public class UnitTest_WriteResponseResolver
    {
        /// <summary>
        /// コンストラクタに有効なパラメータを渡した場合、プロパティが正しく設定されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(MessageType.Binary, DeviceAccessType.Bit, 10)]
        [InlineData(MessageType.ASCII, DeviceAccessType.Word, 20)]
        public void Constructor_ValidParameters_SetsProperties(MessageType messageType, DeviceAccessType devAccessType, ushort numberOfDevicePoints)
        {
            // Act
            var writeResponseResolver = new WriteResponseResolver(messageType, devAccessType, numberOfDevicePoints);

            // Assert
            Assert.Equal(messageType, writeResponseResolver.MessageType);
            Assert.Equal(devAccessType, writeResponseResolver.DeviceAccessType);
            Assert.Equal(numberOfDevicePoints, writeResponseResolver.NumberOfDevicePoints);
        }

        /// <summary>
        /// ASCIIメッセージの応答が正常である場合、例外がスローされないことをテストします。
        /// </summary>
        [Fact]
        public void Resolve_ASCIIResponseNormal_DoesNotThrowException()
        {
            // Arrange
            var writeResponseResolver = new WriteResponseResolver(MessageType.ASCII, DeviceAccessType.Word, 10);
            byte[] response = Encoding.ASCII.GetBytes("D0000000000000000000000000000000");

            // Act & Assert
            var exception = Record.Exception(() => writeResponseResolver.Resolve(response));
            Assert.Null(exception);
        }

        /// <summary>
        /// バイナリメッセージの応答が正常である場合、例外がスローされないことをテストします。
        /// </summary>
        [Fact]
        public void Resolve_BinaryResponseNormal_DoesNotThrowException()
        {
            // Arrange
            var writeResponseResolver = new WriteResponseResolver(MessageType.Binary, DeviceAccessType.Word, 10);
            byte[] response = new byte[] { 0xD0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            // Act & Assert
            var exception = Record.Exception(() => writeResponseResolver.Resolve(response));
            Assert.Null(exception);
        }

        /// <summary>
        /// ASCIIメッセージの応答がエラーである場合、SLMPUnitErrorExceptionがスローされることをテストします。
        /// </summary>
        [Fact]
        public void Resolve_ASCIIResponseError_ThrowsSLMPUnitErrorException()
        {
            // Arrange
            var writeResponseResolver = new WriteResponseResolver(MessageType.ASCII, DeviceAccessType.Word, 10);
            byte[] response = Encoding.ASCII.GetBytes("D0000000000000000000000000000001");

            // Act & Assert
            Assert.Throws<SLMPUnitErrorException>(() => writeResponseResolver.Resolve(response));
        }

        /// <summary>
        /// バイナリメッセージの応答がエラーである場合、SLMPUnitErrorExceptionがスローされることをテストします。
        /// </summary>
        [Fact]
        public void Resolve_BinaryResponseError_ThrowsSLMPUnitErrorException()
        {
            // Arrange
            var writeResponseResolver = new WriteResponseResolver(MessageType.Binary, DeviceAccessType.Word, 10);
            byte[] response = new byte[] { 0xD0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };

            // Act & Assert
            Assert.Throws<SLMPUnitErrorException>(() => writeResponseResolver.Resolve(response));
        }

        /// <summary>
        /// 同じプロパティ値を持つWriteResponseResolverオブジェクトが等しいと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_SameProperties_ReturnsTrue()
        {
            // Arrange
            var obj1 = new WriteResponseResolver(MessageType.Binary, DeviceAccessType.Word, 10);
            var obj2 = new WriteResponseResolver(MessageType.Binary, DeviceAccessType.Word, 10);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// 異なるプロパティ値を持つWriteResponseResolverオブジェクトが等しくないと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_DifferentProperties_ReturnsFalse()
        {
            // Arrange
            var obj1 = new WriteResponseResolver(MessageType.Binary, DeviceAccessType.Word, 10);
            var obj2 = new WriteResponseResolver(MessageType.ASCII, DeviceAccessType.Bit, 10);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// 同じプロパティ値を持つWriteResponseResolverオブジェクトが同じハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_SameProperties_ReturnsSameHashCode()
        {
            // Arrange
            var obj1 = new WriteResponseResolver(MessageType.Binary, DeviceAccessType.Word, 10);
            var obj2 = new WriteResponseResolver(MessageType.Binary, DeviceAccessType.Word, 10);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        /// <summary>
        /// 異なるプロパティ値を持つWriteResponseResolverオブジェクトが異なるハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_DifferentProperties_ReturnsDifferentHashCode()
        {
            // Arrange
            var obj1 = new WriteResponseResolver(MessageType.Binary, DeviceAccessType.Word, 10);
            var obj2 = new WriteResponseResolver(MessageType.ASCII, DeviceAccessType.Bit, 10);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}



