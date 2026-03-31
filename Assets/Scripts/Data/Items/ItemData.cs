using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public string tenMatHang;
    [TextArea] public string moTa;
    public Sprite iconMatHang;
    public int giaMua = 20;

    [Header("--- ĐỘ HIẾM (TIER) ---")]
    [Tooltip("1: Thường (Trắng), 2: Hiếm (Xanh), 3: Sử Thi (Tím), 4: Huyền Thoại (Đỏ)")]
    public int capDo = 1;
    public Color mauCapDo = Color.white;
}