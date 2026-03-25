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
        mayQuet = GetComponentInParent<AutoAim>();
        player = GetComponentInParent<PlayerMovement>();
        if (hinhAnh == null) hinhAnh = GetComponentInChildren<SpriteRenderer>();
    }

    public void XuLyXoay(float tamDanh)
    {
        if (khoaXoay) return;

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
            hinhAnh.flipY = Mathf.Abs(goc) > 90f;
        }
    }
}