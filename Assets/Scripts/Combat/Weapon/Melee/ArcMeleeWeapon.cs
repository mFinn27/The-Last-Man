using UnityEngine;
using System.Collections;

[RequireComponent(typeof(WeaponRotation))]
public class ArcMeleeWeapon : MonoBehaviour
{
    [SerializeField] private Transform visualContainer;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private SpriteRenderer hinhAnhVuKhi;

    private WeaponData data;
    private AutoAim mayQuet;
    private WeaponRotation boXoay;

    private bool dangTanCong;
    private float donDanhTiepTheo;
    private Vector3 viTriGoc;

    void Awake()
    {
        boXoay = GetComponent<WeaponRotation>();
        if (visualContainer != null)
        {
            viTriGoc = visualContainer.localPosition;
        }
    }

    public void Setup(WeaponData newData, AutoAim aim, PlayerMovement movement)
    {
        data = newData;
        mayQuet = aim;
        if (boXoay != null) boXoay.Setup(aim, movement);

        if (hinhAnhVuKhi != null && data.iconMatHang != null) hinhAnhVuKhi.sprite = data.iconMatHang;
    }

    void Update()
    {
        if (data == null || Time.timeScale == 0f) return;

        float tamDanhThuc = data.tamDanh + (PlayerStats.Instance != null ? PlayerStats.Instance.GetBonusTamDanh() : 0f);
        tamDanhThuc = Mathf.Min(tamDanhThuc, 5f);
        boXoay.XuLyXoay(tamDanhThuc);

        if (mayQuet != null && mayQuet.mucTieuHienTai != null && !dangTanCong)
        {
            float khoangCach = (mayQuet.mucTieuHienTai.position - transform.position).sqrMagnitude;
            if (khoangCach <= tamDanhThuc * tamDanhThuc && Time.time >= donDanhTiepTheo)
            {
                StartCoroutine(SwingAttack(tamDanhThuc));
                float tocDoDanhHienTai = DamageCalculator.CalculateAttackSpeed(data.tocDoDanh, data);
                donDanhTiepTheo = Time.time + (1f / tocDoDanhHienTai);
            }
        }
    }

    IEnumerator SwingAttack(float tamDanhThuc)
    {
        dangTanCong = true;
        boXoay.khoaXoay = true;

        if (trail) trail.emitting = true;

        GayDameAoECone(tamDanhThuc);
        float batDau = -data.gocChem * 0.5f;
        float ketThuc = data.gocChem * 0.5f + data.overshoot;
        float khoangCach = tamDanhThuc;
        if (mayQuet != null && mayQuet.mucTieuHienTai != null)
            khoangCach = Vector2.Distance(transform.position, mayQuet.mucTieuHienTai.position);

        float khoangCachVuonRa = Mathf.Clamp(khoangCach - 0.5f, 1f, tamDanhThuc);

        float tocDoDanhHienTai = DamageCalculator.CalculateAttackSpeed(data.tocDoDanh, data);
        float tocDoXoayThucTe = Mathf.Clamp(data.tocDoXoay * tocDoDanhHienTai, 5f, 12f);

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * (tocDoXoayThucTe * 1.5f);
            float eased = Mathf.SmoothStep(0, 1, t);

            float gocHienTai = Mathf.Lerp(0, batDau, eased);
            float doDaiHienTai = Mathf.Lerp(0, khoangCachVuonRa, eased);

            visualContainer.localPosition = viTriGoc + Quaternion.Euler(0, 0, gocHienTai) * (Vector3.right * doDaiHienTai);
            visualContainer.localRotation = Quaternion.Euler(0, 0, gocHienTai);
            yield return null;
        }

        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * tocDoXoayThucTe;
            float eased = 1 - Mathf.Pow(1 - t, 3);

            float gocHienTai = Mathf.Lerp(batDau, ketThuc, eased);

            visualContainer.localPosition = viTriGoc + Quaternion.Euler(0, 0, gocHienTai) * (Vector3.right * khoangCachVuonRa);
            visualContainer.localRotation = Quaternion.Euler(0, 0, gocHienTai);
            yield return null;
        }

        if (trail) trail.emitting = false;

        yield return new WaitForSeconds(0.08f);

        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * (tocDoXoayThucTe * 2f);

            float gocHienTai = Mathf.Lerp(ketThuc, 0, t);
            float doDaiHienTai = Mathf.Lerp(khoangCachVuonRa, 0, t);

            visualContainer.localPosition = viTriGoc + Quaternion.Euler(0, 0, gocHienTai) * (Vector3.right * doDaiHienTai);
            visualContainer.localRotation = Quaternion.Euler(0, 0, gocHienTai);
            yield return null;
        }

        visualContainer.localPosition = viTriGoc;
        visualContainer.localRotation = Quaternion.identity;

        boXoay.khoaXoay = false;
        dangTanCong = false;
    }
    private void GayDameAoECone(float tamDanhThuc)
    {
        Collider2D[] ketQua = Physics2D.OverlapCircleAll(transform.position, tamDanhThuc);
        Vector2 huongNhin = transform.right;

        foreach (Collider2D col in ketQua)
        {
            if (col.CompareTag("Enemy"))
            {
                Vector2 huongDenQuai = (col.transform.position - transform.position).normalized;

                float gocLech = Vector2.Angle(huongNhin, huongDenQuai);
                if (gocLech <= data.gocChem / 2f)
                {
                    EnemyHealth mauEnemy = col.GetComponent<EnemyHealth>();
                    if (mauEnemy != null)
                    {
                        XuLySatThuongTrenMotMucTieu(mauEnemy, col.transform.position);
                    }
                }
            }
        }
    }
    private void XuLySatThuongTrenMotMucTieu(EnemyHealth mauEnemy, Vector3 viTriEnemy)
    {
        bool chiMang;
        float dameCuoiCung = DamageCalculator.CalculateDamage(data.dame, data.tiLeChiMang, data.satThuongChiMang, out chiMang);

        Vector2 huongDayLui = ((Vector2)viTriEnemy - (Vector2)transform.root.position).normalized;
        float dayLuiThuc = data.dayLui + (PlayerStats.Instance != null ? PlayerStats.Instance.GetBonusDayLui() : 0f);

        mauEnemy.TakeDamage(dameCuoiCung, huongDayLui, dayLuiThuc);
        FloatingTextManager.Instance.SpawnText(viTriEnemy, dameCuoiCung, chiMang);

        float hutMauThucTe = DamageCalculator.CalculateLifeSteal(data.hutMau);
        if (hutMauThucTe > 0)
        {
            float luongHoiTiemNang = dameCuoiCung * hutMauThucTe;
            if (luongHoiTiemNang > 0 && PlayerHealth.Instance != null)
            {
                PlayerHealth.Instance.GhiNhanHutMau(luongHoiTiemNang);
            }
        }
    }
}