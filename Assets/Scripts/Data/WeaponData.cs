using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("General")]
    public string tenVuKhi;
    public Sprite hinhAnhVuKhi;

    [Header("Critical")]
    [Range(0, 1)]
    public float tiLeChiMang = 0.1f;     // 10%

    public float satThuongChiMang = 0.1f;   // 200% damage

    [Header("Life Steal")]
    [Range(0, 1)]
    public float hutMau = 0f; // 0.05 = 5%

    [Header("Combat")]
    public float tamDanh = 0;
    public float soDonDanhTrenMoiS = 0;
    public int damage = 0;

    [Header("Swing")]
    public float gocChem = 0;
    public float tocDoXoay = 0;
    public float overshoot = 0;

    [Header("Range")]
    public float doDaiVuKhi = 0;

    [Header("Ranged")]
    public float tocDoBayCuaDan = 0;
    public int xuyenThau = 0;
    public float dayLui = 0;
    public GameObject bulletPrefab;

    [Header("Thrust")]
    public float tocDoDam = 0;
}