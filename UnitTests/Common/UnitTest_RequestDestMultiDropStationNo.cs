using SLMPGenerator.Common;
using Xunit;

namespace SLMPGenerator.Tests.Common
{
    public class UnitTest_RequestDestMultiDropStationNo
    {
        /// <summary>
        /// 有効なパラメータでコンストラクタを呼び出した場合、BinaryCodeとASCIICodeが正しく設定されることをテストします。
        /// </summary>
        [Theory]
        [InlineData((ushort)0x00, new byte[] { 0x00 }, "00")]
        [InlineData((ushort)0x01, new byte[] { 0x01 }, "01")]
        [InlineData((ushort)0xFF, new byte[] { 0xFF }, "FF")]
        public void Constructor_ValidParameters_SetsBinaryCodeAndASCIICode(ushort multiDropStationNo, byte[] expectedBinaryCode, string expectedASCIICode)
        {
            // Arrange & Act
            var requestDestMultiDropStationNo = new RequestDestMultiDropStationNo(multiDropStationNo);

            // Assert
            Assert.Equal(expectedBinaryCode, requestDestMultiDropStationNo.BinaryCode);
            Assert.Equal(expectedASCIICode, requestDestMultiDropStationNo.ASCIICode);
        }

        /// <summary>
        /// 同じASCIICodeを持つRequestDestMultiDropStationNoオブジェクトが等しいと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_SameASCIICode_ReturnsTrue()
        {
            // Arrange
            var obj1 = new RequestDestMultiDropStationNo(0x01);
            var obj2 = new RequestDestMultiDropStationNo(0x01);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// 異なるASCIICodeを持つRequestDestMultiDropStationNoオブジェクトが等しくないと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_DifferentASCIICode_ReturnsFalse()
        {
            // Arrange
            var obj1 = new RequestDestMultiDropStationNo(0x01);
            var obj2 = new RequestDestMultiDropStationNo(0x02);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// 同じASCIICodeを持つRequestDestMultiDropStationNoオブジェクトが同じハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_SameASCIICode_ReturnsSameHashCode()
        {
            // Arrange
            var obj1 = new RequestDestMultiDropStationNo(0x01);
            var obj2 = new RequestDestMultiDropStationNo(0x01);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        /// <summary>
        /// 異なるASCIICodeを持つRequestDestMultiDropStationNoオブジェクトが異なるハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_DifferentASCIICode_ReturnsDifferentHashCode()
        {
            // Arrange
            var obj1 = new RequestDestMultiDropStationNo(0x01);
            var obj2 = new RequestDestMultiDropStationNo(0x02);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}
