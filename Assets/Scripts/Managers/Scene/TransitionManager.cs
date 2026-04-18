using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    [Header("--- THIẾT LẬP UI ---")]
    public Image imgFlash;
    public float thoiGianHieuUng = 0.5f;

    void Awake()
    {
        if (Instance == null) Instance = this;

        if (imgFlash != null)
        {
            imgFlash.raycastTarget = true;
        }
    }

    public IEnumerator FlashIn()
    {
        if (imgFlash == null) yield break;

        imgFlash.raycastTarget = true;
        float t = 0;
        Color c = imgFlash.color;

        while (t < thoiGianHieuUng)
        {
            t += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / thoiGianHieuUng);
            imgFlash.color = c;
            yield return null;
        }
    }

    public IEnumerator FlashOut()
    {
        if (imgFlash == null) yield break;

        float t = 0;
        Color c = imgFlash.color;

        while (t < thoiGianHieuUng)
        {
            t += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / thoiGianHieuUng);
            imgFlash.color = c;
            yield return null;
        }
        imgFlash.raycastTarget = false;
    }
}