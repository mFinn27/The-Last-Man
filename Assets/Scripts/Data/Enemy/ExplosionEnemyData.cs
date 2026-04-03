using UnityEngine;

[CreateAssetMenu(fileName = "ExplosionData", menuName = "Enemies/Explosion Data")]
public class ExplosionEnemyData : EnemyData
{
    [Header("--- THÔNG SỐ NỔ ---")]
    public int damePhatNo = 50;
    public float banKinhNo = 3f;
    public float thoiGianChoNo = 2f;
}