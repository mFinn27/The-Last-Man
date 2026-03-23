using UnityEngine;
using System.Collections;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;

    public Material flashMaterial;
    private Material materialGoc;
    private Coroutine flashCoroutine;

    void Awake()
    {
        if (sr == null) sr = GetComponentInChildren<SpriteRenderer>();

        if (sr != null) materialGoc = sr.material;
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