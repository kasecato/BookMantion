using System;

namespace BookMansionApi.Model.Search
{
    class SearchBase
    {
        #region > Property

        public Uri Url { get; set; }

        public string BookStoreName { get; set; }

        public string BookStoreImage { get; set; }

        #endregion
    }
}
