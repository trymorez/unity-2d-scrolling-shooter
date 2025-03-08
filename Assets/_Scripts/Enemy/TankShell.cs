using System;
using UnityEngine;

public class TankShell : MonoBehaviour
{
    public int Damage = 1;
    [SerializeField] float speed = 5f;

    void OnEnable()
    {
        GameManager.OnPlayingGame += ProcessProjectile;
    }

    void OnDisable()
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
            ShellPoolManager.Release(this);
        }
    }
}
