using UnityEngine;

public class RangedWeapon : MonoBehaviour
{
    [SerializeField] private WeaponData data;
    [SerializeField] private Transform diemBan;
    [SerializeField] private SpriteRenderer hinhAnh;

    private AutoAim mayQuet;
    private PlayerMovement player;

    private float donDanhTiepTheo;

    void Awake()
    {
        mayQuet = GetComponentInParent<AutoAim>();
        player = GetComponentInParent<PlayerMovement>();

        hinhAnh.sprite = data.hinhAnhVuKhi;
    }

    void Update()
    {
        RotateWeapon();

        if (mayQuet.mucTieuHienTai != null &&
            Time.time >= donDanhTiepTheo)
        {
            Shoot();
            donDanhTiepTheo = Time.time + data.soDonDanhTrenMoiS;
        }
    }

    void RotateWeapon()
    {
        Vector2 huong;

        if (mayQuet.mucTieuHienTai != null)
            huong = (mayQuet.mucTieuHienTai.position - transform.position).normalized;
        else
            huong = player.HuongDiChuyenCuoi;

        if (huong == Vector2.zero) return;

        float goc = Mathf.Atan2(huong.y, huong.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0,0,goc);

        hinhAnh.flipY = Mathf.Abs(goc) > 90;
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

        bullet.transform.rotation =
            Quaternion.Euler(0, 0, goc);

        bullet.GetComponent<Bullet>().Setup(
            huong,
            data.tocDoBayCuaDan,
            data.damage,
            data.xuyenThau,
            data.dayLui,
            data.tiLeChiMang,
            data.satThuongChiMang,
            data.hutMau
        );
    }
}