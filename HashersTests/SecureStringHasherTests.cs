namespace HashersTests
{
    using FluentAssertions;

    using Hasher;
    using Hasher.Extensions;

    using Xunit;

    public class SecureStringHasherTests
    {
        private readonly SecureStringHasher secureStringHasher;

        public SecureStringHasherTests()
        {
            this.secureStringHasher = new SecureStringHasher();
        }

        [Fact]
        public void Hashed_password_should_be_not_equal_to_password()
        {
            // Arrange
            var password = "TestPassword";
            var pwdChars = password.ToCharArray();

            // Act
            this.secureStringHasher.Hash(pwdChars.AsSecureString(), out var hashedPassword, out var salt);

            // Assert
            hashedPassword.AsString().Should().NotBe(password);
        }

        [Fact]
        public void Hashed_passwords_should_not_be_equal()
        {
            // Arrange
            var password1 = "TestPassword".ToCharArray();
            var password2 = "TestPassword2".ToCharArray();

            // Act
            this.secureStringHasher.Hash(password1.AsSecureString(), out var hashedPassword1, out var salt1);
            this.secureStringHasher.Hash(password2.AsSecureString(), out var hashedPassword2, out var salt2);

            // Assert
            hashedPassword1.Should().NotBe(hashedPassword2);
        }

        [Theory]
        [InlineData("TestPassword", "TestPassword", true)]
        [InlineData("TestPassword", "TestPassword1", false)]
        public void Password_match_should_be_equal_shouldEqual(string passwordToHash, string passwordToCheck, bool shouldEqual)
        {
            // Arrange
            var passwordToHashChars = passwordToHash.ToCharArray();
            var passwordToCheckChars = passwordToCheck.ToCharArray();

            this.secureStringHasher.Hash(passwordToHashChars.AsSecureString(), out var hashedPassword, out var salt);

            // Act
            var matches = this.secureStringHasher.HashesMatch(hashedPassword, passwordToCheckChars.AsSecureString(), salt);

            // Assert
            matches.Should().Be(shouldEqual);
        }
    }
}