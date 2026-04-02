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

        float tamDanhThuc = data.tamDanh + (PlayerStats.Instance != null ? PlayerStats.Instance.GetBonusTamDanh() : 0f);

        boXoay.XuLyXoay(tamDanhThuc);

        if (mayQuet != null && mayQuet.mucTieuHienTai != null && Time.time >= donDanhTiepTheo)
        {
            float khoangCach = (mayQuet.mucTieuHienTai.position - transform.position).sqrMagnitude;
            if (khoangCach <= tamDanhThuc * tamDanhThuc)
            {
                Shoot();
                float tocDoDanhHienTai = DamageCalculator.CalculateAttackSpeed(data.tocDoDanh);
                if (data.coGioiHanTocDoDanh && tocDoDanhHienTai > data.tocDoDanhToiDa)
                {
                    tocDoDanhHienTai = data.tocDoDanhToiDa;
                }

                donDanhTiepTheo = Time.time + (1f / tocDoDanhHienTai);
            }
        }
    }

    void Shoot()
    {
        if (data.bulletPrefab == null)
        {
            Debug.LogWarning($"[RangedWeapon] Vũ khí {data.tenMatHang} chưa được gắn Bullet Prefab trong WeaponData!");
            return;
        }

        Vector2 huong;
        if (mayQuet != null && mayQuet.mucTieuHienTai != null)
            huong = (mayQuet.mucTieuHienTai.position - diemBan.position).normalized;
        else if (player != null)
            huong = player.HuongDiChuyenCuoi;
        else
            huong = Vector2.right;

        int xuyenThauThuc = data.xuyenThau + (PlayerStats.Instance != null ? PlayerStats.Instance.GetBonusXuyenThau() : 0);
        float dayLuiThuc = data.dayLui + (PlayerStats.Instance != null ? PlayerStats.Instance.GetBonusDayLui() : 0f);
        int soDanThucTe = Mathf.Max(1, data.soLuongDan);
        float gocGoc = Mathf.Atan2(huong.y, huong.x) * Mathf.Rad2Deg;
        float gocBatDau = gocGoc - (soDanThucTe - 1) * data.gocToaDan / 2f;

        for (int i = 0; i < soDanThucTe; i++)
        {
            float gocHienTai = gocBatDau + (i * data.gocToaDan);

            GameObject bulletObj = BulletPool.Instance.GetBullet(data.bulletPrefab);
            if (bulletObj == null) continue;

            bulletObj.transform.position = diemBan.position;
            Vector2 huongVienDan = new Vector2(Mathf.Cos(gocHienTai * Mathf.Deg2Rad), Mathf.Sin(gocHienTai * Mathf.Deg2Rad));

            Bullet bulletScript = bulletObj.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.Setup(
                    huongVienDan,
                    data.tocDoBayCuaDan,
                    data.dame,
                    xuyenThauThuc,
                    dayLuiThuc,
                    data.tiLeChiMang,
                    data.satThuongChiMang,
                    data.hutMau
                );
            }
        }
    }
}