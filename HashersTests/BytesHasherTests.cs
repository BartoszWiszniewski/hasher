namespace HashersTests
{
    using System.Text;

    using FluentAssertions;

    using Hasher;

    using Xunit;

    public class BytesHasherTests
    {
        private readonly BytesHasher bytesHasher;

        public BytesHasherTests()
        {
            this.bytesHasher = new BytesHasher();
        }

        [Fact]
        public void Hashed_password_should_be_not_equal_to_password()
        {
            // Arrange
            var password = Encoding.UTF8.GetBytes("TestPassword");

            // Act
            this.bytesHasher.Hash(password, out var hashedPassword, out var salt);

            // Assert
            hashedPassword.Should().NotBeEquivalentTo(password);
        }

        [Fact]
        public void Hashed_passwords_should_not_be_equal()
        {
            // Arrange
            var password1 = Encoding.UTF8.GetBytes("TestPassword");
            var password2 = Encoding.UTF8.GetBytes("TestPassword2");

            // Act
            this.bytesHasher.Hash(password1, out var hashedPassword1, out var salt1);
            this.bytesHasher.Hash(password2, out var hashedPassword2, out var salt2);

            // Assert
            hashedPassword1.Should().NotBeEquivalentTo(hashedPassword2);
        }

        [Theory]
        [InlineData("TestPassword", "TestPassword", true)]
        [InlineData("TestPassword", "TestPassword1", false)]
        public void Password_match_should_be_equal_shouldEqual(string passwordToHash, string passwordToCheck, bool shouldEqual)
        {
            // Arrange
            var passwordToHashChars = Encoding.UTF8.GetBytes(passwordToHash.ToCharArray());
            var passwordToCheckChars = Encoding.UTF8.GetBytes(passwordToCheck.ToCharArray());

            this.bytesHasher.Hash(passwordToHashChars, out var hashedPassword, out var salt);

            // Act
            var matches = this.bytesHasher.HashesMatch(hashedPassword, passwordToCheckChars, salt);

            // Assert
            matches.Should().Be(shouldEqual);
        }
    }
}