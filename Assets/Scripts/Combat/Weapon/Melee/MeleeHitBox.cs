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
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy == null || data == null) return;

            bool chiMang;
            float dameCuoiCung = DamageCalculator.CalculateDamage(data.dame, data.tiLeChiMang, data.satThuongChiMang, out chiMang);

            Vector2 huongDayLui = (collision.transform.position - transform.root.position).normalized;
            enemy.TakeDamage(dameCuoiCung, huongDayLui, data.dayLui);

            FloatingTextManager.Instance.SpawnText(collision.transform.position, dameCuoiCung, chiMang);

            float hutMauThucTe = DamageCalculator.CalculateLifeSteal(data.hutMau);
            if (hutMauThucTe > 0)
            {
                int hoiMau = Mathf.RoundToInt(dameCuoiCung * hutMauThucTe);
                if (hoiMau > 0) PlayerHealth.Instance.Heal(hoiMau);
            }
        }
    }
}