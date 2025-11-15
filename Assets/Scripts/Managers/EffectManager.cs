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

    public GameObject Spawn(GameObject _Prefab, Vector3 _Position, float _Duration)
    {
        if (_Position == null) return null;

        Debug.Log("스킬 이펙트 생성");
        var Prefab = Instantiate(_Prefab, _Position, Quaternion.identity, DynamicObject.transform);
        Destroy(Prefab, _Duration);
        return Prefab;
    }

    public GameObject Spawn(GameObject _Prefab, Vector3 _Position, Quaternion _Rotation, float _Duration)
    {
        if (_Position == null) return null;

        Debug.Log("스킬 이펙트 생성");
        var Prefab = Instantiate(_Prefab, _Position, _Rotation, DynamicObject.transform);
        Destroy(Prefab, _Duration);
        return Prefab;
    }

}
