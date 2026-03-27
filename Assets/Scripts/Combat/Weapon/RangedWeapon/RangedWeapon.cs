using UnityEngine;

[RequireComponent(typeof(WeaponRotation))]
public class RangedWeapon : MonoBehaviour
{
    [SerializeField] private WeaponData data;
    [SerializeField] private Transform diemBan;
    [SerializeField] private SpriteRenderer hinhAnh;

    private AutoAim mayQuet;
    private PlayerMovement player;
    private WeaponRotation boXoay;
    private float donDanhTiepTheo;

    void Awake()
    {
        mayQuet = GetComponentInParent<AutoAim>();
        player = GetComponentInParent<PlayerMovement>();
        boXoay = GetComponent<WeaponRotation>();
        hinhAnh.sprite = data.hinhAnhVuKhi;
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        boXoay.XuLyXoay(data.tamDanh);

        if (mayQuet.mucTieuHienTai != null && Time.time >= donDanhTiepTheo)
        {
            float khoangCach = (mayQuet.mucTieuHienTai.position - transform.position).sqrMagnitude;
            if (khoangCach <= data.tamDanh * data.tamDanh)
            {
                Shoot();
                float tocDoDanhHienTai = DamageCalculator.CalculateAttackSpeed(data.tocDoDanh);
                donDanhTiepTheo = Time.time + (1f / tocDoDanhHienTai);
            }
        }
    }

    void Shoot()
    {
        GameObject bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = diemBan.position;

        Vector2 huong;
        if (mayQuet.mucTieuHienTai != null)
            huong = (mayQuet.mucTieuHienTai.position - diemBan.position).normalized;
        else
            huong = player.HuongDiChuyenCuoi;

        float goc = Mathf.Atan2(huong.y, huong.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, goc);

        bullet.GetComponent<Bullet>().Setup(
            huong,
            data.tocDoBayCuaDan,
            data.dame,
            data.xuyenThau,
            data.dayLui,
            data.tiLeChiMang,
            data.satThuongChiMang,
            data.hutMau
        );
    }
}