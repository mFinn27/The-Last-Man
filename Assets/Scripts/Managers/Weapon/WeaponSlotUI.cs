using UnityEngine;
using UnityEngine.UI;

public class WeaponSlotUI : MonoBehaviour
{
    public Image imgIconVuKhi;
    public Image imgVienCapDo;
    private int slotIndexHienTai;

    public void Setup(WeaponData data, int index)
    {
        slotIndexHienTai = index;

        if (data == null)
        {
            imgIconVuKhi.gameObject.SetActive(false);
            imgVienCapDo.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
        }
        else
        {
            imgIconVuKhi.gameObject.SetActive(true);
            imgIconVuKhi.sprite = data.iconMatHang;
            imgVienCapDo.color = data.mauCapDo;
        }
    }

    public void BamVaoSlotNay()
    {
        if (WeaponManager.Instance != null && WeaponManager.Instance.danhSachVuKhi.Count > slotIndexHienTai)
        {
            ShopUI.Instance.MoBangThaoTacVuKhi(slotIndexHienTai);
        }
    }
}