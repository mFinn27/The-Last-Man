using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    [Header("--- KẾT NỐI SLIDER ---")]
    public Slider sliderBGM;
    public Slider sliderSFX;

    [Header("--- KẾT NỐI ICON ĐẦU DÒNG ---")]
    public Image iconBGM;
    public Image iconSFX;

    [Header("--- SPRITE (HÌNH ẢNH) ICON ---")]
    public Sprite iconOn;
    public Sprite iconOff;

    [Header("--- KẾT NỐI MÀN HÌNH ---")]
    public TMP_Dropdown dropdownScreenMode;

    public float buocNhay = 0.1f;

    private void OnEnable()
    {
        if (AudioManager.Instance != null)
        {
            sliderBGM.value = AudioManager.Instance.globalBgmVolume;
            sliderSFX.value = AudioManager.Instance.globalSfxVolume;

            CapNhatIcon(iconBGM, sliderBGM.value);
            CapNhatIcon(iconSFX, sliderSFX.value);
        }
        if (dropdownScreenMode != null)
        {
            dropdownScreenMode.value = PlayerPrefs.GetInt("Saved_ScreenMode", 0);
            dropdownScreenMode.RefreshShownValue();
        }
    }

    public void OnScreenModeChanged(int index)
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        if (GameManager.Instance != null) GameManager.Instance.ApDungCheDoManHinh(index);
    }

    public void OnBGMSliderChanged(float value)
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SetGlobalBGMVolume(value);
        CapNhatIcon(iconBGM, value);
    }

    public void BamGiamBGM()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        sliderBGM.value -= buocNhay;
    }

    public void BamTangBGM()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        sliderBGM.value += buocNhay;
    }

    public void OnSFXSliderChanged(float value)
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SetGlobalSFXVolume(value);
        CapNhatIcon(iconSFX, value);
    }

    public void BamGiamSFX()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        sliderSFX.value -= buocNhay;
    }

    public void BamTangSFX()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        sliderSFX.value += buocNhay;
    }

    private void CapNhatIcon(Image imgIcon, float volume)
    {
        if (imgIcon == null) return;

        if (volume <= 0.001f)
            imgIcon.sprite = iconOff;
        else
            imgIcon.sprite = iconOn;
    }

    public void BamDongSettingsUI()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        gameObject.SetActive(false);
    }
}