using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 huongDiChuyen;
    private float tocDoDiChuyen;
    private int xuyenThauHienTai;
    private float lucDayLui;
    private float thoiGianDanTonTai;

    public void Setup(Vector2 huong, float tocDo, int xuyenThau, float dayLui)
    {
        huongDiChuyen = huong;
        tocDoDiChuyen = tocDo;
        xuyenThauHienTai = xuyenThau;
        lucDayLui = dayLui;
        thoiGianDanTonTai = 3f;
    }

    void Update()
    {
        transform.Translate(Vector3.right * tocDoDiChuyen * Time.deltaTime);

        thoiGianDanTonTai -= Time.deltaTime;
        if (thoiGianDanTonTai <= 0) VoHieuHoa();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("co va cham");
            // Logic gây sát thương sẽ code vào ngày 20/03 
            xuyenThauHienTai--;
            if (xuyenThauHienTai <= 0) VoHieuHoa();
        }
    }

    private void VoHieuHoa()
    {
        BulletPool.Instance.ReturnBullet(gameObject);
    }
}