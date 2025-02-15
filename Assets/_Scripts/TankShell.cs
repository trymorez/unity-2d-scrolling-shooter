using System;
using UnityEngine;

public class TankShell : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float life = 3f;
    public static Action<TankShell> OnRelease;

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
        //var pos = transform.position;
        //var newX = pos.x + direction.x * speed * Time.deltaTime;
        //var newY = pos.y + direction.y * speed * Time.deltaTime;
        //transform.position = new Vector2 (newX, newY);

        //transform.Translate(direction * (speed * Time.deltaTime), Space.World);
        transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
        {
            ShellPoolManager.Pool.Release(this);
        }
    }
}
