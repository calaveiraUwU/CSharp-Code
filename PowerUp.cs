using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : ScriptableObject
{
    [SerializeField]
    private HealthBehaviour hb; //Declaramos un objeto de tipo gestor de vida que nosotros hemos creado y lo podamos gestionar des de el editor de Unity.
    private GameObject _collission; //Un objeto vacio para asignarle la colisión.

    public abstract void Apply(); //Generamos que la clase le puedan heredar los scriptable objects.

    private void OnTriggerEnter(Collider other)
    {
        _collission = other.gameObject; //Cuando un objeto entra en la colisión recolectaremos cual es la colisión y le restaremos vida.
        hb.Hurt();
    }

    public GameObject GetCollision() //Función para retornar la colisión.
    {
        return _collission;
    }

    public void SetCollision(GameObject other) //Poder establlecer la colisión des de otro script si es neccesario.
    {
        _collission = other;
    }

}
