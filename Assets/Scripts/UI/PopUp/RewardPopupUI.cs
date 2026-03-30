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
        if (PlayerStats.Instance == null) return;

        foreach (var chiSo in data.danhSachChiSo)
        {
            switch (chiSo.loaiChiSo)
            {
                case LoaiChiSo.MauToiDa:
                    PlayerStats.Instance.bonusMauToiDa += (int)chiSo.giaTriCongThem;
                    if (PlayerHealth.Instance != null) PlayerHealth.Instance.UpdateMaxHealth();
                    break;
                case LoaiChiSo.Giap:
                    PlayerStats.Instance.bonusGiap += (int)chiSo.giaTriCongThem;
                    break;
                case LoaiChiSo.TocDoDiChuyen:
                    PlayerStats.Instance.bonusTocDoDiChuyen += chiSo.giaTriCongThem;
                    break;
                case LoaiChiSo.SatThuong:
                    PlayerStats.Instance.bonusSatThuong += chiSo.giaTriCongThem;
                    break;
                case LoaiChiSo.TocDoDanh:
                    PlayerStats.Instance.bonusTocDoDanh += chiSo.giaTriCongThem;
                    break;
                case LoaiChiSo.TiLeChiMang:
                    PlayerStats.Instance.bonusTiLeChiMang += chiSo.giaTriCongThem;
                    break;
                case LoaiChiSo.SatThuongChiMang:
                    PlayerStats.Instance.bonusSatThuongChiMang += chiSo.giaTriCongThem;
                    break;
                case LoaiChiSo.HutMau:
                    PlayerStats.Instance.bonusHutMau += chiSo.giaTriCongThem;
                    break;
            }
        }

        Debug.Log($"Đã áp dụng toàn bộ chỉ số của thẻ!");
    }
}