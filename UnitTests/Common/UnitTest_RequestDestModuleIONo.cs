using SLMPGenerator.Common;
using Xunit;

namespace SLMPGenerator.Tests.Common
{
    public class UnitTest_RequestDestModuleIONo
    {
        /// <summary>
        /// 有効なパラメータでコンストラクタを呼び出した場合、BinaryCodeとASCIICodeが正しく設定されることをテストします。
        /// </summary>
        [Theory]
        [InlineData(RequestDestModuleIOType.OwnStationCPU, new byte[] { 0xFF, 0x03 }, "03FF")]
        [InlineData(RequestDestModuleIOType.MultidropConnectionCPU, new byte[] { 0x00, 0x00 }, "0000")]
        [InlineData(RequestDestModuleIOType.MultipleSystemCPU1, new byte[] { 0xE0, 0x03 }, "03E0")]
        public void Constructor_ValidParameters_SetsBinaryCodeAndASCIICode(RequestDestModuleIOType requestedModuleIOType, byte[] expectedBinaryCode, string expectedASCIICode)
        {
            // Arrange & Act
            var requestDestModuleIONo = new RequestDestModuleIONo(requestedModuleIOType);

            // Assert
            Assert.Equal(expectedBinaryCode, requestDestModuleIONo.BinaryCode);
            Assert.Equal(expectedASCIICode, requestDestModuleIONo.ASCIICode);
        }

        /// <summary>
        /// 無効なRequestDestModuleIOTypeを渡した場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Theory]
        [InlineData((RequestDestModuleIOType)9999)]
        public void Constructor_InvalidRequestIOType_ThrowsArgumentException(RequestDestModuleIOType invalidIOType)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new RequestDestModuleIONo(invalidIOType));
        }

        /// <summary>
        /// 同じASCIICodeを持つRequestDestModuleIONoオブジェクトが等しいと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_SameASCIICode_ReturnsTrue()
        {
            // Arrange
            var obj1 = new RequestDestModuleIONo(RequestDestModuleIOType.OwnStationCPU);
            var obj2 = new RequestDestModuleIONo(RequestDestModuleIOType.OwnStationCPU);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// 異なるASCIICodeを持つRequestDestModuleIONoオブジェクトが等しくないと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_DifferentASCIICode_ReturnsFalse()
        {
            // Arrange
            var obj1 = new RequestDestModuleIONo(RequestDestModuleIOType.OwnStationCPU);
            var obj2 = new RequestDestModuleIONo(RequestDestModuleIOType.MultidropConnectionCPU);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// 同じASCIICodeを持つRequestDestModuleIONoオブジェクトが同じハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_SameASCIICode_ReturnsSameHashCode()
        {
            // Arrange
            var obj1 = new RequestDestModuleIONo(RequestDestModuleIOType.OwnStationCPU);
            var obj2 = new RequestDestModuleIONo(RequestDestModuleIOType.OwnStationCPU);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        /// <summary>
        /// 異なるASCIICodeを持つRequestDestModuleIONoオブジェクトが異なるハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_DifferentASCIICode_ReturnsDifferentHashCode()
        {
            // Arrange
            var obj1 = new RequestDestModuleIONo(RequestDestModuleIOType.OwnStationCPU);
            var obj2 = new RequestDestModuleIONo(RequestDestModuleIOType.MultidropConnectionCPU);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}
