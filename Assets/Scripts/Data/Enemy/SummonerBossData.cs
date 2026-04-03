using UnityEngine;

[CreateAssetMenu(fileName = "SummonerBossData", menuName = "Enemies/Boss/Summoner Boss Data")]
public class SummonerBossData : EnemyData
{
    [Header("--- SUMMONER BOSS: CHU KỲ & KỸ NĂNG ---")]
    public float thoiGianDuoiBat = 3f;

    [Header("- Pha Triệu Hồi -")]
    public GameObject quaiNhoPrefab;
    public int soLuongQuaiSpawn = 4;

    [Header("- Pha Bắn Liên Tục (Burst) -")]
    public GameObject danPrefab;
    public float tocDoDan = 8f;
    public int soLuongDanBurst = 3;
    public float thoiGianGiuaCacVien = 0.2f;
}