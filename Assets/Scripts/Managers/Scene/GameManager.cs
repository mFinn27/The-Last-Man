using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

[System.Serializable]
public class RunSaveData
{
    public int waveHienTai, vangHienTai, soQuaiDaGiet, mauHienTai, bMau, bXuyenThau, bGiap, trangThaiGiaiDoan;
    public string tenNhanVat;
    public List<string> tenCacVuKhi = new List<string>();
    public float bTocDoChay, bPhamViHut, bSatThuong, bTocDoDanh, bTiLeChiMang, bSatThuongChiMang, bHutMau, bDayLui, bTamDanh;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static event Action<bool> OnGameOverEvent;

    [Header("--- Dữ Liệu Chuẩn Bị Vào Game ---")]
    public CharacterData characterDangChon;
    public WeaponData vuKhiKhoiDauDangChon;

    [Header("--- TIẾN ĐỘ & ĐIỀU HƯỚNG ---")]
    public int waveCaoNhatDaDatDuoc = 0;
    public bool quayLaiChonTuong = false;

    [Header("--- DATABASE CHO SAVE/LOAD ---")]
    public List<CharacterData> tatCaNhanVat;
    public List<WeaponData> tatCaVuKhi;

    [HideInInspector] public int soQuaiDaGiet = 0;
    [HideInInspector] public int soQuaiDaGietKhiBatDauWave = 0;
    [HideInInspector] public bool isLoadingSave = false;
    [HideInInspector] public RunSaveData currentSave;
    [HideInInspector] public bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadTienDo();
            LoadCaiDatManHinh();
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
        isGameOver = false;
        PlayerPrefs.DeleteKey("RunSave");
        PlayerPrefs.Save();

        isLoadingSave = false;
        soQuaiDaGiet = 0;
        Time.timeScale = 1f;
        if (AudioManager.Instance != null) AudioManager.Instance.PlayGameplayBGM();
        SceneManager.LoadScene("Gameplay");
    }

    public void TiepTucGame()
    {
        isGameOver = false;
        isLoadingSave = true;
        string json = PlayerPrefs.GetString("RunSave");
        Debug.Log("<color=yellow>DỮ LIỆU ĐỌC ĐƯỢC TỪ FILE SAVE:</color> " + json);

        PlayerPrefs.DeleteKey("RunSave");
        PlayerPrefs.Save();

        currentSave = JsonUtility.FromJson<RunSaveData>(json);
        characterDangChon = tatCaNhanVat.Find(x => x.tenNhanVat == currentSave.tenNhanVat);
        if (characterDangChon == null) Debug.LogError("LỖI: Không tìm thấy nhân vật, chưa kéo data vào TatCaNhanVat!");

        vuKhiKhoiDauDangChon = null;
        if (currentSave.waveHienTai == 0 && currentSave.trangThaiGiaiDoan == 0)
        {
            soQuaiDaGiet = 0;
        }
        else
        {
            soQuaiDaGiet = currentSave.soQuaiDaGiet;
        }
        Time.timeScale = 1f;
        if (AudioManager.Instance != null) AudioManager.Instance.PlayGameplayBGM();
        SceneManager.LoadScene("Gameplay");
    }

    public bool HasSave() => PlayerPrefs.HasKey("RunSave");

    private void OnApplicationQuit()
    {
        if (!isGameOver && PlayerHealth.Instance != null && PlayerHealth.Instance.GetCurrentHP() > 0)
        {
            LuuTienDoRun();
        }
    }

    public void LuuTienDoRun()
    {
        if (PlayerStats.Instance == null || WaveManager.Instance == null || WeaponManager.Instance == null) return;

        RunSaveData data = new RunSaveData();
        data.waveHienTai = WaveManager.Instance.waveHienTaiIndex;
        data.vangHienTai = PlayerStats.Instance.vangHienTai;
        data.soQuaiDaGiet = this.soQuaiDaGiet;
        data.mauHienTai = PlayerHealth.Instance != null ? PlayerHealth.Instance.GetCurrentHP() : PlayerStats.Instance.GetMaxHP();

        int trangThai = 0;
        if (ShopUI.Instance != null && ShopUI.Instance.panelShop.activeInHierarchy) trangThai = 2;
        else if (RewardPopupUI.Instance != null && RewardPopupUI.Instance.panelPhanThuong.activeInHierarchy) trangThai = 1;

        data.trangThaiGiaiDoan = trangThai;

        if (trangThai == 0)
        {
            data.vangHienTai = PlayerStats.Instance.vangKhiBatDauWave;
            data.soQuaiDaGiet = this.soQuaiDaGietKhiBatDauWave;
        }
        else
        {
            data.vangHienTai = PlayerStats.Instance.vangHienTai;
            data.soQuaiDaGiet = this.soQuaiDaGiet;
        }

        if (characterDangChon != null) data.tenNhanVat = characterDangChon.tenNhanVat;

        foreach (var vk in WeaponManager.Instance.danhSachVuKhi)
            data.tenCacVuKhi.Add(vk.tenMatHang);

        data.bMau = PlayerStats.Instance.bonusMauToiDa;
        data.bGiap = PlayerStats.Instance.bonusGiap;
        data.bTocDoChay = PlayerStats.Instance.bonusTocDoDiChuyen;
        data.bSatThuong = PlayerStats.Instance.bonusSatThuong;
        data.bTocDoDanh = PlayerStats.Instance.bonusTocDoDanh;
        data.bTiLeChiMang = PlayerStats.Instance.bonusTiLeChiMang;
        data.bSatThuongChiMang = PlayerStats.Instance.bonusSatThuongChiMang;
        data.bHutMau = PlayerStats.Instance.bonusHutMau;
        data.bDayLui = PlayerStats.Instance.bonusDayLui;
        data.bTamDanh = PlayerStats.Instance.bonusTamDanh;
        data.bXuyenThau = PlayerStats.Instance.bonusXuyenThau;
        data.bPhamViHut = PlayerStats.Instance.bonusPhamViHut;

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("RunSave", json);
        PlayerPrefs.Save();
        Debug.Log("ĐÃ LƯU GAME!");
    }

    public void KetThucGame(bool chienThang)
    {
        isGameOver = true;
        Time.timeScale = 0f;
        PlayerPrefs.DeleteKey("RunSave");
        PlayerPrefs.Save();

        if (AudioManager.Instance != null) AudioManager.Instance.StopAllGameplaySounds();
        if (WaveManager.Instance != null) LuuTienDoWave(WaveManager.Instance.waveHienTaiIndex);
        OnGameOverEvent?.Invoke(chienThang);
    }

    public void LoadCaiDatManHinh()
    {
        int mode = PlayerPrefs.GetInt("Saved_ScreenMode", 0);
        ApDungCheDoManHinh(mode);
    }

    public void ApDungCheDoManHinh(int modeIndex)
    {
        switch (modeIndex)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                Debug.Log("Toàn Màn Hình");
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Debug.Log("Không Viền");
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                Debug.Log("Cửa Sổ");
                break;
        }
        PlayerPrefs.SetInt("Saved_ScreenMode", modeIndex);
        PlayerPrefs.Save();
    }
    public void CongKill() => soQuaiDaGiet++;
}