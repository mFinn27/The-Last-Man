using UnityEngine;

public class BackBtnLogic : MonoBehaviour
{
    public MenuNavigator navigator;
    public GameObject panelChiTietVuKhi;

    public void BamBack()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();
        if (panelChiTietVuKhi.activeSelf)
        {
            navigator.MoManHinhChonTuong();
        }
        else
        {
            navigator.MoManHinhTitle();
        }
    }
}