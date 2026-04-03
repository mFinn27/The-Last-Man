using UnityEngine;

[CreateAssetMenu(fileName = "DashData", menuName = "Enemies/Dash Data")]
public class DashEnemyData : EnemyData
{
    [Header("--- THÔNG SỐ LƯỚT ---")]
    public float tamKichHoatLuot = 7f;
    public float tocDoLuot = 25f;
    public float thoiGianLuot = 0.35f;
    public float thoiGianHoiChieuLuot = 2f;
}