using BookMansionApi.Exceptions;
using BookMansionApi.Model.Search;
using BookMansionApi.Util;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BookMansionApiTest.Util
{
    [TestClass]
    public class HttpUtilTest
    {
        #region > GetAsync

        #region > Normal

        [TestMethod, TestCategory("Normal")]
        public async Task GetAsyncTest()
        {
            // arrange
            var uri = new Uri("https://www.google.com/");
            var expected = (int)HttpStatusCode.OK;

            // act
            var task = HttpUtil.GetAsync(uri);
            var result = await task;

            // assert
            Assert.AreEqual(expected, result.StatusCode);
        }

        #endregion

        #region > Abnormal

        [TestMethod, TestCategory("Abnormal")]
        public async Task GetAsyncTest_Exception_DNS()
        {
            // arrange
            var uri = new Uri("http://hogepiyofoge.foo.bar/");
            var expected = (int)BookManExceptionType.Dns;

            // act
            var task = HttpUtil.GetAsync(uri);
            try
            {
                var result = await task;
                Assert.Fail();
            }
            catch (Exception e)
            {
                // assert
                Assert.AreEqual(expected, e.HResult);
            }
        }

        #endregion

        #endregion

        #region > PostAsync

        #region > Normal

        [TestMethod, TestCategory("Normal")]
        public async Task PostAsyncTest()
        {
            // arrange
            var uri = new Uri("http://www.youtube.com/results");
            var content = "search_query=visualstudio";
            var expected = (int)HttpStatusCode.OK;

            // act
            var task = HttpUtil.PostAsync(uri, content);
            var result = await task;

            // assert
            Assert.AreEqual(expected, result.StatusCode);
        }

        #endregion

        #region > Abnormal

        [TestMethod, TestCategory("Abnormal")]
        public async Task PostAsyncTest_Exception_DNS()
        {
            // arrange
            var uri = new Uri("http://hogepiyofoge.foo.bar/");
            var expected = (int)BookManExceptionType.Dns;

            // act
            var task = HttpUtil.GetAsync(uri);
            try
            {
                var result = await task;
                Assert.Fail();
            }
            catch (Exception e)
            {
                // assert
                Assert.AreEqual(expected, e.HResult);
            }
        }

        #endregion

        #endregion
    }
}
