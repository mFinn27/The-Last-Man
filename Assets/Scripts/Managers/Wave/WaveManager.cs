using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    public static event Action OnWaveEnded;
    public static event Action<int> OnWaveStarted;

    [Header("--- CHIẾN DỊCH ---")]
    public List<WaveData> danhSachWave;
    public int waveHienTaiIndex { get; private set; } = 0;

    public float thoiGianWaveHienTai { get; private set; }
    public float thoiGianDaQua { get; private set; }
    public bool dangTrongWave { get; private set; }
    public void SetWaveIndex(int index) { waveHienTaiIndex = index; }

    [Header("--- CƠ CHẾ BÁO HIỆU & GIỚI HẠN ---")]
    public GameObject warningPrefab;
    public float thoiGianCanhBao = 1f;
    public Vector2 mapMin = new Vector2(-15f, -10f);
    public Vector2 mapMax = new Vector2(15f, 10f);
    public float khoangCachAnToanVoiPlayer = 3f;

    [HideInInspector] public int soLuongBossTrenMap = 0;

    private WaveData waveDataHienTai;
    private Dictionary<WaveEvent, float> soTayThoiGian = new Dictionary<WaveEvent, float>();

    private bool daPhatCanhBaoBoss = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (!dangTrongWave) return;

        if (thoiGianDaQua < thoiGianWaveHienTai)
        {
            thoiGianDaQua += Time.deltaTime;
            KiemTraVaChayKichBan();
        }

        if (thoiGianDaQua >= thoiGianWaveHienTai)
        {
            thoiGianDaQua = thoiGianWaveHienTai;

            if (waveDataHienTai != null && waveDataHienTai.batBuocGietBoss)
            {
                if (soLuongBossTrenMap <= 0)
                {
                    KetThucWave();
                }
            }
            else
            {
                KetThucWave();
            }
        }
    }

    public void BatDauWave()
    {
        if (waveHienTaiIndex >= danhSachWave.Count)
        {
            Debug.Log("CHÚC MỪNG! BẠN ĐÃ HOÀN THÀNH TẤT CẢ CÁC WAVE!");
            return;
        }

        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.vangKhiBatDauWave = PlayerStats.Instance.vangHienTai;
        }
        if (GameManager.Instance != null)
        {
            GameManager.Instance.soQuaiDaGietKhiBatDauWave = GameManager.Instance.soQuaiDaGiet;
        }

        waveDataHienTai = danhSachWave[waveHienTaiIndex];
        thoiGianWaveHienTai = waveDataHienTai.thoiGianWave;
        thoiGianDaQua = 0f;
        dangTrongWave = true;
        soLuongBossTrenMap = 0;
        daPhatCanhBaoBoss = false;
        soTayThoiGian.Clear();

        foreach (var suKien in waveDataHienTai.danhSachSuKien)
        {
            soTayThoiGian.Add(suKien, suKien.giayBatDau);
        }

        if (AudioManager.Instance != null) AudioManager.Instance.PlayGameplayBGM();

        OnWaveStarted?.Invoke(waveHienTaiIndex + 1);
        Debug.Log($"BẮT ĐẦU {waveDataHienTai.tenWave}!");
    }

    public void DangKyBoss()
    {
        soLuongBossTrenMap++;
    }

    public void BossDaChet()
    {
        soLuongBossTrenMap--;
        if (soLuongBossTrenMap <= 0 && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameplayBGM();
        }

        if (thoiGianDaQua >= thoiGianWaveHienTai && waveDataHienTai != null && waveDataHienTai.batBuocGietBoss)
        {
            if (soLuongBossTrenMap <= 0)
            {
                KetThucWave();
            }
        }
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
                    bool laBoss = false;
                    if (suKien.quaiPrefab != null)
                    {
                        EnemyHealth mauQuai = suKien.quaiPrefab.GetComponent<EnemyHealth>();
                        if (mauQuai != null && mauQuai.data != null && mauQuai.data.loaiQuai == EnemyType.Boss)
                        {
                            laBoss = true;
                        }
                    }

                    if (laBoss && !daPhatCanhBaoBoss)
                    {
                        daPhatCanhBaoBoss = true;
                        float thoiGianDelay = 2f;

                        if (AudioManager.Instance != null)
                        {
                            AudioManager.Instance.TriggerBossWave();
                            thoiGianDelay = AudioManager.Instance.GetBossWarningLength();
                        }
                        StartCoroutine(ThucHienSpawnCoDelay(suKien, thoiGianDelay));
                    }
                    else if (!laBoss)
                    {
                        ThucHienSpawn(suKien);
                    }

                    soTayThoiGian[suKien] = thoiGianDaQua + suKien.thoiGianGiuaCacLan;
                }
            }
        }
    }

    private IEnumerator ThucHienSpawnCoDelay(WaveEvent suKien, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if (dangTrongWave)
        {
            ThucHienSpawn(suKien);
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
        if (dangTrongWave)
        {
            if (EnemyPool.Instance != null)
            {
                GameObject quaiMoi = EnemyPool.Instance.GetEnemy(quaiPrefab);
                quaiMoi.transform.position = viTri;
                quaiMoi.transform.rotation = Quaternion.identity;
            }
            else
            {
                Instantiate(quaiPrefab, viTri, Quaternion.identity);
            }
        }
    }

    private bool TryLayViTriTrongArena(out Vector2 viTriAnToan)
    {
        viTriAnToan = Vector2.zero;
        if (PlayerHealth.Instance == null) return false;

        Vector2 viTriPlayer = PlayerHealth.Instance.transform.position;

        for (int i = 0; i < 20; i++)
        {
            float randomX = UnityEngine.Random.Range(mapMin.x, mapMax.x);
            float randomY = UnityEngine.Random.Range(mapMin.y, mapMax.y);
            Vector2 viTriDuKien = new Vector2(randomX, randomY);

            if ((viTriDuKien - viTriPlayer).sqrMagnitude >= (khoangCachAnToanVoiPlayer * khoangCachAnToanVoiPlayer))
            {
                Collider2D hit = Physics2D.OverlapCircle(viTriDuKien, 0.5f);
                if (hit == null || hit.isTrigger)
                {
                    viTriAnToan = viTriDuKien;
                    return true;
                }
            }
        }
        return false;
    }

    private void KetThucWave()
    {
        dangTrongWave = false;
        bool coRuongPhaDao = false;
        if (waveDataHienTai != null && waveHienTaiIndex >= danhSachWave.Count - 1)
        {
            foreach (var sukien in waveDataHienTai.danhSachSuKien)
            {
                if (sukien.quaiPrefab != null)
                {
                    EnemyHealth mau = sukien.quaiPrefab.GetComponent<EnemyHealth>();
                    if (mau != null && mau.data != null && mau.data.loaiQuai == EnemyType.Boss && mau.data.vatPhamDacBietPrefab != null)
                    {
                        coRuongPhaDao = true;
                        break;
                    }
                }
            }
        }

        OnWaveEnded?.Invoke();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.LuuTienDoWave(waveHienTaiIndex + 1);
        }

        if (waveHienTaiIndex >= danhSachWave.Count - 1)
        {
            Debug.Log("Đã vượt qua Wave cuối cùng!");

            if (!coRuongPhaDao)
            {
                if (StoryDirector.Instance != null)
                {
                    StartCoroutine(DelayChayEnding(2.5f));
                }
            }
            else
            {
                Debug.Log("Đang chờ người chơi nhặt Rương Thành Tựu để End Game...");
            }
        }
    }

    private IEnumerator DelayChayEnding(float delay)
    {
        yield return new WaitForSeconds(delay);

        bool coChayCutscene = false;

        if (StoryDirector.Instance != null)
        {
            coChayCutscene = StoryDirector.Instance.KiemTraVaChayCutscene(waveHienTaiIndex + 2);
        }
        if (!coChayCutscene)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.KetThucGame(true);
            }
        }
    }

    public void ChuyenSangWaveTiepTheo()
    {
        if (!dangTrongWave)
        {
            waveHienTaiIndex++;
            if (StoryDirector.Instance == null || !StoryDirector.Instance.KiemTraVaChayCutscene(waveHienTaiIndex + 1))
            {
                BatDauWave();
            }
        }
    }

    public void CapNhatGioiHanSpawn(Bounds gioiHanKhungCamera)
    {
        float paddingX = 5.5f;
        float paddingY = 6.0f;
        float chieuRongMap = gioiHanKhungCamera.max.x - gioiHanKhungCamera.min.x;
        float chieuCaoMap = gioiHanKhungCamera.max.y - gioiHanKhungCamera.min.y;
        paddingX = Mathf.Min(paddingX, chieuRongMap * 0.35f);
        paddingY = Mathf.Min(paddingY, chieuCaoMap * 0.35f);
        mapMin = new Vector2(gioiHanKhungCamera.min.x + paddingX, gioiHanKhungCamera.min.y + paddingY);
        mapMax = new Vector2(gioiHanKhungCamera.max.x - paddingX, gioiHanKhungCamera.max.y - paddingY);
        Debug.Log($"[WaveManager] Giới hạn Spawn MỚI: Min({mapMin}), Max({mapMax})");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 center = (mapMin + mapMax) / 2f;
        Vector2 size = new Vector2(Mathf.Abs(mapMax.x - mapMin.x), Mathf.Abs(mapMax.y - mapMin.y));
        Gizmos.DrawWireCube(center, size);
    }
}