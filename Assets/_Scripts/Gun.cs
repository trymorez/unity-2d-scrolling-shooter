using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] BulletPoolManager bulletPoolManager;
    Bullet bullet;

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
        bullet = bulletPoolManager.Pool.Get();
        bullet.transform.position = transform.position;
    }
}
