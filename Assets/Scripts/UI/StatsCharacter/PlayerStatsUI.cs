using UnityEngine;
using System.Collections.Generic;

public class PlayerStatsUI : MonoBehaviour
{
    [Header("--- CẤU HÌNH ---")]
    public GameObject statRowPrefab;
    public Transform container;

    private List<GameObject> activeRows = new List<GameObject>();

    private void OnEnable()
    {
        HienThiDanhSachChiSo();
    }

    private string DinhDangMau(float giaTri, string format = "0", string hauTo = "", bool hienDauCong = false)
    {
        string textGiaTri = giaTri.ToString(format) + hauTo;

        if (hienDauCong && giaTri > 0)
        {
            textGiaTri = "+" + textGiaTri;
        }

        if (giaTri > 0)
        {
            return $"<color=#00FF00>{textGiaTri}</color>";
        }
        else if (giaTri < 0)
        {
            return $"<color=#FF0000>{textGiaTri}</color>";
        }
        else
        {
            return textGiaTri;
        }
    }

    public void HienThiDanhSachChiSo()
    {
        foreach (var row in activeRows) Destroy(row);
        activeRows.Clear();

        if (PlayerStats.Instance == null) return;

        TaoDong("Máu tối đa:", DinhDangMau(PlayerStats.Instance.GetMaxHP()));
        TaoDong("Giáp:", DinhDangMau(PlayerStats.Instance.GetArmor()));
        TaoDong("Tốc độ chạy:", DinhDangMau(PlayerStats.Instance.GetMoveSpeed(), "F1"));
        TaoDong("Sát thương:", DinhDangMau(PlayerStats.Instance.GetDamage(), "0", "", true));
        TaoDong("Tốc độ đánh:", DinhDangMau(PlayerStats.Instance.GetAttackSpeed() * 100, "F0", "<size=70%>%</size>", true));

        int phanTramChiMang = Mathf.RoundToInt(PlayerStats.Instance.GetCritChance() * 100);
        if (phanTramChiMang >= 100)
        {
            TaoDong("TL chí mạng:", "<color=yellow>+100<size=70%>%</size> (MAX)</color>");
        }
        else
        {
            TaoDong("TL chí mạng:", DinhDangMau(phanTramChiMang, "0", "<size=70%>%</size>", true));
        }

        TaoDong("ST Chí mạng:", DinhDangMau(PlayerStats.Instance.GetCritDamage() * 100, "F0", "<size=70%>%</size>", true));

        int phanTramHutMau = Mathf.RoundToInt(PlayerStats.Instance.GetLifeSteal() * 100);
        if (phanTramHutMau >= 40)
        {
            TaoDong("Hút máu:", "<color=yellow>+40<size=70%>%</size> (MAX)</color>");
        }
        else
        {
            TaoDong("Hút máu:", DinhDangMau(phanTramHutMau, "0", "<size=70%>%</size>", true));
        }

        TaoDong("Đẩy lùi:", DinhDangMau(PlayerStats.Instance.GetBonusDayLui(), "F1"));
        TaoDong("Tầm đánh:", DinhDangMau(PlayerStats.Instance.GetBonusTamDanh(), "0", "", true));
        TaoDong("Xuyên thấu:", DinhDangMau(PlayerStats.Instance.GetBonusXuyenThau(), "0", "", true));
        TaoDong("Phạm vi nhặt:", DinhDangMau(PlayerStats.Instance.GetMagnetRange(), "F1"));
    }

    private void TaoDong(string label, string value)
    {
        GameObject rowObj = Instantiate(statRowPrefab, container);
        StatRowUI rowScript = rowObj.GetComponent<StatRowUI>();

        if (rowScript != null)
        {
            rowScript.Setup(label, value);
            activeRows.Add(rowObj);
        }
    }
}