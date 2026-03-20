using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Characters/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("--- THÔNG TIN ---")]
    public string tenNhanVat;
    public Sprite hinhAnhNhanVat;

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