using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("--- Dữ Liệu Chuẩn Bị Vào Game ---")]
    public CharacterData characterDangChon;
    public WeaponData vuKhiKhoiDauDangChon;

    [Header("--- Tiến Độ & Thống Kê ---")]
    public int waveCaoNhatDaDatDuoc = 0;

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
        // win lose
    }

    public void CongKill() => soQuaiDaGiet++;
}