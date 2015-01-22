using BookMansionApi.Model.Http;
using BookMansionApi.Model.Search;
using BookMansionApi.Util;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookMansionApi.MaterialAnalysis
{
    class Honto : ISearchCompletion
    {
        public async Task<IList<BookMansionCompletion>> Search(string keyword)
        {
            /**********************************************************************
             * HTTP GET
             **********************************************************************/
            SearchBase search = new HontoSearch(keyword);
            HttpResponseEntity response = await HttpUtil.GetAsync(search.Url);

            /**********************************************************************
             * HTML to Object
             **********************************************************************/
            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(response.Content);

            /**********************************************************************
             * Analyze
             **********************************************************************/
            IList<BookMansionCompletion> books = new List<BookMansionCompletion>();

            var stInfos = HtmlUtil.GetElementByClassName(html.DocumentNode.Descendants("div"), "stInfo");
            int similarityRank = 1;
            foreach (var stInfo in stInfos)
            {
                BookMansionCompletion book = new BookMansionCompletion(search.BookStoreName, search.BookStoreImage);
                book.SimilarityRank = similarityRank++;

                var div = HtmlUtil.GetElementByTagName(stInfo, "div");

                /**********************************************************************
                 * Image
                 **********************************************************************/
                var stImg = HtmlUtil.GetElementByClassName(div, "stImg").FirstOrDefault();
                var stCover = HtmlUtil.GetElementByTagClassName(stImg, "p", "stCover").FirstOrDefault();
                var stCoverA = HtmlUtil.GetElementByTagName(stCover, "a").FirstOrDefault();
                var stCoverImg = HtmlUtil.GetElementByTagName(stCoverA, "img").FirstOrDefault();
                book.Image = new Uri(stCoverImg.Attributes.FirstOrDefault().Value);

                /**********************************************************************
                 * Title
                 **********************************************************************/
                var stContents = HtmlUtil.GetElementByClassName(div, "stContents").FirstOrDefault();
                var stHeading = HtmlUtil.GetElementByTagClassName(stContents, "h2", "stHeading").FirstOrDefault();
                var stHeadingA = HtmlUtil.GetElementByTagName(stHeading, "a").FirstOrDefault();
                book.Title = stHeadingA.InnerText;

                /**********************************************************************
                 * URL
                 **********************************************************************/
                book.Url = new Uri(stHeadingA.Attributes.FirstOrDefault().Value);

                /**********************************************************************
                 * Author
                 **********************************************************************/
                var stData = HtmlUtil.GetElementByTagClassName(stContents, "ul", "stData").FirstOrDefault();
                var stAuthor = HtmlUtil.GetElementByTagClassName(stData, "li", "stAuthor").FirstOrDefault();
                if (stAuthor != null)
                {
                    var stAuthorA = HtmlUtil.GetElementByTagName(stAuthor, "a").FirstOrDefault();
                    book.Author = stAuthorA.InnerText;
                }

                /**********************************************************************
                 * Price
                 **********************************************************************/
                var stPrice = HtmlUtil.GetElementByTagClassName(stData, "li", "stPrice").FirstOrDefault();
                var stYen = HtmlUtil.GetElementByTagClassName(stPrice, "span", "stYen").FirstOrDefault();
                book.Price = StringUtil.ToInt(stYen.InnerText);

                /**********************************************************************
                 * Publisher
                 **********************************************************************/
                var stDataLi = HtmlUtil.GetElementByTagName(stData, "li");
                int publisher_index = stDataLi.Count() == 6 ? stDataLi.Count() - 3 : stDataLi.Count() - 2;
                var publisher = HtmlUtil.GetElementByTagName(stDataLi.ElementAt(publisher_index), "a").FirstOrDefault();
                book.Publisher = publisher.InnerText;

                /**********************************************************************
                 * SalesDate
                 **********************************************************************/
                int salesDate_index = stDataLi.Count() - 1;
                var salesDate = stDataLi.ElementAt(salesDate_index);
                book.SalesDate = StringUtil.ToDate(salesDate.InnerText);

                books.Add(book);
            }

            return books;
        }
    }
}
