using UnityEngine;

[RequireComponent(typeof(WeaponRotation))]
public class RangedWeapon : MonoBehaviour
{
    [SerializeField] private Transform diemBan;
    [SerializeField] private SpriteRenderer hinhAnh;

    private WeaponData data;
    private AutoAim mayQuet;
    private PlayerMovement player;
    private WeaponRotation boXoay;
    private float donDanhTiepTheo;

    void Awake()
    {
        boXoay = GetComponent<WeaponRotation>();
    }

    public void Setup(WeaponData newData, AutoAim aim, PlayerMovement movement)
    {
        data = newData;
        mayQuet = aim;
        player = movement;

        if (boXoay != null) boXoay.Setup(aim, movement);

        if (hinhAnh != null && data.iconMatHang != null) hinhAnh.sprite = data.iconMatHang;
    }

    void Update()
    {
        if (data == null || Time.timeScale == 0f) return;

        boXoay.XuLyXoay(data.tamDanh);

        if (mayQuet != null && mayQuet.mucTieuHienTai != null && Time.time >= donDanhTiepTheo)
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
        if (mayQuet != null && mayQuet.mucTieuHienTai != null)
            huong = (mayQuet.mucTieuHienTai.position - diemBan.position).normalized;
        else if (player != null)
            huong = player.HuongDiChuyenCuoi;
        else
            huong = Vector2.right;

        float goc = Mathf.Atan2(huong.y, huong.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, goc);

        bullet.GetComponent<Bullet>().Setup(
            huong, data.tocDoBayCuaDan, data.dame, data.xuyenThau,
            data.dayLui, data.tiLeChiMang, data.satThuongChiMang, data.hutMau
        );
    }
}