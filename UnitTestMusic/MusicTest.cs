using BackendMusica.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;

namespace UnitTestMusic
{
    [TestClass]
    public class MusicTest
    {
        #region Configurations
        private MusicServices _musicServices;

        public MusicTest()
        {
            _musicServices = new MusicServices();
        }
        #endregion

        [TestMethod]
        public void T001_GetSong()
        {
            HttpResponseMessage expected = new HttpResponseMessage();

            bool fileContent = true;

            var response = _musicServices.GetCanciones(3, fileContent);

            Assert.AreEqual(expected, response);
        }
    }
}
