using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuFadeEffect : MonoBehaviour
{
    [Header("Thời gian màn hình sáng dần lên (giây)")]
    public float thoiGianSangDan = 1.5f;

    private Image imgNenDen;

    void Start()
    {
        imgNenDen = GetComponent<Image>();
        Color c = imgNenDen.color;
        c.a = 1f;
        imgNenDen.color = c;
        StartCoroutine(HieuUngSangDan());
    }

    private IEnumerator HieuUngSangDan()
    {
        float t = 0f;
        Color c = imgNenDen.color;

        while (t < thoiGianSangDan)
        {
            t += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / thoiGianSangDan);
            imgNenDen.color = c;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}