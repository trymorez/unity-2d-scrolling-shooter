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
    bool isAttacking;
    public int CurrentUpgrade;
    [SerializeField] int maxUpgrade = 4;
    bool[,] upgradeMatrix =
    {
        { false, true, false, true, false },
        { true, false, true, false, true },
        { true, true, false, true, true },
        { true, true, true, true, true }
    };

    public static Action OnGunFire;

    void Start()
    {
        GameManager.OnPlaying += OnPlayingGame;
        GameManager.OnEnterGameState += OnEnterGameState;
        CalculateNextFireTime();
        SetGun();
    }

    void OnDestroy()
    {
        GameManager.OnPlaying -= OnPlayingGame;
        GameManager.OnEnterGameState -= OnEnterGameState;
    }

    void OnEnterGameState(GameManager.GameState state)
    {
        //reset attack key pressing state
        if (state == Playing)
        {
            isAttacking = false;
        }
    }

    void CalculateNextFireTime()
    {
        nextFireTime = Time.time + fireRate;
    }

    void OnPlayingGame()
    {
        if (isAttacking && isNextFireTime)
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
        if (++CurrentUpgrade >= maxUpgrade)
        {
            CurrentUpgrade = maxUpgrade - 1;
        }

        SetGun();
    }

    public void SetGun()
    {
        for (int i = 0; i < gun.Length; i++)
        {
            gun[i].gameObject.SetActive(upgradeMatrix[CurrentUpgrade, i]);
        }
    }

    public void OnFirePressed(InputAction.CallbackContext context)
    {
        if (GameManager.State == Playing || GameManager.State == Restarting)
        {
            if (context.started)
            {
                isAttacking = true;
            }
            else if (context.canceled)
            {
                isAttacking = false;
            }
        }
    }
}
