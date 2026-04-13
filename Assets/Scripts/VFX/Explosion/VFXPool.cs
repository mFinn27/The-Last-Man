using System.Collections.Generic;
using UnityEngine;

public class VFXPool : MonoBehaviour
{
    public static VFXPool Instance;
    private Dictionary<GameObject, Queue<GameObject>> pool = new Dictionary<GameObject, Queue<GameObject>>();
    private Dictionary<GameObject, GameObject> objectToPrefab = new Dictionary<GameObject, GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public GameObject SpawnVFX(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!pool.ContainsKey(prefab))
        {
            pool[prefab] = new Queue<GameObject>();
        }
        if (pool[prefab].Count > 0)
        {
            GameObject obj = pool[prefab].Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
            return obj;
        }
        GameObject newObj = Instantiate(prefab, position, rotation, transform);
        objectToPrefab[newObj] = prefab;
        return newObj;
    }
    public void ReturnVFX(GameObject obj)
    {
        if (objectToPrefab.ContainsKey(obj))
        {
            GameObject prefabGoc = objectToPrefab[obj];
            obj.SetActive(false);
            pool[prefabGoc].Enqueue(obj);
        }
        else
        {
            Destroy(obj);
        }
    }
}