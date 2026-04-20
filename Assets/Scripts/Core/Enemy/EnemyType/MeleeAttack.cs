using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public MeleeEnemyData data;
    private Transform player;
    private float thoiGianCanTiepTheo;

    private void OnEnable()
    {
        thoiGianCanTiepTheo = 0f;
        if (PlayerHealth.Instance != null) player = PlayerHealth.Instance.transform;
    }

    void Update()
    {
        if (player == null || data == null) return;

        float khoangCachSqr = (player.position - transform.position).sqrMagnitude;
        if (khoangCachSqr <= data.khoangCachCanChien * data.khoangCachCanChien)
        {
            if (Time.time >= thoiGianCanTiepTheo)
            {
                if (PlayerHealth.Instance != null) PlayerHealth.Instance.TakeDamage(data.dame);
                thoiGianCanTiepTheo = Time.time + data.tgThucHienDonDanhTiepTheo;
            }
        }
    }
}