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

    private Vector3 viTriGoc; // vị trí gốc

    void Awake()
    {
        mayQuet = GetComponentInParent<AutoAim>();
        player = GetComponentInParent<PlayerMovement>();

        viTriGoc = visualContainer.localPosition;

        if (hinhAnhVuKhi != null && data.hinhAnhVuKhi != null)
            hinhAnhVuKhi.sprite = data.hinhAnhVuKhi;

        if (hitBox) hitBox.SetActive(false);
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
                donDanhTiepTheo = Time.time + data.soDonDanhTrenMoiS;
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

        float reach = Mathf.Max(data.doDaiVuKhi, data.tamDanh);

        Vector3 mucTieu =
            viTriGoc + Vector3.up * reach;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * data.tocDoXoay;

            float eased = 1 - Mathf.Pow(1 - t, 3); // tốc độ anim làm mượt hơn

            float rot = Mathf.Lerp(batDau, ketThuc, eased);

            visualContainer.localRotation =
                Quaternion.Euler(0, 0, rot);

            visualContainer.localPosition =
                Vector3.Lerp(viTriGoc, mucTieu, eased);

            yield return null;
        }

        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * data.tocDoXoay * 0.5f;

            visualContainer.localPosition =
                Vector3.Lerp(mucTieu, viTriGoc, t);

            yield return null;
        }

        visualContainer.localRotation = Quaternion.identity;
        visualContainer.localPosition = viTriGoc;

        if (hitBox) hitBox.SetActive(false);
        if (trail) trail.emitting = false;

        dangTanCong = false;
    }
}