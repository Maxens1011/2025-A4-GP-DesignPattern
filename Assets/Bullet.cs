using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] float _speed;
    BulletHell _bh;
    
    public void Init(BulletHell bulletHell)
    {
        _bh = bulletHell;
    }
    async void OnEnable()
    {
        await UniTask.NextFrame();  // On attend juste une frame histoire que la position et rotation de l'objet soit bien udpate 
        _rb.AddForce(transform.forward * _speed);
    }
    
    void OnCollisionEnter(Collision other)
    {
        _rb.linearVelocity=Vector3.zero;
        _bh.TryDestroy(this);
    }

    
}
