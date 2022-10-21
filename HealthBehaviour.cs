using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthBehaviour : MonoBehaviour
{
    public UnityEvent OnDie; //Creamos un evento para poder avisar a otros objetos del juego que el objeto se ha quedado sin vida.

    [SerializeField]
    private int health; //Declaramos la vida privada pero que la podamos editar des de el editor.

    public int Lifes { get { return health; } set { health = value; } } //Un give/setter para poder recojer las vidas y establecerlas si lo neccesitamos.
    
    //Una funcion para restarle la vida que le pasemos por parametro.
    public void Hurt(int damage)
    {
        health -= damage;
        IsDeath();
    }
    
    //Una funcion para restarle un punto de vida a la actual.
    public void Hurt()
    {
        health -= 1;
        IsDeath();
    }

    //Comprueba cada vez que la función es llamada si tiene menos o 0 de vida, si es así se invoca el evento.
    private void IsDeath()
    {
        if (health <= 0)
            OnDie.Invoke();
    }
}
