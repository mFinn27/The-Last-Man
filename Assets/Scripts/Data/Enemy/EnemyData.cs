using UnityEngine;

public enum EnemyType { Melee, Ranged }
public enum LoaiPhanThuong { vang, VatPhamDacBiet }

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemies/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("--- THÔNG TIN CƠ BẢN ---")]
    public string tenQuaiVat;
    public EnemyType loaiQuai = EnemyType.Melee;

    [Header("--- CHỈ SỐ SINH TỒN ---")]
    public float mauToiDa = 20f;
    public float tocDoDiChuyen = 2.5f;

    [Header("--- CHỈ SỐ TẤN CÔNG CHUNG ---")]
    public int dame = 10;
    public float tgThucHienDonDanhTiepTheo = 1f;

    [Header("--- CẬN CHIẾN (Dành cho Melee) ---")]
    public float khoangCachCanChien = 1.2f;

    [Header("--- ĐÁNH XA (Dành cho Ranged) ---")]
    public float tamDanhXa = 7f;
    public GameObject danPrefab;
    public float tocDoDan = 8f;
    public int soLuongDan = 1;
    public float gocToaDan = 15f;

    [Header("--- Quasi có đòn tấn công đặc biệt ---")]
    public float thoiGianGongDon = 0.5f;

    [Header("--- THÔNG SỐ NỔ (Chỉ dành cho Quái Nổ) ---")]
    public bool laQuaiPhatNo = false;
    public int damePhatNo = 50;
    public float banKinhNo = 3f;

    [Tooltip("Thời gian chờ trước khi nổ")]
    public float thoiGianChoNo = 2f;

    [Header("--- PHẦN THƯỞNG KHI CHẾT ---")]
    [Range(0, 1)] public float tiLeRotThuong = 1f;

    [Tooltip("loại đồ sẽ rớt ra khi con quái này chết")]
    public LoaiPhanThuong phanThuongRotRa = LoaiPhanThuong.vang;

    [Header("- Dành cho Ngọc Kinh Nghiệm (Dùng Pool) -")]
    public int minSoLuongVang = 1;
    public int maxSoLuongVang = 3;
    public int minGiaTriMoiVien = 1;
    public int maxGiaTriMoiVien = 2;

    [Header("- Dành cho Máu/Bom/Hòm đồ (Không dùng Pool) -")]
    public GameObject vatPhamDacBietPrefab;
}