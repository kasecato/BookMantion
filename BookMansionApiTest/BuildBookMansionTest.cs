using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Threading.Tasks;

namespace BookMansionApiTest
{
    [TestClass]
    public class BuildBookMansionTest
    {
        #region > SearchAsync

        #region > Normal

        [TestMethod, TestCategory("Normal")]
        public async Task SearchAsyncTest()
        {
            // arrange
            var keyword = "アジャイル";

            // act
            var actual = await BookMansionApi.BuildBookMansion.SearchAsync(keyword);

            // assert
            Assert.IsTrue(0 < actual.Count);
        }

        #endregion

        #endregion
    }
}
