using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RewardPopupUI : MonoBehaviour
{
    [Header("--- THIẾT LẬP UI ---")]
    public GameObject panelPhanThuong;
    public RewardCardUI[] cacTheUI;

    [Header("--- KHO DỮ LIỆU ---")]
    public List<UpgradeData> khoDuLieu;

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
                int rand = Random.Range(0, theTamThoi.Count);
                cacTheUI[i].Setup(theTamThoi[rand], this);
                theTamThoi.RemoveAt(rand);
            }
        }
    }
    public void XuLyChonPhanThuong(UpgradeData data)
    {
        ApDungChiSo(data);
        panelPhanThuong.SetActive(false);

        Debug.Log("Đã nhận phần thưởng! Đang mở Cửa Hàng (Shop)...");
        if (ShopUI.Instance != null)
        {
            ShopUI.Instance.MoCuaHang();
        }
    }

    private void ApDungChiSo(UpgradeData data)
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.ThemChiSoTuThe(data);
            Debug.Log($"Đã áp dụng toàn bộ chỉ số của thẻ: {data.tenMatHang}!");
        }
        else
        {
            Debug.LogWarning("Lỗi: Không tìm thấy PlayerStats.Instance để cộng chỉ số!");
        }
    }
}