using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public EnemyData data;
    [SerializeField] private float mauHienTai;

    private Rigidbody2D rb;
    private EnemyMovement diChuyen;
    private EnemyVisuals hinhAnh;
    private bool daChet = false;

    private void OnEnable()
    {
        WaveManager.OnWaveEnded += TuSatKhiHetWave;
    }
    private void OnDisable()
    {
        WaveManager.OnWaveEnded -= TuSatKhiHetWave;
    }

    void Start()
    {
        if (data != null) mauHienTai = data.mauToiDa;
        rb = GetComponent<Rigidbody2D>();
        diChuyen = GetComponent<EnemyMovement>();
        hinhAnh = GetComponent<EnemyVisuals>();

        if (data != null && data.loaiQuai == EnemyType.Boss && WaveManager.Instance != null)
        {
            WaveManager.Instance.DangKyBoss();
        }
    }

    public void TakeDamage(float dame, Vector2 huongDayLui, float lucDayLui)
    {
        if (daChet) return;

        mauHienTai -= dame;

        if (hinhAnh != null) hinhAnh.PlayFlashWhite();

        if (rb != null && lucDayLui > 0 && (data == null || data.loaiQuai != EnemyType.Boss))
        {
            StartCoroutine(KnockbackRoutine(huongDayLui, lucDayLui));
        }

        if (mauHienTai <= 0)
        {
            daChet = true;
            ExplosionAttack bom = GetComponent<ExplosionAttack>();
            if (bom != null) bom.KichHoatNo();
            else StartCoroutine(DieRoutine());
        }
    }

    private IEnumerator KnockbackRoutine(Vector2 huong, float luc)
    {
        if (diChuyen != null) diChuyen.dangBiDayLui = true;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(huong * luc, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.15f);

        if (!daChet)
        {
            rb.linearVelocity = Vector2.zero;
            if (diChuyen != null) diChuyen.dangBiDayLui = false;
        }
    }

    private IEnumerator DieRoutine()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        if (diChuyen != null) diChuyen.enabled = false;
        if (hinhAnh != null) hinhAnh.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        HoanThanhChet(false);

        float thoiGianChet = 0.2f;
        float thoiGianDaQua = 0f;
        Vector3 scaleBanDau = transform.localScale;
        float tocDoXoay = 1500f;

        while (thoiGianDaQua < thoiGianChet)
        {
            thoiGianDaQua += Time.deltaTime;
            float phanTram = thoiGianDaQua / thoiGianChet;
            transform.localScale = Vector3.Lerp(scaleBanDau, Vector3.zero, phanTram);
            transform.Rotate(0, 0, tocDoXoay * Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject);
    }

    public void HoanThanhChet(bool xoaNgayLapTuc = true)
    {
        RotDo();
        if (xoaNgayLapTuc) Destroy(gameObject);
    }

    private void RotDo()
    {
        if (data != null && Random.value <= data.tiLeRotThuong)
        {
            if (data.phanThuongRotRa == LoaiPhanThuong.vang && CoinPool.Instance != null)
            {
                int soLuongVien = Random.Range(data.minSoLuongVang, data.maxSoLuongVang + 1);
                for (int i = 0; i < soLuongVien; i++)
                {
                    GameObject ngoc = CoinPool.Instance.GetCoin();
                    ngoc.transform.position = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
                    Coin scriptCoin = ngoc.GetComponent<Coin>();
                    if (scriptCoin != null) scriptCoin.Setup(Random.Range(data.minGiaTriMoiVien, data.maxGiaTriMoiVien + 1));
                }
            }
        }
    }

    private void TuSatKhiHetWave()
    {
        if (daChet) return;
        daChet = true;
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (data != null && data.loaiQuai == EnemyType.Boss && mauHienTai <= 0)
        {
            if (WaveManager.Instance != null)
            {
                WaveManager.Instance.BossDaChet();
            }
        }
    }
}