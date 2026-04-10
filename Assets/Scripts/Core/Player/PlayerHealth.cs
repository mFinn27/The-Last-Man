using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    private int mauHientai;
    private int mauToiDa;

    private PlayerVisuals hinhAnh;

    public event Action<int, int> OnHealthChanged;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        mauToiDa = PlayerStats.Instance != null ? PlayerStats.Instance.GetMaxHP() : 100;
        mauHientai = mauToiDa;

        hinhAnh = GetComponent<PlayerVisuals>();

        OnHealthChanged?.Invoke(mauHientai, mauToiDa);
    }

    public void UpdateMaxHealth()
    {
        if (PlayerStats.Instance == null) return;

        int mauToiDaMoi = PlayerStats.Instance.GetMaxHP();
        int luongMauChenhLech = mauToiDaMoi - mauToiDa;

        if (luongMauChenhLech > 0)
        {
            mauHientai += luongMauChenhLech;
        }
        mauToiDa = mauToiDaMoi;

        if (mauHientai > mauToiDa) mauHientai = mauToiDa;

        OnHealthChanged?.Invoke(mauHientai, mauToiDa);
    }

    public void Heal(int soLuong)
    {
        mauHientai += soLuong;
        if (mauHientai > mauToiDa) mauHientai = mauToiDa;

        OnHealthChanged?.Invoke(mauHientai, mauToiDa);
    }

    public void TakeDamage(int dame)
    {
        int giap = PlayerStats.Instance != null ? PlayerStats.Instance.GetArmor() : 0;
        float phanTramSatThuongPhaiChiu = (float)mauToiDa / (mauToiDa + giap);
        int dameCuoiCung = Mathf.RoundToInt(dame * phanTramSatThuongPhaiChiu);
        dameCuoiCung = Mathf.Max(1, dameCuoiCung);

        mauHientai -= dameCuoiCung;
        OnHealthChanged?.Invoke(mauHientai, mauToiDa);

        if (AudioManager.Instance != null) AudioManager.Instance.PlayPlayerHitSFX();

        if (hinhAnh != null) hinhAnh.PlayFlashWhite();

        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.AddTrauma(0.6f);
        }

        if (mauHientai <= 0) Die();
    }

    void Die()
    {
        Debug.Log("<color=yellow>1. PlayerHealth: gọi hàm Die()</color>");

        if (GameManager.Instance != null)
        {
            Debug.Log("<color=yellow>2. PlayerHealth: thấy GameManager, gọi KetThucGame!</color>");
            GameManager.Instance.KetThucGame(false);
        }
        else
        {
            Debug.LogError("<color=red>LỖI: Không tìm thấy GameManager!");
        }
    }

    public int GetCurrentHP() => mauHientai;
    public int GetMaxHP() => mauToiDa;
}