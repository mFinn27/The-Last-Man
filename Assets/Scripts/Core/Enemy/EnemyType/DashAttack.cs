using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class DashAttack : MonoBehaviour
{
    public DashEnemyData data;
    private Transform player;
    private float thoiGianLuotTiepTheo;

    private EnemyMovement diChuyen;
    private EnemyVisuals hinhAnh;
    private Rigidbody2D rb;
    private LineRenderer canhBaoDuongLuot;
    private bool dangLuot = false;

    void Start()
    {
        if (PlayerHealth.Instance != null) player = PlayerHealth.Instance.transform;

        diChuyen = GetComponent<EnemyMovement>();
        hinhAnh = GetComponent<EnemyVisuals>();
        rb = GetComponent<Rigidbody2D>();

        canhBaoDuongLuot = GetComponent<LineRenderer>();
        canhBaoDuongLuot.enabled = false;
        canhBaoDuongLuot.positionCount = 2;
        canhBaoDuongLuot.startWidth = 0.8f;
        canhBaoDuongLuot.endWidth = 0.8f;
        canhBaoDuongLuot.material = new Material(Shader.Find("Sprites/Default"));
        canhBaoDuongLuot.startColor = new Color(1f, 0f, 0f, 0.4f);
        canhBaoDuongLuot.endColor = new Color(1f, 0f, 0f, 0.1f);
        canhBaoDuongLuot.sortingOrder = -1;
    }

    void Update()
    {
        if (player == null || data == null || dangLuot) return;
        if (diChuyen != null && (diChuyen.dangBiDayLui || diChuyen.isCharging)) return;

        float khoangCachSqr = (player.position - transform.position).sqrMagnitude;
        if (khoangCachSqr <= data.tamKichHoatLuot * data.tamKichHoatLuot)
        {
            if (Time.time >= thoiGianLuotTiepTheo) StartCoroutine(DashRoutine());
        }
    }

    private IEnumerator DashRoutine()
    {
        dangLuot = true;
        if (diChuyen != null) diChuyen.isCharging = true;
        if (rb != null) rb.linearVelocity = Vector2.zero;

        Vector2 viTriBatDau = transform.position;
        Vector2 huongLuot = (player.position - transform.position).normalized;
        float quangDuongLuot = data.tocDoLuot * data.thoiGianLuot;
        Vector2 diemKetThuc = viTriBatDau + huongLuot * quangDuongLuot;

        canhBaoDuongLuot.SetPosition(0, viTriBatDau);
        canhBaoDuongLuot.SetPosition(1, diemKetThuc);
        canhBaoDuongLuot.enabled = true;

        if (hinhAnh != null) yield return StartCoroutine(hinhAnh.GongDonRoutine(data.thoiGianGongDon));
        else yield return new WaitForSeconds(data.thoiGianGongDon);

        canhBaoDuongLuot.enabled = false;

        float thoiGianDaLuot = 0f;
        bool daGayDame = false;

        while (thoiGianDaLuot < data.thoiGianLuot)
        {
            thoiGianDaLuot += Time.deltaTime;
            rb.linearVelocity = huongLuot * data.tocDoLuot;

            if (!daGayDame && Vector2.Distance(transform.position, player.position) <= 1.2f)
            {
                PlayerHealth.Instance.TakeDamage(data.dame);
                daGayDame = true;
            }
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        if (hinhAnh != null) StartCoroutine(hinhAnh.NayLenSauKhiBanRoutine());

        yield return new WaitForSeconds(0.2f);

        if (diChuyen != null) diChuyen.isCharging = false;
        dangLuot = false;
        thoiGianLuotTiepTheo = Time.time + data.thoiGianHoiChieuLuot;
    }
}