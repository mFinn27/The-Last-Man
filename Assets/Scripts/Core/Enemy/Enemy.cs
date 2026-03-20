using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Health Stats")]
    public float mauToiDa = 20;
    [SerializeField] private float mauHienTai;

    [Header("Attack Player Stats")]
    public int dameMoiLanCham = 10;
    public float tocDoGayDame = 1f; // 1 = 1s 1 d
    private float thoiGianDonDanhTiepTheo;

    private SpriteRenderer sr;
    private Color mauSac;
    private Rigidbody2D rb;

    void Awake()
    {
        mauHienTai = mauToiDa;
        sr = GetComponent<SpriteRenderer>();
        mauSac = sr.color;
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(float dame, Vector2 huongDayLui, float lucDayLui)
    {
        mauHienTai -= dame;
        Debug.Log($"Enemy {gameObject.name} nhận {dame} sát thương. Máu còn: {mauHienTai}");

        if (rb != null && lucDayLui > 0)
        {
            StartCoroutine(KnockbackRoutine(huongDayLui, lucDayLui));
        }

        StartCoroutine(HitFlash());

        if (mauHienTai <= 0)
        {
            Die();
        }
    }

    private IEnumerator KnockbackRoutine(Vector2 huong, float luc)
    {
        rb.AddForce(huong * luc, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.15f);
        rb.linearVelocity = Vector2.zero;
    }

    private IEnumerator HitFlash()
    {
        sr.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        sr.color = mauSac;
    }

    void Die()
    {
        // them hieu ung enemy chet / roi vang,...
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time >= thoiGianDonDanhTiepTheo)
            {
                if (PlayerHealth.Instance != null)
                {
                    PlayerHealth.Instance.TakeDamage(dameMoiLanCham);
                }

                thoiGianDonDanhTiepTheo = Time.time + tocDoGayDame;
            }
        }
    }
}