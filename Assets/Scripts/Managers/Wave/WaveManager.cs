using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    public static event Action OnWaveEnded;
    public static event Action<int> OnWaveStarted;

    [Header("--- CHIẾN DỊCH (TỔNG HỢP CÁC WAVE) ---")]
    public List<WaveData> danhSachWave;
    public int waveHienTaiIndex { get; private set; } = 0;

    public float thoiGianWaveHienTai { get; private set; }
    public float thoiGianDaQua { get; private set; }
    public bool dangTrongWave { get; private set; }

    [Header("--- CƠ CHẾ BÁO HIỆU & GIỚI HẠN ---")]
    public GameObject warningPrefab;
    public float thoiGianCanhBao = 1f;
    public Vector2 mapMin = new Vector2(-15f, -10f);
    public Vector2 mapMax = new Vector2(15f, 10f);
    public float khoangCachAnToanVoiPlayer = 3f;

    private WaveData waveDataHienTai;
    private Dictionary<WaveEvent, float> soTayThoiGian = new Dictionary<WaveEvent, float>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        BatDauWave();
    }

    void Update()
    {
        if (!dangTrongWave) return;

        thoiGianDaQua += Time.deltaTime;
        KiemTraVaChayKichBan();

        if (thoiGianDaQua >= thoiGianWaveHienTai)
        {
            KetThucWave();
        }
    }

    public void BatDauWave()
    {
        if (waveHienTaiIndex >= danhSachWave.Count)
        {
            Debug.Log("CHÚC MỪNG! BẠN ĐÃ HOÀN THÀNH TẤT CẢ CÁC WAVE!");
            return;
        }

        waveDataHienTai = danhSachWave[waveHienTaiIndex];
        thoiGianWaveHienTai = waveDataHienTai.thoiGianWave;
        thoiGianDaQua = 0f;
        dangTrongWave = true;
        soTayThoiGian.Clear();
        foreach (var suKien in waveDataHienTai.danhSachSuKien)
        {
            soTayThoiGian.Add(suKien, suKien.giayBatDau);
        }

        OnWaveStarted?.Invoke(waveHienTaiIndex + 1);
        Debug.Log($"BẮT ĐẦU {waveDataHienTai.tenWave}!");
    }

    private void KiemTraVaChayKichBan()
    {
        foreach (var suKien in waveDataHienTai.danhSachSuKien)
        {
            if (thoiGianDaQua >= suKien.giayBatDau && thoiGianDaQua <= suKien.giayKetThuc)
            {
                float thoiGianToiHan = soTayThoiGian[suKien];

                if (thoiGianDaQua >= thoiGianToiHan)
                {
                    ThucHienSpawn(suKien);
                    soTayThoiGian[suKien] = thoiGianDaQua + suKien.thoiGianGiuaCacLan;
                }
            }
        }
    }

    private void ThucHienSpawn(WaveEvent suKien)
    {
        if (suKien.quaiPrefab == null) return;

        if (suKien.deTheoCum)
        {
            if (TryLayViTriTrongArena(out Vector2 viTriTrungTam))
            {
                for (int i = 0; i < suKien.soLuongMoiLan; i++)
                {
                    Vector2 viTriChenhLech = viTriTrungTam + UnityEngine.Random.insideUnitCircle * 2f;
                    viTriChenhLech.x = Mathf.Clamp(viTriChenhLech.x, mapMin.x, mapMax.x);
                    viTriChenhLech.y = Mathf.Clamp(viTriChenhLech.y, mapMin.y, mapMax.y);
                    StartCoroutine(BaoHieuVaSpawnRoutine(viTriChenhLech, suKien.quaiPrefab));
                }
            }
        }
        else
        {
            for (int i = 0; i < suKien.soLuongMoiLan; i++)
            {
                if (TryLayViTriTrongArena(out Vector2 viTriDocLap))
                {
                    StartCoroutine(BaoHieuVaSpawnRoutine(viTriDocLap, suKien.quaiPrefab));
                }
            }
        }
    }

    private IEnumerator BaoHieuVaSpawnRoutine(Vector2 viTri, GameObject quaiPrefab)
    {
        GameObject warning = null;
        if (WarningPool.Instance != null) warning = WarningPool.Instance.GetWarning(viTri);
        else if (warningPrefab != null) warning = Instantiate(warningPrefab, viTri, Quaternion.identity);

        yield return new WaitForSeconds(thoiGianCanhBao);

        if (warning != null)
        {
            if (WarningPool.Instance != null) WarningPool.Instance.ReturnWarning(warning);
            else Destroy(warning);
        }

        if (dangTrongWave) Instantiate(quaiPrefab, viTri, Quaternion.identity);
    }

    private bool TryLayViTriTrongArena(out Vector2 viTriAnToan)
    {
        viTriAnToan = Vector2.zero;
        if (PlayerHealth.Instance == null) return false;

        Vector2 viTriPlayer = PlayerHealth.Instance.transform.position;

        for (int i = 0; i < 10; i++)
        {
            float randomX = UnityEngine.Random.Range(mapMin.x, mapMax.x);
            float randomY = UnityEngine.Random.Range(mapMin.y, mapMax.y);
            Vector2 viTriDuKien = new Vector2(randomX, randomY);

            if ((viTriDuKien - viTriPlayer).sqrMagnitude >= (khoangCachAnToanVoiPlayer * khoangCachAnToanVoiPlayer))
            {
                viTriAnToan = viTriDuKien;
                return true;
            }
        }
        return false;
    }

    private void KetThucWave()
    {
        dangTrongWave = false;
        OnWaveEnded?.Invoke();
    }

    public void ChuyenSangWaveTiepTheo()
    {
        if (!dangTrongWave)
        {
            waveHienTaiIndex++;
            BatDauWave();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 center = (mapMin + mapMax) / 2f;
        Vector2 size = new Vector2(Mathf.Abs(mapMax.x - mapMin.x), Mathf.Abs(mapMax.y - mapMin.y));
        Gizmos.DrawWireCube(center, size);
    }
}