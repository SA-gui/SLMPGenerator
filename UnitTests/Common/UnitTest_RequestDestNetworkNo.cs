using SLMPGenerator.Common;
using Xunit;

namespace SLMPGenerator.Tests.Common
{
    public class UnitTest_RequestDestNetworkNo
    {
        /// <summary>
        /// 有効なパラメータでコンストラクタを呼び出した場合、BinaryCodeとASCIICodeが正しく設定されることをテストします。
        /// </summary>
        [Theory]
        [InlineData((ushort)0, new byte[] { 0x00 }, "00")]
        [InlineData((ushort)1, new byte[] { 0x01 }, "01")]
        [InlineData((ushort)239, new byte[] { 0xEF }, "EF")]
        public void Constructor_ValidParameters_SetsBinaryCodeAndASCIICode(ushort networkNo, byte[] expectedBinaryCode, string expectedASCIICode)
        {
            // Arrange & Act
            var requestDestNetworkNo = new RequestDestNetworkNo(networkNo);

            // Assert
            Assert.Equal(expectedBinaryCode, requestDestNetworkNo.BinaryCode);
            Assert.Equal(expectedASCIICode, requestDestNetworkNo.ASCIICode);
        }

        /// <summary>
        /// 無効なネットワーク番号を渡した場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Theory]
        [InlineData((ushort)240)]
        [InlineData((ushort)300)]
        [InlineData((ushort)ushort.MaxValue)]
        public void Constructor_InvalidNetworkNo_ThrowsArgumentException(ushort invalidNetworkNo)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new RequestDestNetworkNo(invalidNetworkNo));
        }

        /// <summary>
        /// 同じASCIICodeを持つRequestDestNetworkNoオブジェクトが等しいと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_SameASCIICode_ReturnsTrue()
        {
            // Arrange
            var obj1 = new RequestDestNetworkNo(1);
            var obj2 = new RequestDestNetworkNo(1);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// 異なるASCIICodeを持つRequestDestNetworkNoオブジェクトが等しくないと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_DifferentASCIICode_ReturnsFalse()
        {
            // Arrange
            var obj1 = new RequestDestNetworkNo(1);
            var obj2 = new RequestDestNetworkNo(2);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// 同じASCIICodeを持つRequestDestNetworkNoオブジェクトが同じハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_SameASCIICode_ReturnsSameHashCode()
        {
            // Arrange
            var obj1 = new RequestDestNetworkNo(1);
            var obj2 = new RequestDestNetworkNo(1);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        /// <summary>
        /// 異なるASCIICodeを持つRequestDestNetworkNoオブジェクトが異なるハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_DifferentASCIICode_ReturnsDifferentHashCode()
        {
            // Arrange
            var obj1 = new RequestDestNetworkNo(1);
            var obj2 = new RequestDestNetworkNo(2);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}

