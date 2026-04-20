using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public GameObject GetEnemy(GameObject prefab)
    {
        if (prefab == null) return null;

        if (!poolDictionary.ContainsKey(prefab))
        {
            poolDictionary[prefab] = new Queue<GameObject>();
        }

        GameObject obj;

        if (poolDictionary[prefab].Count > 0)
        {
            obj = poolDictionary[prefab].Dequeue();
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(prefab, transform);
        }
        EnemyHealth enemyHealth = obj.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.SetPrefabGoc(prefab);
        }

        return obj;
    }

    public void ReturnEnemy(GameObject obj, GameObject prefab)
    {
        obj.SetActive(false);

        if (prefab != null && poolDictionary.ContainsKey(prefab))
        {
            poolDictionary[prefab].Enqueue(obj);
        }
        else
        {
            Destroy(obj);
        }
    }
}