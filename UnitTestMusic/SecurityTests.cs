using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BackendMusica.Services;
using BackendMusica.Models;

namespace UnitTestMusic
{
    [TestClass]
    public class SecurityTests
    {
        [TestMethod]
        public void T001_IsBase64()
        {
            string base64_2 = "wcxrHuIFTwsROc4SG57LtQ==";

            var response = Security.IsBase64String(base64_2);

            Assert.AreEqual(true, response);

        }

        [TestMethod]
        public void T002_EncryptParams()
        {
            string data = "47";

            var response = Security.EncryptParams(data);

            Assert.AreEqual("wcxrHuIFTwsROc4SG57LtQ==", response);
        }
    }
}
