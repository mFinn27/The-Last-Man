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
                float tocDoDanhHienTai = DamageCalculator.CalculateAttackSpeed(data.tocDoDanh, data);
                donDanhTiepTheo = Time.time + (1f / tocDoDanhHienTai);
            }
        }
    }

    void Shoot()
    {
        if (data.bulletPrefab == null || BulletPool.Instance == null) return;

        Vector2 huongGoc;
        if (mayQuet != null && mayQuet.mucTieuHienTai != null)
            huongGoc = (mayQuet.mucTieuHienTai.position - diemBan.position).normalized;
        else if (player != null)
            huongGoc = player.HuongDiChuyenCuoi;
        else
            huongGoc = Vector2.right;

        float gocTrungTam = Mathf.Atan2(huongGoc.y, huongGoc.x) * Mathf.Rad2Deg;
        float gocBatDau = gocTrungTam - (data.soLuongDan - 1) * data.gocToaDan / 2f;

        for (int i = 0; i < data.soLuongDan; i++)
        {
            float gocHienTai = gocBatDau + (i * data.gocToaDan);
            GameObject bulletObj = BulletPool.Instance.GetBullet(data.bulletPrefab);
            bulletObj.transform.position = diemBan.position;

            Vector2 huongTiaDan = new Vector2(
                Mathf.Cos(gocHienTai * Mathf.Deg2Rad),
                Mathf.Sin(gocHienTai * Mathf.Deg2Rad)
            );

            Bullet bulletScript = bulletObj.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.Setup(
                    huongTiaDan,
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
    }
}