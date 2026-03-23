using UnityEngine;
using System.Collections;

public class ExplosionAttack : MonoBehaviour
{
    public EnemyData data;

    [Header("--- HIỆU ỨNG (VFX) ---")]
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] float heSoDieuChinh = 2f;

    private EnemyMovement diChuyen;
    private EnemyVisuals hinhAnh;
    private Rigidbody2D rb;
    private EnemyHealth sucKhoe;

    void Awake()
    {
        diChuyen = GetComponent<EnemyMovement>();
        hinhAnh = GetComponent<EnemyVisuals>();
        rb = GetComponent<Rigidbody2D>();
        sucKhoe = GetComponent<EnemyHealth>();
    }

    public void KichHoatNo()
    {
        StartCoroutine(DieExplosionRoutine());
    }

    private IEnumerator DieExplosionRoutine()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        if (diChuyen != null) diChuyen.enabled = false;

        MeleeAttack melee = GetComponent<MeleeAttack>();
        if (melee != null) melee.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        if (hinhAnh != null)
        {
            yield return StartCoroutine(hinhAnh.GongDonRoutine(data.thoiGianChoNo));
        }
        else
        {
            yield return new WaitForSeconds(data.thoiGianChoNo);
        }

        if (explosionEffectPrefab != null)
        {
            GameObject hieuUng = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            float kichThuocThucTe = data.banKinhNo * heSoDieuChinh;
            hieuUng.transform.localScale = new Vector3(kichThuocThucTe, kichThuocThucTe, 1f);
        }

        if (PlayerHealth.Instance != null)
        {
            float khoangCach = Vector2.Distance(transform.position, PlayerHealth.Instance.transform.position);
            if (khoangCach <= data.banKinhNo)
            {
                PlayerHealth.Instance.TakeDamage(data.damePhatNo);
            }
        }

        if (sucKhoe != null)
        {
            sucKhoe.HoanThanhChet(true);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (data != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            Gizmos.DrawSphere(transform.position, data.banKinhNo);
        }
    }
}