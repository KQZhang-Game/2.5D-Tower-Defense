using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

public class DictSerializable<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
{
    public XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
        XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
        reader.Read(); //跳过根节点
        while (reader.NodeType!=XmlNodeType.EndElement)
        {
            TKey key = (TKey)keySerializer.Deserialize(reader);
            TValue value = (TValue)valueSerializer.Deserialize(reader);
            this.Add(key, value);
        }
        reader.Read();
    }

    public void WriteXml(XmlWriter writer)
    {
        XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
        XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
        foreach (TKey key in this.Keys)
        {
            keySerializer.Serialize(writer, key);
            valueSerializer.Serialize(writer, this[key]);
        }
    }
}
