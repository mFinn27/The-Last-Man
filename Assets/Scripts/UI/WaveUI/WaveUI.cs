using UnityEngine;
using TMPro;

public class WaveUI : MonoBehaviour
{
    public TextMeshProUGUI textThoiGian;
    public TextMeshProUGUI textTenWave;

    private void OnEnable()
    {
        WaveManager.OnWaveStarted += CapNhatTenWave;
    }

    private void OnDisable()
    {
        WaveManager.OnWaveStarted -= CapNhatTenWave;
    }

    private void CapNhatTenWave(int soThuTuWave)
    {
        if (textTenWave != null) textTenWave.text = "WAVE " + soThuTuWave;
    }

    void Update()
    {
        if (WaveManager.Instance != null && WaveManager.Instance.dangTrongWave)
        {
            float thoiGianConLai = WaveManager.Instance.thoiGianWaveHienTai - WaveManager.Instance.thoiGianDaQua;
            if (thoiGianConLai < 0) thoiGianConLai = 0;

            int phut = Mathf.FloorToInt(thoiGianConLai / 60);
            int giay = Mathf.FloorToInt(thoiGianConLai % 60);
            textThoiGian.text = string.Format("{0:00}:{1:00}", phut, giay);
        }
        else if (textThoiGian != null)
        {
            textThoiGian.text = "00:00";
        }
    }
}