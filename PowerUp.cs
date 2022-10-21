using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : ScriptableObject
{
    [SerializeField]
    private HealthBehaviour hb;
    private GameObject _collission;

    public abstract void Apply();

    private void OnTriggerEnter(Collider other)
    {
        _collission = other.gameObject;
        hb.Hurt();
    }

    public GameObject GetCollision()
    {
        return _collission;
    }

    public void SetCollision(GameObject other)
    {
        _collission = other;
    }

}
