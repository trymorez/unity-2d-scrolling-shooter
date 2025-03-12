using System;
using UnityEngine;
using static GameManager.GameState;

public class TankShell : EnemyProjectileBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.OnExitGameState += OnExitGameState;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameManager.OnExitGameState -= OnExitGameState;
    }

    void OnExitGameState(GameManager.GameState state)
    {
        switch (state)
        {
            case Exploding:
                //ShellPoolManager.Release(this);
                gameObject.SetActive(false);
                break;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Activator"))
        {
            ShellPoolManager.Release(this);
        }
    }


}
