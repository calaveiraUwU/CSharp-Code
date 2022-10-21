using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;


public class PowerUpBehaviour : MonoBehaviour
{
    public UnityEvent<GameObject> BulletPointToInstanciate; //Un evento para poder saber cuál es el punto de instanciar la bala/proyectul u objeto a instanciar des de cierto punto.
    private HealthBehaviour hb; //Sistema de vida para la bala/proyectul u objeto a instanciar des de cierto punto.
    [SerializeField]
    private GameObject extractedGameObject;//Aquí podremos arrastrarle de del editor de Unity el objeto que queremos instanciar.

    private void Start()
    {
        hb = GetComponent<HealthBehaviour>(); //Iniciaremos el sistema de vida para que lo busque en su arbol de scripts.
    }
    private void OnTriggerEnter(Collider other)
    {
        if (extractedGameObject) //Si entra algo que decidamos, comprobamos que el objeto existe poder entrar.
            BulletPointToInstanciate.Invoke(extractedGameObject); //Le pasamos al evento el objeto que hemos asignado des de el editor.
            hb.Hurt();//Le restamos un punto de vida al objeto.
    }
}
