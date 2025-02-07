using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] BulletPoolManager bulletPoolManager;
    Bullet bullet;

    void OnEnable()
    {
        Player.OnGunFire += OnFire;
    }

    void DisEnable()
    {
        Player.OnGunFire -= OnFire;
    }

    void OnFire()
    {
        bullet = bulletPoolManager.Pool.Get();
        bullet.transform.position = transform.position;
    }

    void Start()
    {

    }

    void Update()
    {
        
    }
}
