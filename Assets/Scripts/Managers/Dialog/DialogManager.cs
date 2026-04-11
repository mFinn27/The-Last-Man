using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;

[System.Serializable]
public class DialogLine
{
    public string tenNhanVat;
    public Sprite hinhAnhDaiDien;
    [TextArea(3, 5)]
    public string noiDung;
}

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    [Header("--- KẾT NỐI UI ---")]
    public GameObject panelDialog;
    public Image imgAvatar;
    public TextMeshProUGUI txtTenNhanVat;
    public TextMeshProUGUI txtNoiDung;

    [Header("--- CÀI ĐẶT ---")]
    public float tocDoChu = 0.03f;

    private Queue<DialogLine> hangDoiDialog = new Queue<DialogLine>();
    private bool dangChayChu = false;
    private string cauThoaiHienTai = "";

    public event Action OnDialogEnded;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void BatDauHoiThoai(DialogLine[] kịchBản)
    {
        panelDialog.SetActive(true);
        hangDoiDialog.Clear();

        foreach (DialogLine line in kịchBản)
        {
            hangDoiDialog.Enqueue(line);
        }

        HienThiCauTiepTheo();
    }

    public void BamKhungDialog()
    {
        if (Time.timeScale == 0f) return;

        if (AudioManager.Instance != null) AudioManager.Instance.PlayClickSFX();

        if (dangChayChu)
        {
            StopAllCoroutines();
            txtNoiDung.text = cauThoaiHienTai;
            dangChayChu = false;
        }
        else
        {
            HienThiCauTiepTheo();
        }
    }

    private void HienThiCauTiepTheo()
    {
        if (hangDoiDialog.Count == 0)
        {
            KetThucHoiThoai();
            return;
        }

        DialogLine line = hangDoiDialog.Dequeue();
        txtTenNhanVat.text = line.tenNhanVat;
        cauThoaiHienTai = line.noiDung;

        if (imgAvatar != null)
        {
            if (line.hinhAnhDaiDien != null)
            {
                imgAvatar.sprite = line.hinhAnhDaiDien;
                imgAvatar.gameObject.SetActive(true);
            }
            else
            {
                imgAvatar.gameObject.SetActive(false);
            }
        }

        StopAllCoroutines();
        StartCoroutine(ChayChuTypewriter(line.noiDung));
    }

    private IEnumerator ChayChuTypewriter(string noiDung)
    {
        dangChayChu = true;
        txtNoiDung.text = "";

        foreach (char c in noiDung.ToCharArray())
        {
            txtNoiDung.text += c;
            yield return new WaitForSeconds(tocDoChu);
        }

        dangChayChu = false;
    }

    private void KetThucHoiThoai()
    {
        panelDialog.SetActive(false);
        OnDialogEnded?.Invoke();
    }
}