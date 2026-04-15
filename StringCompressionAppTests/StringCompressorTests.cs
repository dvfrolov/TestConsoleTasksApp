using System;
using Xunit;

namespace StringCompressionApp.Tests
{
    public class StringCompressorTests
    {
        [Theory]
        [InlineData("a", "a")]
        [InlineData("aa", "a2")]
        [InlineData("aaa", "a3")]
        [InlineData("ab", "ab")]
        [InlineData("aaabbcccdde", "a3b2c3d2e")]
        [InlineData("abc", "abc")]
        [InlineData("zzzzzz", "z6")]
        [InlineData("abbbbbbbbbbbb", "ab12")]
        public void Compress_ShouldReturnExpectedResult(string source, string expected)
        {
            string actual = StringCompressor.Compress(source);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("a", "a")]
        [InlineData("a2", "aa")]
        [InlineData("a3", "aaa")]
        [InlineData("ab", "ab")]
        [InlineData("a3b2c3d2e", "aaabbcccdde")]
        [InlineData("abc", "abc")]
        [InlineData("z6", "zzzzzz")]
        [InlineData("ab12", "abbbbbbbbbbbb")]
        public void Decompress_ShouldReturnExpectedResult(string compressed, string expected)
        {
            string actual = StringCompressor.Decompress(compressed);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("aa")]
        [InlineData("aaabbcccdde")]
        [InlineData("abc")]
        [InlineData("zzzzzzzzzzzzzz")]
        public void Compress_ThenDecompress_ShouldRestoreOriginalString(string source)
        {
            string compressed = StringCompressor.Compress(source);
            string restored = StringCompressor.Decompress(compressed);

            Assert.Equal(source, restored);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("aA")]
        [InlineData("a1")]
        [InlineData("привет")]
        [InlineData("a-b")]
        public void Compress_ShouldThrow_ForInvalidSource(string source)
        {
            Assert.Throws<ArgumentException>(() => StringCompressor.Compress(source));
        }

        [Theory]
        [InlineData("1a")]
        [InlineData("a0")]
        [InlineData("a1")]
        [InlineData("a01")]
        [InlineData("-")]
        public void Decompress_ShouldThrow_ForInvalidCompressedString(string compressed)
        {
            Assert.Throws<ArgumentException>(() => StringCompressor.Decompress(compressed));
        }
    }
}