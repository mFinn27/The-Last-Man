using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class WeaponSelector : MonoBehaviour
{
    [Header("--- DỮ LIỆU SÚNG ---")]
    public List<WeaponData> danhSachKhoVuKhi;
    public Transform contentListWeapon;
    public GameObject weaponCardPrefab;
    public Button btnStartGame;

    [Header("--- CHI TIẾT SÚNG ---")]
    public Image imgIcon;
    public TextMeshProUGUI txtTen;
    public TextMeshProUGUI txtChiSo;


    public void KhoiTaoKhoVuKhi()
    {
        if (imgIcon != null) imgIcon.color = new Color(0, 0, 0, 0);
        if (txtTen != null) txtTen.text = "Chọn một vũ khí";
        if (txtChiSo != null) txtChiSo.text = "";
        if (btnStartGame != null) btnStartGame.interactable = false;

        if (GameManager.Instance != null) GameManager.Instance.vuKhiKhoiDauDangChon = null;

        foreach (Transform child in contentListWeapon) Destroy(child.gameObject);

        foreach (var vk in danhSachKhoVuKhi)
        {
            GameObject the = Instantiate(weaponCardPrefab, contentListWeapon);
            Image[] imgs = the.GetComponentsInChildren<Image>();
            if (imgs.Length > 1) { imgs[1].sprite = vk.iconMatHang; imgs[1].gameObject.SetActive(true); }

            Button btn = the.GetComponent<Button>();
            if (btn != null) btn.onClick.AddListener(() => ChonVuKhi(vk));
        }
    }

    public void BamGoiBatDauGame()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();

        if (GameManager.Instance != null) GameManager.Instance.BatDauGame();
    }

    private void ChonVuKhi(WeaponData vk)
    {
        if (GameManager.Instance != null) GameManager.Instance.vuKhiKhoiDauDangChon = vk;
        if (btnStartGame != null) btnStartGame.interactable = true;

        if (imgIcon != null) { imgIcon.sprite = vk.iconMatHang; imgIcon.color = Color.white; }
        if (txtTen != null) txtTen.text = vk.tenMatHang;
        if (txtChiSo != null) txtChiSo.text = XuLyMauMoTa(vk.moTa);
    }

    private string XuLyMauMoTa(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";
        string[] lines = text.Split('\n');
        string res = "";
        foreach (var l in lines)
        {
            string trim = l.Trim();
            if (string.IsNullOrEmpty(trim)) continue;
            int idx = trim.IndexOf(':');
            if (idx != -1) res += $"<color=yellow>{trim.Substring(0, idx + 1)}</color>{trim.Substring(idx + 1)}\n";
            else res += $"{trim}\n";
        }
        return res;
    }
}