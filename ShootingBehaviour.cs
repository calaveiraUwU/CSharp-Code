using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Shooting")]
public class ShootingBehaviour : PowerUp
{
    public GameObject _bulletPrefab;
    public GameObject PointOfInstantiate;
    public override void Apply()
    {
        if (_bulletPrefab)
            if (_bulletPrefab.TryGetComponent<BulletController>(out BulletController B))
                Instantiate(B, GetCollision().transform.position, _bulletPrefab.transform.rotation);
    }
}
