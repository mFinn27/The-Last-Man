using UnityEngine;
using System.Collections;

public class ThrustWeapon : MonoBehaviour
{
    [SerializeField] private WeaponData data;
    [SerializeField] private Transform visualContainer;
    [SerializeField] private GameObject hitBox;
    [SerializeField] private SpriteRenderer hinhAnh;

    private AutoAim mayQuet;
    private PlayerMovement player;

    private float donDanhTiepTheo;
    private bool dangTanCong;
    private Vector3 viTriGoc;

    void Awake()
    {
        mayQuet = GetComponentInParent<AutoAim>();
        player = GetComponentInParent<PlayerMovement>();
        viTriGoc = visualContainer.localPosition;
        hinhAnh.sprite = data.hinhAnhVuKhi;

        if (hitBox != null)
        {
            hitBox.GetComponent<MeleeHitBox>().Setup(data);
            hitBox.SetActive(false);
        }
    }

    void Update()
    {
        RotateWeapon();

        if (mayQuet.mucTieuHienTai != null && !dangTanCong)
        {
            float huong = (mayQuet.mucTieuHienTai.position - transform.position).sqrMagnitude;

            if (huong <= data.tamDanh * data.tamDanh && Time.time >= donDanhTiepTheo)
            {
                StartCoroutine(ThrustAttack());

                float tocDoDanhHienTai = DamageCalculator.CalculateAttackSpeed(data.tocDoDanh);
                donDanhTiepTheo = Time.time + (1f / tocDoDanhHienTai);
            }
        }
    }

    void RotateWeapon()
    {
        if (dangTanCong) return;

        Vector2 huong;
        if (mayQuet.mucTieuHienTai != null)
            huong = (mayQuet.mucTieuHienTai.position - transform.position).normalized;
        else
            huong = player.HuongDiChuyenCuoi;

        if (huong == Vector2.zero) return;

        float goc = Mathf.Atan2(huong.y, huong.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, goc - 90);
    }

    IEnumerator ThrustAttack()
    {
        dangTanCong = true;
        float khoangCach = data.tamDanh;

        if (mayQuet.mucTieuHienTai != null)
            khoangCach = Vector2.Distance(transform.position, mayQuet.mucTieuHienTai.position);

        float khoangCachDam = Mathf.Min(khoangCach, data.tamDanh);
        Vector3 mucTieu = viTriGoc + Vector3.up * (khoangCachDam + data.overshoot);

        if (hitBox) hitBox.SetActive(true);

        float tocDoDanhHienTai = DamageCalculator.CalculateAttackSpeed(data.tocDoDanh);
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
        dangTanCong = false;
    }
}