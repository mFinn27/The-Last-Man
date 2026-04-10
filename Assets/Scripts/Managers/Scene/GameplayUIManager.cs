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

    [Header("--- GAME OVER ---")]
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private TextMeshProUGUI txtGameOverMessage;

    private bool isPaused = false;

    void Start()
    {
        if (panelPause != null) panelPause.SetActive(false);
        if (panelGameOver != null) panelGameOver.SetActive(false);
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

    public void BamMoSettings()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        panelSettings.SetActive(true);
    }

    public void HienThiGameOver(bool chienThang)
    {
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);

            if (txtGameOverMessage != null)
            {
                txtGameOverMessage.text = chienThang ? "CHIẾN THẮNG!" : "BẠN ĐÃ CHẾT!";
                txtGameOverMessage.color = chienThang ? Color.yellow : Color.red;
            }
        }
    }

    public void BamNutThuLai()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        Time.timeScale = 1f;
        if (GameManager.Instance != null) GameManager.Instance.BatDauGame();
    }

    public void BamNutChoiLai()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        Time.timeScale = 1f;
        if (GameManager.Instance != null) GameManager.Instance.quayLaiChonTuong = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    public void BamNutVeMenu()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        Time.timeScale = 1f;
        if (GameManager.Instance != null) GameManager.Instance.quayLaiChonTuong = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}