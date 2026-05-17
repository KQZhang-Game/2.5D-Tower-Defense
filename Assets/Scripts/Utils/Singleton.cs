using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    protected static T _instance;
    public static T Instance => _instance;
    protected virtual bool IsPersistent => true;
    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this as T;
        if (IsPersistent)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
