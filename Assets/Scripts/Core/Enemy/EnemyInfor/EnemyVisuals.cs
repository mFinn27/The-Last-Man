using UnityEngine;
using System.Collections;

public class EnemyVisuals : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private EnemyMovement movement;

    private Color mauSacGoc;
    private Vector3 kichThuocGoc;
    private Coroutine flashCoroutine;

    private Material materialGoc;
    public Material flashMaterial;

    void Awake()
    {
        KiemTraKhoiTao();
    }

    private void OnEnable()
    {
        KiemTraKhoiTao();
        if (sr != null)
        {
            sr.material = materialGoc;
            sr.color = mauSacGoc;
            sr.transform.localScale = kichThuocGoc;
        }
    }

    private void KiemTraKhoiTao()
    {
        if (sr != null) return;

        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<EnemyMovement>();

        if (sr != null)
        {
            mauSacGoc = sr.color;
            kichThuocGoc = sr.transform.localScale;
            materialGoc = sr.material;
        }
    }

    void Update()
    {
        if (rb.linearVelocity.x != 0) sr.flipX = rb.linearVelocity.x < 0;

        bool dangGong = movement != null && movement.isCharging;

        if (rb.linearVelocity.magnitude > 0.1f && !dangGong)
        {
            float nhapNho = Mathf.Sin(Time.time * 15f) * 0.08f;
            sr.transform.localScale = new Vector3(kichThuocGoc.x - (nhapNho / 2f), kichThuocGoc.y + nhapNho, kichThuocGoc.z);
        }
        else if (!dangGong)
        {
            sr.transform.localScale = kichThuocGoc;
        }
    }

    public void PlayFlashWhite()
    {
        if (flashCoroutine != null) StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashWhiteRoutine());
    }

    private IEnumerator FlashWhiteRoutine()
    {
        if (sr == null) yield break;
        sr.material = flashMaterial;
        yield return new WaitForSeconds(0.05f);

        if (sr != null) sr.material = materialGoc;
    }

    public IEnumerator GongDonRoutine(float thoiGianGong)
    {
        float thoiGianDaQua = 0f;
        Vector3 scaleBanDau = kichThuocGoc;
        Vector3 scaleEpXuong = new Vector3(kichThuocGoc.x * 1.3f, kichThuocGoc.y * 0.7f, kichThuocGoc.z);

        while (thoiGianDaQua < thoiGianGong)
        {
            thoiGianDaQua += Time.deltaTime;
            float phanTram = thoiGianDaQua / thoiGianGong;
            if (sr != null)
            {
                sr.color = Color.Lerp(mauSacGoc, Color.red, phanTram);
                sr.transform.localScale = Vector3.Lerp(scaleBanDau, scaleEpXuong, phanTram);
            }
            yield return null;
        }
    }

    public IEnumerator NayLenSauKhiBanRoutine()
    {
        KiemTraKhoiTao();
        if (sr == null) yield break;
        sr.color = mauSacGoc;
        sr.transform.localScale = new Vector3(kichThuocGoc.x * 0.8f, kichThuocGoc.y * 1.2f, kichThuocGoc.z);
        yield return new WaitForSeconds(0.1f);
        if (sr != null) sr.transform.localScale = kichThuocGoc;
    }
}