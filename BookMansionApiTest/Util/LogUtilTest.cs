using BookMansionApi.Util;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;

namespace BookMansionApiTest.Util
{
    [TestClass]
    public class LogUtilTest
    {
        #region > Error

        [TestMethod, TestCategory("Normal")]
        public void Logging()
        {
            // act
            try
            {
                LogUtil.Logging.Error("");
            } 
            catch(Exception)
            {
                Assert.Fail();
            }
        }

        #endregion
    }
}
