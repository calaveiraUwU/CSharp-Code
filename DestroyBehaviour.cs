using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject FXSystem;
    //^Atributo público des de el editor para arrastrarle el prefab de las particulas.
    public void DestroyObject()
    {
        //Comprobamos si las particulas no están vacias.
        if(FXSystem)
            //Comprueba si el componente de el sistema de particulas existe.
            if (FXSystem.TryGetComponent<ParticleSystem>(out ParticleSystem p))
                  Instantiate(p, transform.position, Quaternion.identity);//Aquí instanciamos las particulas en la misma ubicación que el objeto.
        Destroy(gameObject);//Destruimos el objeto asociado.
    }
}
