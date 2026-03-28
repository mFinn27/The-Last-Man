using UnityEngine;

public enum ShopItemType { VuKhi, ChiSo }

[CreateAssetMenu(fileName = "New Shop Item", menuName = "Game Data/Shop Item")]
public class ShopItemData : ScriptableObject
{
    [Header("--- THÔNG TIN CHUNG ---")]
    public ShopItemType loaiMatHang;
    public string tenMatHang;
    [TextArea] public string moTa;
    public Sprite icon;
    public int giaGoc = 20;

    [Header("--- DỮ LIỆU ĐÍNH KÈM (Kéo đúng loại) ---")]
    public WeaponData duLieuVuKhi;
    public UpgradeData duLieuChiSo;
}