using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPool : MonoBehaviour
{
    public static EnemyBulletPool Instance;
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

    void Awake()
    {
        Instance = this;
    }

    public GameObject GetBullet(GameObject prefabGoc)
    {
        if (prefabGoc == null) return null;

        if (!poolDictionary.ContainsKey(prefabGoc))
        {
            poolDictionary[prefabGoc] = new Queue<GameObject>();
        }

        if (poolDictionary[prefabGoc].Count > 0)
        {
            GameObject obj = poolDictionary[prefabGoc].Dequeue();
            obj.SetActive(true);
            return obj;
        }

        GameObject newBullet = Instantiate(prefabGoc, transform);

        EnemyBullet bulletScript = newBullet.GetComponent<EnemyBullet>();
        if (bulletScript != null)
        {
            bulletScript.SetPrefabGoc(prefabGoc);
        }

        return newBullet;
    }

    public void ReturnBullet(GameObject obj, GameObject prefabGoc)
    {
        obj.SetActive(false);
        if (poolDictionary.ContainsKey(prefabGoc))
        {
            poolDictionary[prefabGoc].Enqueue(obj);
        }
        else
        {
            Destroy(obj);
        }
    }
}