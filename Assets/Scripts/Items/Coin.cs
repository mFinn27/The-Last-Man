using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    private int giaTri;
    private void OnEnable()
    {
        WaveManager.OnWaveEnded += KichHoatHutVaoNguoi;
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
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
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(HutRoutine());
        }
    }

    private IEnumerator HutRoutine()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        if (PlayerHealth.Instance == null) yield break;

        Transform player = PlayerHealth.Instance.transform;
        float tocDoHut = 5f;

        while (Vector2.Distance(transform.position, player.position) > 0.5f)
        {
            tocDoHut += Time.deltaTime * 30f;
            transform.position = Vector3.MoveTowards(transform.position, player.position, tocDoHut * Time.deltaTime);
            yield return null;
        }

        NhatCoin();
    }
}