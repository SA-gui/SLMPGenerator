using SLMPGenerator.Common;
using Xunit;

namespace SLMPGenerator.Tests.Common
{
    public class UnitTest_RequestDestStationNo
    {
        /// <summary>
        /// 有効なパラメータでコンストラクタを呼び出した場合、BinaryCodeとASCIICodeが正しく設定されることをテストします。
        /// </summary>
        [Theory]
        [InlineData((ushort)1, new byte[] { 0x01 }, "01")]
        [InlineData((ushort)120, new byte[] { 0x78 }, "78")]
        [InlineData((ushort)125, new byte[] { 0x7D }, "7D")]
        [InlineData((ushort)126, new byte[] { 0x7E }, "7E")]
        [InlineData((ushort)255, new byte[] { 0xFF }, "FF")]
        public void Constructor_ValidParameters_SetsBinaryCodeAndASCIICode(ushort stationNo, byte[] expectedBinaryCode, string expectedASCIICode)
        {
            // Arrange & Act
            var requestDestStationNo = new RequestDestStationNo(stationNo);

            // Assert
            Assert.Equal(expectedBinaryCode, requestDestStationNo.BinaryCode);
            Assert.Equal(expectedASCIICode, requestDestStationNo.ASCIICode);
        }

        /// <summary>
        /// 無効なステーション番号を渡した場合、ArgumentExceptionがスローされることをテストします。
        /// </summary>
        [Theory]
        [InlineData((ushort)0)]
        [InlineData((ushort)121)]
        [InlineData((ushort)124)]
        [InlineData((ushort)127)]
        [InlineData((ushort)254)]
        [InlineData((ushort)256)]
        public void Constructor_InvalidStationNo_ThrowsArgumentException(ushort invalidStationNo)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new RequestDestStationNo(invalidStationNo));
        }

        /// <summary>
        /// 同じASCIICodeを持つRequestDestStationNoオブジェクトが等しいと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_SameASCIICode_ReturnsTrue()
        {
            // Arrange
            var obj1 = new RequestDestStationNo(1);
            var obj2 = new RequestDestStationNo(1);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// 異なるASCIICodeを持つRequestDestStationNoオブジェクトが等しくないと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_DifferentASCIICode_ReturnsFalse()
        {
            // Arrange
            var obj1 = new RequestDestStationNo(1);
            var obj2 = new RequestDestStationNo(2);

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// 同じASCIICodeを持つRequestDestStationNoオブジェクトが同じハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_SameASCIICode_ReturnsSameHashCode()
        {
            // Arrange
            var obj1 = new RequestDestStationNo(1);
            var obj2 = new RequestDestStationNo(1);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }

        /// <summary>
        /// 異なるASCIICodeを持つRequestDestStationNoオブジェクトが異なるハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_DifferentASCIICode_ReturnsDifferentHashCode()
        {
            // Arrange
            var obj1 = new RequestDestStationNo(1);
            var obj2 = new RequestDestStationNo(2);

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}


