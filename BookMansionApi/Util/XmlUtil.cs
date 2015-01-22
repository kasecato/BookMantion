using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Windows.Data.Xml.Dom;

namespace BookMansionApi.Util
{
    class XmlUtil
    {
        #region > Public Method

        public static T Deserialize<T>(string xml)
        {
            Byte[] _Bytes = Encoding.Unicode.GetBytes(xml);
            using (MemoryStream _Stream = new MemoryStream(_Bytes))
            {
                XmlSerializer _Serializer = new XmlSerializer(typeof(T));
                return (T)_Serializer.Deserialize(_Stream);
            }
        }

        public static string Serialize(object instance)
        {
            using (MemoryStream _Stream = new MemoryStream())
            {
                XmlSerializer _Serializer = new XmlSerializer(instance.GetType());
                _Serializer.Serialize(_Stream, instance);
                _Stream.Position = 0;
                using (StreamReader _Reader = new StreamReader(_Stream))
                { return _Reader.ReadToEnd(); }
            }
        }

        public static IList<string> GetAttribute(string xml, string path, string attributeName)
        {
            XmlDocument root = new XmlDocument();
            root.LoadXml(xml);
            XmlNodeList nodes = root.SelectNodes(path);
            IList<string> result = new List<string>();
            foreach (IXmlNode node in nodes)
            {
                result.Add(node.Attributes.GetNamedItem(attributeName).InnerText);
            }
            return result;
        }

        #endregion
    }
}