using System;

namespace BookMansionApi.Model.Search
{
    class KindleSearch : SearchBase
    {
        #region > Const

        private const string SEARCH_URL = "http://www.amazon.co.jp/s/?url=search-alias%3Ddigital-text&field-keywords={0}";

        private const string BOOK_STORE_NAME = "Kindle";

        private const string BOOK_STORE_IMAGE = "BookMansionApi/Images/BookStore/kindle.png";

        #endregion

        #region > Constructor

        public KindleSearch(String keyword)
        {
            base.Url = new Uri(String.Format(SEARCH_URL, keyword));
            base.BookStoreName = BOOK_STORE_NAME;
            base.BookStoreImage = BOOK_STORE_IMAGE;
        }

        #endregion
    }
}
