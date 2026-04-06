using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CharacterSelector : MonoBehaviour
{
    [Header("--- DỮ LIỆU TƯỚNG ---")]
    public List<CharacterData> tatCaNhanVat;
    public Transform contentListCharacter;
    public GameObject characterCardPrefab;
    public Button btnNext;

    [Header("--- THÔNG TIN CHI TIẾT TƯỚNG ---")]
    public Image imgPreview;
    public Animator animPreview;
    public TextMeshProUGUI txtTen;
    public TextMeshProUGUI txtChiSo;
    public Transform trangBiContainer;
    public GameObject startingItemPrefab;

    [Header("--- KHÓA/MỞ KHÓA ---")]
    public GameObject panelDaMoKhoa;
    public GameObject panelChuaMoKhoa;
    public TextMeshProUGUI txtDieuKienKhoa;

    void Start()
    {
        SinhDanhSach();
        if (tatCaNhanVat.Count > 0) ChonNhanVat(tatCaNhanVat[0]);
    }

    private void SinhDanhSach()
    {
        foreach (var tuong in tatCaNhanVat)
        {
            GameObject theMoi = Instantiate(characterCardPrefab, contentListCharacter);
            CharacterCardUI cardScript = theMoi.GetComponent<CharacterCardUI>();
            if (cardScript != null) cardScript.Setup(tuong, this);
        }
    }

    public void ChonNhanVat(CharacterData data)
    {
        if (data == null) return;
        int waveCaoNhat = GameManager.Instance != null ? GameManager.Instance.waveCaoNhatDaDatDuoc : 0;
        bool daMoKhoa = data.moKhoaSan || (waveCaoNhat >= data.dieuKienWave);

        if (imgPreview != null) imgPreview.sprite = data.hinhAnhNhanVat;

        if (animPreview != null)
        {
            animPreview.runtimeAnimatorController = data.animatorNhanVat;
        }

        if (txtTen != null) txtTen.text = data.tenNhanVat;

        if (daMoKhoa)
        {
            if (panelDaMoKhoa != null) panelDaMoKhoa.SetActive(true);
            if (panelChuaMoKhoa != null) panelChuaMoKhoa.SetActive(false);

            if (txtChiSo != null) txtChiSo.text = XuLyMauChiSo(data.moTaNhanVat);
            HienThiTrangBi(data.danhSachVuKhiChoPhepChon);
            if (btnNext != null) btnNext.interactable = true;
            if (GameManager.Instance != null) GameManager.Instance.characterDangChon = data;
        }
        else
        {
            if (panelDaMoKhoa != null) panelDaMoKhoa.SetActive(false);
            if (panelChuaMoKhoa != null) panelChuaMoKhoa.SetActive(true);

            if (txtDieuKienKhoa != null) txtDieuKienKhoa.text = data.textDieuKienMoKhoa;
            if (txtChiSo != null) txtChiSo.text = "";
            HienThiTrangBi(null);
            if (btnNext != null) btnNext.interactable = false;
            if (GameManager.Instance != null) GameManager.Instance.characterDangChon = null;
        }
    }

    private void HienThiTrangBi(List<WeaponData> dsVuKhi)
    {
        if (trangBiContainer == null) return;
        foreach (Transform child in trangBiContainer) Destroy(child.gameObject);
        if (dsVuKhi == null) return;

        foreach (var vk in dsVuKhi)
        {
            GameObject item = Instantiate(startingItemPrefab, trangBiContainer);
            Image[] imgs = item.GetComponentsInChildren<Image>();
            if (imgs.Length > 1) { imgs[1].sprite = vk.iconMatHang; imgs[1].gameObject.SetActive(true); }
        }
    }

    private string XuLyMauChiSo(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";
        string[] lines = text.Split('\n');
        string res = "";
        foreach (var l in lines)
        {
            string trim = l.Trim();
            if (string.IsNullOrEmpty(trim)) continue;
            if (trim.Contains("+")) res += $"<color=#00FF00>{trim}</color>\n";
            else if (trim.Contains("-")) res += $"<color=#FF0000>{trim}</color>\n";
            else res += $"{trim}\n";
        }
        return res;
    }
}