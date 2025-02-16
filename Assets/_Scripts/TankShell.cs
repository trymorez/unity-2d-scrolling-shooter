using System;
using UnityEngine;

public class TankShell : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float life = 3f;

    void Awake()
    {
        GameManager.OnPlayingGame += ProcessProjectile;
    }

    void OnDestroy()
    {
        GameManager.OnPlayingGame -= ProcessProjectile;
    }

    void ProcessProjectile()
    {
        transform.Translate(Vector3.up * (speed * Time.deltaTime), Space.Self);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
        {
            ShellPoolManager.Pool.Release(this);
        }
    }
}
