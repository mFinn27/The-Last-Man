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
                ItemData itemRandom = LayItemNgauNhienTheoWave();
                the.Setup(itemRandom, this, giaRerollBanDau);
            }
            else the.ResetChoWaveMoi(giaRerollBanDau);
        }
    }

    public void RerollJustThisCard(ShopCardUI theYeuCau, int giaRerollMoi)
    {
        ItemData itemRandom = LayItemNgauNhienTheoWave();
        theYeuCau.Setup(itemRandom, this, giaRerollMoi);
    }

    private ItemData LayItemNgauNhienTheoWave()
    {
        if (khoHangHoa.Count == 0) return null;

        int waveHienTai = WaveManager.Instance != null ? WaveManager.Instance.waveHienTaiIndex + 1 : 1;
        int tierMucTieu = TinhToanTier(waveHienTai);

        List<ItemData> dsPhuHop = new List<ItemData>();
        foreach (var item in khoHangHoa)
        {
            if (item.capDo == tierMucTieu) dsPhuHop.Add(item);
        }

        if (dsPhuHop.Count > 0)
        {
            return dsPhuHop[Random.Range(0, dsPhuHop.Count)];
        }
        else
        {
            Debug.LogWarning($"[Shop] Không tìm thấy Item Tier {tierMucTieu} trong kho hàng! Đang bốc ngẫu nhiên.");
            return khoHangHoa[Random.Range(0, khoHangHoa.Count)];
        }
    }

    private int TinhToanTier(int wave)
    {
        int xucXac = Random.Range(1, 101);

        int tierKetQua = 1;

        if (wave <= 2)
        {
            // Wave 1-2: 100% Trắng
            tierKetQua = 1;
        }
        else if (wave <= 5)
        {
            // Wave 3-5: Trắng (80%) | Xanh (20%)
            if (xucXac <= 80) tierKetQua = 1;
            else tierKetQua = 2;
        }
        else if (wave <= 10)
        {
            // Wave 6-10: Trắng (60%) | Xanh (30%) | Tím (10%)
            if (xucXac <= 60) tierKetQua = 1;
            else if (xucXac <= 90) tierKetQua = 2;
            else tierKetQua = 3;
        }
        else if (wave <= 14)
        {
            // Wave 11-14: Trắng (45%) | Xanh (35%) | Tím (15%) | Đỏ (5%)
            if (xucXac <= 45) tierKetQua = 1;
            else if (xucXac <= 80) tierKetQua = 2;
            else if (xucXac <= 95) tierKetQua = 3;
            else tierKetQua = 4;
        }
        else
        {
            // Wave 15+ (End Game): Trắng (30%) | Xanh (40%) | Tím (22%) | Đỏ (8%)
            if (xucXac <= 30) tierKetQua = 1;
            else if (xucXac <= 70) tierKetQua = 2;
            else if (xucXac <= 92) tierKetQua = 3;
            else tierKetQua = 4;
        }

        string mauLog = tierKetQua == 4 ? "red" : (tierKetQua == 3 ? "purple" : (tierKetQua == 2 ? "cyan" : "white"));
        Debug.Log($"<color=orange>[SHOP LOG]</color> Wave {wave} | Xúc xắc: {xucXac} | Kết quả: <color={mauLog}>Tier {tierKetQua}</color>");

        return tierKetQua;
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
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        WeaponManager.Instance.BanVuKhi(slotDangDuocChon);
        panelHanhDong.SetActive(false);
        CapNhatVangUITong();
    }

    public void BamNutGhepTrangBip()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        WeaponManager.Instance.ThuGhepThuCong(slotDangDuocChon);
        panelHanhDong.SetActive(false);
    }

    public void DongBangThaoTac()
    {
        panelHanhDong.SetActive(false);
    }

    public void CapNhatVangUITong()
    {
        if (txtVangHienTai != null && PlayerStats.Instance != null)
            txtVangHienTai.text = PlayerStats.Instance.vangHienTai.ToString();
    }

    public void BamChuyenWaveMoi()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        panelShop.SetActive(false);
        Time.timeScale = 1f;
        if (WaveManager.Instance != null) WaveManager.Instance.ChuyenSangWaveTiepTheo();
    }
}