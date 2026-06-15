using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text.RegularExpressions;


namespace Cnp.Sdk.Test.Unit
{
    [TestFixture]
    class TestQueryDpoWalletBalance
    {

        private CnpOnline cnp;
        Dictionary<String, String> config;

        [OneTimeSetUp]
        public void SetUpCnp()
        {
            cnp = new CnpOnline();
            config = new ConfigManager().getConfig();
        }

        [Test]
        public void AuthWithIdentityBundle()
        {
            var queryDpoWallet = new queryDpoWalletBalance();
            queryDpoWallet.reportGroup = "Planets";

            var mock = new Mock<Communications>();
            if (config["encryptOltpPayload"] == "true")
            {
                mock.Setup(Communications => Communications.HttpPost(It.IsRegex(".*<cnpOnlineRequest.*<encryptedPayload.*</encryptedPayload>.*", RegexOptions.Singleline)))
                .Returns("<cnpOnlineResponse version=12.50' response='0' message='Valid Format' xmlns='http://www.vantivcnp.com/schema'><queryDpoWalletBalanceResponse><cnpTxnId>123</cnpTxnId></queryDpoWalletBalanceResponse></cnpOnlineResponse>");
            }
            else
            {
                mock.Setup(Communications => Communications.HttpPost(It.IsRegex(".*<queryDpoWalletBalance.*", RegexOptions.Singleline)))
                .Returns("<cnpOnlineResponse version='12.50' response='0' message='Valid Format' xmlns='http://www.vantivcnp.com/schema'><queryDpoWalletBalanceResponse><cnpTxnId>123</cnpTxnId></queryDpoWalletBalanceResponse></cnpOnlineResponse>");
            }
            var mockedCommunication = mock.Object;
            cnp.SetCommunication(mockedCommunication);
            var queryDpoWalletBalanceResponse = cnp.QueryDpo(queryDpoWallet);

            Assert.NotNull(queryDpoWalletBalanceResponse);
            Assert.AreEqual(123, queryDpoWalletBalanceResponse.cnpTxnId);
        }
    }
}

 



