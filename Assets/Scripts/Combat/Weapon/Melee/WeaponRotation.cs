using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
    [Header("--- CẤU HÌNH XOAY ---")]
    public float offsetGoc = 0f;
    public bool latAnhKhiQuayTrai = false;
    public SpriteRenderer hinhAnh;

    [HideInInspector] public bool khoaXoay = false;

    private AutoAim mayQuet;
    private PlayerMovement player;

    void Awake()
    {
        if (hinhAnh == null) hinhAnh = GetComponentInChildren<SpriteRenderer>();
    }

    public void Setup(AutoAim aim, PlayerMovement movement)
    {
        mayQuet = aim;
        player = movement;
    }

    public void XuLyXoay(float tamDanh)
    {
        if (khoaXoay || Time.timeScale == 0f) return;

        Vector2 huong;

        if (mayQuet != null && mayQuet.mucTieuHienTai != null && (mayQuet.mucTieuHienTai.position - transform.position).sqrMagnitude <= tamDanh * tamDanh)
        {
            huong = (mayQuet.mucTieuHienTai.position - transform.position).normalized;
        }
        else if (player != null)
        {
            huong = player.HuongDiChuyenCuoi;
        }
        else return;

        if (huong == Vector2.zero) return;

        float goc = Mathf.Atan2(huong.y, huong.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, goc + offsetGoc);

        if (latAnhKhiQuayTrai && hinhAnh != null)
        {
            bool quayTrai = Mathf.Abs(goc) > 90f;

            if (Mathf.Abs(offsetGoc) == 90f)
            {
                hinhAnh.flipX = quayTrai;
                hinhAnh.flipY = false;
            }
            else
            {
                hinhAnh.flipY = quayTrai;
                hinhAnh.flipX = false;
            }
        }
    }

    public Vector3 GetHuongTanCongLocal()
    {
        return Quaternion.Euler(0, 0, -offsetGoc) * Vector3.right;
    }
}