using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Vector2 direction = Vector2.up;
    [SerializeField] float speed = 5.0f;
    public int Damage = 1;

    public static Action<Bullet> OnRelease;

    void OnEnable()
    {
        GameManager.OnPlaying += BulletFlying;
    }

    void OnDisable()
    {
        GameManager.OnPlaying -= BulletFlying;
    }

    void BulletFlying()
    {
        transform.Translate(direction * (speed * Time.deltaTime));
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
        {
            BulletPoolManager.Release(this);
        }
    }
}
