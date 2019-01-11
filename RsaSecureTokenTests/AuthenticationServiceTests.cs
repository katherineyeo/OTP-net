using NSubstitute;
using NUnit.Framework;
using RsaSecureToken;
using Assert = NUnit.Framework.Assert;

namespace RsaSecureToken.Tests
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private readonly IProfile _fakeProfile = Substitute.For<IProfile>();
        private readonly IToken _fakeToken = Substitute.For<IToken>();
        private readonly AuthenticationService _target;
        private readonly ILog _fakeLog = Substitute.For<ILog>();

        public AuthenticationServiceTests()
        {
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
        public void IsValidTest_如何驗證當非法登入時有正確紀錄log()
        {
            GivenProfile("joey", "91");
            GivenToken("000000");

            _target.IsValid("joey", "wrong password");

            _fakeLog.Received(1).Save(Arg.Is<string>(m => m.Contains("joey") && m.Contains("login failed")));
            //_fakeLog.Received(1).Save("account:joey try to login failed");
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