using NSubstitute;
using NUnit.Framework;
using RsaSecureToken;
using Assert = NUnit.Framework.Assert;

namespace RsaSecureToken.Tests
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private IProfile _fakeProfile;
        private IToken _fakeToken;
        private AuthenticationService _target;
        private ILog _fakeLog;

        [SetUp]
        public void Setup()
        {
            _fakeProfile = Substitute.For<IProfile>();
            _fakeToken = Substitute.For<IToken>();
            _fakeLog = Substitute.For<ILog>();
            _target = new AuthenticationService(_fakeProfile, _fakeToken, _fakeLog);
        }

        [Test()]
        public void is_valid()
        {
            GivenProfile("joey", "91");
            GivenToken("000000");

            ShouldBeValid("joey", "91000000");
        }

        [Test()]
        public void should_log_when_invalid()
        {
            GivenProfile("joey", "91");
            GivenToken("000000");

            WhenInvalid();

            ShouldLogWith("joey", "login failed");
        }

        [Test()]
        public void should_not_log_when_valid()
        {
            GivenProfile("joey", "91");
            GivenToken("000000");

            WhenValid();

            ShouldNotLog();
        }

        private void ShouldNotLog()
        {
            _fakeLog.DidNotReceiveWithAnyArgs().Save(Arg.Any<string>());
        }

        private void WhenValid()
        {
            _target.IsValid("joey", "91000000");
        }

        private void ShouldLogWith(string account, string status)
        {
            _fakeLog.Received().Save(Arg.Is<string>(m => m.Contains(account) && m.Contains(status)));
            //_fakeLog.Received(1).Save("account:joey try to login failed");
        }

        private void WhenInvalid()
        {
            _target.IsValid("joey", "wrong password");
        }

        private void ShouldBeValid(string account, string password)
        {
            Assert.IsTrue(_target.IsValid(account, password));
        }

        private void GivenToken(string token)
        {
            _fakeToken.GetRandom("").ReturnsForAnyArgs(token);
        }

        private void GivenProfile(string account, string password)
        {
            _fakeProfile.GetPassword(account).Returns(password);
        }
    }
}

internal class FakeProfile : IProfile
{
    public string GetPassword(string account)
    {
        if (account == "joey")
        {
            return "91";
        }
        return "";
    }
}

internal class FakeToken : IToken
{
    public string GetRandom(string account)
    {
        return "000000";
    }
}