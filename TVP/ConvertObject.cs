using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace TVP
{
    public sealed class ConvertObject
    {
        private static readonly ConvertObject instance = new ConvertObject();

        private ConvertObject()
        {

        }

        public static ConvertObject Instance
        {
            get
            {
                return instance;
            }
        }

        public string DictionaryToXml(Dictionary<string, string> requestParam)
        {
            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8
                //Indent = true //xml을 보기 좋게는 하는 api에서 사용 못함.
            };

            //엠프런티어 솔류션 Request양식
            StringBuilder sbXML = new StringBuilder();
            using (XmlWriter wr = XmlWriter.Create(sbXML, settings))
            {
                wr.WriteStartDocument();
                    wr.WriteStartElement("XML");
                        wr.WriteStartElement("Table");
                            wr.WriteAttributeString("Name", "DATA");
                                wr.WriteStartElement("rs");
                                foreach (KeyValuePair<string, string> kv in requestParam)
                                {
                                    wr.WriteElementString(kv.Key, kv.Value);
                                }
                                wr.WriteEndElement();
                        wr.WriteEndElement();
                    wr.WriteEndElement();
                wr.WriteEndDocument();

                wr.Flush();
            }

            return sbXML.ToString();
        }
    }
}
