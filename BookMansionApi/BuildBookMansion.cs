using BookMansionApi.Exceptions;
using BookMansionApi.MaterialAnalysis;
using BookMansionApi.Model.Http;
using BookMansionApi.Model.Search;
using BookMansionApi.Settings;
using BookMansionApi.Util;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace BookMansionApi
{
    public sealed class BuildBookMansion
    {
        #region > Public Method

        public static IAsyncOperation<IList<BookMansionCompletion>> SearchAsync(string keyword)
        {
            return Search(keyword).AsAsyncOperation<IList<BookMansionCompletion>>();
        }

        #endregion

        #region > Private Method

        private async static Task<IList<BookMansionCompletion>> Search(string keyword)
        {
            List<BookMansionCompletion> books = new List<BookMansionCompletion>();

            /**********************************************************************
             * honto
             **********************************************************************/
            var honto = await new Honto().Search(keyword);
            books.AddRange(honto);

            /**********************************************************************
             * Kindle
             **********************************************************************/
            //var kindle = await new Kindle().Search(keyword);
            //books.AddRange(kindle);

            return books;
        }

        #endregion
    }
}
