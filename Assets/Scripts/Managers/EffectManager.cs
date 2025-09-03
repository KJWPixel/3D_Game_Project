using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    [SerializeField] GameObject DynamicObject;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject Spawn(GameObject _Prefab, Vector3 _Position)
    {
        var Prefab = Instantiate(_Prefab, _Position, Quaternion.identity, DynamicObject.transform);
        Destroy(Prefab, 2f);
        return Prefab;
    }

    public GameObject Spawn(GameObject _Prefab, Vector3 _Position, Quaternion _Rotation)
    {
        var Prefab = Instantiate(_Prefab, _Position, _Rotation, DynamicObject.transform);
        Destroy(Prefab, 2f);
        return Prefab;
    }

}
