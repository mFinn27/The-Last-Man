using UnityEngine;

[CreateAssetMenu(fileName = "RangedData", menuName = "Enemies/Ranged Data")]
public class RangedEnemyData : EnemyData
{
    [Header("--- ĐÁNH XA ---")]
    public float tamDanhXa = 7f;
    public GameObject danPrefab;
    public float tocDoDan = 8f;
    public int soLuongDan = 1;
    public float gocToaDan = 15f;
}