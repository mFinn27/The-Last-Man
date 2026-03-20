using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("Data Nhân Vật Gốc")]
    public CharacterData dataNhanVat; // Nhớ kéo SO Character vào đây

    [Header("--- BONUS TỪ VẬT PHẨM/LEVEL UP ---")]
    public int bonusMauToiDa = 0;
    public int bonusGiap = 0;
    public float bonusTocDoDiChuyen = 0f;

    public float bonusSatThuong = 0f;
    public float bonusTocDoDanh = 0f;
    public float bonusTiLeChiMang = 0f;
    public float bonusSatThuongChiMang = 0f;
    public float bonusHutMau = 0f;


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // --- CÁC HÀM XUẤT CHỈ SỐ ĐỂ GAME SỬ DỤNG ---
    public int GetMaxHP() => dataNhanVat != null ? dataNhanVat.mauToiDaGoc + bonusMauToiDa : 100;
    public int GetArmor() => dataNhanVat != null ? dataNhanVat.giapGoc + bonusGiap : 0;
    public float GetMoveSpeed() => dataNhanVat != null ? dataNhanVat.tocDoDiChuyenGoc + bonusTocDoDiChuyen : 5f;

    public float GetDamage() => dataNhanVat != null ? dataNhanVat.satThuongGoc + bonusSatThuong : 0f;
    public float GetAttackSpeed() => dataNhanVat != null ? dataNhanVat.tocDoDanhGoc + bonusTocDoDanh : 0f;
    public float GetCritChance() => dataNhanVat != null ? dataNhanVat.tiLeChiMangGoc + bonusTiLeChiMang : 0f;
    public float GetCritDamage() => dataNhanVat != null ? dataNhanVat.satThuongChiMangGoc + bonusSatThuongChiMang : 0f;
    public float GetLifeSteal() => dataNhanVat != null ? dataNhanVat.hutMauGoc + bonusHutMau : 0f;
}