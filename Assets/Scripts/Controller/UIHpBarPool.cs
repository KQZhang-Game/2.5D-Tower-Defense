using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class UIHpBarPool : Singleton<UIHpBarPool>
{
    [SerializeField] private UnitHpBar barPrefab;
    [SerializeField] private Transform fatherObj;
    protected override bool IsPersistent => false;
    private ObjectPool<UnitHpBar> hpBarImagePool;
    protected override void Awake()
    {
        base.Awake();
        hpBarImagePool = new ObjectPool<UnitHpBar>(createFunc: () => Instantiate(barPrefab),
            actionOnGet: (obj) => { obj.gameObject.SetActive(true); }, actionOnRelease: (obj) => { obj.gameObject.SetActive(false); },
            actionOnDestroy: (obj) => Destroy(obj),collectionCheck:false,30,50);
    }
    public UnitHpBar GetHpBar()
    {
        UnitHpBar newHpBar = hpBarImagePool.Get();
        newHpBar.transform.SetParent(fatherObj,false);
        return newHpBar;
    }
    public void Release(UnitHpBar obj)
    {
        if (obj == null) return;
        obj.ResetHpBar();
        hpBarImagePool.Release(obj);
    }
}
