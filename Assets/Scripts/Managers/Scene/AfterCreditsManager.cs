using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class AfterCreditsManager : MonoBehaviour
{
    [Header("--- HIỆU ỨNG ---")]
    public float thoiGianFadeIn = 1.5f;
    private CanvasGroup canvasGroup;

    [Header("--- CẤU HÌNH CUỘN ---")]
    public RectTransform creditsContent;
    public float tocDoCuon = 100f;
    public float thoiGianDungCuoi = 1f;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void BatDauChayCredits()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 0f;
        if (creditsContent != null) creditsContent.gameObject.SetActive(false);
        StartCoroutine(ChayCreditsRoutine());
    }

    private IEnumerator ChayCreditsRoutine()
    {
        float t = 0f;
        while (t < thoiGianFadeIn)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / thoiGianFadeIn);
            yield return null;
        }
        canvasGroup.alpha = 1f;
        creditsContent.gameObject.SetActive(true);
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(creditsContent);
        float chieuCaoManHinhUI = creditsContent.parent.GetComponent<RectTransform>().rect.height;

        creditsContent.anchoredPosition = new Vector2(0, -chieuCaoManHinhUI);
        float diemDung = creditsContent.sizeDelta.y + 50f;

        while (creditsContent.anchoredPosition.y < diemDung)
        {
            creditsContent.anchoredPosition += new Vector2(0, tocDoCuon * Time.unscaledDeltaTime);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(thoiGianDungCuoi);
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}