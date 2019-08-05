using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using RsaSecureToken;
using Assert = NUnit.Framework.Assert;

namespace RsaSecureToken.Tests
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        [Test()]
        public void is_valid()
        {
            var target = new AuthenticationService();

            var actual = target.IsValid("joey", "91000000");

            //always failed
            Assert.IsTrue(actual);
        }
    }
}