using UnityEngine;

public enum EnemyType { Melee, Ranged, Explosion, Dash, Boss }
public enum LoaiPhanThuong { vang, VatPhamDacBiet }

public abstract class EnemyData : ScriptableObject
{
    [Header("--- THÔNG TIN CHUNG ---")]
    public string tenQuaiVat;
    public EnemyType loaiQuai;

    [Header("--- CHỈ SỐ SINH TỒN ---")]
    public float mauToiDa = 0;
    public float tocDoDiChuyen = 0f;

    [Header("--- CHỈ SỐ TẤN CÔNG CƠ BẢN ---")]
    public int dame = 0;
    public float tgThucHienDonDanhTiepTheo = 0;
    public float thoiGianGongDon = 0;

    [Header("--- PHẦN THƯỞNG KHI CHẾT ---")]
    [Range(0, 1)] public float tiLeRotThuong = 0;
    public LoaiPhanThuong phanThuongRotRa = LoaiPhanThuong.vang;

    [Header("- Dành cho Ngọc Vàng -")]
    public int minSoLuongVang = 0;
    public int maxSoLuongVang = 0;
    public int minGiaTriMoiVien = 0;
    public int maxGiaTriMoiVien = 0;

    [Header("- Dành cho Hòm đồ/Bom -")]
    public GameObject vatPhamDacBietPrefab;
}