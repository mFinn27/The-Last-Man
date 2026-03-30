using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/Weapon Data")]
public class WeaponData : ItemData
{
    [Header("--- THÔNG TIN VŨ KHÍ ---")]
    public WeaponType loaiVuKhi;
    public GameObject weaponPrefab;

    [Header("--- HỆ THỐNG GHÉP ĐỒ & MUA BÁN ---")]
    [Tooltip("1: Thường (Trắng), 2: Hiếm (Xanh), 3: Sử Thi (Tím), 4: Huyền Thoại (Đỏ)")]
    public int capDo = 1;
    public Color mauCapDo = Color.white;
    public WeaponData vuKhiCapTiepTheo;
    public int giaBan = 10;

    [Header("--- CHỈ SỐ CHIẾN ĐẤU CỐT LÕI ---")]
    public float dame = 10f;
    public float tocDoDanh = 1f;
    public float tamDanh = 5f;
    public float dayLui = 2f;

    [Header("--- CHỈ SỐ ĐẶC BIỆT ---")]
    [Range(0, 1)] public float tiLeChiMang = 0.1f;
    public float satThuongChiMang = 2f;
    [Range(0, 0.4f)] public float hutMau = 0f;

    [Header("--- CƠ CHẾ VŨ KHÍ TẦM XA ---")]
    public GameObject bulletPrefab;
    public float tocDoBayCuaDan = 15f;
    public int xuyenThau = 1;

    [Header("--- CƠ CHẾ VŨ KHÍ CẬN CHIẾN ---")]
    public float doDaiVuKhi = 1.5f;
    public float overshoot = 0.25f;
    public float gocChem = 120f;
    public float tocDoXoay = 10f;
    public float tocDoDam = 15f;
}