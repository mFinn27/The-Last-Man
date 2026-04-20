using System.Collections.Generic;
using UnityEngine;

public class CoinPool : MonoBehaviour
{
    public static CoinPool Instance;

    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int poolSize = 500;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(coinPrefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetCoin()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        GameObject newObj = Instantiate(coinPrefab, transform);
        return newObj;
    }

    public void ReturnCoin(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}