using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#nullable enable
public class PlayerPrefsTool
{
    private static PlayerPrefsTool? instance;
    public static PlayerPrefsTool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerPrefsTool();
            }
            return instance;
        }
    }
    public void SaveData(object data, string keyName)
    {
        if (data == null) return;
        Type dataType = data.GetType();
        if (dataType == typeof(int))
        {
            PlayerPrefs.SetInt(keyName, (int)data);
            return;
        }
        else if (dataType == typeof(float))
        {
            PlayerPrefs.SetFloat(keyName, (float)data);
            return;
        }
        else if (dataType == typeof(string))
        {
            PlayerPrefs.SetString(keyName, data.ToString());
            return;
        }
        else if (dataType == typeof(bool))
        {
            PlayerPrefs.SetInt(keyName, (bool)data ? 1 : 0);
            return;
        }
        FieldInfo[] fields = dataType.GetFields();
        Type fieldType;
        foreach (FieldInfo field in fields)
        {
            fieldType = field.FieldType;
            //存储规则:传进来的keyName + 对象的类型 + 对象名 + 字段类型 + 字段名 
            string saveKeyName = $"{keyName}_{field.Name}";
            if (typeof(IList).IsAssignableFrom(fieldType))
            {
                IList list = (IList)field.GetValue(data);
                if (list is null) continue;
                PlayerPrefs.SetInt($"{saveKeyName}_Count", list.Count);
                for (int i = 0; i < list.Count; i++)
                {
                    SaveData(list[i], $"{saveKeyName}_Item{i}");
                }
            }
            else if (typeof(IDictionary).IsAssignableFrom(fieldType))
            {
                IDictionary dict = (IDictionary)field.GetValue(data);
                if (dict is null) continue;
                PlayerPrefs.SetInt($"{saveKeyName}_Count", dict.Count);
                int index = 0;
                foreach (var key in dict.Keys)
                {
                    SaveData(key, $"{saveKeyName}_Key{index}");
                    SaveData(dict[key], $"{saveKeyName}_Value{index}");
                    index++;
                }
            }
            else
            {
                SaveData(field.GetValue(data), saveKeyName);
            }
        }
        PlayerPrefs.Save();
    }
    public object LoadData(Type type, string keyName)
    {
        if (type == typeof(int))
            return PlayerPrefs.GetInt(keyName);
        if (type == typeof(float))
            return PlayerPrefs.GetFloat(keyName);
        if (type == typeof(string))
            return PlayerPrefs.GetString(keyName);
        if (type == typeof(bool))
            return PlayerPrefs.GetInt(keyName) == 1;
        object data = Activator.CreateInstance(type);
        FieldInfo[] fields = type.GetFields();
        foreach (FieldInfo field in fields)
        {
            Type fieldType = field.FieldType;
            //相同规则还原keyName
            string loadKeyName = $"{keyName}_{field.Name}";
            if (typeof(IList).IsAssignableFrom(fieldType))
            {
                IList? list = Activator.CreateInstance(fieldType) as IList;
                if (list is null) continue;
                Type[] argsType = fieldType.GetGenericArguments();
                int itemCount = PlayerPrefs.GetInt($"{loadKeyName}_Count");
                for (int i = 0; i < itemCount; i++)
                {
                    list.Add(LoadData(argsType[0], $"{loadKeyName}_Item{i}"));
                }
                field.SetValue(data, list);
            }
            else if (typeof(IDictionary).IsAssignableFrom(fieldType))
            {
                IDictionary? dict = Activator.CreateInstance(fieldType) as IDictionary;
                if (dict is null) continue;
                Type keyType = fieldType.GetGenericArguments()[0];
                Type valueType = fieldType.GetGenericArguments()[1];
                int itemCount = PlayerPrefs.GetInt($"{loadKeyName}_Count");
                for (int i = 0; i < itemCount; i++)
                {
                    object key = LoadData(keyType, $"{loadKeyName}_Key{i}");
                    object value = LoadData(valueType, $"{loadKeyName}_Value{i}");
                    if (key == null || string.IsNullOrEmpty(key.ToString())) continue;
                    if (!dict.Contains(key)) dict.Add(key, value);
                }
                field.SetValue(data, dict);
            }
            else
            {
                object obj = LoadData(fieldType, loadKeyName);
                field.SetValue(data, obj);
            }
        }
        return data;
    }
}
