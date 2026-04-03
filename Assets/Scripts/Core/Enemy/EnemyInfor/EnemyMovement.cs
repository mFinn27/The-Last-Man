using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public EnemyData data;

    [HideInInspector] public bool dangBiDayLui = false;
    [HideInInspector] public bool isCharging = false;

    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (PlayerHealth.Instance != null)
        {
            player = PlayerHealth.Instance.transform;
        }
    }

    void FixedUpdate()
    {
        if (player == null || data == null) return;

        if (dangBiDayLui || isCharging) return;

        Vector2 huong = (player.position - transform.position).normalized;
        rb.linearVelocity = huong * data.tocDoDiChuyen;
    }
}