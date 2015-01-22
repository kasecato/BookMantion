using BookMansionApi.Model.Http;
using BookMansionApi.Model.Search;
using BookMansionApi.Util;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookMansionApi.MaterialAnalysis
{
    class Kindle : ISearchCompletion
    {
        public async Task<IList<BookMansionCompletion>> Search(string keyword)
        {
            /**********************************************************************
             * HTTP GET
             **********************************************************************/
            SearchBase search = new KindleSearch(keyword);
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

            var rsltGrid = HtmlUtil.GetElementByClassName(html.DocumentNode.Descendants("div"), "rsltGrid prod celwidget");
            int similarityRank = 1;
            foreach (var row in rsltGrid)
            {
                BookMansionCompletion book = new BookMansionCompletion(search.BookStoreName, search.BookStoreImage);
                book.SimilarityRank = similarityRank++;

                /**********************************************************************
                 * Image
                 **********************************************************************/
                var imageContainer = HtmlUtil.GetElementByTagClassName(row, "div", "image imageContainer").FirstOrDefault();
                var imageContainerA = HtmlUtil.GetElementByTagName(imageContainer, "a").FirstOrDefault();
                var imageContainerDiv = HtmlUtil.GetElementByTagName(imageContainerA, "div").FirstOrDefault();
                var imageContainerImg = HtmlUtil.GetElementByTagName(imageContainerDiv, "img").FirstOrDefault();
                book.Image = new Uri(HtmlUtil.GetAttributeByName(imageContainerImg, "src"));

                /**********************************************************************
                 * Title
                 **********************************************************************/
                var newaps = HtmlUtil.GetElementByTagClassName(row, "h3", "newaps").FirstOrDefault();
                var newapsA = HtmlUtil.GetElementByTagName(newaps, "a").FirstOrDefault();
                book.Title = WebUtility.HtmlDecode(newapsA.InnerText);

                /**********************************************************************
                 * URL
                 **********************************************************************/
                var imageContainerAHref = imageContainerA.Attributes.FirstOrDefault();
                book.Url = new Uri(imageContainerAHref.Value + String.Format("&tag={0}&SubscriptionId={1}", ASSOCIATE_TAG, AWS_ACCESS_KEY_ID));

                /**********************************************************************
                 * Author
                 **********************************************************************/
                var newapsSpanMedReg = HtmlUtil.GetElementByTagClassName(newaps, "span", "med reg").FirstOrDefault();
                book.Author = GetAuthor(newapsSpanMedReg.InnerText);

                /**********************************************************************
                 * Price
                 **********************************************************************/
                var rsltGridListGrey = HtmlUtil.GetElementByTagClassName(row, "ul", "rsltGridList grey").FirstOrDefault();
                book.Price = StringUtil.ToInt(rsltGridListGrey.InnerText);

                /**********************************************************************
                 * Publisher
                 **********************************************************************/

                /**********************************************************************
                 * SalesDate
                 **********************************************************************/
                book.SalesDate = StringUtil.ToDate(newapsSpanMedReg.InnerText);

                books.Add(book);
            }

            return books;
        }

        #region > Field

        // "Jonathan Rasmusson、近藤 修平、角掛 拓未、 西村 直人  (2014/6/25) - Kindle本"
        private static Regex Author = new Regex("(?<author>.+)[ ]*\\((?<salesdate>[0-9]{2,4}\\/[0-9]{1,2}\\/[0-9]{1,2})\\)");

        private const string ASSOCIATE_TAG = "bookmansion-22";

        private const string AWS_ACCESS_KEY_ID = "AKIAJIMCBSISIKKGKE5Q";

        #endregion

        #region > Private Method

        private static string GetAuthor(string text)
        {
            return Author.Match(text).Groups["author"].Value;
        }

        #endregion
    }
}
