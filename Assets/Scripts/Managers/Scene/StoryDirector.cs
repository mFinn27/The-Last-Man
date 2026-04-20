using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class StoryEvent
{
    public string idSuKien = "Cutscene_WaveX";
    public string idNhanVatBoQua;
    public int truocWaveSo = 1;
    public string tenMapChuyenToi;
    public GameObject npcPrefab;
    public DialogLine[] kichBan;

    [Header("--- KẾT THÚC GAME (ENDING) ---")]
    public bool laCutscenePhaDao = false;
    public GameObject giaDinhPrefab;
    public DialogLine[] kichBanGiaDinh;
}

public class StoryDirector : MonoBehaviour
{
    public static StoryDirector Instance;
    public static event Action OnAfterCreditsTriggered;

    [Header("--- DEBUG (DÀNH CHO DEV TEST) ---")]
    public bool testMode = false;

    [Header("--- NHÂN VẬT MẶC ĐỊNH CHO CỐT TRUYỆN ---")]
    public CharacterData nhanVatMacDinhTrongCutscene;

    [Header("--- DANH SÁCH CÁC CÂU CHUYỆN ---")]
    public List<StoryEvent> danhSachCotTruyen;

    private GameObject npcHienTai;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        int waveChuanBiChay = 1;
        int trangThaiDangDo = 0;

        if (GameManager.Instance != null && GameManager.Instance.isLoadingSave)
        {
            waveChuanBiChay = GameManager.Instance.currentSave.waveHienTai + 1;
            trangThaiDangDo = GameManager.Instance.currentSave.trangThaiGiaiDoan;
            if (WaveManager.Instance != null) WaveManager.Instance.SetWaveIndex(GameManager.Instance.currentSave.waveHienTai);
        }
        if (trangThaiDangDo == 1)
        {
            if (RewardPopupUI.Instance != null) RewardPopupUI.Instance.KichHoatHienThiNgayLapTuc();
            return;
        }
        else if (trangThaiDangDo == 2)
        {
            if (ShopUI.Instance != null) ShopUI.Instance.MoCuaHang();
            return;
        }

        if (!KiemTraVaChayCutscene(waveChuanBiChay))
        {
            if (WaveManager.Instance != null) WaveManager.Instance.BatDauWave();
        }
    }

    public bool KiemTraVaChayCutscene(int waveChuanBiChay)
    {
        StoryEvent suKien = danhSachCotTruyen.Find(x => x.truocWaveSo == waveChuanBiChay);

        if (suKien != null)
        {
            bool dangChoiChinhNPCNay = false;
            if (GameManager.Instance != null && GameManager.Instance.characterDangChon != null)
            {
                if (!string.IsNullOrEmpty(suKien.idNhanVatBoQua) &&
                    GameManager.Instance.characterDangChon.idNhanVat == suKien.idNhanVatBoQua)
                {
                    dangChoiChinhNPCNay = true;
                }
            }

            bool daXem = PlayerPrefs.GetInt(suKien.idSuKien, 0) == 1;
            if (testMode || (!daXem && !dangChoiChinhNPCNay))
            {
                StartCoroutine(ChayKichBanRoutine(suKien, waveChuanBiChay == 1));
                return true;
            }
            else
            {
                if (!string.IsNullOrEmpty(suKien.tenMapChuyenToi))
                {
                    StartCoroutine(ChuyenMapKhongHoiThoai(suKien));
                    return true;
                }
            }
        }
        return false;
    }

    public void KichHoatEndingTuRuong()
    {
        StoryEvent suKienEnding = danhSachCotTruyen.Find(x => x.laCutscenePhaDao);

        if (suKienEnding != null)
        {
            bool daXem = PlayerPrefs.GetInt(suKienEnding.idSuKien, 0) == 1;

            if (testMode || !daXem)
            {
                StartCoroutine(ChayKichBanRoutine(suKienEnding, false));
            }
            else
            {
                Debug.Log("đã xem Ending trước đó. Hiển thị bảng Chiến Thắng!");
                if (GameManager.Instance != null) GameManager.Instance.KetThucGame(true);
            }
        }
        else
        {
            Debug.LogWarning("Không tìm thấy Cutscene Phá Đảo nào được đánh dấu. End Game.");
            if (GameManager.Instance != null) GameManager.Instance.KetThucGame(true);
        }
    }

    private IEnumerator ChuyenMapKhongHoiThoai(StoryEvent suKien)
    {
        Time.timeScale = 0f;
        yield return StartCoroutine(MapManager.Instance.BatDauChuyenMap(suKien.tenMapChuyenToi, null));
        Time.timeScale = 1f;
        if (WaveManager.Instance != null) WaveManager.Instance.BatDauWave();
    }

    private IEnumerator ChayKichBanRoutine(StoryEvent suKien, bool laDauGame)
    {
        PlayerMovement scriptDiChuyen = null;
        GameObject player = null;
        Animator anim = null;
        PlayerVisuals playerVisuals = null;

        if (PlayerHealth.Instance != null)
        {
            player = PlayerHealth.Instance.gameObject;
            scriptDiChuyen = player.GetComponent<PlayerMovement>();
            anim = player.GetComponentInChildren<Animator>();
            playerVisuals = player.GetComponent<PlayerVisuals>();

            if (playerVisuals != null && nhanVatMacDinhTrongCutscene != null)
            {
                playerVisuals.SetVisualsTemporary(nhanVatMacDinhTrongCutscene);
            }

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }

        if (WeaponManager.Instance != null && WeaponManager.Instance.weaponPivot != null)
            WeaponManager.Instance.weaponPivot.gameObject.SetActive(false);

        Vector2 viTriNPC = Vector2.zero;
        Vector2 viTriPlayerDichDen = new Vector2(0, -1.5f);

        if (suKien.npcPrefab != null)
            npcHienTai = Instantiate(suKien.npcPrefab, viTriNPC, Quaternion.identity);

        if (player != null)
        {
            if (laDauGame) player.transform.position = new Vector2(0f, -10f);

            float speed = 4f;

            if (scriptDiChuyen != null)
            {
                while (Vector2.Distance(player.transform.position, viTriPlayerDichDen) > 0.1f)
                {
                    Vector2 huong = (viTriPlayerDichDen - (Vector2)player.transform.position).normalized;
                    scriptDiChuyen.ForceMoveTo(huong * speed);
                    yield return null;
                }
                scriptDiChuyen.ForceMoveTo(Vector2.zero);
            }
            else
            {
                while (Vector2.Distance(player.transform.position, viTriPlayerDichDen) > 0.1f)
                {
                    player.transform.position = Vector2.MoveTowards(player.transform.position, viTriPlayerDichDen, speed * Time.deltaTime);
                    yield return null;
                }
            }
            if (anim != null)
            {
                anim.SetBool("IsMoving", false);
                Vector2 huongNhinNPC = (viTriNPC - (Vector2)player.transform.position).normalized;
                anim.SetFloat("MoveX", huongNhinNPC.x);
                anim.SetFloat("MoveY", huongNhinNPC.y);
            }
        }
        yield return new WaitForSeconds(0.5f);

        if (suKien.kichBan != null && suKien.kichBan.Length > 0)
        {
            bool dialog1Xong = false;
            Action onDone1 = () => dialog1Xong = true;
            DialogManager.Instance.OnDialogEnded += onDone1;
            DialogManager.Instance.BatDauHoiThoai(suKien.kichBan);
            yield return new WaitUntil(() => dialog1Xong);
            DialogManager.Instance.OnDialogEnded -= onDone1;
        }

        if (!string.IsNullOrEmpty(suKien.tenMapChuyenToi))
        {
            Time.timeScale = 0f;
            yield return StartCoroutine(MapManager.Instance.BatDauChuyenMap(suKien.tenMapChuyenToi, () =>
            {
                if (npcHienTai != null)
                {
                    Destroy(npcHienTai);
                    npcHienTai = null;
                }
            }));
            Time.timeScale = 1f;
        }

        if (suKien.laCutscenePhaDao)
        {
            if (npcHienTai != null) Destroy(npcHienTai);

            Vector2 viTriGiaDinh = new Vector2(5f, 0f);
            GameObject giaDinhHienTai = null;
            if (suKien.giaDinhPrefab != null) giaDinhHienTai = Instantiate(suKien.giaDinhPrefab, viTriGiaDinh, Quaternion.identity);

            if (player != null)
            {
                Vector2 diemDungChan = viTriGiaDinh + new Vector2(-1.5f, 0);
                float speed = 5f;

                if (scriptDiChuyen != null)
                {
                    while (Vector2.Distance(player.transform.position, diemDungChan) > 0.1f)
                    {
                        Vector2 huong = (diemDungChan - (Vector2)player.transform.position).normalized;
                        scriptDiChuyen.ForceMoveTo(huong * speed);
                        yield return null;
                    }
                    scriptDiChuyen.ForceMoveTo(Vector2.zero);
                }
                else
                {
                    while (Vector2.Distance(player.transform.position, diemDungChan) > 0.1f)
                    {
                        player.transform.position = Vector2.MoveTowards(player.transform.position, diemDungChan, speed * Time.deltaTime);
                        yield return null;
                    }
                }

                if (anim != null)
                {
                    anim.SetBool("IsMoving", false);
                    Vector2 huongNhinGiaDinh = (viTriGiaDinh - (Vector2)player.transform.position).normalized;
                    anim.SetFloat("MoveX", huongNhinGiaDinh.x);
                    anim.SetFloat("MoveY", huongNhinGiaDinh.y);
                }
            }

            bool dialog2Xong = false;
            Action onDone2 = () => dialog2Xong = true;
            DialogManager.Instance.OnDialogEnded += onDone2;
            DialogManager.Instance.BatDauHoiThoai(suKien.kichBanGiaDinh);
            yield return new WaitUntil(() => dialog2Xong);
            DialogManager.Instance.OnDialogEnded -= onDone2;
            PlayerPrefs.SetInt(suKien.idSuKien, 1);
            PlayerPrefs.Save();

            if (playerVisuals != null) playerVisuals.CapNhatHinhAnhVaAnimation();
            OnAfterCreditsTriggered?.Invoke();
        }
        else
        {
            if (npcHienTai != null) Destroy(npcHienTai);
            KhoiPhucDieuKhienPlayer();
            PlayerPrefs.SetInt(suKien.idSuKien, 1);
            PlayerPrefs.Save();

            if (WaveManager.Instance != null) WaveManager.Instance.BatDauWave();
        }
    }

    private void KhoiPhucDieuKhienPlayer()
    {
        if (WeaponManager.Instance != null && WeaponManager.Instance.weaponPivot != null)
            WeaponManager.Instance.weaponPivot.gameObject.SetActive(true);

        if (PlayerHealth.Instance != null)
        {
            PlayerVisuals pv = PlayerHealth.Instance.GetComponent<PlayerVisuals>();
            if (pv != null) pv.CapNhatHinhAnhVaAnimation();

            PlayerMovement pm = PlayerHealth.Instance.GetComponent<PlayerMovement>();
            if (pm != null) pm.StopForceMove();
        }
    }
}