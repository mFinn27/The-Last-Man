using UnityEngine;
using System.Collections;

public class ArcMeleeWeapon : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private WeaponData data;

    [Header("Setup")]
    [SerializeField] private Transform visualContainer;
    [SerializeField] private GameObject hitBox;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private SpriteRenderer hinhAnhVuKhi;

    private AutoAim mayQuet;
    private PlayerMovement player;

    private bool dangTanCong;
    private float donDanhTiepTheo;
    private Vector3 viTriGoc;

    void Awake()
    {
        mayQuet = GetComponentInParent<AutoAim>();
        player = GetComponentInParent<PlayerMovement>();
        viTriGoc = visualContainer.localPosition;

        if (hinhAnhVuKhi != null && data.hinhAnhVuKhi != null)
            hinhAnhVuKhi.sprite = data.hinhAnhVuKhi;

        if (hitBox != null)
        {
            hitBox.GetComponent<MeleeHitBox>().Setup(data);
            hitBox.SetActive(false);
        }
    }

    void Update()
    {
        HandleRotation();

        if (mayQuet.mucTieuHienTai != null && !dangTanCong)
        {
            float khoangCach = (mayQuet.mucTieuHienTai.position - transform.position).sqrMagnitude;

            if (khoangCach <= data.tamDanh * data.tamDanh && Time.time >= donDanhTiepTheo)
            {
                StartCoroutine(SwingAttack());

                float tocDoDanhHienTai = DamageCalculator.CalculateAttackSpeed(data.tocDoDanh);
                donDanhTiepTheo = Time.time + (1f / tocDoDanhHienTai);
            }
        }
    }

    void HandleRotation()
    {
        if (dangTanCong) return;

        Vector2 huong;
        if (mayQuet.mucTieuHienTai != null)
            huong = (mayQuet.mucTieuHienTai.position - transform.position).normalized;
        else
            huong = player.HuongDiChuyenCuoi;

        if (huong == Vector2.zero) return;

        float goc = Mathf.Atan2(huong.y, huong.x) * Mathf.Rad2Deg;
        goc -= 90f;
        transform.rotation = Quaternion.Euler(0, 0, goc);
    }

    IEnumerator SwingAttack()
    {
        dangTanCong = true;

        if (trail) trail.emitting = true;
        if (hitBox) hitBox.SetActive(true);

        float batDau = -data.gocChem * 0.5f;
        float ketThuc = data.gocChem * 0.5f + data.overshoot;
        float khoangCach = data.tamDanh;

        if (mayQuet.mucTieuHienTai != null)
            khoangCach = Vector2.Distance(transform.position, mayQuet.mucTieuHienTai.position);

        float khoangCachToiDich = Mathf.Min(khoangCach, data.tamDanh);
        Vector3 mucTieu = viTriGoc + Vector3.up * (khoangCachToiDich + data.overshoot);

        float tocDoDanhHienTai = DamageCalculator.CalculateAttackSpeed(data.tocDoDanh);
        float tocDoXoayThucTe = data.tocDoXoay * tocDoDanhHienTai;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * tocDoXoayThucTe;
            float eased = 1 - Mathf.Pow(1 - t, 3);
            float gocXoay = Mathf.Lerp(batDau, ketThuc, eased);
            visualContainer.localRotation = Quaternion.Euler(0, 0, gocXoay);
            visualContainer.localPosition = Vector3.Lerp(viTriGoc, mucTieu, eased);
            yield return null;
        }

        if (hitBox) hitBox.SetActive(false);
        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * tocDoXoayThucTe * 0.5f;
            float rot = Mathf.Lerp(ketThuc, 0, t);
            visualContainer.localRotation = Quaternion.Euler(0, 0, rot);
            visualContainer.localPosition = Vector3.Lerp(mucTieu, viTriGoc, t);
            yield return null;
        }

        visualContainer.localRotation = Quaternion.identity;
        visualContainer.localPosition = viTriGoc;

        if (trail) trail.emitting = false;
        dangTanCong = false;
    }
}