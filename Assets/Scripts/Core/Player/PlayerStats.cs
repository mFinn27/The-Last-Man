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

    public float bonusDayLui = 0f;
    public float bonusTamDanh = 0f;
    public int bonusXuyenThau = 0;

    [Header("--- CHỈ SỐ THU THẬP ---")]
    public float phamViHutNamChamGoc = 3f;
    public float bonusPhamViHut = 0f;

    [Header("--- THÔNG TIN TRẬN ĐẤU ---")]
    public int vangHienTai = 0;
    public int vangKhiBatDauWave = 0;

    [Header("--- XEM CHỈ SỐ TỔNG HIỆN TẠI (DEBUG) ---")]
    public int tongMauToiDa;
    public int tongGiap;
    public float tongTocDoDiChuyen;
    public float tongSatThuong;
    public float tongTocDoDanh;
    public float tongTiLeChiMang;
    public float tongSatThuongChiMang;
    public float tongHutMau;
    public float tongPhamViHut;
    public float tongLucDayLui;
    public float tongTamDanh;
    public float tongXuyenThau;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        if (GameManager.Instance != null && GameManager.Instance.characterDangChon != null)
        {
            dataNhanVat = GameManager.Instance.characterDangChon;
            PlayerVisuals visuals = GetComponent<PlayerVisuals>();
            if (visuals != null)
            {
                visuals.CapNhatHinhAnhVaAnimation();
            }
        }

        if (GameManager.Instance != null && GameManager.Instance.isLoadingSave)
        {
            RunSaveData data = GameManager.Instance.currentSave;
            vangHienTai = data.vangHienTai;
            bonusMauToiDa = data.bMau;
            bonusGiap = data.bGiap;
            bonusTocDoDiChuyen = data.bTocDoChay;
            bonusSatThuong = data.bSatThuong;
            bonusTocDoDanh = data.bTocDoDanh;
            bonusTiLeChiMang = data.bTiLeChiMang;
            bonusSatThuongChiMang = data.bSatThuongChiMang;
            bonusHutMau = data.bHutMau;
            bonusDayLui = data.bDayLui;
            bonusTamDanh = data.bTamDanh;
            bonusXuyenThau = data.bXuyenThau;
            bonusPhamViHut = data.bPhamViHut;
        }
    }

    void Start()
    {

        if (PlayerHealth.Instance != null) PlayerHealth.Instance.UpdateMaxHealth();
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
        tongLucDayLui = GetBonusDayLui();
        tongTamDanh = GetBonusTamDanh();
        tongXuyenThau = GetBonusXuyenThau();
    }

    public int GetMaxHP() => dataNhanVat != null ? dataNhanVat.mauToiDaGoc + bonusMauToiDa : 100;
    public int GetArmor() => dataNhanVat != null ? dataNhanVat.giapGoc + bonusGiap : 0;
    public float GetMoveSpeed() => dataNhanVat != null ? dataNhanVat.tocDoDiChuyenGoc + bonusTocDoDiChuyen : 5f;

    public float GetDamage() => dataNhanVat != null ? dataNhanVat.satThuongGoc + bonusSatThuong : 0f;
    public float GetAttackSpeed() => dataNhanVat != null ? dataNhanVat.tocDoDanhGoc + bonusTocDoDanh : 0f;
    public float GetCritChance() => dataNhanVat != null ? dataNhanVat.tiLeChiMangGoc + bonusTiLeChiMang : 0f;
    public float GetCritDamage() => dataNhanVat != null ? dataNhanVat.satThuongChiMangGoc + bonusSatThuongChiMang : 0f;
    public float GetLifeSteal() => dataNhanVat != null ? dataNhanVat.hutMauGoc + bonusHutMau : 0f;

    public float GetMagnetRange() => phamViHutNamChamGoc + bonusPhamViHut;

    public float GetBonusDayLui() => bonusDayLui;
    public float GetBonusTamDanh() => bonusTamDanh;
    public int GetBonusXuyenThau() => bonusXuyenThau;

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
                case LoaiChiSo.DayLui:
                    bonusDayLui += chiSo.giaTriCongThem;
                    break;
                case LoaiChiSo.TamDanh:
                    bonusTamDanh += chiSo.giaTriCongThem;
                    break;
                case LoaiChiSo.XuyenThau:
                    bonusXuyenThau += (int)chiSo.giaTriCongThem;
                    break;
            }
        }
        if (PlayerHealth.Instance != null) PlayerHealth.Instance.UpdateMaxHealth();
    }
}