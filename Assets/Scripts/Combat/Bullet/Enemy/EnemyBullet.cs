using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private int damage;
    private float speed;

    [SerializeField] private float lifeTimeMacDinh = 4f;
    private float lifeTimeHienTai;

    private GameObject prefabGoc;

    public void SetPrefabGoc(GameObject prefab)
    {
        prefabGoc = prefab;
    }

    public void Setup(int dame, float tocDo)
    {
        damage = dame;
        speed = tocDo;
        lifeTimeHienTai = lifeTimeMacDinh;
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        lifeTimeHienTai -= Time.deltaTime;
        if (lifeTimeHienTai <= 0)
        {
            TraVeKho();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PlayerHealth.Instance != null)
            {
                PlayerHealth.Instance.TakeDamage(damage);
            }
            TraVeKho();
        }
    }

    private void TraVeKho()
    {
        if (EnemyBulletPool.Instance != null && prefabGoc != null)
        {
            EnemyBulletPool.Instance.ReturnBullet(gameObject, prefabGoc);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}