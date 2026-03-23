using UnityEngine;
public class SelfDestroy : MonoBehaviour
{
    [Tooltip("Thời gian tồn tại của vụ nổ (bằng độ dài animation)")]
    public float thoiGianTonTai = 0.5f;
    void Start() { Destroy(gameObject, thoiGianTonTai); }
}