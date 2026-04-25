using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

[System.Serializable] public class WeaponPayload { public string targetSheet; public List<WeaponSyncModel> items; }
[System.Serializable] public class CharacterPayload { public string targetSheet; public List<CharacterSyncModel> items; }
[System.Serializable] public class EnemyPayload { public string targetSheet; public List<EnemySyncModel> items; }
[System.Serializable] public class ItemPayload { public string targetSheet; public List<ItemSyncModel> items; }
[System.Serializable] public class WavePayload { public string targetSheet; public List<WaveSyncModel> items; }

[System.Serializable]
public class WeaponSyncModel
{
    public string TenMatHang;
    public string LoaiVuKhi;
    public int CapDo;
    public int GiaMua;
    public int GiaBan;
    public double Dame;
    public double TocDoDanh;
    public double TamDanh;
    public double DayLui;
    public double TiLeChiMang;
    public double SatThuongChiMang;
    public double HutMau;
    public double TocDoBayCuaDan;
    public int XuyenThau;
}

[System.Serializable]
public class CharacterSyncModel
{
    public string IDNhanVat;
    public string TenNhanVat;
    public bool MoKhoaSan;
    public int DieuKienWave;
    public int MauToiDaGoc;
    public int GiapGoc;
    public double TocDoDiChuyenGoc;
    public double SatThuongGoc;
    public double TocDoDanhGoc;
    public double TiLeChiMangGoc;
    public double SatThuongChiMangGoc;
    public double HutMauGoc;
}

[System.Serializable]
public class EnemySyncModel
{
    public string TenQuaiVat;
    public string LoaiQuai;
    public double MauToiDa;
    public double TocDoDiChuyen;
    public int Dame;
    public double TgThucHienDonDanhTiepTheo;
    public double ThoiGianGongDon;
    public double TiLeRotThuong;
    public string PhanThuongRotRa;
    public int MinSoLuongVang;
    public int MaxSoLuongVang;
}

[System.Serializable]
public class ItemSyncModel
{
    public string TenMatHang;
    public int CapDo;
    public int GiaMua;
    public string MoTa;
    public string CacChiSoCongThem;
}

[System.Serializable]
public class WaveSyncModel
{
    public string KichBan;
    public double ThoiGian;
    public int TongQuai;
    public string Quai;
}

public class GoogleSheetSyncTool : EditorWindow
{
    private const string WEB_APP_URL = "https://script.google.com/macros/s/AKfycbyIUMG0UoRl8V9U8iAwmCw4lCvR1JQE1CPZ70Y-W7830tDoJfhZmYUvUanEkM0LewWN/exec";

    [MenuItem("Tools/Database/Đồng bộ Tất cả Dữ liệu")]
    public static void SyncAll()
    {
        SyncWeapons();
        SyncCharacters();
        SyncEnemies();
        SyncItems();
        SyncWaves();
    }

    public static void SyncWeapons()
    {
        var list = new List<WeaponSyncModel>();
        foreach (var guid in AssetDatabase.FindAssets("t:WeaponData"))
        {
            var w = AssetDatabase.LoadAssetAtPath<WeaponData>(AssetDatabase.GUIDToAssetPath(guid));
            if (w != null)
            {
                list.Add(new WeaponSyncModel
                {
                    TenMatHang = w.tenMatHang,
                    LoaiVuKhi = w.loaiVuKhi.ToString(),
                    CapDo = w.capDo,
                    GiaMua = w.giaMua,
                    GiaBan = w.giaBan,
                    Dame = Math.Round(w.dame, 2),
                    TocDoDanh = Math.Round(w.tocDoDanh, 2),
                    TamDanh = Math.Round(w.tamDanh, 2),
                    DayLui = Math.Round(w.dayLui, 2),
                    TiLeChiMang = Math.Round(w.tiLeChiMang, 2),
                    SatThuongChiMang = Math.Round(w.satThuongChiMang, 2),
                    HutMau = Math.Round(w.hutMau, 2),
                    TocDoBayCuaDan = Math.Round(w.tocDoBayCuaDan, 2),
                    XuyenThau = w.xuyenThau
                });
            }
        }
        string json = JsonUtility.ToJson(new WeaponPayload { targetSheet = "VuKhi", items = list });
        EditorCoroutine.Start(SendRequest(json, "VuKhi"));
    }

    public static void SyncCharacters()
    {
        var list = new List<CharacterSyncModel>();
        foreach (var guid in AssetDatabase.FindAssets("t:CharacterData"))
        {
            var c = AssetDatabase.LoadAssetAtPath<CharacterData>(AssetDatabase.GUIDToAssetPath(guid));
            if (c != null)
            {
                list.Add(new CharacterSyncModel
                {
                    IDNhanVat = c.idNhanVat,
                    TenNhanVat = c.tenNhanVat,
                    MoKhoaSan = c.moKhoaSan,
                    DieuKienWave = c.dieuKienWave,
                    MauToiDaGoc = c.mauToiDaGoc,
                    GiapGoc = c.giapGoc,
                    TocDoDiChuyenGoc = Math.Round(c.tocDoDiChuyenGoc, 2),
                    SatThuongGoc = Math.Round(c.satThuongGoc, 2),
                    TocDoDanhGoc = Math.Round(c.tocDoDanhGoc, 2),
                    TiLeChiMangGoc = Math.Round(c.tiLeChiMangGoc, 2),
                    SatThuongChiMangGoc = Math.Round(c.satThuongChiMangGoc, 2),
                    HutMauGoc = Math.Round(c.hutMauGoc, 2)
                });
            }
        }
        string json = JsonUtility.ToJson(new CharacterPayload { targetSheet = "NhanVat", items = list });
        EditorCoroutine.Start(SendRequest(json, "NhanVat"));
    }

    public static void SyncEnemies()
    {
        var list = new List<EnemySyncModel>();
        foreach (var guid in AssetDatabase.FindAssets("t:EnemyData"))
        {
            var e = AssetDatabase.LoadAssetAtPath<EnemyData>(AssetDatabase.GUIDToAssetPath(guid));
            if (e != null)
            {
                list.Add(new EnemySyncModel
                {
                    TenQuaiVat = e.tenQuaiVat,
                    LoaiQuai = e.loaiQuai.ToString(),
                    MauToiDa = Math.Round(e.mauToiDa, 2),
                    TocDoDiChuyen = Math.Round(e.tocDoDiChuyen, 2),
                    Dame = e.dame,
                    TgThucHienDonDanhTiepTheo = Math.Round(e.tgThucHienDonDanhTiepTheo, 2),
                    ThoiGianGongDon = Math.Round(e.thoiGianGongDon, 2),
                    TiLeRotThuong = Math.Round(e.tiLeRotThuong, 2),
                    PhanThuongRotRa = e.phanThuongRotRa.ToString(),
                    MinSoLuongVang = e.minSoLuongVang,
                    MaxSoLuongVang = e.maxSoLuongVang
                });
            }
        }
        string json = JsonUtility.ToJson(new EnemyPayload { targetSheet = "Enemy", items = list });
        EditorCoroutine.Start(SendRequest(json, "Enemy"));
    }

    private static string SanitizeForGoogleSheet(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";
        string cleanText = input.Replace("\n", " | ").Replace("\r", "");
        if (cleanText.StartsWith("=") || cleanText.StartsWith("+") || cleanText.StartsWith("-") || cleanText.StartsWith("@"))
        {
            cleanText = "'" + cleanText;
        }
        return cleanText;
    }

    public static void SyncItems()
    {
        var list = new List<ItemSyncModel>();
        foreach (var guid in AssetDatabase.FindAssets("t:ItemData"))
        {
            var i = AssetDatabase.LoadAssetAtPath<ItemData>(AssetDatabase.GUIDToAssetPath(guid));

            if (i != null && !(i is WeaponData))
            {
                string chiSoGopTuList = "";

                if (i is UpgradeData)
                {
                    UpgradeData upgrade = (UpgradeData)i;
                    if (upgrade.danhSachChiSo != null && upgrade.danhSachChiSo.Count > 0)
                    {
                        List<string> cacChiSo = new List<string>();
                        foreach (var chiSo in upgrade.danhSachChiSo)
                        {
                            string dauCongTru = chiSo.giaTriCongThem > 0 ? "+" : "";
                            cacChiSo.Add($"{chiSo.loaiChiSo}: {dauCongTru}{chiSo.giaTriCongThem}");
                        }
                        chiSoGopTuList = string.Join(", ", cacChiSo);
                    }
                }

                list.Add(new ItemSyncModel
                {
                    TenMatHang = i.tenMatHang,
                    CapDo = i.capDo,
                    GiaMua = i.giaMua,
                    MoTa = SanitizeForGoogleSheet(i.moTa),
                    CacChiSoCongThem = chiSoGopTuList
                });
            }
        }
        string json = JsonUtility.ToJson(new ItemPayload { targetSheet = "Items", items = list });
        EditorCoroutine.Start(SendRequest(json, "Items"));
    }

    public static void SyncWaves()
    {
        var list = new List<WaveSyncModel>();
        foreach (var guid in AssetDatabase.FindAssets("t:WaveData"))
        {
            var w = AssetDatabase.LoadAssetAtPath<WaveData>(AssetDatabase.GUIDToAssetPath(guid));
            if (w != null)
            {

                Dictionary<string, int> tuDienQuai = new Dictionary<string, int>();
                int tongQuaiTrongWave = 0;

                if (w.danhSachSuKien != null)
                {
                    foreach (var suKien in w.danhSachSuKien)
                    {
                        string tenQuai = suKien.quaiPrefab != null ? suKien.quaiPrefab.name : "Trống";
                        if (tenQuai == "Trống") continue;

                        float duration = suKien.giayKetThuc - suKien.giayBatDau;
                        if (duration < 0) duration = 0;

                        int soLanSpawn = 0;
                        if (suKien.thoiGianGiuaCacLan > 0)
                        {
                            soLanSpawn = Mathf.FloorToInt(duration / suKien.thoiGianGiuaCacLan) + 1;
                        }

                        int soQuaiDotNay = soLanSpawn * suKien.soLuongMoiLan;
                        tongQuaiTrongWave += soQuaiDotNay;

                        if (tuDienQuai.ContainsKey(tenQuai))
                        {
                            tuDienQuai[tenQuai] += soQuaiDotNay;
                        }
                        else
                        {
                            tuDienQuai[tenQuai] = soQuaiDotNay;
                        }
                    }
                }

                List<string> listChuoiQuai = new List<string>();
                foreach (var kvp in tuDienQuai)
                {
                    listChuoiQuai.Add($"{kvp.Key} x{kvp.Value}");
                }
                string chuoiQuaiGop = string.Join(", ", listChuoiQuai);

                list.Add(new WaveSyncModel
                {
                    KichBan = w.tenWave,
                    ThoiGian = Math.Round(w.thoiGianWave, 2),
                    TongQuai = tongQuaiTrongWave,
                    Quai = SanitizeForGoogleSheet(chuoiQuaiGop)
                });
            }
        }
        string json = JsonUtility.ToJson(new WavePayload { targetSheet = "Wave", items = list });
        EditorCoroutine.Start(SendRequest(json, "Wave"));
    }

    private static System.Collections.IEnumerator SendRequest(string jsonPayload, string sheetName)
    {
        using (UnityWebRequest request = new UnityWebRequest(WEB_APP_URL, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            var operation = request.SendWebRequest();
            while (!operation.isDone) yield return null;

            if (request.result == UnityWebRequest.Result.Success)
                Debug.Log($"<color=green>[Google Sheets] Đồng bộ bảng {sheetName} hoàn tất!</color>");
            else
                Debug.LogError($"[Google Sheets] Lỗi đồng bộ {sheetName}: {request.error}");
        }
    }
}

public class AutoSyncProcessor : UnityEditor.AssetModificationProcessor
{
    public static string[] OnWillSaveAssets(string[] paths)
    {
        foreach (string path in paths)
        {
            if (!path.EndsWith(".asset")) continue;

            if (path.Contains("Weapon")) EditorApplication.delayCall += () => GoogleSheetSyncTool.SyncWeapons();
            else if (path.Contains("Character")) EditorApplication.delayCall += () => GoogleSheetSyncTool.SyncCharacters();
            else if (path.Contains("Enemy")) EditorApplication.delayCall += () => GoogleSheetSyncTool.SyncEnemies();
            else if (path.Contains("StatsUpgrade") || path.Contains("Chest")) EditorApplication.delayCall += () => GoogleSheetSyncTool.SyncItems();
            else if (path.Contains("Wave")) EditorApplication.delayCall += () => GoogleSheetSyncTool.SyncWaves();
        }
        return paths;
    }
}

public static class EditorCoroutine
{
    public static void Start(System.Collections.IEnumerator routine)
    {
        EditorApplication.CallbackFunction updateCallback = null;
        updateCallback = () => { if (routine.MoveNext() == false) EditorApplication.update -= updateCallback; };
        EditorApplication.update += updateCallback;
    }
}