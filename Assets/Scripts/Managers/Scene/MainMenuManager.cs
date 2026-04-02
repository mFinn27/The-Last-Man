using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour
{
    [Header("--- 1. KHU VỰC CHỌN TƯỚNG ---")]
    public List<CharacterData> tatCaNhanVat;
    public GameObject scrollViewTuong;
    public Transform contentListTuong;
    public GameObject characterCardPrefab;
    public Button btnTiepTheo;

    [Header("--- 2. THÔNG TIN TƯỚNG (BÊN TRÁI) ---")]
    public Image imgPreviewTuong;
    public TextMeshProUGUI txtTenTuong;
    public GameObject panelDaMoKhoa;
    public TextMeshProUGUI txtChiSoTuong;
    public Transform trangBiMacDinhContainer;
    public GameObject startingItemPrefab;
    public GameObject panelChuaMoKhoa;
    public TextMeshProUGUI txtDieuKienMoKhoa;

    [Header("--- 3. KHU VỰC CHỌN VŨ KHÍ ---")]
    [SerializeField] private List<WeaponData> danhSachKhoVuKhi;

    public GameObject scrollViewVuKhi;
    public Transform contentListVuKhi;
    public GameObject weaponCardPrefab;
    public GameObject panelChiTietVuKhi;

    [Header("--- CHI TIẾT VŨ KHÍ BẢNG BÊN PHẢI ---")]
    public Image imgIconVuKhiDangChon;
    public TextMeshProUGUI txtTenVuKhiDangChon;
    public TextMeshProUGUI txtHeVuKhiDangChon;
    public TextMeshProUGUI txtChiSoVuKhiDangChon;
    public Button btnBatDau;

    [Header("--- ĐIỀU HƯỚNG ---")]
    public TextMeshProUGUI txtTieuDeTopBar;

    void Start()
    {
        MoGiaoDienChonTuong();
        SinhDanhSachTuong();

        if (tatCaNhanVat.Count > 0)
            ChonNhanVat(tatCaNhanVat[0]);
    }

    private void SinhDanhSachTuong()
    {
        foreach (var tuong in tatCaNhanVat)
        {
            GameObject theMoi = Instantiate(characterCardPrefab, contentListTuong);
            CharacterCardUI cardScript = theMoi.GetComponent<CharacterCardUI>();
            if (cardScript != null) cardScript.Setup(tuong, this);
        }
    }

    public void ChonNhanVat(CharacterData data)
    {
        if (data == null) return;

        int waveCaoNhat = GameManager.Instance != null ? GameManager.Instance.waveCaoNhatDaDatDuoc : 0;
        bool daMoKhoa = data.moKhoaSan || (waveCaoNhat >= data.dieuKienWave);

        if (imgPreviewTuong != null) imgPreviewTuong.sprite = data.hinhAnhNhanVat;
        if (txtTenTuong != null) txtTenTuong.text = data.tenNhanVat;

        if (daMoKhoa)
        {
            if (panelDaMoKhoa != null) panelDaMoKhoa.SetActive(true);
            if (panelChuaMoKhoa != null) panelChuaMoKhoa.SetActive(false);

            if (txtChiSoTuong != null) txtChiSoTuong.text = XuLyMauChiSoTuMoTa(data.moTaNhanVat);
            HienThiTrangBiMacDinh(data.danhSachVuKhiChoPhepChon);

            if (btnTiepTheo != null) btnTiepTheo.interactable = true;
            if (GameManager.Instance != null) GameManager.Instance.characterDangChon = data;
        }
        else
        {
            if (panelDaMoKhoa != null) panelDaMoKhoa.SetActive(false);
            if (panelChuaMoKhoa != null) panelChuaMoKhoa.SetActive(true);
            if (txtDieuKienMoKhoa != null) txtDieuKienMoKhoa.text = data.textDieuKienMoKhoa;

            if (btnTiepTheo != null) btnTiepTheo.interactable = false;
            if (GameManager.Instance != null) GameManager.Instance.characterDangChon = null;
        }
    }

    private void HienThiTrangBiMacDinh(List<WeaponData> dsVuKhi)
    {
        foreach (Transform child in trangBiMacDinhContainer) { Destroy(child.gameObject); }
        if (dsVuKhi == null || dsVuKhi.Count == 0) return;

        foreach (var vk in dsVuKhi)
        {
            GameObject itemIcon = Instantiate(startingItemPrefab, trangBiMacDinhContainer);
            Image[] cacHinhAnh = itemIcon.GetComponentsInChildren<Image>();
            if (cacHinhAnh.Length > 1)
            {
                cacHinhAnh[1].sprite = vk.iconMatHang;
                cacHinhAnh[1].gameObject.SetActive(true);
            }
        }
    }

    private string XuLyMauChiSoTuMoTa(string moTaGoc)
    {
        if (string.IsNullOrEmpty(moTaGoc)) return "";
        string[] cacDong = moTaGoc.Split('\n');
        string ketQua = "";

        foreach (string dong in cacDong)
        {
            string dongDaTrim = dong.Trim();
            if (string.IsNullOrEmpty(dongDaTrim)) continue;

            if (dongDaTrim.Contains("+")) ketQua += $"<color=#00FF00>{dongDaTrim}</color>\n";
            else if (dongDaTrim.Contains("-")) ketQua += $"<color=#FF0000>{dongDaTrim}</color>\n";
            else ketQua += $"{dongDaTrim}\n";
        }
        return ketQua;
    }

    private void MoGiaoDienChonTuong()
    {
        if (scrollViewTuong != null) scrollViewTuong.SetActive(true);
        if (btnTiepTheo != null) btnTiepTheo.gameObject.SetActive(true);
        if (scrollViewVuKhi != null) scrollViewVuKhi.SetActive(false);
        if (panelChiTietVuKhi != null) panelChiTietVuKhi.SetActive(false);
        if (btnBatDau != null) btnBatDau.gameObject.SetActive(false);
        if (txtTieuDeTopBar != null) txtTieuDeTopBar.text = "Chọn Nhân Vật";
    }

    public void BamNutTiepTheo()
    {
        if (GameManager.Instance != null && GameManager.Instance.characterDangChon != null)
        {
            if (scrollViewTuong != null) scrollViewTuong.SetActive(false);
            if (btnTiepTheo != null) btnTiepTheo.gameObject.SetActive(false);

            if (scrollViewVuKhi != null) scrollViewVuKhi.SetActive(true);
            if (panelChiTietVuKhi != null) panelChiTietVuKhi.SetActive(true);
            if (btnBatDau != null) btnBatDau.gameObject.SetActive(true);
            if (txtTieuDeTopBar != null) txtTieuDeTopBar.text = "Chọn Vũ Khí";

            if (imgIconVuKhiDangChon != null) { imgIconVuKhiDangChon.sprite = null; imgIconVuKhiDangChon.color = new Color(0, 0, 0, 0); }
            if (txtTenVuKhiDangChon != null) txtTenVuKhiDangChon.text = "Nhấp để chọn một vũ khí";
            if (txtHeVuKhiDangChon != null) txtHeVuKhiDangChon.text = "";
            if (txtChiSoVuKhiDangChon != null) txtChiSoVuKhiDangChon.text = "";
            if (btnBatDau != null) btnBatDau.interactable = false;

            SinhDanhSachVuKhi(danhSachKhoVuKhi);
        }
    }

    public void BamNutQuayLai()
    {
        MoGiaoDienChonTuong();
    }

    private void SinhDanhSachVuKhi(List<WeaponData> dsVuKhi)
    {
        foreach (Transform child in contentListVuKhi) { Destroy(child.gameObject); }
        if (GameManager.Instance != null) GameManager.Instance.vuKhiKhoiDauDangChon = null;

        if (dsVuKhi == null || dsVuKhi.Count == 0) return;

        foreach (var vk in dsVuKhi)
        {
            GameObject theVuKhi = Instantiate(weaponCardPrefab, contentListVuKhi);
            Image[] cacHinhAnh = theVuKhi.GetComponentsInChildren<Image>();
            if (cacHinhAnh.Length > 1)
            {
                cacHinhAnh[1].sprite = vk.iconMatHang;
                cacHinhAnh[1].gameObject.SetActive(true);
            }

            Button btn = theVuKhi.GetComponent<Button>();
            if (btn != null) btn.onClick.AddListener(() => ChonVuKhi(vk));
        }
    }

    private void ChonVuKhi(WeaponData vk)
    {
        if (GameManager.Instance != null) GameManager.Instance.vuKhiKhoiDauDangChon = vk;
        if (btnBatDau != null) btnBatDau.interactable = true;

        if (imgIconVuKhiDangChon != null)
        {
            imgIconVuKhiDangChon.sprite = vk.iconMatHang;
            imgIconVuKhiDangChon.color = Color.white;
        }

        if (txtTenVuKhiDangChon != null) txtTenVuKhiDangChon.text = vk.tenMatHang;

        if (txtHeVuKhiDangChon != null) txtHeVuKhiDangChon.text = vk.loaiVuKhi.ToString();

        if (txtChiSoVuKhiDangChon != null)
        {
            txtChiSoVuKhiDangChon.text = XuLyMauMoTaVuKhi(vk.moTa);
        }
    }

    private string XuLyMauMoTaVuKhi(string moTaGoc)
    {
        if (string.IsNullOrEmpty(moTaGoc)) return "";

        string[] cacDong = moTaGoc.Split('\n');
        string ketQua = "";

        foreach (string dong in cacDong)
        {
            string dongDaTrim = dong.Trim();
            if (string.IsNullOrEmpty(dongDaTrim)) continue;

            int viTriHaiCham = dongDaTrim.IndexOf(':');

            if (viTriHaiCham != -1)
            {
                string truocHaiCham = dongDaTrim.Substring(0, viTriHaiCham + 1);
                string sauHaiCham = dongDaTrim.Substring(viTriHaiCham + 1);

                ketQua += $"<color=yellow>{truocHaiCham}</color>{sauHaiCham}\n";
            }
            else
            {
                ketQua += $"{dongDaTrim}\n";
            }
        }
        return ketQua;
    }

    public void BamNutBatDauGame()
    {
        if (GameManager.Instance != null && GameManager.Instance.characterDangChon != null && GameManager.Instance.vuKhiKhoiDauDangChon != null)
        {
            GameManager.Instance.BatDauGame();
        }
    }
}