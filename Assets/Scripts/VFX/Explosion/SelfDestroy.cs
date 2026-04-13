using UnityEngine;
using System.Collections;

public class SelfDestroy : MonoBehaviour
{
    [Tooltip("Thời gian tồn tại của vụ nổ (bằng độ dài animation)")]
    public float thoiGianTonTai = 0.5f;

    void OnEnable()
    {
        StartCoroutine(ThuHoiSauThoiGian());
    }

    private IEnumerator ThuHoiSauThoiGian()
    {
        yield return new WaitForSeconds(thoiGianTonTai);
        if (VFXPool.Instance != null)
        {
            VFXPool.Instance.ReturnVFX(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}