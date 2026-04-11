using UnityEngine;

public class MeleeHitBox : MonoBehaviour
{
    private WeaponData data;

    public void Setup(WeaponData dataVuKhi)
    {
        data = dataVuKhi;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth mauEnemy = collision.GetComponent<EnemyHealth>();
            if (mauEnemy == null || data == null) return;

            bool chiMang;
            float dameCuoiCung = DamageCalculator.CalculateDamage(data.dame, data.tiLeChiMang, data.satThuongChiMang, out chiMang);
            Vector2 huongDayLui = (collision.transform.position - transform.root.position).normalized;

            float dayLuiThuc = data.dayLui + (PlayerStats.Instance != null ? PlayerStats.Instance.GetBonusDayLui() : 0f);
            mauEnemy.TakeDamage(dameCuoiCung, huongDayLui, dayLuiThuc);

            FloatingTextManager.Instance.SpawnText(collision.transform.position, dameCuoiCung, chiMang);
            float hutMauThucTe = DamageCalculator.CalculateLifeSteal(data.hutMau);
            if (hutMauThucTe > 0)
            {
                float luongHoiTiemNang = dameCuoiCung * hutMauThucTe;
                if (luongHoiTiemNang > 0 && PlayerHealth.Instance != null)
                {
                    PlayerHealth.Instance.GhiNhanHutMau(luongHoiTiemNang);
                }
            }
        }
    }
}