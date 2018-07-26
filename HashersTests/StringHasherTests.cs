namespace HashersTests
{
    using FluentAssertions;

    using Hasher;

    using Xunit;

    public class StringHasherTests
    {
        private readonly StringHasher stringHasher;

        public StringHasherTests()
        {
            this.stringHasher = new StringHasher();
        }

        [Fact]
        public void Hashed_password_should_be_not_equal_to_password()
        {
            // Arrange
            var password = "TestPassword";

            // Act
            this.stringHasher.Hash(password, out var hashedPassword, out var salt);

            // Assert
            hashedPassword.Should().NotBe(password);
        }

        [Fact]
        public void Hashed_passwords_should_not_be_equal()
        {
            // Arrange
            var password1 = "TestPassword";
            var password2 = "TestPassword2";

            // Act
            this.stringHasher.Hash(password1, out var hashedPassword1, out var salt1);
            this.stringHasher.Hash(password2, out var hashedPassword2, out var salt2);

            // Assert
            hashedPassword1.Should().NotBe(hashedPassword2);
        }

        [Theory]
        [InlineData("TestPassword", "TestPassword", true)]
        [InlineData("TestPassword", "TestPassword1", false)]
        public void Password_match_should_be_equal_shouldEqual(string passwordToHash, string passwordToCheck, bool shouldEqual)
        {
            // Arrange
            this.stringHasher.Hash(passwordToHash, out var hashedPassword, out var salt);

            // Act
            var matches = this.stringHasher.HashesMatch(hashedPassword, passwordToCheck, salt);

            // Assert
            matches.Should().Be(shouldEqual);
        }
    }
}