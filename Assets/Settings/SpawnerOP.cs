using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpawnerOP : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _buttontxt;
    [SerializeField] Button _toggleButton;
    [SerializeField] GameObject _prefab;
    [SerializeField] float _interval;
    [SerializeField] float _destroyDelay;
    [SerializeField] float _randomSphereRadius;
    [SerializeField] Transform _center;
    [SerializeField] bool _log;
        
    bool _isOn;
    Coroutine _routine;
    ObjectPool<GameObject> _pool;
    
    void UpdateTxt() => _buttontxt.text = _isOn ? "ON" : "OFF";

    void Reset()
    {
        _buttontxt = GetComponentInChildren<TextMeshProUGUI>();
        _prefab = null;
        _interval = 0.2f;
        _randomSphereRadius = 1f;
        _center = transform;
    }

    void Start()
    {
        _pool = new ObjectPool<GameObject>(Create, Get,Release,Dest);
        
        _toggleButton.onClick.AddListener(Toggle);
        UpdateTxt();
    }
    void OnDestroy()
    {
        _toggleButton.onClick.RemoveListener(Toggle);
    }
    GameObject Create()
    {
        if(_log) Debug.Log("Create");
        return Instantiate(_prefab, _center.position + (Random.insideUnitSphere * _randomSphereRadius),
            Quaternion.identity);
    }
    void Get(GameObject o)
    {
        if(_log) Debug.Log("Get");
        o.SetActive(true);
    }
    void Release(GameObject o)
    {
        if(_log) Debug.Log("Release");
        o.SetActive(false);
    }
    void Dest(GameObject o)
    {
        if(_log) Debug.Log("Destroy");
        Destroy(o);
    }
    

    public void Toggle()
    {
        _isOn = !_isOn;
        UpdateTxt();

        if (_isOn)
        {

            _routine = StartCoroutine(SpawnRoutine());
        }
        else
        {
            if(_routine!=null) StopCoroutine(_routine);
            _routine = null;
        }

        IEnumerator SpawnRoutine()
        {
            var w = new WaitForSeconds(_interval);
            while (true)
            {
                var randomPoint = _center.position + (Random.insideUnitSphere * _randomSphereRadius);
                var go = _pool.Get();
                go.transform.position = randomPoint;
                go.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                
                if (Mathf.Approximately(0f, _destroyDelay) == false && _destroyDelay > 0f)
                {
                    StartCoroutine(WaitAndRelease(go));
                }
                yield return w;
            }
        }
        
        IEnumerator WaitAndRelease(GameObject go)
        {
            yield return new WaitForSeconds(_destroyDelay);
            _pool.Release(go);
        }
    }
}