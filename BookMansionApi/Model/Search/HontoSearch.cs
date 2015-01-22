using System;

namespace BookMansionApi.Model.Search
{
    class HontoSearch : SearchBase
    {
        #region > Const

        private const string SEARCH_URL = "http://honto.jp/ebook/search_07{0}_022.html?prdNm={1}";

        private const string BOOK_STORE_NAME = "honto";

        private const string BOOK_STORE_IMAGE = "BookMansionApi/Images/BookStore/honto.png";

        #endregion

        #region > Constructor

        public HontoSearch(string keyword)
        {
            base.Url = new Uri(String.Format(SEARCH_URL, 10, keyword));
            base.BookStoreName = BOOK_STORE_NAME;
            base.BookStoreImage = BOOK_STORE_IMAGE;
        }

        #endregion
    }
}
