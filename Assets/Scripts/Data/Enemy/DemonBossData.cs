using UnityEngine;

[CreateAssetMenu(fileName = "DemonBossData", menuName = "Enemies/Boss/Demon Boss Data")]
public class DemonBossData : EnemyData
{
    [Header("--- DEMON BOSS: CHU KỲ & KỸ NĂNG ---")]
    public float thoiGianDuoiBat = 3.5f;

    [Header("- Pha Xả Đạn 360 -")]
    public GameObject danPrefab;
    public float tocDoDan = 8f;
    public int soLuongDanToaTron = 16;

    [Header("- Pha Lướt Chém -")]
    public int soLanLuot = 3;
}