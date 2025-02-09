using SLMPGenerator.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Common
{
    public class UnitTest_BitHelper
    {
        [Fact]
        public void ToBytesLittleEndian_UShortValue_ReturnsCorrectByteArray()
        {
            // Arrange
            ushort value = 0x1234;

            // Act
            byte[] result = BitHelper.ToBytesLittleEndian(value);

            // Assert
            Assert.Equal(new byte[] { 0x34, 0x12 }, result);
        }

        [Fact]
        public void ToBytesLittleEndian_IntValue_ReturnsCorrectByteArray()
        {
            // Arrange
            int value = 0x12345678;

            // Act
            byte[] result = BitHelper.ToBytesLittleEndian(value);

            // Assert
            Assert.Equal(new byte[] { 0x78, 0x56, 0x34, 0x12 }, result);
        }

        [Fact]
        public void ToBytesBigEndian_UShortValue_ReturnsCorrectByteArray()
        {
            // Arrange
            ushort value = 0x1234;

            // Act
            byte[] result = BitHelper.ToBytesBigEndian(value);

            // Assert
            Assert.Equal(new byte[] { 0x12, 0x34 }, result);
        }

        [Fact]
        public void ToBytesBigEndian_IntValue_ReturnsCorrectByteArray()
        {
            // Arrange
            int value = 0x12345678;

            // Act
            byte[] result = BitHelper.ToBytesBigEndian(value);

            // Assert
            Assert.Equal(new byte[] { 0x12, 0x34, 0x56, 0x78 }, result);
        }

        [Fact]
        public void ToString_ByteArray_ReturnsCorrectString()
        {
            // Arrange
            byte[] binaryCode = new byte[] { 0x12, 0x34, 0x56, 0x78 };

            // Act
            string result = BitHelper.ToString(binaryCode);

            // Assert
            Assert.Equal("12345678", result);
        }

        [Fact]
        public void ToReverseString_ByteArray_ReturnsCorrectString()
        {
            // Arrange
            byte[] binaryCode = new byte[] { 0x12, 0x34, 0x56, 0x78 };

            // Act
            string result = BitHelper.ToReverseString(binaryCode);

            // Assert
            Assert.Equal("78563412", result);
        }
    }
}
