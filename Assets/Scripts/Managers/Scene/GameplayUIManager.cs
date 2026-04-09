using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameplayUIManager : MonoBehaviour
{
    [Header("--- THÔNG SỐ HUD ---")]
    [SerializeField] private TextMeshProUGUI txtVang;
    [SerializeField] private TextMeshProUGUI txtKillCount;

    [Header("--- TẠM DỪNG (PAUSE) ---")]
    [SerializeField] private GameObject panelPause;
    public GameObject panelSettings;

    private bool isPaused = false;

    void Start()
    {
        if (panelPause != null) panelPause.SetActive(false);
    }

    void Update()
    {
        CapNhatHUD();
        XuLyInput();
    }

    private void CapNhatHUD()
    {
        if (txtVang != null && PlayerStats.Instance != null)
            txtVang.text = PlayerStats.Instance.vangHienTai.ToString();

        if (txtKillCount != null && GameManager.Instance != null)
            txtKillCount.text = GameManager.Instance.soQuaiDaGiet.ToString();
    }

    private void XuLyInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }

    public void TogglePause()
    {
        if (Time.timeScale == 0f && !isPaused) return;

        isPaused = !isPaused;
        if (panelPause != null) panelPause.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void BamNutVeMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void BamMoSettings()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        panelSettings.SetActive(true);
    }
}