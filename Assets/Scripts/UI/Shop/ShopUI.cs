using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public static ShopUI Instance;

    [Header("--- KẾT NỐI UI ---")]
    public GameObject panelShop;
    public TextMeshProUGUI txtVangHienTai;
    public ShopCardUI[] cacTheTrenKe;

    [Header("--- KHO VŨ KHÍ (INVENTORY) ---")]
    public WeaponSlotUI[] danhSachSlotUI;

    [Header("--- BẢNG THAO TÁC (ACTION PANEL) ---")]
    public GameObject panelHanhDong;
    public TextMeshProUGUI txtGiaBan;
    public Button btnGhep;

    [Header("--- THIẾT LẬP REROLL ---")]
    public int giaRerollBanDau = 2;
    public int buocNhayGiaReroll = 2;

    [Header("--- KHO HÀNG HÓA ---")]
    public List<ItemData> khoHangHoa;

    private int slotDangDuocChon = -1;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void MoCuaHang()
    {
        panelShop.SetActive(true);
        Time.timeScale = 0f;
        CapNhatVangUITong();
        panelHanhDong.SetActive(false);
        CapNhatGiaoDienKhoVuKhi();

        foreach (var the in cacTheTrenKe)
        {
            if (!the.dangBiKhoa)
            {
                if (khoHangHoa.Count > 0)
                {
                    int rand = Random.Range(0, khoHangHoa.Count);
                    the.Setup(khoHangHoa[rand], this, giaRerollBanDau);
                }
                else the.Setup(null, this, 0);
            }
            else the.ResetChoWaveMoi(giaRerollBanDau);
        }
    }

    public void CapNhatGiaoDienKhoVuKhi()
    {
        for (int i = 0; i < danhSachSlotUI.Length; i++)
        {
            if (i < WeaponManager.Instance.danhSachVuKhi.Count)
                danhSachSlotUI[i].Setup(WeaponManager.Instance.danhSachVuKhi[i], i);
            else
                danhSachSlotUI[i].Setup(null, i);
        }
    }

    public void MoBangThaoTacVuKhi(int index)
    {
        slotDangDuocChon = index;
        WeaponData vuKhi = WeaponManager.Instance.danhSachVuKhi[index];

        panelHanhDong.SetActive(true);
        txtGiaBan.text = $"Bán (+{vuKhi.giaBan})";
        btnGhep.interactable = WeaponManager.Instance.CoTheGhepKhong(index);
    }

    public void BamNutBanTrangBip()
    {
        WeaponManager.Instance.BanVuKhi(slotDangDuocChon);
        panelHanhDong.SetActive(false);
        CapNhatVangUITong();
    }

    public void BamNutGhepTrangBip()
    {
        WeaponManager.Instance.ThuGhepThuCong(slotDangDuocChon);
        panelHanhDong.SetActive(false);
    }

    public void DongBangThaoTac()
    {
        panelHanhDong.SetActive(false);
    }

    public void RerollJustThisCard(ShopCardUI theYeuCau, int giaRerollMoi)
    {
        if (khoHangHoa.Count > 0)
        {
            int rand = Random.Range(0, khoHangHoa.Count);
            theYeuCau.Setup(khoHangHoa[rand], this, giaRerollMoi);
        }
    }

    public void CapNhatVangUITong()
    {
        if (txtVangHienTai != null && PlayerStats.Instance != null)
            txtVangHienTai.text = PlayerStats.Instance.vangHienTai.ToString();
    }

    public void BamChuyenWaveMoi()
    {
        panelShop.SetActive(false);
        Time.timeScale = 1f;
        if (WaveManager.Instance != null) WaveManager.Instance.ChuyenSangWaveTiepTheo();
    }
}