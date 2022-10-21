using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthBehaviour : MonoBehaviour
{
    public UnityEvent OnDie;

    [SerializeField]
    private int health;

    public int Lifes { get { return health; } set { health = value; } }
    public void Hurt(int damage)
    {
        health -= damage;
        IsDeath();
    }
    public void Hurt()
    {
        health -= 1;
        IsDeath();
    }

    private void IsDeath()
    {
        if (health <= 0)
            OnDie.Invoke();
    }
}