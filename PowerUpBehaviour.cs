using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;


public class PowerUpBehaviour : MonoBehaviour
{
    public UnityEvent<GameObject> BulletPointToInstanciate;
    private HealthBehaviour hb;
    [SerializeField]
    private GameObject extractedGameObject;

    private void Start()
    {
        hb = GetComponent<HealthBehaviour>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (extractedGameObject)
            BulletPointToInstanciate.Invoke(extractedGameObject);
        hb.Hurt();
    }
}
