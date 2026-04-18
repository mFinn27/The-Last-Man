using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.Cinemachine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [Header("--- CÀI ĐẶT CAMERA ---")]
    public CinemachineConfiner2D confiner;

    [Header("--- TRẠNG THÁI ---")]
    public string tenMapHienTai = "Map_01_Garden";

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private IEnumerator Start()
    {
        if (TransitionManager.Instance != null && TransitionManager.Instance.imgFlash != null)
        {
            Color c = TransitionManager.Instance.imgFlash.color;
            c.a = 1f;
            TransitionManager.Instance.imgFlash.color = c;
            TransitionManager.Instance.imgFlash.raycastTarget = true;
        }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(tenMapHienTai, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        CapNhatCameraBounds();

        if (TransitionManager.Instance != null)
        {
            yield return StartCoroutine(TransitionManager.Instance.FlashOut());
        }
    }

    public IEnumerator BatDauChuyenMap(string tenMapMoi, System.Action onMidTransition = null)
    {
        if (TransitionManager.Instance != null)
            yield return StartCoroutine(TransitionManager.Instance.FlashIn());

        onMidTransition?.Invoke();

        if (!string.IsNullOrEmpty(tenMapHienTai))
        {
            yield return SceneManager.UnloadSceneAsync(tenMapHienTai);
        }
        yield return SceneManager.LoadSceneAsync(tenMapMoi, LoadSceneMode.Additive);
        tenMapHienTai = tenMapMoi;
        CapNhatCameraBounds();

        if (TransitionManager.Instance != null)
            yield return StartCoroutine(TransitionManager.Instance.FlashOut());
    }

    private void CapNhatCameraBounds()
    {
        GameObject boundsObj = GameObject.FindGameObjectWithTag("CameraBounds");
        if (boundsObj != null && confiner != null)
        {
            Collider2D col = boundsObj.GetComponent<Collider2D>();
            if (col != null)
            {
                confiner.BoundingShape2D = col;
                confiner.InvalidateBoundingShapeCache();
                if (WaveManager.Instance != null)
                {
                    WaveManager.Instance.CapNhatGioiHanSpawn(col.bounds);
                }
            }
        }
    }
}