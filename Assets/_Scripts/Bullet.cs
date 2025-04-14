using System;
using UnityEngine;
using static GameManager.GameState;

public class Bullet : MonoBehaviour
{
    [SerializeField] Vector2 direction = Vector2.up;
    [SerializeField] float speed = 5.0f;
    public int Damage = 1;

    public static Action<Bullet> OnRelease;

    void OnEnable()
    {
        GameManager.OnPlaying += BulletFlying;
        GameManager.OnExitGameState += OnExitGameState;
    }

    void OnDisable()
    {
        GameManager.OnPlaying -= BulletFlying;
        GameManager.OnExitGameState -= OnExitGameState;
    }

    void BulletFlying()
    {
        transform.Translate(direction * (speed * Time.deltaTime));
    }

    void OnExitGameState(GameManager.GameState state)
    {
        switch (state)
        {
            case Exploding:
                //BulletPoolManager.Release(this);
                gameObject.SetActive(false);
                break;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
        {
            BulletPoolManager.Release(this);
        }
    }
}
