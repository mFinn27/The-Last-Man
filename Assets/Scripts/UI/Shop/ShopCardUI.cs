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

    [Tooltip("Kéo ảnh viền của thẻ vào đây để đổi màu theo độ hiếm")]
    public Image imgVienCapDo;

    [Header("--- TÍNH NĂNG KHÓA ---")]
    public Image imgNenNutKhoa;
    public Color mauBinhThuong = Color.white;
    public Color mauDaKhoa = Color.gray;
    public bool dangBiKhoa { get; private set; } = false;

    private ItemData dataHienTai;
    private ShopUI shopManager;
    private int giaRerollHienTai;
    private bool daBiMua = false;
    private int giaMuaThucTe;

    public void Setup(ItemData data, ShopUI manager, int giaRerollBanDau)
    {
        dataHienTai = data;
        shopManager = manager;
        giaRerollHienTai = giaRerollBanDau;
        daBiMua = false;
        dangBiKhoa = false;
        CapNhatGiaoDienKhoa();

        if (data == null) { gameObject.SetActive(false); return; }
        gameObject.SetActive(true);

        if (txtTen != null)
        {
            txtTen.text = data.tenMatHang;
            txtTen.color = data.mauCapDo;
        }

        if (imgVienCapDo != null) imgVienCapDo.color = data.mauCapDo;

        int waveHienTai = WaveManager.Instance != null ? WaveManager.Instance.waveHienTaiIndex + 1 : 1;
        float giaTinhToan = data.giaMua + waveHienTai + (data.giaMua * 0.1f * waveHienTai);
        float heSoCuaHang = 1f;
        giaMuaThucTe = Mathf.FloorToInt(giaTinhToan * heSoCuaHang);
        if (txtMoTaGomGiaTien != null) txtMoTaGomGiaTien.text = $"{data.moTa}\n\n<color=yellow>Giá: {giaMuaThucTe} Vàng</color>";

        if (imgIcon != null && data.iconMatHang != null) imgIcon.sprite = data.iconMatHang;
        CapNhatGiaRerollUI();
    }

    public void BamKhoaThe()
    {
        if (dataHienTai == null) return;
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        dangBiKhoa = !dangBiKhoa;
        CapNhatGiaoDienKhoa();
    }

    private void CapNhatGiaoDienKhoa()
    {
        if (imgNenNutKhoa != null) imgNenNutKhoa.color = dangBiKhoa ? mauDaKhoa : mauBinhThuong;
    }

    public void BamMuaMonNay()
    {
        if (daBiMua || dataHienTai == null) return;

        if (PlayerStats.Instance.vangHienTai >= giaMuaThucTe)
        {
            if (AudioManager.Instance != null) AudioManager.Instance.PlayEquipSFX();
            if (dataHienTai is UpgradeData dataChiSo)
            {
                PlayerStats.Instance.vangHienTai -= giaMuaThucTe;
                PlayerStats.Instance.ThemChiSoTuThe(dataChiSo);
                shopManager.RerollJustThisCard(this, giaRerollHienTai);
            }
            else if (dataHienTai is WeaponData dataVuKhi)
            {
                if (WeaponManager.Instance.ThuMuaVuKhi(dataVuKhi))
                {
                    PlayerStats.Instance.vangHienTai -= giaMuaThucTe;
                    shopManager.RerollJustThisCard(this, giaRerollHienTai);
                }
                else
                {
                    Debug.Log("Túi đồ đã đầy 6 slot! Vui lòng bán bớt hoặc ghép vũ khí.");
                    return;
                }
            }
            shopManager.CapNhatVangUITong();
        }
        else
        {
            Debug.Log("Không đủ vàng để mua món này!");
        }
    }

    public void BamDoiLaiTheNay()
    {
        if (daBiMua || shopManager == null || dataHienTai == null) return;
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();

        if (PlayerStats.Instance.vangHienTai >= giaRerollHienTai)
        {
            PlayerStats.Instance.vangHienTai -= giaRerollHienTai;
            giaRerollHienTai += shopManager.buocNhayGiaReroll;
            shopManager.RerollJustThisCard(this, giaRerollHienTai);
            shopManager.CapNhatVangUITong();
        }
    }

    private void CapNhatGiaRerollUI()
    {
        if (txtGiaReroll != null)
        {
            txtGiaReroll.text = $"<color=#0080FF>{giaRerollHienTai}</color>";
        }
    }

    public void ResetChoWaveMoi(int giaMoi)
    {
        giaRerollHienTai = giaMoi;
        CapNhatGiaRerollUI();
    }
}