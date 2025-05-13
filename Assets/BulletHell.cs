using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHell : MonoBehaviour
{
    [SerializeField] List<Transform> _roots;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] float _cooldown;

    [SerializeField] ObjectPool _pool;


    void Awake()
    {
        (string name, int age) data = ("coucou", 12);

        Debug.Log(data.Item1);
        Debug.Log(data.name);
    }


    IEnumerator Start()
    {
        var waiter = new WaitForSeconds(_cooldown);
        while (true)
        {
            foreach (var el in _roots)
            {
                var obj = _pool.GetItem(el.position, el.rotation);
                var b = obj.GetComponent<Bullet>();
                b.Init(this);
            }
            yield return waiter;
        }
    }

    public void TryDestroy(Bullet bullet)
    {
        bullet.GetComponent<Rigidbody>().linearVelocity=Vector3.zero;
        _pool.ReleaseItem(bullet.gameObject);
    }
}
