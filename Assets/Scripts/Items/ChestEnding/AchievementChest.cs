using UnityEngine;
using System.Collections;

public class AchievementChest : MonoBehaviour
{
    [Header("--- THÔNG TIN THÀNH TỰU ---")]
    public Sprite iconVatPham;
    public string tenVatPham = "Lõi Quỷ Vương";
    [TextArea(3, 5)]
    public string moTaVatPham = "Vật phẩm minh chứng cho sức mạnh tuyệt đối.\nBạn đã thanh tẩy hoàn toàn vùng đất này!";

    [Header("--- CÀI ĐẶT HIỆU ỨNG ---")]
    public float thoiGianNay = 0.6f;
    public float doCaoNay = 2f;

    private bool daMo = false;
    private Vector3 viTriGoc;

    void Start()
    {
        Collider2D[] cols = GetComponentsInChildren<Collider2D>();
        foreach (var c in cols) c.enabled = true;

        if (cols.Length == 0)
        {
            Debug.LogError("rương CHƯA CÓ Collider2D (BoxCollider2D, CircleCollider2D...). Hãy gắn vào nó ngay!");
        }

        viTriGoc = transform.position;
        StartCoroutine(HieuUngNayRa());
    }

    void Update()
    {
        if (daMo || Time.timeScale == 0f) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (Camera.main == null)
            {
                Debug.LogError("LỖI: Không tìm thấy Camera nào có tag là 'MainCamera'. Hàm click chuột không thể hoạt động!");
                return;
            }
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] hits = Physics2D.OverlapPointAll(mousePosition);

            bool trungRuong = false;

            foreach (var hit in hits)
            {
                if (hit.GetComponentInParent<AchievementChest>() == this)
                {
                    trungRuong = true;
                    break;
                }
            }

            if (trungRuong)
            {
                Debug.Log("<color=green>ĐÃ CLICK TRÚNG RƯƠNG! Đang tiến hành mở...</color>");
                MoRuong();
            }
        }
    }

    private void MoRuong()
    {
        daMo = true;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayEquipSFX();

        Time.timeScale = 0f;

        if (AchievementUI.Instance != null)
        {
            AchievementUI.Instance.HienThiThanhTuu(iconVatPham, tenVatPham, moTaVatPham, () =>
            {
                if (StoryDirector.Instance != null)
                {
                    StoryDirector.Instance.KichHoatEndingTuRuong();
                }
                Destroy(gameObject);
            });
        }
        else
        {
            Debug.LogError("LỖI: Không tìm thấy AchievementUI trên Scene. Mở bảng thành tựu thất bại!");
            Time.timeScale = 1f;
        }
    }

    private IEnumerator HieuUngNayRa()
    {
        Vector3 viTriDich = viTriGoc + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, -0.5f), 0);
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime / thoiGianNay;
            float chieuCaoToaDoY = Mathf.Sin(t * Mathf.PI) * doCaoNay;
            transform.position = Vector3.Lerp(viTriGoc, viTriDich, t) + new Vector3(0, chieuCaoToaDoY, 0);
            yield return null;
        }
        transform.position = viTriDich;

        float yGoc = transform.position.y;
        while (!daMo)
        {
            transform.position = new Vector3(transform.position.x, yGoc + Mathf.Sin(Time.time * 4f) * 0.15f, transform.position.z);
            yield return null;
        }
    }
}