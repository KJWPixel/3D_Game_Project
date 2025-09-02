using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject Spawn(GameObject _Prefab, Vector3 _Position)
    {
        var fx = Instantiate(_Prefab, _Position, Quaternion.identity);
        Destroy(fx, 2f);
        return fx;
    }

    public GameObject Spawn(GameObject _Prefab, Vector3 _Position, Quaternion _Rotation)
    {
        var fx = Instantiate(_Prefab, _Position, _Rotation);
        Destroy(fx, 2f);
        return fx;
    }

}
