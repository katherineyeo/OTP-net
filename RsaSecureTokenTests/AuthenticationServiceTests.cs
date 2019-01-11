using NSubstitute;
using NUnit.Framework;
using RsaSecureToken;
using Assert = NUnit.Framework.Assert;

namespace RsaSecureToken.Tests
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        [Test()]
        public void IsValidTest()
        {
            var fakeProfile = Substitute.For<IProfile>();
            fakeProfile.GetPassword("joey").Returns("91");

            var fakeToken = Substitute.For<IToken>();
            fakeToken.GetRandom("").ReturnsForAnyArgs("000000");

            var target = new AuthenticationService(fakeProfile, fakeToken);
            //var target = new AuthenticationService(new FakeProfile(), new FakeToken());

            var actual = target.IsValid("joey", "91000000");

            //always failed
            Assert.IsTrue(actual);
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