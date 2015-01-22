using System;
using System.Runtime.Serialization;

namespace BookMansionApi.Model.Search
{
    [DataContract(Name = "bookMansionCompletion")]
    public sealed class BookMansionCompletion
    {
        #region > Property

        [DataMember(Order = 0850, IsRequired = false, Name = "bookStoreName")]
        public string BookStoreName { get; set; }

        [DataMember(Order = 0900, IsRequired = false, Name = "bookStoreImage")]
        public string BookStoreImage { get; set; }

        [DataMember(Order = 0950, IsRequired = false, Name = "similarityRank")]
        public int SimilarityRank { get; set; }

        [DataMember(Order = 1000, IsRequired = false, Name = "title")]
        public string Title { get; set; }

        [DataMember(Order = 1050, IsRequired = false, Name = "author")]
        public string Author { get; set; }

        [DataMember(Order = 1100, IsRequired = false, Name = "Publisher")]
        public string Publisher { get; set; }

        [DataMember(Order = 1150, IsRequired = false, Name = "salesDate")]
        public string SalesDate { get; set; }

        [DataMember(Order = 1200, IsRequired = false, Name = "price")]
        public int Price { get; set; }

        [DataMember(Order = 1250, IsRequired = false, Name = "url")]
        public Uri Url { get; set; }

        [DataMember(Order = 1300, IsRequired = false, Name = "image")]
        public Uri Image { get; set; }

        #endregion

        #region > Constructor

        public BookMansionCompletion(string bookstorename, string bookstoreimage)
        {
            BookStoreName = bookstorename;
            BookStoreImage = bookstoreimage;
        }

        #endregion
    }
}
