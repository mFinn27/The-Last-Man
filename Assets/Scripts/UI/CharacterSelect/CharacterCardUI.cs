using UnityEngine;
using UnityEngine.UI;

public class CharacterCardUI : MonoBehaviour
{
    public CharacterData dataNhanVat;
    public Image imgIcon;
    public GameObject iconKhoa;

    [Header("--- THIẾT LẬP MÀU LÀM MỜ ---")]
    public Image imgBackground;
    public Color mauChuaMoKhoa = new Color(0.3f, 0.3f, 0.3f, 1f);
    public Color mauDaMoKhoa = Color.white;

    private MainMenuManager menuManager;
    private bool daMoKhoa = false;

    public void Setup(CharacterData data, MainMenuManager manager)
    {
        dataNhanVat = data;
        menuManager = manager;

        if (data == null) return;

        if (data.moKhoaSan)
        {
            daMoKhoa = true;
        }
        else
        {
            int waveCaoNhat = GameManager.Instance != null ? GameManager.Instance.waveCaoNhatDaDatDuoc : 0;
            daMoKhoa = (waveCaoNhat >= data.dieuKienWave);
        }

        if (imgIcon != null)
        {
            imgIcon.sprite = data.hinhAnhNhanVat;
            imgIcon.color = daMoKhoa ? mauDaMoKhoa : mauChuaMoKhoa;
        }

        if (imgBackground != null)
        {
            imgBackground.color = daMoKhoa ? mauDaMoKhoa : mauChuaMoKhoa;
        }

        if (iconKhoa != null) iconKhoa.SetActive(!daMoKhoa);
    }

    public void BamChonTuong()
    {
        if (menuManager != null && dataNhanVat != null)
        {
            menuManager.ChonNhanVat(dataNhanVat);
        }
    }
}