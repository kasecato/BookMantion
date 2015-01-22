using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Web.Http;

namespace BookMansionApi.Util
{
    public sealed class GoogleSuggestUtil
    {
        #region > Const

        private const string URL = "http://www.google.com/complete/search?hl=ja&output=toolbar&ie=utf_8&oe=utf_8&q={0}";

        private const string XPATH = "//suggestion";

        private const string ATTRIBUTE_NAME = "data";

        #endregion


        #region > Public Method

        public static IAsyncOperation<IList<string>> GetSuggestAsync(string query)
        {
            return GetSuggest(query).AsAsyncOperation<IList<string>>();
        }

        #endregion

        #region > Private Method

        private static async Task<IList<string>> GetSuggest(string query)
        {
            var response = await HttpUtil.GetAsync(new Uri(String.Format(URL, query)));
            return XmlUtil.GetAttribute(response.Content, XPATH, ATTRIBUTE_NAME);
        }

        #endregion
    }
}
