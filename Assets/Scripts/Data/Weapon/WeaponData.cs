using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/Weapon Data")]
public class WeaponData : ItemData
{
    [Header("--- THÔNG TIN VŨ KHÍ ---")]
    public WeaponType loaiVuKhi;
    public GameObject weaponPrefab;

    [Header("--- HỆ THỐNG GHÉP ĐỒ & MUA BÁN ---")]
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

    [Header("--- GIỚI HẠN CHỈ SỐ ---")]
    public bool coGioiHanTocDoDanh = false;
    public float tocDoDanhToiDa = 0.65f;

    [Header("--- CƠ CHẾ VŨ KHÍ TẦM XA (RANGED) ---")]
    public GameObject bulletPrefab;
    public float tocDoBayCuaDan = 15f;
    public int xuyenThau = 1;
    public int soLuongDan = 1;
    public float gocToaDan = 0f;

    [Header("--- CƠ CHẾ VŨ KHÍ CẬN CHIẾN (MELEE) ---")]
    public float doDaiVuKhi = 1.5f;
    public float overshoot = 0.25f;
    public float gocChem = 120f;
    public float tocDoXoay = 10f;
    public float tocDoDam = 15f;
}