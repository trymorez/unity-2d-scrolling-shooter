using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameManager.GameState;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] Gun[] gun;
    [SerializeField] float fireRate = 0.3f;
    float nextFireTime;
    bool isNextFireTime { get => Time.time >= nextFireTime; }
    bool isShooting;
    bool isFirePressed;

    public int currentUpgrade = 0;
    [SerializeField] int maxUpgrade = 4;
    bool[,] upgradeMatrix =
    {
        { false, false, true, false, true, false, false },
        { false, true, false, true, false, true, false },
        { true, false, true, false, true, false, true },
        { true, true, false, true, false, true, true },
    };

    public static Action OnGunFire;

    void Start()
    {
        GameManager.OnPlaying += OnPlayingGame;
        GameManager.OnEnterGameState += OnEnterGameState;
        GameManager.OnExitGameState += OnExitGameState;

        //initialize fire timing
        CalculateNextFireTime();
        //initialize gun placement
        SetGun();
    }

    void OnDestroy()
    {
        GameManager.OnPlaying -= OnPlayingGame;
        GameManager.OnEnterGameState -= OnEnterGameState;
        GameManager.OnExitGameState -= OnExitGameState;
    }

    void OnEnterGameState(GameManager.GameState state)
    {
        //reset attack key pressing state
        if (state == Playing && isFirePressed)
        {
            isShooting = true;
        }
    }

    void OnExitGameState(GameManager.GameState state)
    {
        //reset attack key pressing state
        if (state == Exploding)
        {
            isShooting = false;
        }
    }

    void CalculateNextFireTime()
    {
        nextFireTime = Time.time + fireRate;
    }

    void OnPlayingGame()
    {
        if (isShooting && isNextFireTime)
        {
            Attack();
        }
    }

    void Attack()
    {
        CalculateNextFireTime();
        OnGunFire?.Invoke();
    }

    [ContextMenu("Upgrade Gun")]
    public void UpgradeGun()
    {
        if (++currentUpgrade >= maxUpgrade)
        {
            currentUpgrade = maxUpgrade - 1;
        }

        SetGun();
    }

    public void SetGun()
    {
        for (int i = 0; i < gun.Length; i++)
        {
            gun[i].gameObject.SetActive(upgradeMatrix[currentUpgrade, i]);
        }
    }

    public void OnFirePressed(InputAction.CallbackContext context)
    {
        //if (GameManager.State == Playing || GameManager.State == Restarting)
        {
            if (context.started)
            {
                isShooting = true;
                isFirePressed = true;
            }
            else if (context.canceled)
            {
                isShooting = false;
                isFirePressed = false;
            }
        }
    }
}
