using UnityEngine;

public class WaveCleanup : MonoBehaviour
{
    [Tooltip("Tick nếu vật thể này dùng Object Pool. Bỏ tick nếu dùng Instantiate/Destroy bình thường")]
    public bool isPooledObject = false;

    private void OnEnable()
    {
        WaveManager.OnWaveEnded += TuHuy;
    }

    private void OnDisable()
    {
        WaveManager.OnWaveEnded -= TuHuy;
    }

    private void TuHuy()
    {
        if (isPooledObject)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}