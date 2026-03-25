using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WaveEvent
{
    public string tenKichBan = "Giai đoạn 1";

    [Header("THỜI GIAN (Tính từ lúc bắt đầu Wave)")]
    public float giayBatDau = 0f;
    public float giayKetThuc = 15f;

    [Header("THÔNG SỐ QUÁI")]
    public GameObject quaiPrefab;
    public float thoiGianGiuaCacLan = 1f;
    public int soLuongMoiLan = 1;

    [Header("CƠ CHẾ BẦY ĐÀN (HORDE)")]
    public bool deTheoCum = false;
}

[CreateAssetMenu(fileName = "New Wave", menuName = "Game Data/Wave Data")]
public class WaveData : ScriptableObject
{
    [Header("--- THÔNG TIN ĐỢT TẤN CÔNG ---")]
    public string tenWave = "Wave 1";
    public float thoiGianWave = 60f;

    [Header("--- KỊCH BẢN ĐẠO DIỄN ---")]
    public List<WaveEvent> danhSachSuKien;
}