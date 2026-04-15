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
    private float previousTimeScale = 1f;

    [Header("--- GAME OVER ---")]
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private TextMeshProUGUI txtGameOverMessage;
    [SerializeField] private GameObject btnThuLai;

    [Header("--- AFTER CREDITS ---")]
    public AfterCreditsManager creditsManager;

    private bool isPaused = false;

    private void OnEnable()
    {
        GameManager.OnGameOverEvent += HienThiGameOver;
        StoryDirector.OnAfterCreditsTriggered += BatDauAfterCredits;
    }
    private void OnDisable()
    {
        GameManager.OnGameOverEvent -= HienThiGameOver;
        StoryDirector.OnAfterCreditsTriggered -= BatDauAfterCredits;
    }

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelSettings != null && panelSettings.activeInHierarchy)
            {
                if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
                panelSettings.SetActive(false);
            }
            else
            {
                TogglePause();
            }
        }
    }

    public void TogglePause()
    {
        if (panelGameOver != null && panelGameOver.activeSelf) return;

        if (!isPaused)
        {
            previousTimeScale = Time.timeScale;
            isPaused = true;
            if (panelPause != null) panelPause.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            isPaused = false;
            if (panelPause != null) panelPause.SetActive(false);

            if (panelSettings != null) panelSettings.SetActive(false);
            Time.timeScale = previousTimeScale;
        }
    }

    public void BamMoSettings()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        panelSettings.SetActive(true);
    }
    private void HienThiGameOver(bool chienThang)
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
        if (btnThuLai != null)
        {
            btnThuLai.SetActive(!chienThang);
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

        if (PlayerHealth.Instance != null && PlayerHealth.Instance.GetCurrentHP() > 0)
        {
            if (GameManager.Instance != null) GameManager.Instance.LuuTienDoRun();
        }

        if (GameManager.Instance != null) GameManager.Instance.quayLaiChonTuong = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    private void BatDauAfterCredits()
    {
        Transform hud = transform.Find("HUD");
        if (hud != null) hud.gameObject.SetActive(false);

        if (creditsManager != null) creditsManager.BatDauChayCredits();
    }
}