using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private CinemachineCamera vCam;
    private CinemachineBasicMultiChannelPerlin vCamNoise;

    [Header("--- GIỚI HẠN RUNG (TRAUMA SYSTEM) ---")]
    [Tooltip("Lực rung tối đa khi chạm đỉnh (để 3.0 đến 5.0 là đẹp)")]
    public float maxAmplitude = 3.5f;
    [Tooltip("Tần số rung tối đa (15 - 20)")]
    public float maxFrequency = 15f;
    [Tooltip("Tốc độ giảm rung (Nên để 2 - 3 để camera êm lại nhanh)")]
    public float tocDoHoiPhuc = 2.5f;

    private float trauma = 0f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        vCam = GetComponent<CinemachineCamera>();
        vCamNoise = GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Start()
    {
        if (vCamNoise != null)
        {
            vCamNoise.AmplitudeGain = 0f;
            vCamNoise.FrequencyGain = 0f;
        }
    }

    void Update()
    {
        if (vCamNoise == null) return;

        if (trauma > 0)
        {
            trauma -= Time.deltaTime * tocDoHoiPhuc;
            trauma = Mathf.Clamp01(trauma);
            float doRung = trauma * trauma;
            vCamNoise.AmplitudeGain = maxAmplitude * doRung;
            vCamNoise.FrequencyGain = maxFrequency * trauma;
        }
        else
        {
            vCamNoise.AmplitudeGain = 0f;
            vCamNoise.FrequencyGain = 0f;
        }
    }

    public void AddTrauma(float amount)
    {
        trauma += amount;
        trauma = Mathf.Clamp01(trauma);
    }

    public void RungNhe(float _ = 0, float __ = 0)
    {
        AddTrauma(0.35f);
    }

    public void RungTuyChinh(float thoiGian, float doManh)
    {
        AddTrauma(0.8f);
    }
}