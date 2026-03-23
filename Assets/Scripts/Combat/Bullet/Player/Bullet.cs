using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 huongDiChuyen;
    private float tocDoDiChuyen;
    private float damage;
    private int xuyenThauHienTai;
    private float lucDayLui;
    private float tiLeChiMang;
    private float satThuongChiMang;
    private float hutMau;
    private float thoiGianDanTonTai;

    public void Setup(Vector2 huong, float tocDo, float dame, int xuyenThau, float dayLui, float tlChiMang, float stChiMang, float hutMau)
    {
        huongDiChuyen = huong;
        tocDoDiChuyen = tocDo;
        damage = dame;
        xuyenThauHienTai = xuyenThau;
        lucDayLui = dayLui;
        tiLeChiMang = tlChiMang;
        satThuongChiMang = stChiMang;
        this.hutMau = hutMau;
        thoiGianDanTonTai = 3f;
    }

    void Update()
    {
        transform.Translate(Vector3.right * tocDoDiChuyen * Time.deltaTime);
        thoiGianDanTonTai -= Time.deltaTime;

        if (thoiGianDanTonTai <= 0)
            VoHieuHoa();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();

            if (enemyHealth == null) return;

            bool chiMang;
            float dameCuoiCung = DamageCalculator.CalculateDamage(damage, tiLeChiMang, satThuongChiMang, out chiMang);

            Vector2 huongDayLui = (collision.transform.position - transform.position).normalized;
            enemyHealth.TakeDamage(dameCuoiCung, huongDayLui, lucDayLui);

            FloatingTextManager.Instance.SpawnText(collision.transform.position, dameCuoiCung, chiMang);

            float hutMauThucTe = DamageCalculator.CalculateLifeSteal(hutMau);
            if (hutMauThucTe > 0)
            {
                int hoiMau = Mathf.RoundToInt(dameCuoiCung * hutMauThucTe);
                if (hoiMau > 0) PlayerHealth.Instance.Heal(hoiMau);
            }
            xuyenThauHienTai--;

            if (xuyenThauHienTai <= 0)
                VoHieuHoa();
        }
    }

    private void VoHieuHoa()
    {
        BulletPool.Instance.ReturnBullet(gameObject);
    }
}