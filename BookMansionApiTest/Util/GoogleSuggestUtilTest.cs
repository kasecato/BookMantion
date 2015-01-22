using BookMansionApi.Util;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookMansionApiTest.Util
{
    [TestClass]
    public class GoogleSuggestUtilTest
    {
        #region > GetSuggestAsync

        [TestMethod, TestCategory("Normal")]
        public async Task GetSuggestAsyncTest()
        {
            // arrange
            int expected = 10;

            // act
            var actual = await GoogleSuggestUtil.GetSuggestAsync("test");

            // assert
            Assert.AreEqual(expected, actual.Count);
        }

        [TestMethod, TestCategory("Normal")]
        public async Task GetSuggestAsyncTest_NoResult()
        {
            // arrange
            int expected = 0;

            // act
            var actual = await GoogleSuggestUtil.GetSuggestAsync("arga4t5q34gq43tgq43tqgb");

            // assert
            Assert.AreEqual(expected, actual.Count);
        }

        #endregion
    }
}
