using UnityEngine;

public class WaveCleanup : MonoBehaviour
{
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