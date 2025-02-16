using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Vector2 direction = Vector2.up;
    [SerializeField] float speed = 5.0f;
    [SerializeField] int damage = 1;

    public static Action<Bullet> OnRelease;

    void OnEnable()
    {
        GameManager.OnPlayingGame += BulletFlying;
    }

    void OnDisable()
    {
        GameManager.OnPlayingGame -= BulletFlying;
    }

    void BulletFlying()
    {
        transform.Translate(direction * (speed * Time.deltaTime));
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
        {
            BulletPoolManager.Pool.Release(this);
        }
    }
}
