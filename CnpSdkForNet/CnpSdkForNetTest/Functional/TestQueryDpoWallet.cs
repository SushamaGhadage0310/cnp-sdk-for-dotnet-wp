using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cnp.Sdk.Test.Functional
{
    internal class TestQueryDpoWallet
    {

        private CnpOnline _cnp;

        [OneTimeSetUp]
        public void SetUpCnp()
        {
            _cnp = new CnpOnline();
        }

        [Test]
        public void QueryDpoWalletBalance()
        {
            var queryDpoWallet = new queryDpoWalletBalance
            {
                reportGroup = "Planets",
                id = "1",

            };

            var responseObj = _cnp.QueryDpo(queryDpoWallet);
            StringAssert.AreEqualIgnoringCase("Generic Decline", responseObj.message);
        }
    }
}
