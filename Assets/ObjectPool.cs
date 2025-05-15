using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Android.Types;
using UnityEngine;

public interface IResetable
{
    void ResetData();
}

public class ObjectPool: MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] int _warmup;

    List<GameObject> _pool;

    void Awake()
    {
        _pool = new();

        for (int i = 0; i < _warmup; i++)
        {
            var go = Instantiate(_prefab, transform);
            go.SetActive(false);
            _pool.Add(go);
        }
    }

    public GameObject GetItem(Vector3 pos, Quaternion rot)
    {
        GameObject objectToReturn = null;
        if (_pool.Count <= 0)
        {
            objectToReturn = Instantiate(_prefab, transform);
        }
        else
        {
            objectToReturn = _pool[0];
            _pool.Remove(objectToReturn);
            
            objectToReturn.transform.position = pos;
            objectToReturn.transform.rotation = rot;
            objectToReturn.SetActive(true);
        }

        objectToReturn.transform.position = pos;
        objectToReturn.transform.rotation = rot;
        return objectToReturn;
    }

    public void ReleaseItem(GameObject go)
    {
        go.SetActive(false);
        _pool.Add(go);
    }
    
}
