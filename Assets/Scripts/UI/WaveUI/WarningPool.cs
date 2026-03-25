using System.Collections.Generic;
using UnityEngine;

public class WarningPool : MonoBehaviour
{
    public static WarningPool Instance;

    [Header("--- CÀI ĐẶT POOL CẢNH BÁO ---")]
    [SerializeField] private GameObject warningPrefab;
    [SerializeField] private int poolSize = 30;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(warningPrefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetWarning(Vector2 viTri)
    {
        GameObject obj;
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = Instantiate(warningPrefab, transform);
        }

        obj.transform.position = viTri;
        obj.SetActive(true);
        return obj;
    }

    public void ReturnWarning(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}