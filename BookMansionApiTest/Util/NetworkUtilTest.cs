using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace BookMansionApiTest.Util
{
    [TestClass]
    public class NetworkUtilTest
    {
        #region > IsNetworkAvailable

        [TestMethod, TestCategory("Normal")]
        public void IsNetworkAvailableTest_Available()
        {
            // arrange
            bool expected = true;

            // act
            bool actual = BookMansionApi.Util.NetworkUtil.IsNetworkAvailable();

            // assert
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
