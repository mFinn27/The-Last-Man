using UnityEngine;
using System.Collections;

public class RangedAttack : MonoBehaviour
{
    public RangedEnemyData data;
    private Transform player;
    private float thoiGianBanTiepTheo;
    private EnemyMovement diChuyen;
    private EnemyVisuals hinhAnh;
    private Rigidbody2D rb;

    void Awake()
    {
        diChuyen = GetComponent<EnemyMovement>();
        hinhAnh = GetComponent<EnemyVisuals>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        thoiGianBanTiepTheo = 0f;
        if (PlayerHealth.Instance != null) player = PlayerHealth.Instance.transform;
    }

    void Update()
    {
        if (player == null || data == null) return;
        if (diChuyen != null && (diChuyen.dangBiDayLui || diChuyen.isCharging)) return;

        float khoangCachSqr = (player.position - transform.position).sqrMagnitude;
        if (khoangCachSqr <= data.tamDanhXa * data.tamDanhXa)
        {
            if (Time.time >= thoiGianBanTiepTheo) StartCoroutine(ShootRoutine());
        }
    }

    private IEnumerator ShootRoutine()
    {
        if (diChuyen != null) diChuyen.isCharging = true;
        if (rb != null) rb.linearVelocity = Vector2.zero;

        if (hinhAnh != null) yield return StartCoroutine(hinhAnh.GongDonRoutine(data.thoiGianGongDon));
        else yield return new WaitForSeconds(data.thoiGianGongDon);

        Shoot();

        if (hinhAnh != null) yield return StartCoroutine(hinhAnh.NayLenSauKhiBanRoutine());
        if (diChuyen != null) diChuyen.isCharging = false;
        thoiGianBanTiepTheo = Time.time + data.tgThucHienDonDanhTiepTheo;
    }

    private void Shoot()
    {
        if (data.danPrefab == null || EnemyBulletPool.Instance == null) return;
        Vector2 huongDenPlayer = (player.position - transform.position).normalized;
        float gocGoc = Mathf.Atan2(huongDenPlayer.y, huongDenPlayer.x) * Mathf.Rad2Deg;
        float gocBatDau = gocGoc - (data.soLuongDan - 1) * data.gocToaDan / 2f;

        for (int i = 0; i < data.soLuongDan; i++)
        {
            float gocHienTai = gocBatDau + (i * data.gocToaDan);
            GameObject danObj = EnemyBulletPool.Instance.GetBullet(data.danPrefab);
            danObj.transform.position = transform.position;
            danObj.transform.rotation = Quaternion.Euler(0, 0, gocHienTai);

            EnemyBullet bullet = danObj.GetComponent<EnemyBullet>();
            if (bullet != null) bullet.Setup(data.dame, data.tocDoDan);
        }
    }
}