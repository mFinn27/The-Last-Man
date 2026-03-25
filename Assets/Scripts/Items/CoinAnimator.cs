using UnityEngine;

public class CoinAnimator : MonoBehaviour
{
    [Header("--- CÀI ĐẶT HOẠT ẢNH ---")]
    public Sprite[] frames;
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
        if (frames.Length > 0)
        {
            frameHienTai = Random.Range(0, frames.Length);
            sr.sprite = frames[frameHienTai];
            thoiGianCho = tocDoChuyenFrame;
        }
    }

    void Update()
    {
        if (frames.Length == 0) return;

        thoiGianCho -= Time.deltaTime;

        if (thoiGianCho <= 0)
        {
            frameHienTai = (frameHienTai + 1) % frames.Length;
            sr.sprite = frames[frameHienTai];
            thoiGianCho = tocDoChuyenFrame;
        }
    }
}