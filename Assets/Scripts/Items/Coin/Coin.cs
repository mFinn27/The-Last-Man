using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    private int giaTri;
    private bool dangBiHut = false;
    private bool biGiamGiaTri = false;
    private Transform playerTransform;

    [SerializeField] private float tocDoHut = 5f;

    private void OnEnable()
    {
        WaveManager.OnWaveEnded += XuLyHutCuoiWave;
        dangBiHut = false;
        biGiamGiaTri = false;
    }

    private void OnDisable()
    {
        WaveManager.OnWaveEnded -= XuLyHutCuoiWave;
    }

    private void Start()
    {
        if (PlayerHealth.Instance != null)
            playerTransform = PlayerHealth.Instance.transform;
    }

    public void Setup(int giaTriNapVao)
    {
        giaTri = giaTriNapVao;
        biGiamGiaTri = false;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = true;
        dangBiHut = false;
    }

    void Update()
    {
        if (!dangBiHut && playerTransform != null && PlayerStats.Instance != null)
        {
            float khoangCachBinhPhuong = (transform.position - playerTransform.position).sqrMagnitude;
            float phamViHut = PlayerStats.Instance.GetMagnetRange();

            if (khoangCachBinhPhuong <= phamViHut * phamViHut)
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
        int giaTriThucTe = biGiamGiaTri ? Mathf.Max(1, giaTri / 2) : giaTri;

        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.AddCoin(giaTriThucTe);
        }

        if (AudioManager.Instance != null) AudioManager.Instance.PlayCoinSFX();
        gameObject.SetActive(false);
    }

    private void XuLyHutCuoiWave()
    {
        if (gameObject.activeInHierarchy && !dangBiHut)
        {
            biGiamGiaTri = true;
            KichHoatHutVaoNguoi();
        }
    }

    public void KichHoatHutVaoNguoi()
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