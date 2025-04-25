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

        //initialize shoot timing
        CalculateNextFireTime();
        //initialize gun placement
        SetGun();
    }

    void OnDestroy()
    {
        GameManager.OnPlaying -= OnPlayingGame;
    }

    void CalculateNextFireTime()
    {
        nextFireTime = Time.time + fireRate;
    }

    void OnPlayingGame()
    {
        if (isFirePressed && isNextFireTime)
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
        if (context.started)
        {
            isFirePressed = true;
        }
        else if (context.canceled)
        {
            isFirePressed = false;
        }
    }
}
