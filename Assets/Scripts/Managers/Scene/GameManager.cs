using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("--- Dữ Liệu Chuẩn Bị Vào Game ---")]
    public CharacterData characterDangChon;
    public WeaponData vuKhiKhoiDauDangChon;

    [Header("--- Tiến Độ & Điều Hướng ---")]
    public int waveCaoNhatDaDatDuoc = 0;
    public bool quayLaiChonTuong = false;

    [HideInInspector] public int soQuaiDaGiet = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadTienDo();
        }
        else Destroy(gameObject);
    }

    public void LoadTienDo() => waveCaoNhatDaDatDuoc = PlayerPrefs.GetInt("WaveCaoNhat", 0);

    public void LuuTienDoWave(int waveHienTai)
    {
        if (waveHienTai > waveCaoNhatDaDatDuoc)
        {
            waveCaoNhatDaDatDuoc = waveHienTai;
            PlayerPrefs.SetInt("WaveCaoNhat", waveCaoNhatDaDatDuoc);
            PlayerPrefs.Save();
        }
    }

    public void BatDauGame()
    {
        soQuaiDaGiet = 0;
        Time.timeScale = 1f;
        if (AudioManager.Instance != null) AudioManager.Instance.PlayGameplayBGM();
        SceneManager.LoadScene("Gameplay");
    }

    public void KetThucGame(bool chienThang)
    {
        Time.timeScale = 0f;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAllGameplaySounds();
        }
        if (WaveManager.Instance != null) LuuTienDoWave(WaveManager.Instance.waveHienTaiIndex);

        GameplayUIManager uiManager = FindFirstObjectByType<GameplayUIManager>();
        if (uiManager != null) uiManager.HienThiGameOver(chienThang);
    }

    public void CongKill() => soQuaiDaGiet++;

    [ContextMenu("Xoa Het Tien Do")]
    public void DeleteAllProgress()
    {
        PlayerPrefs.DeleteKey("WaveCaoNhat");
        PlayerPrefs.Save();
        Debug.Log("Da xoa het tien do Wave!");
    }
}