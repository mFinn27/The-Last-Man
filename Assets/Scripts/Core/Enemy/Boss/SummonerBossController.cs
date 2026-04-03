using UnityEngine;
using System.Collections;

public class SummonerBossController : MonoBehaviour
{
    public SummonerBossData data;

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
            yield return StartCoroutine(PhaseTrieuHoi());
            yield return StartCoroutine(PhaseDuoiBat());
            yield return StartCoroutine(PhaseBanLienTuc());
        }
    }

    private IEnumerator PhaseDuoiBat()
    {
        if (diChuyen != null) diChuyen.isCharging = false;
        yield return new WaitForSeconds(data.thoiGianDuoiBat);
    }

    private IEnumerator PhaseTrieuHoi()
    {
        if (diChuyen != null) diChuyen.isCharging = true;

        rb.linearVelocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (hinhAnh != null) yield return StartCoroutine(hinhAnh.GongDonRoutine(1.2f));

        if (data.quaiNhoPrefab != null)
        {
            for (int i = 0; i < data.soLuongQuaiSpawn; i++)
            {
                Vector2 viTriSpawn = (Vector2)transform.position + Random.insideUnitCircle * 2f;
                Instantiate(data.quaiNhoPrefab, viTriSpawn, Quaternion.identity);
            }
        }

        if (hinhAnh != null) yield return StartCoroutine(hinhAnh.NayLenSauKhiBanRoutine());

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator PhaseBanLienTuc()
    {
        if (diChuyen != null) diChuyen.isCharging = true;

        rb.linearVelocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (hinhAnh != null) yield return StartCoroutine(hinhAnh.GongDonRoutine(0.5f));

        for (int i = 0; i < data.soLuongDanBurst; i++)
        {
            if (player == null) break;

            Vector2 huongDenPlayer = (player.position - transform.position).normalized;
            float goc = Mathf.Atan2(huongDenPlayer.y, huongDenPlayer.x) * Mathf.Rad2Deg;

            if (data.danPrefab != null && EnemyBulletPool.Instance != null)
            {
                GameObject danObj = EnemyBulletPool.Instance.GetBullet(data.danPrefab);
                danObj.transform.position = transform.position;
                danObj.transform.rotation = Quaternion.Euler(0, 0, goc);

                EnemyBullet bullet = danObj.GetComponent<EnemyBullet>();
                if (bullet != null) bullet.Setup(data.dame, data.tocDoDan);
            }

            yield return new WaitForSeconds(data.thoiGianGiuaCacVien);
        }

        if (hinhAnh != null) yield return StartCoroutine(hinhAnh.NayLenSauKhiBanRoutine());

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(1.5f);
    }
}