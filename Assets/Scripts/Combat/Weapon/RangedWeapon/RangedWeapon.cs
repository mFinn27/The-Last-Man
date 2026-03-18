using UnityEngine;

public class RangedWeapon : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Transform diemBan;
    [SerializeField] private SpriteRenderer spriteVuKhi;

    private AutoAim mayQuet;
    private PlayerMovement player;
    private float lanBanTiepTheo;

    public float soLuongDanBanRaTrenS = 0.5f;
    public float tocDoDiChuyenCuaDan = 12f;

    void Awake()
    {
        mayQuet = GetComponentInParent<AutoAim>();
        player = GetComponentInParent<PlayerMovement>();
    }

    void Update()
    {
        HandleRotation();

        if (mayQuet.mucTieuHienTai != null && Time.time >= lanBanTiepTheo)
        {
            Shoot();
            lanBanTiepTheo = Time.time + soLuongDanBanRaTrenS;
        }
    }

    private void HandleRotation()
    {
        Vector2 huongMucTieu;

        if (mayQuet.mucTieuHienTai != null)
        {
            huongMucTieu = (mayQuet.mucTieuHienTai.position - transform.position).normalized;
        }
        else
        {
            huongMucTieu = player.HuongDiChuyenCuoi;
        }

        float angle = Mathf.Atan2(huongMucTieu.y, huongMucTieu.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (spriteVuKhi != null)
        {
            spriteVuKhi.flipY = Mathf.Abs(angle) > 90;
        }
    }

    private void Shoot()
    {
        GameObject bulletObj = BulletPool.Instance.GetBullet();
        bulletObj.transform.position = diemBan.position;

        Vector2 huongBan;
        if (mayQuet.mucTieuHienTai != null)
        {
            huongBan = (mayQuet.mucTieuHienTai.position - diemBan.position).normalized;
        }
        else
        {
            huongBan = player.HuongDiChuyenCuoi;
        }

        float goc = Mathf.Atan2(huongBan.y, huongBan.x) * Mathf.Rad2Deg;
        bulletObj.transform.rotation = Quaternion.Euler(0, 0, goc);
        bulletObj.GetComponent<Bullet>().Setup(huongBan, tocDoDiChuyenCuaDan, 1, 2f);
    }
}