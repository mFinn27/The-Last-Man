using UnityEngine;

public class CoinAnimator : MonoBehaviour
{
    [Header("--- CÀI ĐẶT HOẠT ẢNH ---")]
    [Tooltip("Kéo 6 tấm ảnh của đồng xu vào đây theo đúng thứ tự")]
    public Sprite[] frames;

    [Tooltip("Thời gian chuyển giữa 2 ảnh. Gợi ý: 0.08f (Xoay vừa) hoặc 0.05f (Xoay nhanh)")]
    public float tocDoChuyenFrame = 0.08f;

    private SpriteRenderer sr;
    private int frameHienTai = 0;
    private float thoiGianCho;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // BÍ KÍP TỐI ƯU THỊ GIÁC: Random frame bắt đầu để các đồng xu không bị xoay đồng bộ
        if (frames.Length > 0)
        {
            frameHienTai = Random.Range(0, frames.Length);
            sr.sprite = frames[frameHienTai];
            thoiGianCho = tocDoChuyenFrame;
        }
    }

    void Update()
    {
        // Nếu bạn chưa kéo ảnh vào thì code tự dừng để không báo lỗi
        if (frames.Length == 0) return;

        thoiGianCho -= Time.deltaTime;

        // Khi hết thời gian chờ -> Chuyển sang ảnh tiếp theo
        if (thoiGianCho <= 0)
        {
            frameHienTai = (frameHienTai + 1) % frames.Length; // Dùng phép chia lấy dư (%) để tạo vòng lặp
            sr.sprite = frames[frameHienTai];

            // Đặt lại đồng hồ đếm ngược
            thoiGianCho = tocDoChuyenFrame;
        }
    }
}