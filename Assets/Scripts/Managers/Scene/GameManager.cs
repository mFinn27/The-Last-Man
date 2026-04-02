using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("--- Dữ Liệu Chuẩn Bị Vào Game ---")]
    public CharacterData characterDangChon;
    public WeaponData vuKhiKhoiDauDangChon;

    [Header("--- Tiến Độ ---")]
    public int waveCaoNhatDaDatDuoc = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadTienDo();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadTienDo()
    {
        waveCaoNhatDaDatDuoc = PlayerPrefs.GetInt("WaveCaoNhat", 0);
    }

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
        SceneManager.LoadScene("Gameplay");
    }
}