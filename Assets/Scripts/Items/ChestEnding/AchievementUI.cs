using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class AchievementUI : MonoBehaviour
{
    public static AchievementUI Instance;

    [Header("--- THÀNH PHẦN UI ---")]
    public GameObject panelMain;
    public Image imgIcon;
    public TextMeshProUGUI txtTitle;
    public TextMeshProUGUI txtDescription;

    [Header("--- CƠ CHẾ CHỐNG SPAM ---")]
    public GameObject textClickToClose;
    public float thoiGianChoPhepDong = 2.5f;
    private bool choPhepDong = false;
    private Action onPanelClosed;

    void Awake()
    {
        if (Instance == null) Instance = this;
        if (panelMain != null) panelMain.SetActive(false);
    }

    public void HienThiThanhTuu(Sprite icon, string title, string desc, Action callback)
    {
        onPanelClosed = callback;

        if (imgIcon != null) imgIcon.sprite = icon;
        if (txtTitle != null) txtTitle.text = title;
        if (txtDescription != null) txtDescription.text = desc;

        panelMain.SetActive(true);
        textClickToClose.SetActive(false);
        choPhepDong = false;

        StartCoroutine(DemNguocChoPhepDong());
    }

    private IEnumerator DemNguocChoPhepDong()
    {
        yield return new WaitForSecondsRealtime(thoiGianChoPhepDong);
        choPhepDong = true;
        if (textClickToClose != null) textClickToClose.SetActive(true);
    }

    void Update()
    {
        if (choPhepDong && panelMain.activeSelf && Input.GetMouseButtonDown(0))
        {
            DongPanel();
        }
    }

    private void DongPanel()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();

        panelMain.SetActive(false);
        Time.timeScale = 1f;
        onPanelClosed?.Invoke();
    }
}