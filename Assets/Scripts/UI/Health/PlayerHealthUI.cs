using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image healthFillimg;

    void Start()
    {
        if (PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.OnHealthChanged += UpdateHealthBar;
            UpdateHealthBar(PlayerHealth.Instance.GetCurrentHP(), PlayerHealth.Instance.GetMaxHP());
        }
    }

    private void UpdateHealthBar(int mauHienTai, int mauToiDa)
    {
        float phanTramThanhMau = (float)mauHienTai / mauToiDa;

        if (healthFillimg != null)
        {
            healthFillimg.fillAmount = phanTramThanhMau;
        }
    }

    void OnDestroy()
    {
        if (PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.OnHealthChanged -= UpdateHealthBar;
        }
    }
}