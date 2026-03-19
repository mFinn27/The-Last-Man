using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 huongDiChuyen;
    private float tocDoDiChuyen;

    private int damage;
    private int xuyenThauHienTai;
    private float lucDayLui;

    private float tiLeChiMang;
    private float satThuongChiMang;
    private float hutMau;

    private float thoiGianDanTonTai;

    public void Setup(
        Vector2 huong,
        float tocDo,
        int dmg,
        int xuyenThau,
        float dayLui,
        float tlChiMang,
        float stChiMang,
        float GThutMau)
    {
        huongDiChuyen = huong;
        tocDoDiChuyen = tocDo;

        damage = dmg;
        xuyenThauHienTai = xuyenThau;
        lucDayLui = dayLui;

        tiLeChiMang = tlChiMang;
        satThuongChiMang = stChiMang;
        hutMau = GThutMau;

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
            int dameCuoiCung = damage;

            bool chiMang = Random.value <= tiLeChiMang;

            if (chiMang)
                dameCuoiCung = Mathf.RoundToInt(damage * satThuongChiMang);

            Debug.Log("Damage: " + dameCuoiCung + (chiMang ? " CRIT!" : ""));

            // TODO: Enemy.TakeDamage(finalDamage);

            // Life steal
            if (hutMau > 0)
            {
                int hoiMau = Mathf.RoundToInt(dameCuoiCung * hutMau);

                if (hoiMau > 0)
                {
                    Debug.Log("Heal player: " + hoiMau);
                    // PlayerHealth.Instance.Heal(heal);
                }
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