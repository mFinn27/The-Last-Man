using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopCardUI : MonoBehaviour
{
    [Header("--- KẾT NỐI UI ---")]
    public TextMeshProUGUI txtTen;
    public TextMeshProUGUI txtMoTaGomGiaTien;
    public Image imgIcon;
    public TextMeshProUGUI txtGiaReroll;

    [Header("--- TÍNH NĂNG KHÓA ---")]
    public Image imgNenNutKhoa;
    public Color mauBinhThuong = Color.white;
    public Color mauDaKhoa = Color.gray;

    public bool dangBiKhoa { get; private set; } = false;

    private ShopItemData dataHienTai;
    private ShopUI shopManager;
    private int giaRerollHienTai;

    public void Setup(ShopItemData data, ShopUI manager, int giaRerollBanDau)
    {
        dataHienTai = data;
        shopManager = manager;
        giaRerollHienTai = giaRerollBanDau;

        dangBiKhoa = false;
        CapNhatGiaoDienKhoa();

        if (data == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        if (txtTen != null) txtTen.text = data.tenMatHang;

        if (txtMoTaGomGiaTien != null)
        {
            txtMoTaGomGiaTien.text = $"{data.moTa}\n\n<color=yellow>Giá: {data.giaGoc} Vàng</color>";
        }

        if (imgIcon != null && data.icon != null) imgIcon.sprite = data.icon;

        CapNhatGiaRerollUI();
    }

    public void BamKhoaThe()
    {
        if (dataHienTai == null) return;

        dangBiKhoa = !dangBiKhoa;
        CapNhatGiaoDienKhoa();
    }

    private void CapNhatGiaoDienKhoa()
    {
        if (imgNenNutKhoa != null)
        {
            imgNenNutKhoa.color = dangBiKhoa ? mauDaKhoa : mauBinhThuong;
        }
    }

    public void BamMuaMonNay()
    {
        if (dataHienTai == null) return;

        if (PlayerStats.Instance.vangHienTai >= dataHienTai.giaGoc)
        {
            PlayerStats.Instance.vangHienTai -= dataHienTai.giaGoc;

            if (dataHienTai.loaiMatHang == ShopItemType.ChiSo && dataHienTai.duLieuChiSo != null)
            {
                PlayerStats.Instance.ThemChiSoTuThe(dataHienTai.duLieuChiSo);
            }
            else if (dataHienTai.loaiMatHang == ShopItemType.VuKhi && dataHienTai.duLieuVuKhi != null)
            {
                Debug.Log($"ĐÃ MUA VŨ KHÍ: {dataHienTai.duLieuVuKhi.tenVuKhi} - Chờ code Add Súng!");
            }
            shopManager.RerollJustThisCard(this, giaRerollHienTai);
            shopManager.CapNhatVangUITong();
        }
        else
        {
            Debug.Log("Không đủ vàng để mua món này!");
        }
    }

    public void BamDoiLaiTheNay()
    {
        if (shopManager == null || dataHienTai == null) return;

        if (PlayerStats.Instance.vangHienTai >= giaRerollHienTai)
        {
            PlayerStats.Instance.vangHienTai -= giaRerollHienTai;
            giaRerollHienTai += shopManager.buocNhayGiaReroll;

            shopManager.RerollJustThisCard(this, giaRerollHienTai);
            shopManager.CapNhatVangUITong();
        }
        else
        {
            Debug.Log("Không đủ vàng để đổi lại!");
        }
    }

    private void CapNhatGiaRerollUI()
    {
        if (txtGiaReroll != null)
        {
            txtGiaReroll.text = (giaRerollHienTai == 0) ? "Đổi lại (Miễn phí)" : $"Đổi lại\n({giaRerollHienTai} Vàng)";
        }
    }

    public void ResetChoWaveMoi(int giaMoi)
    {
        giaRerollHienTai = giaMoi;
        CapNhatGiaRerollUI();
    }
}