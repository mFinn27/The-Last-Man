using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ShopUI : MonoBehaviour
{
    public static ShopUI Instance;

    [Header("--- KẾT NỐI UI ---")]
    public GameObject panelShop;
    public TextMeshProUGUI txtVangHienTai;
    public ShopCardUI[] cacTheTrenKe;

    [Header("--- THIẾT LẬP REROLL ---")]
    public int giaRerollBanDau = 2;
    public int buocNhayGiaReroll = 2;

    [Header("--- KHO HÀNG HÓA ---")]
    public List<ShopItemData> khoHangHoa;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void MoCuaHang()
    {
        panelShop.SetActive(true);
        Time.timeScale = 0f;
        CapNhatVangUITong();

        foreach (var the in cacTheTrenKe)
        {
            if (!the.dangBiKhoa)
            {
                if (khoHangHoa.Count > 0)
                {
                    int rand = Random.Range(0, khoHangHoa.Count);
                    the.Setup(khoHangHoa[rand], this, giaRerollBanDau);
                }
                else
                {
                    the.Setup(null, this, 0);
                }
            }
            else
            {
                the.ResetChoWaveMoi(giaRerollBanDau);
            }
        }
    }

    public void RerollJustThisCard(ShopCardUI theYeuCau, int giaRerollMoi)
    {
        if (khoHangHoa.Count > 0)
        {
            int rand = Random.Range(0, khoHangHoa.Count);
            theYeuCau.Setup(khoHangHoa[rand], this, giaRerollMoi);
        }
    }

    public void CapNhatVangUITong()
    {
        if (txtVangHienTai != null && PlayerStats.Instance != null)
        {
            txtVangHienTai.text = PlayerStats.Instance.vangHienTai.ToString();
        }
    }

    public void BamChuyenWaveMoi()
    {
        panelShop.SetActive(false);
        Time.timeScale = 1f;

        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.ChuyenSangWaveTiepTheo();
        }
    }
}