using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Characters/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("--- THÔNG TIN CƠ BẢN ---")]
    public string tenNhanVat;
    public Sprite hinhAnhNhanVat;
    [TextArea] public string moTaNhanVat;
    public RuntimeAnimatorController animatorNhanVat;

    [Header("--- ĐIỀU KIỆN MỞ KHÓA (Cho UI Chọn Tướng) ---")]
    public string idNhanVat;
    public bool moKhoaSan = true;
    public int dieuKienWave = 0;
    public string textDieuKienMoKhoa = "Vượt qua Wave 5 để mở khóa";

    [Header("--- TRANG BỊ KHỞI ĐẦU ---")]
    public List<WeaponData> danhSachVuKhiChoPhepChon = new List<WeaponData>();

    [Header("--- CHỈ SỐ SINH TỒN GỐC ---")]
    public int mauToiDaGoc = 100;
    public int giapGoc = 0;
    public float tocDoDiChuyenGoc = 5f;

    [Header("--- CHỈ SỐ TẤN CÔNG GỐC ---")]
    public float satThuongGoc = 0f;       // dame cộng thẳng
    public float tocDoDanhGoc = 0f;       // % tốc đánh cộng thêm
    public float tiLeChiMangGoc = 0f;     // % tỉ lệ Chí mạng
    public float satThuongChiMangGoc = 0f;// % sát thương chí mạng
    public float hutMauGoc = 0f;          // % Hút máu
}