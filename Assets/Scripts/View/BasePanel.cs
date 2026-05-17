using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel<T> : Singleton<T> where T : BasePanel<T>
{
    protected override bool IsPersistent => false;

    public virtual void ShowMe()
    {
        gameObject.SetActive(true);
    }

    public virtual void HideMe()
    {
        gameObject.SetActive(false);
    }
}