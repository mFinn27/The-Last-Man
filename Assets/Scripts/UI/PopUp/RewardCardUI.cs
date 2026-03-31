using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardCardUI : MonoBehaviour
{
    [Header("--- KẾT NỐI UI ---")]
    public TextMeshProUGUI txtTen;
    public TextMeshProUGUI txtMoTa;
    public Image imgIcon;
    public Image imgVienCapDo;

    private UpgradeData dataHienTai;
    private RewardPopupUI manager;

    public void Setup(UpgradeData data, RewardPopupUI uiManager)
    {
        dataHienTai = data;
        manager = uiManager;

        if (txtTen != null)
        {
            txtTen.text = data.tenMatHang;
            txtTen.color = data.mauCapDo;
        }

        if (imgVienCapDo != null)
        {
            imgVienCapDo.color = data.mauCapDo;
        }

        if (txtMoTa != null) txtMoTa.text = data.moTa;
        if (imgIcon != null && data.iconMatHang != null) imgIcon.sprite = data.iconMatHang;
    }

    public void ChonTheNay()
    {
        if (manager != null && dataHienTai != null)
        {
            manager.XuLyChonPhanThuong(dataHienTai);
        }
    }
}