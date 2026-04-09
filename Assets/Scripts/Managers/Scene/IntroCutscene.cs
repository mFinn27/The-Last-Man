using UnityEngine;
using System.Collections;

public class IntroCutscene : MonoBehaviour
{
    [Header("--- KỊCH BẢN ---")]
    public DialogLine[] kichBanMoDau;

    [Header("--- ĐỐI TƯỢNG ---")]
    public Transform viTriDichDen;
    public GameObject npcObject;

    private bool daXemCutscene = false;

    void Start()
    {
        if (WeaponManager.Instance != null && WeaponManager.Instance.weaponPivot != null)
        {
            WeaponManager.Instance.weaponPivot.gameObject.SetActive(false);
        }

        if (!daXemCutscene)
        {
            StartCoroutine(ChayKichBanRoutine());
        }
        else
        {
            KichBanXong();
        }
    }

    private IEnumerator ChayKichBanRoutine()
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

        if (player != null)
        {
            player.transform.position = new Vector2(0f, -10f);

            if (anim != null)
            {
                anim.SetBool("IsMoving", true);
                anim.SetFloat("MoveX", 0);
                anim.SetFloat("MoveY", 1);
            }

            float speed = 3f;
            while (Vector2.Distance(player.transform.position, viTriDichDen.position) > 0.1f)
            {
                player.transform.position = Vector2.MoveTowards(player.transform.position, viTriDichDen.position, speed * Time.deltaTime);
                yield return null;
            }

            if (anim != null) anim.SetBool("IsMoving", false);
        }
        yield return new WaitForSeconds(0.5f);
        DialogManager.Instance.OnDialogEnded += KichBanXong;
        DialogManager.Instance.BatDauHoiThoai(kichBanMoDau);
    }

    private void KichBanXong()
    {
        DialogManager.Instance.OnDialogEnded -= KichBanXong;

        if (PlayerHealth.Instance != null)
        {
            PlayerMovement scriptDiChuyen = PlayerHealth.Instance.GetComponent<PlayerMovement>();
            if (scriptDiChuyen != null) scriptDiChuyen.enabled = true;
        }
        if (npcObject != null) Destroy(npcObject);
        daXemCutscene = true;

        if (WeaponManager.Instance != null && WeaponManager.Instance.weaponPivot != null)
        {
            WeaponManager.Instance.weaponPivot.gameObject.SetActive(true);
        }
        if (WaveManager.Instance != null) WaveManager.Instance.BatDauWave();
    }
}