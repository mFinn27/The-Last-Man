using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    private int giaTri;
    private bool dangBiHut = false;
    [SerializeField] private float tocDoHut = 5f;

    private void OnEnable()
    {
        WaveManager.OnWaveEnded += KichHoatHutVaoNguoi;
        dangBiHut = false;
    }

    private void OnDisable()
    {
        WaveManager.OnWaveEnded -= KichHoatHutVaoNguoi;
    }

    public void Setup(int giaTriNapVao)
    {
        giaTri = giaTriNapVao;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;
        dangBiHut = false;
    }

    void Update()
    {
        if (!dangBiHut && PlayerHealth.Instance != null && PlayerStats.Instance != null)
        {
            float khoangCach = Vector2.Distance(transform.position, PlayerHealth.Instance.transform.position);
            if (khoangCach <= PlayerStats.Instance.GetMagnetRange())
            {
                KichHoatHutVaoNguoi();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !dangBiHut)
        {
            NhatCoin();
        }
    }

    private void NhatCoin()
    {
        if (PlayerStats.Instance != null) PlayerStats.Instance.AddCoin(giaTri);
        gameObject.SetActive(false);
    }

    private void KichHoatHutVaoNguoi()
    {
        if (gameObject.activeInHierarchy && !dangBiHut)
        {
            dangBiHut = true;
            StartCoroutine(HutRoutine());
        }
    }

    private IEnumerator HutRoutine()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        if (PlayerHealth.Instance == null) yield break;

        Transform player = PlayerHealth.Instance.transform;

        while (Vector2.Distance(transform.position, player.position) > 0.5f)
        {
            tocDoHut += Time.unscaledDeltaTime * 40f;
            transform.position = Vector3.MoveTowards(transform.position, player.position, tocDoHut * Time.unscaledDeltaTime);
            yield return null;
        }

        NhatCoin();
    }
}