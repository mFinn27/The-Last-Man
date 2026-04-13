using UnityEngine;
using TMPro;

public class MenuNavigator : MonoBehaviour
{
    [Header("--- SAVE GAME UI ---")]
    public GameObject panelContinuePrompt;

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
    public SettingsUI settingsUI;

    void Start()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayMenuBGM();

        if (GameManager.Instance != null && GameManager.Instance.quayLaiChonTuong)
        {
            GameManager.Instance.quayLaiChonTuong = false;
            MoManHinhChonTuong();
        }
        else
        {
            MoManHinhTitle();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelSettings != null && panelSettings.activeSelf)
            {
                settingsUI.BamDongSettingsUI();
            }
            else if (panelContinuePrompt != null && panelContinuePrompt.activeSelf)
            {
                DongBangChoiTiep();
            }
            else
            {
                BamMoSettings();
            }
        }
    }

    public void DongBangChoiTiep()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        if (panelContinuePrompt != null) panelContinuePrompt.SetActive(false);
    }

    public void BamNutPlayGame()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();

        if (GameManager.Instance != null && GameManager.Instance.HasSave())
        {
            panelContinuePrompt.SetActive(true);
        }
        else
        {
            MoManHinhChonTuong();
        }
    }

    public void BamChoiTiep()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        if (GameManager.Instance != null) GameManager.Instance.TiepTucGame();
    }

    public void BamChoiMoiTuBangHoi()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        panelContinuePrompt.SetActive(false);
        MoManHinhChonTuong();
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