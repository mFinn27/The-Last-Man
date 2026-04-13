using UnityEngine;
using System.Collections;

[RequireComponent(typeof(WeaponRotation))]
public class ThrustWeapon : MonoBehaviour
{
    [SerializeField] private Transform visualContainer;
    [SerializeField] private GameObject hitBox;
    [SerializeField] private SpriteRenderer hinhAnh;

    private WeaponData data;
    private AutoAim mayQuet;
    private WeaponRotation boXoay;

    private float donDanhTiepTheo;
    private bool dangTanCong;
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

        if (hinhAnh != null && data.iconMatHang != null) hinhAnh.sprite = data.iconMatHang;

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
            float khoangCachSqr = (mayQuet.mucTieuHienTai.position - transform.position).sqrMagnitude;
            if (khoangCachSqr <= tamDanhThuc * tamDanhThuc && Time.time >= donDanhTiepTheo)
            {
                StartCoroutine(ThrustAttack(tamDanhThuc));
                float tocDoDanhHienTai = DamageCalculator.CalculateAttackSpeed(data.tocDoDanh, data);
                donDanhTiepTheo = Time.time + (1f / tocDoDanhHienTai);
            }
        }
    }

    IEnumerator ThrustAttack(float tamDanhThuc)
    {
        dangTanCong = true;
        boXoay.khoaXoay = true;

        float khoangCach = tamDanhThuc;
        if (mayQuet != null && mayQuet.mucTieuHienTai != null)
            khoangCach = Vector2.Distance(transform.position, mayQuet.mucTieuHienTai.position);

        float khoangCachDam = Mathf.Min(khoangCach, tamDanhThuc);
        Vector3 huongTanCong = boXoay != null ? boXoay.GetHuongTanCongLocal() : Vector3.right;
        Vector3 mucTieu = viTriGoc + huongTanCong * (khoangCachDam + data.overshoot);

        if (hitBox) hitBox.SetActive(true);

        float tocDoDanhHienTai = DamageCalculator.CalculateAttackSpeed(data.tocDoDanh, data);
        float tocDoDamThucTe = data.tocDoDam * tocDoDanhHienTai;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * tocDoDamThucTe;
            visualContainer.localPosition = Vector3.Lerp(viTriGoc, mucTieu, t);
            yield return null;
        }

        if (hitBox) hitBox.SetActive(false);
        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * tocDoDamThucTe * 0.5f;
            visualContainer.localPosition = Vector3.Lerp(mucTieu, viTriGoc, t);
            yield return null;
        }

        visualContainer.localPosition = viTriGoc;
        boXoay.khoaXoay = false;
        dangTanCong = false;
    }
}