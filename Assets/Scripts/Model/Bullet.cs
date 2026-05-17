using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]private float moveSpeed;
    private UnitBase source;
    [SerializeField] private float releaseDelay = 0.8f;

    private void OnEnable()
    {
        CancelInvoke(nameof(AutoRelease));
        Invoke(nameof(AutoRelease), releaseDelay);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (source == null)
        {
            AutoRelease();
            return;
        }
        if (source.CompareTag("Player") && other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out EnemyBase enemy))
            {
                int damage = DamageCalculator.CalcDamage(source as TowerBase, enemy);
                enemy.OnTakeDamage?.Invoke(damage);
                AutoRelease();
            }
        }
        else if (source.CompareTag("Enemy") && other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out TowerBase player))
            {
                int damage = DamageCalculator.CalcDamage(source, player);
                player.OnTakeDamage?.Invoke(damage);
                AutoRelease();
            }
        }
        else if (other.CompareTag("Wall"))
        {
            AutoRelease();
        }
    }

    private void AutoRelease()
    {
        CancelInvoke(nameof(AutoRelease));
        BulletPool.Instance.Release(this);
    }
    public void SetSource(UnitBase source)
    {
        this.source = source;
    }
    private void OnDisable()
    {
        source = null;
    }
}