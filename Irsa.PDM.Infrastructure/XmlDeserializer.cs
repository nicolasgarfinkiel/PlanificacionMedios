using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Irsa.PDM.Infrastructure
{
    public static class XmlDeserializer
    {
        public static T DeserializeXML<T>(this Stream @this) where T : class
        {
            try
            {
                var reader = XmlReader.Create(@this, new XmlReaderSettings { ConformanceLevel = ConformanceLevel.Document });
                return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
