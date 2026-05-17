using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class XmlDataMgr
{
    private static XmlDataMgr instance = new XmlDataMgr();
    public static XmlDataMgr Instance => instance;
    private XmlDataMgr() { }
    public void SaveData(object data, string filePath)
    {
        if (data == null) return;
        string path = Application.persistentDataPath + "/" + filePath + ".xml";
        using (StreamWriter sw = new StreamWriter(path))
        {
            XmlSerializer serializer = new XmlSerializer(data.GetType());
            serializer.Serialize(sw, data);
        }
    }
    public object LoadData(Type type, string filePath)
    {
        string path = Application.persistentDataPath + "/" + filePath + ".xml";
        if (!File.Exists(path))
        {
            path = Application.streamingAssetsPath + "/" + filePath + ".xml";
            if (!File.Exists(path))
            {
                return null;
            }
        }
        using (StreamReader sr = new StreamReader(path))
        {
            XmlSerializer serializer = new XmlSerializer(type);
            return serializer.Deserialize(sr);  
        }
    }
}
