using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public string tenMatHang;
    [TextArea] public string moTa;
    public Sprite iconMatHang;
    public int giaMua = 20;
}