using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Shooting")] //La clase hereda de PowerUps y se lo decimos para que se vea en el menú.
public class ShootingBehaviour : PowerUp //Hereda de PowerUp
{
    public GameObject _bulletPrefab; //Objeto prefab que será la bala.
    public GameObject PointOfInstantiate; //Donde se instancia la bala.
    public override void Apply()//El evento que hereda para poder generar el evento sea de tipo bala o otro tipo al ser heredado.
    {
        if (_bulletPrefab) //Comprobamos si existe la bala y que no está vacio.
            if (_bulletPrefab.TryGetComponent<BulletController>(out BulletController B))//Vemos si existe el componente que hemos creado que controla la bala.
                Instantiate(B, GetCollision().transform.position, _bulletPrefab.transform.rotation);//Instanciamos el objeto generado arriba asegurandonos que tiene los componentes deseados.
    }
}
