using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("Data Nhân Vật Gốc")]
    public CharacterData dataNhanVat;

    [Header("--- BONUS TỪ VẬT PHẨM/LEVEL UP ---")]
    public int bonusMauToiDa = 0;
    public int bonusGiap = 0;
    public float bonusTocDoDiChuyen = 0f;

    public float bonusSatThuong = 0f;
    public float bonusTocDoDanh = 0f;
    public float bonusTiLeChiMang = 0f;
    public float bonusSatThuongChiMang = 0f;
    public float bonusHutMau = 0f;

    [Header("--- CHỈ SỐ THU THẬP ---")]
    public float phamViHutNamChamGoc = 3f;
    public float bonusPhamViHut = 0f;

    [Header("--- THÔNG TIN TRẬN ĐẤU ---")]
    public int vangHienTai = 0;

    [Header("--- XEM CHỈ SỐ TỔNG HIỆN TẠI (DEBUG) ---")]
    [Tooltip("Chỉ để quan sát kết quả, đừng sửa bằng tay ô này nhé")]
    public int tongMauToiDa;
    public int tongGiap;
    public float tongTocDoDiChuyen;
    public float tongSatThuong;
    public float tongTocDoDanh;
    public float tongTiLeChiMang;
    public float tongSatThuongChiMang;
    public float tongHutMau;
    public float tongPhamViHut;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        tongMauToiDa = GetMaxHP();
        tongGiap = GetArmor();
        tongTocDoDiChuyen = GetMoveSpeed();
        tongSatThuong = GetDamage();
        tongTocDoDanh = GetAttackSpeed();
        tongTiLeChiMang = GetCritChance();
        tongSatThuongChiMang = GetCritDamage();
        tongHutMau = GetLifeSteal();
        tongPhamViHut = GetMagnetRange();
    }

    public int GetMaxHP() => dataNhanVat != null ? dataNhanVat.mauToiDaGoc + bonusMauToiDa : 100;
    public int GetArmor() => dataNhanVat != null ? dataNhanVat.giapGoc + bonusGiap : 0;
    public float GetMoveSpeed() => dataNhanVat != null ? dataNhanVat.tocDoDiChuyenGoc + bonusTocDoDiChuyen : 5f;

    public float GetDamage() => dataNhanVat != null ? dataNhanVat.satThuongGoc + bonusSatThuong : 0f;
    public float GetAttackSpeed() => dataNhanVat != null ? dataNhanVat.tocDoDanhGoc + bonusTocDoDanh : 0f;
    public float GetCritChance() => dataNhanVat != null ? dataNhanVat.tiLeChiMangGoc + bonusTiLeChiMang : 0f;
    public float GetCritDamage() => dataNhanVat != null ? dataNhanVat.satThuongChiMangGoc + bonusSatThuongChiMang : 0f;
    public float GetLifeSteal() => dataNhanVat != null ? dataNhanVat.hutMauGoc + bonusHutMau : 0f;

    public float GetMagnetRange()
    {
        return phamViHutNamChamGoc + bonusPhamViHut;
    }

    public void AddCoin(int amount)
    {
        vangHienTai += amount;
        Debug.Log($"Nhặt được {amount} vàng. Tổng vàng: {vangHienTai}");
    }

    public void ThemChiSoTuThe(UpgradeData data)
    {
        foreach (var chiSo in data.danhSachChiSo)
        {
            switch (chiSo.loaiChiSo)
            {
                case LoaiChiSo.MauToiDa: 
                    bonusMauToiDa += (int)chiSo.giaTriCongThem; 
                    break;
                case LoaiChiSo.Giap: 
                    bonusGiap += (int)chiSo.giaTriCongThem; 
                    break;
                case LoaiChiSo.TocDoDiChuyen: 
                    bonusTocDoDiChuyen += chiSo.giaTriCongThem; 
                    break;
                case LoaiChiSo.SatThuong: 
                    bonusSatThuong += chiSo.giaTriCongThem; 
                    break;
                case LoaiChiSo.TocDoDanh: 
                    bonusTocDoDanh += chiSo.giaTriCongThem; 
                    break;
                case LoaiChiSo.TiLeChiMang: 
                    bonusTiLeChiMang += chiSo.giaTriCongThem; 
                    break;
                case LoaiChiSo.SatThuongChiMang: 
                    bonusSatThuongChiMang += chiSo.giaTriCongThem; 
                    break;
                case LoaiChiSo.HutMau: 
                    bonusHutMau += chiSo.giaTriCongThem; 
                    break;
                case LoaiChiSo.PhamViHut: 
                    bonusPhamViHut += chiSo.giaTriCongThem; 
                    break;
            }
        }
        if (PlayerHealth.Instance != null) PlayerHealth.Instance.UpdateMaxHealth();
    }
}