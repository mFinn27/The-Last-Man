using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class StoryEvent
{
    [Tooltip("Mã ID để lưu lịch sử (Ví dụ: Wave1_MaVuong, Wave6_CuuThoRen)")]
    public string idSuKien = "Cutscene_WaveX";

    [Tooltip("Cutscene này xuất hiện TRƯỚC Wave mấy? (VD: 1 = Đầu game, 6 = Trước khi bắt đầu Wave 6)")]
    public int truocWaveSo = 1;

    [Tooltip("NPC sẽ xuất hiện để nói chuyện")]
    public GameObject npcPrefab;

    [Header("Lời Thoại")]
    public DialogLine[] kichBan;
}

public class StoryDirector : MonoBehaviour
{
    public static StoryDirector Instance;

    [Header("--- DEBUG (DÀNH CHO DEV TEST) ---")]
    [Tooltip("Bật cái này lên để luôn luôn chiếu Cutscene, bất chấp đã xem hay chưa")]
    public bool testMode = true;

    [Header("--- DANH SÁCH CÁC CÂU CHUYỆN ---")]
    public List<StoryEvent> danhSachCotTruyen;

    private GameObject npcHienTai;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        if (!KiemTraVaChayCutscene(1))
        {
            if (WaveManager.Instance != null) WaveManager.Instance.BatDauWave();
        }
    }

    public bool KiemTraVaChayCutscene(int waveChuanBiChay)
    {
        StoryEvent suKien = danhSachCotTruyen.Find(x => x.truocWaveSo == waveChuanBiChay);

        if (suKien != null)
        {
            bool daXem = PlayerPrefs.GetInt(suKien.idSuKien, 0) == 1;

            if (testMode || !daXem)
            {
                PlayerPrefs.SetInt(suKien.idSuKien, 1);
                PlayerPrefs.Save();
                StartCoroutine(ChayKichBanRoutine(suKien, waveChuanBiChay == 1));
                return true;
            }
        }
        return false;
    }

    private IEnumerator ChayKichBanRoutine(StoryEvent suKien, bool laDauGame)
    {
        PlayerMovement scriptDiChuyen = null;
        GameObject player = null;
        Animator anim = null;

        if (PlayerHealth.Instance != null)
        {
            player = PlayerHealth.Instance.gameObject;
            scriptDiChuyen = player.GetComponent<PlayerMovement>();
            anim = player.GetComponentInChildren<Animator>();
            if (scriptDiChuyen != null) scriptDiChuyen.enabled = false;
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

            if (anim != null)
            {
                anim.SetBool("IsMoving", true);
                Vector2 huongNhin = (viTriPlayerDichDen - (Vector2)player.transform.position).normalized;
                anim.SetFloat("MoveX", huongNhin.x);
                anim.SetFloat("MoveY", huongNhin.y);
            }

            float speed = 4f;
            while (Vector2.Distance(player.transform.position, viTriPlayerDichDen) > 0.1f)
            {
                player.transform.position = Vector2.MoveTowards(player.transform.position, viTriPlayerDichDen, speed * Time.deltaTime);
                yield return null;
            }

            if (anim != null) anim.SetBool("IsMoving", false);
        }
        yield return new WaitForSeconds(0.5f);
        DialogManager.Instance.OnDialogEnded += KichBanXong;
        DialogManager.Instance.BatDauHoiThoai(suKien.kichBan);
    }

    private void KichBanXong()
    {
        DialogManager.Instance.OnDialogEnded -= KichBanXong;

        if (PlayerHealth.Instance != null)
        {
            PlayerMovement scriptDiChuyen = PlayerHealth.Instance.GetComponent<PlayerMovement>();
            if (scriptDiChuyen != null) scriptDiChuyen.enabled = true;
        }

        if (npcHienTai != null) Destroy(npcHienTai);

        if (WeaponManager.Instance != null && WeaponManager.Instance.weaponPivot != null)
            WeaponManager.Instance.weaponPivot.gameObject.SetActive(true);

        if (WaveManager.Instance != null) WaveManager.Instance.BatDauWave();
    }

    [ContextMenu("Xóa Lịch Sử Cutscene (Reset)")]
    public void ResetToanBoLichSuCutscene()
    {
        foreach (var suKien in danhSachCotTruyen)
        {
            PlayerPrefs.DeleteKey(suKien.idSuKien);
        }
        PlayerPrefs.Save();
        Debug.Log("<color=green>Đã xóa toàn bộ lịch sử Cutscene! Lần tới chơi sẽ chiếu lại từ đầu.</color>");
    }
}