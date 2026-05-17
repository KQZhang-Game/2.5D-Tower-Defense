using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : Singleton<BulletPool>
{
    protected override bool IsPersistent => false;
    private ObjectPool<Bullet> bulletPool;
    [SerializeField]private int defaultCount = 100;
    [SerializeField]private int maxCount = 150;
    [SerializeField]private Bullet bulletPrefab;
    private void Start()
    {
        bulletPool = new ObjectPool<Bullet>(createFunc: () => Instantiate(bulletPrefab),
            actionOnGet: (bullet) => { bullet.gameObject.SetActive(true); },
            actionOnRelease: (bullet) =>
            {
                bullet.gameObject.SetActive(false);
                bullet.transform.rotation = Quaternion.identity;
            },
            actionOnDestroy: (bullet) => { Destroy(bullet.gameObject); },
            collectionCheck: true,
            defaultCapacity: defaultCount, 
            maxSize: maxCount
            );
    }
    public Bullet GetBullet(UnitBase tower)
    {
        Bullet bullet = bulletPool.Get();
        bullet.SetSource(tower);
        return bullet;
    }
    public void Release(Bullet bullet)
    {
        bulletPool.Release(bullet);
    }
}
