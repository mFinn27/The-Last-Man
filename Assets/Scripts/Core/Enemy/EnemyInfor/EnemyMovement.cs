using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public EnemyData data;

    [HideInInspector] public bool dangBiDayLui = false;
    [HideInInspector] public bool isCharging = false;

    private Transform player;
    private Rigidbody2D rb;
    private Collider2D myCollider;
    private Collider2D playerCollider;
    private float thoiGianGayDameTiepTheo = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();

        if (PlayerHealth.Instance != null)
        {
            player = PlayerHealth.Instance.transform;
            playerCollider = PlayerHealth.Instance.GetComponent<Collider2D>();
        }
    }

    void FixedUpdate()
    {
        if (player == null || data == null) return;

        if (dangBiDayLui || isCharging) return;

        Vector2 huong = (player.position - transform.position).normalized;
        rb.linearVelocity = huong * data.tocDoDiChuyen;
    }

    void Update()
    {
        if (player == null || data == null || data.loaiQuai == EnemyType.Melee) return;

        if (Time.time >= thoiGianGayDameTiepTheo)
        {
            bool dangChamVaoPlayer = false;
            float khoangCachSqr = (transform.position - player.position).sqrMagnitude;
            float tamVaCham = data.loaiQuai == EnemyType.Boss ? 2f : 1.2f;

            if (khoangCachSqr <= tamVaCham * tamVaCham)
            {
                dangChamVaoPlayer = true;
            }

            if (dangChamVaoPlayer)
            {
                PlayerHealth.Instance.TakeDamage(data.dame);
                float thoiGianHoi = data.tgThucHienDonDanhTiepTheo > 0 ? data.tgThucHienDonDanhTiepTheo : 1f;
                thoiGianGayDameTiepTheo = Time.time + thoiGianHoi;
            }
        }
    }
}