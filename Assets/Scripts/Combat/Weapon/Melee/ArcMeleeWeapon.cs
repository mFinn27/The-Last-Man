using UnityEngine;
using System.Collections;

[RequireComponent(typeof(WeaponRotation))]
public class ArcMeleeWeapon : MonoBehaviour
{
    [SerializeField] private Transform visualContainer;
    [SerializeField] private GameObject hitBox;
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
        if (visualContainer != null) viTriGoc = visualContainer.localPosition;
    }

    public void Setup(WeaponData newData, AutoAim aim, PlayerMovement movement)
    {
        data = newData;
        mayQuet = aim;
        if (boXoay != null) boXoay.Setup(aim, movement);

        if (hinhAnhVuKhi != null && data.iconMatHang != null) hinhAnhVuKhi.sprite = data.iconMatHang;

        if (hitBox != null)
        {
            hitBox.GetComponent<MeleeHitBox>().Setup(data);
            hitBox.SetActive(false);
        }
    }

    void Update()
    {
        if (data == null || Time.timeScale == 0f) return;

        float tamDanhThuc = data.tamDanh + (PlayerStats.Instance != null ? PlayerStats.Instance.GetBonusTamDanh() : 0f);
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
        if (hitBox) hitBox.SetActive(true);

        float batDau = -data.gocChem * 0.5f;
        float ketThuc = data.gocChem * 0.5f + data.overshoot;

        float khoangCach = tamDanhThuc;
        if (mayQuet != null && mayQuet.mucTieuHienTai != null)
            khoangCach = Vector2.Distance(transform.position, mayQuet.mucTieuHienTai.position);

        float khoangCachToiDich = Mathf.Min(khoangCach, tamDanhThuc);
        Vector3 huongTanCong = boXoay != null ? boXoay.GetHuongTanCongLocal() : Vector3.right;
        Vector3 mucTieu = viTriGoc + huongTanCong * (khoangCachToiDich + data.overshoot);

        float tocDoDanhHienTai = DamageCalculator.CalculateAttackSpeed(data.tocDoDanh, data);
        float tocDoXoayThucTe = data.tocDoXoay * tocDoDanhHienTai;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * tocDoXoayThucTe;
            float eased = 1 - Mathf.Pow(1 - t, 3);
            visualContainer.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(batDau, ketThuc, eased));
            visualContainer.localPosition = Vector3.Lerp(viTriGoc, mucTieu, eased);
            yield return null;
        }

        if (hitBox) hitBox.SetActive(false);
        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * tocDoXoayThucTe * 0.5f;
            visualContainer.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(ketThuc, 0, t));
            visualContainer.localPosition = Vector3.Lerp(mucTieu, viTriGoc, t);
            yield return null;
        }

        visualContainer.localRotation = Quaternion.identity;
        visualContainer.localPosition = viTriGoc;
        if (trail) trail.emitting = false;

        boXoay.khoaXoay = false;
        dangTanCong = false;
    }
}