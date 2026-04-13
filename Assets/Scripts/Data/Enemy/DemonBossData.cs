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

    [Header("- Pha Triệu Hồi (Mới) -")]
    public GameObject quaiNhoPrefab;
    public int soLuongQuaiSpawn = 4;

    [Header("- Pha Bắn Liên Tục (Mới) -")]
    public int soLuongDanBurst = 5;
    public float thoiGianGiuaCacVien = 0.15f;
}