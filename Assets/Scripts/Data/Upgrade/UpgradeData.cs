using UnityEngine;
using System.Collections.Generic;

public enum LoaiChiSo
{
    MauToiDa, Giap, TocDoDiChuyen,
    SatThuong, TocDoDanh, TiLeChiMang, SatThuongChiMang,
    HutMau, PhamViHut
}

[System.Serializable]
public class ChiSoNangCap
{
    public LoaiChiSo loaiChiSo;
    public float giaTriCongThem;
}

[CreateAssetMenu(fileName = "New Reward", menuName = "Game Data/Reward Upgrade")]
public class UpgradeData : ScriptableObject
{
    [Header("--- HIỂN THỊ UI ---")]
    public string tenNangCap;
    [TextArea] public string moTa;
    public Sprite icon;

    [Header("--- DANH SÁCH CHỈ SỐ CỘNG THÊM ---")]
    public List<ChiSoNangCap> danhSachChiSo;
}