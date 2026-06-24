using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text.RegularExpressions;


namespace Cnp.Sdk.Test.Unit
{
    [TestFixture]
    class TestEcheckRedeposit
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
        public void TestMerchantData()
        {
            echeckRedeposit echeckRedeposit = new echeckRedeposit();
            echeckRedeposit.cnpTxnId = 1;
            echeckRedeposit.merchantData = new merchantDataType();
            echeckRedeposit.merchantData.campaign = "camp";
            echeckRedeposit.merchantData.affiliate = "affil";
            echeckRedeposit.merchantData.merchantGroupingId = "mgi";
            echeckRedeposit.customIdentifier = "customIdent";
           
            var mock = new Mock<Communications>();
            if (config["encryptOltpPayload"] == "true")
            {
                mock.Setup(Communications => Communications.HttpPost(It.IsRegex(".*<cnpOnlineRequest.*<encryptedPayload.*</encryptedPayload>.*", RegexOptions.Singleline)))
                .Returns("<cnpOnlineResponse version='8.13' response='0' message='Valid Format' xmlns='http://www.vantivcnp.com/schema'><echeckRedepositResponse><cnpTxnId>123</cnpTxnId></echeckRedepositResponse></cnpOnlineResponse>");
            }
            else
            {
                mock.Setup(Communications => Communications.HttpPost(It.IsRegex(".*<echeckRedeposit.*<cnpTxnId>1</cnpTxnId>.*<merchantData>.*<campaign>camp</campaign>.*<affiliate>affil</affiliate>.*<merchantGroupingId>mgi</merchantGroupingId>.*</merchantData>.*", RegexOptions.Singleline)))
                .Returns("<cnpOnlineResponse version='8.13' response='0' message='Valid Format' xmlns='http://www.vantivcnp.com/schema'><echeckRedepositResponse><cnpTxnId>123</cnpTxnId></echeckRedepositResponse></cnpOnlineResponse>");
            }
            Communications mockedCommunication = mock.Object;
            cnp.SetCommunication(mockedCommunication);
            cnp.EcheckRedeposit(echeckRedeposit);
        }     
        
        [Test]
        public void TestEcheckRedepositWithLocation()
        {
            echeckRedeposit echeckRedeposit = new echeckRedeposit();
            echeckRedeposit.cnpTxnId = 1;
            echeckRedeposit.merchantData = new merchantDataType();
            echeckRedeposit.merchantData.campaign = "camp";
            echeckRedeposit.merchantData.affiliate = "affil";
            echeckRedeposit.merchantData.merchantGroupingId = "mgi";
            echeckRedeposit.customIdentifier = "customIdent";
           
            var mock = new Mock<Communications>();
            if (config["encryptOltpPayload"] == "true")
            {
                mock.Setup(Communications => Communications.HttpPost(It.IsRegex(".*<cnpOnlineRequest.*<encryptedPayload.*</encryptedPayload>.*", RegexOptions.Singleline)))
                .Returns("<cnpOnlineResponse version='8.13' response='0' message='Valid Format' xmlns='http://www.vantivcnp.com/schema'><echeckRedepositResponse><cnpTxnId>123</cnpTxnId><location>sandbox</location></echeckRedepositResponse></cnpOnlineResponse>");
            }
            else
            {
                mock.Setup(Communications => Communications.HttpPost(It.IsRegex(".*<echeckRedeposit.*<cnpTxnId>1</cnpTxnId>.*<merchantData>.*<campaign>camp</campaign>.*<affiliate>affil</affiliate>.*<merchantGroupingId>mgi</merchantGroupingId>.*</merchantData>.*", RegexOptions.Singleline)))
                .Returns("<cnpOnlineResponse version='8.13' response='0' message='Valid Format' xmlns='http://www.vantivcnp.com/schema'><echeckRedepositResponse><cnpTxnId>123</cnpTxnId><location>sandbox</location></echeckRedepositResponse></cnpOnlineResponse>");
            }
            Communications mockedCommunication = mock.Object;
            cnp.SetCommunication(mockedCommunication);
            var response = cnp.EcheckRedeposit(echeckRedeposit);
            
            Assert.NotNull(response);
            Assert.AreEqual("sandbox", response.location);
        }

        [Test]
        public void TestEcheckRedepositWithIdentityBundle()
        {
            echeckRedeposit echeckRedeposit = new echeckRedeposit();
            echeckRedeposit.cnpTxnId = 1;
            echeckRedeposit.merchantData = new merchantDataType();
            echeckRedeposit.merchantData.campaign = "camp";
            echeckRedeposit.merchantData.affiliate = "affil";
            echeckRedeposit.merchantData.merchantGroupingId = "mgi";
            echeckRedeposit.customIdentifier = "customIdent";

            var identityBundle = new identityBundle
            {
                merchantId = "2222",
                entityId = "3333",
                entityReference = "3batchauthandcapture",
                resourceId = "12",
                resourceReference = "111111111111111",
                commandId = "111",
                commandReference = "12345",
                orderReference = "123"
            };
            echeckRedeposit.identityBundle = identityBundle;

            var mock = new Mock<Communications>();
            if (config["encryptOltpPayload"] == "true")
            {
                mock.Setup(Communications => Communications.HttpPost(It.IsRegex(".*<cnpOnlineRequest.*<encryptedPayload.*</encryptedPayload>.*", RegexOptions.Singleline)))
                .Returns("<cnpOnlineResponse version='8.13' response='0' message='Valid Format' xmlns='http://www.vantivcnp.com/schema'><echeckRedepositResponse><cnpTxnId>123</cnpTxnId><location>sandbox</location></echeckRedepositResponse></cnpOnlineResponse>");
            }
            else
            {
                mock.Setup(Communications => Communications.HttpPost(It.IsRegex(".*<echeckRedeposit.*<identityBundle>\r\n<merchantId>2222</merchantId>.*<entityId>3333</entityId>.*<entityReference>3batchauthandcapture</entityReference>.*<resourceId>12</resourceId>.*<resourceReference>111111111111111</resourceReference>.*<commandId>111</commandId>.*<commandReference>12345</commandReference>.*<orderReference>123</orderReference></identityBundle>.*", RegexOptions.Singleline)))
                .Returns("<cnpOnlineResponse version='8.13' response='0' message='Valid Format' xmlns='http://www.vantivcnp.com/schema'><echeckRedepositResponse><cnpTxnId>123</cnpTxnId></echeckRedepositResponse></cnpOnlineResponse>");
            }
            Communications mockedCommunication = mock.Object;
            cnp.SetCommunication(mockedCommunication);
            cnp.EcheckRedeposit(echeckRedeposit);
        }
    }
}
