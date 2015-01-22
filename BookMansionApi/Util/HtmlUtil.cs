using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using Windows.Networking.Connectivity;

namespace BookMansionApi.Util
{
    class HtmlUtil
    {
        #region > Public Method

        public static IEnumerable<HtmlNode> GetElementByClassName(IEnumerable<HtmlNode> htmlNodes, string className)
        {
            return htmlNodes.Where(x => x.Attributes.Contains("class")
                                     && x.Attributes["class"].Value.Contains(className));
        }

        public static IEnumerable<HtmlNode> GetElementByTagName(HtmlNode htmlNode, string tagName)
        {
            return htmlNode.ChildNodes.Where(x => x.Name == tagName);
        }

        public static IEnumerable<HtmlNode> GetElementByTagClassName(HtmlNode htmlNode, string tagName, string className)
        {
            return GetElementByClassName(GetElementByTagName(htmlNode, tagName), className);
        }

        public static string GetAttributeByName(HtmlNode htmlNode, string attributeName)
        {
            return htmlNode.Attributes[attributeName].Value;
        }

        #endregion
    }
}
