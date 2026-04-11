using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Animator))]
public class UIAnimationProxy : MonoBehaviour
{
    private Image img;
    private SpriteRenderer sr_chimMoi;

    void Awake()
    {
        img = GetComponent<Image>();
        sr_chimMoi = gameObject.AddComponent<SpriteRenderer>();
        sr_chimMoi.enabled = false;
    }

    void LateUpdate()
    {
        if (sr_chimMoi != null && sr_chimMoi.sprite != null && img != null)
        {
            img.sprite = sr_chimMoi.sprite;
        }
    }
}