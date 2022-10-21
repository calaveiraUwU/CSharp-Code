using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject FXSystem;
    public void DestroyObject()
    {
        if(FXSystem)
            if (FXSystem.TryGetComponent<ParticleSystem>(out ParticleSystem p))
                  Instantiate(p, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
