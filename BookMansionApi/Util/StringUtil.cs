using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.Networking.Connectivity;

namespace BookMansionApi.Util
{
    public sealed class StringUtil
    {
        #region > Field

        private static Regex PRICE = new Regex("(?<price>[0-9,]+)");

        private static Regex DATE = new Regex("(?<date>[0-9]{4}/[0-9]{1,2}/[0-9]{1,2})");

        #endregion

        #region > Public Method

        public static int ToInt(string price)
        {
            return int.Parse(PRICE.Match(price).Groups["price"].Value.Replace(",", String.Empty));
        }

        public static string ToDate(string date)
        {
            return DATE.Match(date).Groups["date"].Value;
        }

        #endregion
    }
}
