using UnityEngine;

[CreateAssetMenu(fileName = "MeleeData", menuName = "Enemies/Melee Data")]
public class MeleeEnemyData : EnemyData
{
    [Header("--- CẬN CHIẾN ---")]
    public float khoangCachCanChien = 1.2f;
}