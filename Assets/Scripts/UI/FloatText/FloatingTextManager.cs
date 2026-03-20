using UnityEngine;
using System.Collections.Generic;

public class FloatingTextManager : MonoBehaviour
{
    public static FloatingTextManager Instance;

    [SerializeField] private GameObject floatingTextPrefab;
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
            GameObject obj = Instantiate(floatingTextPrefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public void SpawnText(Vector3 viTri, float dame, bool chiMang)
    {
        viTri += new Vector3(
            Random.Range(-0.3f, 0.3f),
            Random.Range(0.5f, 0.8f),
            0
        );
        viTri.z = 0;

        GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = Instantiate(floatingTextPrefab, transform);
        }

        obj.transform.position = viTri;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);

        FloatingText text = obj.GetComponent<FloatingText>();
        text.Setup(dame, chiMang);
    }

    public void ReturnText(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}