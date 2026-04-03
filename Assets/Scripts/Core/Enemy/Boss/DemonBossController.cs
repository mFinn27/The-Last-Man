using UnityEngine;
using System.Collections;

public class DemonBossController : MonoBehaviour
{
    public DemonBossData data;

    private Transform player;
    private Rigidbody2D rb;
    private EnemyMovement diChuyen;
    private EnemyVisuals hinhAnh;

    void Start()
    {
        if (PlayerHealth.Instance != null) player = PlayerHealth.Instance.transform;
        rb = GetComponent<Rigidbody2D>();
        diChuyen = GetComponent<EnemyMovement>();
        hinhAnh = GetComponent<EnemyVisuals>();

        if (GetComponent<MeleeAttack>()) GetComponent<MeleeAttack>().enabled = false;
        if (GetComponent<RangedAttack>()) GetComponent<RangedAttack>().enabled = false;
        if (GetComponent<DashAttack>()) GetComponent<DashAttack>().enabled = false;

        StartCoroutine(BossCycleRoutine());
    }

    private IEnumerator BossCycleRoutine()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            yield return StartCoroutine(PhaseDuoiBat());
            yield return StartCoroutine(PhaseBanToaTron());
            yield return StartCoroutine(PhaseLuot());
        }
    }

    private IEnumerator PhaseDuoiBat()
    {
        if (diChuyen != null) diChuyen.isCharging = false;
        yield return new WaitForSeconds(data.thoiGianDuoiBat);
    }

    private IEnumerator PhaseBanToaTron()
    {
        if (diChuyen != null) diChuyen.isCharging = true;

        rb.linearVelocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (hinhAnh != null) yield return StartCoroutine(hinhAnh.GongDonRoutine(1f));

        if (data.danPrefab != null && EnemyBulletPool.Instance != null)
        {
            float gocChia = 360f / data.soLuongDanToaTron;
            for (int i = 0; i < data.soLuongDanToaTron; i++)
            {
                float gocHienTai = i * gocChia;
                GameObject danObj = EnemyBulletPool.Instance.GetBullet(data.danPrefab);
                danObj.transform.position = transform.position;
                danObj.transform.rotation = Quaternion.Euler(0, 0, gocHienTai);

                EnemyBullet bullet = danObj.GetComponent<EnemyBullet>();
                if (bullet != null) bullet.Setup(data.dame, data.tocDoDan);
            }
        }

        if (hinhAnh != null) yield return StartCoroutine(hinhAnh.NayLenSauKhiBanRoutine());

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(1.5f);
    }

    private IEnumerator PhaseLuot()
    {
        for (int i = 0; i < data.soLanLuot; i++)
        {
            if (diChuyen != null) diChuyen.isCharging = true;

            rb.linearVelocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

            Vector2 huongLuot = (player.position - transform.position).normalized;
            if (hinhAnh != null) yield return StartCoroutine(hinhAnh.GongDonRoutine(0.5f));

            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            float thoiGianDaLuot = 0f;
            bool daGayDame = false;
            float tocDoLuotThuc = data.tocDoDiChuyen * 6f;

            while (thoiGianDaLuot < 0.35f)
            {
                thoiGianDaLuot += Time.deltaTime;
                rb.linearVelocity = huongLuot * tocDoLuotThuc;

                if (!daGayDame && Vector2.Distance(transform.position, player.position) <= 1.5f)
                {
                    PlayerHealth.Instance.TakeDamage(data.dame);
                    daGayDame = true;
                }
                yield return null;
            }

            rb.linearVelocity = Vector2.zero;
            if (hinhAnh != null) StartCoroutine(hinhAnh.NayLenSauKhiBanRoutine());
            yield return new WaitForSeconds(0.6f);
        }
    }
}