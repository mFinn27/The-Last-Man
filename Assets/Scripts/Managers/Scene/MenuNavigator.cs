using UnityEngine;
using TMPro;

public class MenuNavigator : MonoBehaviour
{
    [Header("--- MÀN HÌNH CHÍNH ---")]
    public GameObject panelTitleScreen;
    public GameObject panelChonTuongVaVuKhi;

    [Header("--- CÁC BẢNG UI ---")]
    public GameObject panelSettings;

    [Header("--- THÀNH PHẦN CHỌN TƯỚNG ---")]
    public GameObject scrollViewCharacter;
    public GameObject panelCharacterInfo;
    public GameObject btnNext;

    [Header("--- THÀNH PHẦN CHỌN SÚNG ---")]
    public GameObject scrollViewWeapon;
    public GameObject panelChiTietVuKhi;
    public GameObject btnStartGame;

    [Header("--- ĐIỀU HƯỚNG ---")]
    public TextMeshProUGUI txtTittle;
    public WeaponSelector weaponSelector;

    void Start()
    {
        MoManHinhTitle();
        if (AudioManager.Instance != null) AudioManager.Instance.PlayMenuBGM();
    }

    public void MoManHinhTitle()
    {
        panelTitleScreen.SetActive(true);
        panelChonTuongVaVuKhi.SetActive(false);
    }

    public void MoManHinhChonTuong()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        panelTitleScreen.SetActive(false);
        panelChonTuongVaVuKhi.SetActive(true);

        scrollViewCharacter.SetActive(true);
        panelCharacterInfo.SetActive(true);
        btnNext.SetActive(true);

        scrollViewWeapon.SetActive(false);
        panelChiTietVuKhi.SetActive(false);
        btnStartGame.SetActive(false);

        if (txtTittle != null) txtTittle.text = "CHỌN NHÂN VẬT";
    }

    public void MoManHinhChonVuKhi()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        scrollViewCharacter.SetActive(false);
        btnNext.SetActive(false);
        scrollViewWeapon.SetActive(true);
        panelChiTietVuKhi.SetActive(true);
        btnStartGame.SetActive(true);
        panelCharacterInfo.SetActive(true);

        if (txtTittle != null) txtTittle.text = "CHỌN VŨ KHÍ";

        if (weaponSelector != null) weaponSelector.KhoiTaoKhoVuKhi();
    }

    public void BamMoSettings()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        panelSettings.SetActive(true);
    }

    public void BamDongSettings()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        panelSettings.SetActive(false);
    }

    public void ThoatGame()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}