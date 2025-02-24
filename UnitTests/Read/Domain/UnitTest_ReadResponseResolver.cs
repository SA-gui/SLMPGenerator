using SLMPGenerator.Common;
using SLMPGenerator.Common.Response;
using SLMPGenerator.Read.Domain;
using System.Text;
using Xunit;

namespace UnitTests.Read.Domain
{
    public class UnitTest_ReadResponseResolver
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
            var readResponseResolver = new ReadResponseResolver(messageType, devAccessType, numberOfDevicePoints);

            // Assert
            Assert.Equal(messageType, readResponseResolver.MessageType);
            Assert.Equal(devAccessType, readResponseResolver.DeviceAccessType);
            Assert.Equal(numberOfDevicePoints, readResponseResolver.NumberOfDevicePoints);
        }

        /// <summary>
        /// ASCIIメッセージの応答が正常である場合、正しいデータが返されることをテストします。
        /// </summary>
        [Fact]
        public void Resolve_ASCIIResponseNormal_ReturnsCorrectData()
        {
            // Arrange
            var readResponseResolver = new ReadResponseResolver(MessageType.ASCII, DeviceAccessType.Word, 3);
            byte[] response = Encoding.ASCII.GetBytes("D0000000000000000000000000000000");

            // Act
            var result = readResponseResolver.Resolve(response);

            // Assert
            Assert.Equal(new List<short> { 0, 0, 0 }, result);
        }

        /// <summary>
        /// バイナリメッセージの応答が正常である場合、正しいデータが返されることをテストします。
        /// </summary>
        [Fact]
        public void Resolve_BinaryResponseNormal_ReturnsCorrectData()
        {
            // Arrange
            var readResponseResolver = new ReadResponseResolver(MessageType.Binary, DeviceAccessType.Word, 3);
            byte[] response = new byte[] { 0xD0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            // Act
            var result = readResponseResolver.Resolve(response);

            // Assert
            Assert.Equal(new List<short> { 0, 0, 0 }, result);
        }

        /// <summary>
        /// ASCIIメッセージの応答がエラーである場合、SLMPUnitErrorExceptionがスローされることをテストします。
        /// </summary>
        [Fact]
        public void Resolve_ASCIIResponseError_ThrowsSLMPUnitErrorException()
        {
            // Arrange
            var readResponseResolver = new ReadResponseResolver(MessageType.ASCII, DeviceAccessType.Word, 3);
            byte[] response = Encoding.ASCII.GetBytes("D0000000000000000000000000000001");

            // Act & Assert
            Assert.Throws<SLMPUnitErrorException>(() => readResponseResolver.Resolve(response));
        }

        /// <summary>
        /// バイナリメッセージの応答がエラーである場合、SLMPUnitErrorExceptionがスローされることをテストします。
        /// </summary>
        [Fact]
        public void Resolve_BinaryResponseError_ThrowsSLMPUnitErrorException()
        {
            // Arrange
            var readResponseResolver = new ReadResponseResolver(MessageType.Binary, DeviceAccessType.Word, 3);
            byte[] response = new byte[] { 0xD0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00 };

            // Act & Assert
            Assert.Throws<SLMPUnitErrorException>(() => readResponseResolver.Resolve(response));
        }

        /// <summary>
        /// 同じプロパティ値を持つReadResponseResolverオブジェクトが等しいと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_SameProperties_ReturnsTrue()
        {
            // Arrange
            var obj1 = new ReadResponseResolver(MessageType.Binary, DeviceAccessType.Word, 3);
            var obj2 = new ReadResponseResolver(MessageType.Binary, DeviceAccessType.Word, 3);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// 異なるプロパティ値を持つReadResponseResolverオブジェクトが等しくないと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_DifferentProperties_ReturnsFalse()
        {
            // Arrange
            var obj1 = new ReadResponseResolver(MessageType.Binary, DeviceAccessType.Word, 3);
            var obj2 = new ReadResponseResolver(MessageType.ASCII, DeviceAccessType.Bit, 3);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// 同じプロパティ値を持つReadResponseResolverオブジェクトが同じハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_SameProperties_ReturnsSameHashCode()
        {
            // Arrange
            var obj1 = new ReadResponseResolver(MessageType.Binary, DeviceAccessType.Word, 3);
            var obj2 = new ReadResponseResolver(MessageType.Binary, DeviceAccessType.Word, 3);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        /// <summary>
        /// 異なるプロパティ値を持つReadResponseResolverオブジェクトが異なるハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_DifferentProperties_ReturnsDifferentHashCode()
        {
            // Arrange
            var obj1 = new ReadResponseResolver(MessageType.Binary, DeviceAccessType.Word, 3);
            var obj2 = new ReadResponseResolver(MessageType.ASCII, DeviceAccessType.Bit, 3);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }

        /*
         
                 /// <summary>
        /// 有効なASCIIレスポンスを解析する場合、正しいデータが返されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x31, 0x32, 0x33, 0x34 }, 1, new short[] { 0x1234 })]
        [InlineData(new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38 }, 2, new short[] { 0x1234, 0x5678 })]
                 /// <summary>
        /// 有効なバイナリレスポンスを解析する場合、正しいデータが返されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x12, 0x34 }, 1, new short[] { 0x3412 })]
        [InlineData(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x12, 0x34, 0x56, 0x78 }, 2, new short[] { 0x3412, 0x7856 })]
         */


    }
}


