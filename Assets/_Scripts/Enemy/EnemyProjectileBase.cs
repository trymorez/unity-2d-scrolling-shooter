using System;
using UnityEngine;

public class EnemyProjectileBase : MonoBehaviour
{
    public int Damage = 1;
    [SerializeField] float speed = 5f;

    protected virtual void OnEnable()
    {
        GameManager.OnPlaying += ProcessProjectile;
    }

    protected virtual void OnDisable()
    {
        GameManager.OnPlaying -= ProcessProjectile;
    }

    void ProcessProjectile()
    {
        transform.Translate(Vector3.up * (speed * Time.deltaTime), Space.Self);
    }
}
