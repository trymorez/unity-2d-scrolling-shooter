using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    void OnEnable()
    {
        PlayerGun.OnGunFire += OnFire;
    }

    void OnDisable()
    {
        PlayerGun.OnGunFire -= OnFire;
    }

    void OnFire()
    {
        var bullet = BulletPoolManager.Get();
        bullet.transform.position = transform.position;
    }
}
