using UnityEngine;
using System.Collections;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    private Animator anim;

    public Material flashMaterial;
    private Material materialGoc;
    private Coroutine flashCoroutine;

    void Awake()
    {
        if (sr == null) sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();

        if (sr != null) materialGoc = sr.material;
    }

    void Start()
    {
        CapNhatHinhAnhVaAnimation();
    }

    public void CapNhatHinhAnhVaAnimation()
    {
        if (sr == null) sr = GetComponentInChildren<SpriteRenderer>();
        if (anim == null) anim = GetComponentInChildren<Animator>();

        if (PlayerStats.Instance != null && PlayerStats.Instance.dataNhanVat != null)
        {
            if (PlayerStats.Instance.dataNhanVat.hinhAnhNhanVat != null)
            {
                sr.sprite = PlayerStats.Instance.dataNhanVat.hinhAnhNhanVat;
            }
            if (PlayerStats.Instance.dataNhanVat.animatorNhanVat != null && anim != null)
            {
                anim.runtimeAnimatorController = PlayerStats.Instance.dataNhanVat.animatorNhanVat;
            }
        }
    }

    public void PlayFlashWhite()
    {
        if (sr == null || flashMaterial == null) return;

        if (flashCoroutine != null) StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashWhiteRoutine());
    }

    private IEnumerator FlashWhiteRoutine()
    {
        sr.material = flashMaterial;
        yield return new WaitForSeconds(0.1f);
        sr.material = materialGoc;
    }
}