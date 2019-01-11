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
            // 試著使用 stub object 的 ReturnsForAnyArgs() 方法
            //例如：profile.GetPassword("").ReturnsForAnyArgs("91"); // 不管GetPassword()傳入任何參數，都要回傳 "91"

            // step 1: arrange, 建立 mock object
            // ILog log = Substitute.For<ILog>();

            // step 2: act

            // step 3: assert, mock object 是否有正確互動
            //log.Received(1).Save("account:Joey try to login failed"); //Received(1) 可以簡化成 Received()
            Assert.Fail();
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