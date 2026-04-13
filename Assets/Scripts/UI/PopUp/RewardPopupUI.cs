using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RewardPopupUI : MonoBehaviour
{
    public static RewardPopupUI Instance;

    [Header("--- THIẾT LẬP UI ---")]
    public GameObject panelPhanThuong;
    public RewardCardUI[] cacTheUI;

    [Header("--- KHO DỮ LIỆU CHỈ SỐ ---")]
    public List<UpgradeData> khoDuLieu;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        WaveManager.OnWaveEnded += KichHoatHienThi;
    }

    private void OnDisable()
    {
        WaveManager.OnWaveEnded -= KichHoatHienThi;
    }

    private void KichHoatHienThi()
    {
        StartCoroutine(HienThiSauDelay(2.5f));
    }

    private IEnumerator HienThiSauDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0f;
        panelPhanThuong.SetActive(true);

        List<UpgradeData> theTamThoi = new List<UpgradeData>(khoDuLieu);

        for (int i = 0; i < cacTheUI.Length; i++)
        {
            if (theTamThoi.Count > 0)
            {
                UpgradeData dataDuocChon = LayItemNgauNhienTheoWave(theTamThoi);
                cacTheUI[i].Setup(dataDuocChon, this);
                theTamThoi.Remove(dataDuocChon);
            }
        }
    }

    private UpgradeData LayItemNgauNhienTheoWave(List<UpgradeData> danhSachNguon)
    {
        int waveHienTai = WaveManager.Instance != null ? WaveManager.Instance.waveHienTaiIndex + 1 : 1;
        int tierMucTieu = TinhToanTier(waveHienTai);

        List<UpgradeData> dsPhuHop = new List<UpgradeData>();
        foreach (var item in danhSachNguon)
        {
            if (item.capDo == tierMucTieu) dsPhuHop.Add(item);
        }

        if (dsPhuHop.Count > 0)
        {
            return dsPhuHop[Random.Range(0, dsPhuHop.Count)];
        }
        else
        {
            return danhSachNguon[Random.Range(0, danhSachNguon.Count)];
        }
    }

    private int TinhToanTier(int wave)
    {
        int xucXac = Random.Range(1, 101);
        int tierKetQua = 1;

        if (wave <= 2) tierKetQua = 1;
        else if (wave <= 5)
        {
            if (xucXac <= 80) tierKetQua = 1;
            else tierKetQua = 2;
        }
        else if (wave <= 10)
        {
            if (xucXac <= 60) tierKetQua = 1;
            else if (xucXac <= 90) tierKetQua = 2;
            else tierKetQua = 3;
        }
        else if (wave <= 14)
        {
            if (xucXac <= 45) tierKetQua = 1;
            else if (xucXac <= 80) tierKetQua = 2;
            else if (xucXac <= 95) tierKetQua = 3;
            else tierKetQua = 4;
        }
        else
        {
            if (xucXac <= 30) tierKetQua = 1;
            else if (xucXac <= 70) tierKetQua = 2;
            else if (xucXac <= 92) tierKetQua = 3;
            else tierKetQua = 4;
        }

        string mauLog = tierKetQua == 4 ? "red" : (tierKetQua == 3 ? "purple" : (tierKetQua == 2 ? "cyan" : "white"));
        Debug.Log($"<color=green>[REWARD LOG]</color> Wave {wave} | Xúc xắc: {xucXac} | Kết quả: <color={mauLog}>Tier {tierKetQua}</color>");

        return tierKetQua;
    }

    public void XuLyChonPhanThuong(UpgradeData data)
    {
        ApDungChiSo(data);
        panelPhanThuong.SetActive(false);
        if (ShopUI.Instance != null) ShopUI.Instance.MoCuaHang();
    }

    private void ApDungChiSo(UpgradeData data)
    {
        if (PlayerStats.Instance != null) PlayerStats.Instance.ThemChiSoTuThe(data);
    }

    public void KichHoatHienThiNgayLapTuc()
    {
        StartCoroutine(HienThiSauDelay(0.1f));
    }
}