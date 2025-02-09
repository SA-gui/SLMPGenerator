using SLMPGenerator.Common;
using Xunit;

namespace SLMPGenerator.Tests.Common
{
    public class UnitTest_SubHeader
    {
        /// <summary>
        /// コンストラクタを呼び出した場合、BinaryCodeとASCIICodeが正しく設定されることをテストします。
        /// </summary>
        [Fact]
        public void Constructor_SetsBinaryCodeAndASCIICode()
        {
            // Arrange & Act
            var subHeader = new SubHeader();

            // Assert
            Assert.Equal(new byte[] { 0x50, 0x00 }, subHeader.BinaryCode);
            Assert.Equal("5000", subHeader.ASCIICode);
        }

        /// <summary>
        /// 同じASCIICodeを持つSubHeaderオブジェクトが等しいと判断されることをテストします。
        /// </summary>
        [Fact]
        public void Equals_SameASCIICode_ReturnsTrue()
        {
            // Arrange
            var obj1 = new SubHeader();
            var obj2 = new SubHeader();

            // Act
            bool result = obj1.Equals(obj2);

            // Assert
            Assert.True(result);
        }



        /// <summary>
        /// 同じASCIICodeを持つSubHeaderオブジェクトが同じハッシュコードを返すことをテストします。
        /// </summary>
        [Fact]
        public void GetHashCode_SameASCIICode_ReturnsSameHashCode()
        {
            // Arrange
            var obj1 = new SubHeader();
            var obj2 = new SubHeader();

            // Act
            int hashCode1 = obj1.GetHashCode();
            int hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }


    }
}

