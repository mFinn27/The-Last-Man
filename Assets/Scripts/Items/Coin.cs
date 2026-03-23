using UnityEngine;

public class Coin : MonoBehaviour
{
    private int giaTri;
    private Transform player;
    private bool dangBiHut = false;

    [Header("Setup")]
    public float tocDoHutBanDau = 5f;
    private float tocDoHutHienTai;

    void Start()
    {
        if (PlayerHealth.Instance != null)
        {
            player = PlayerHealth.Instance.transform;
        }
    }

    public void Setup(int value)
    {
        giaTri = value;
        dangBiHut = false;
        tocDoHutHienTai = tocDoHutBanDau;
    }

    void Update()
    {
        if (player == null) return;

        if (!dangBiHut)
        {
            float khoangCachSqr = (player.position - transform.position).sqrMagnitude;
            float tamHut = PlayerStats.Instance != null ? PlayerStats.Instance.GetMagnetRange() : 3f;

            if (khoangCachSqr <= tamHut * tamHut)
            {
                dangBiHut = true;
            }
        }
        else
        {
            tocDoHutHienTai += Time.deltaTime * 15f;
            transform.position = Vector3.MoveTowards(transform.position, player.position, tocDoHutHienTai * Time.deltaTime);

            float khoangCachSqr = (player.position - transform.position).sqrMagnitude;
            if (khoangCachSqr < 0.05f)
            {
                if (PlayerStats.Instance != null)
                {
                    PlayerStats.Instance.AddCoin(giaTri);
                }

                CoinPool.Instance.ReturnCoin(gameObject);
            }
        }
    }
}