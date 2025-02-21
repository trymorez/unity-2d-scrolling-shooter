using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] Gun[] gun;
    [SerializeField] float fireRate = 0.3f;
    float nextFireTime;
    bool isNextFireTime { get => Time.time >= nextFireTime; }
    bool isAttacking;
    [SerializeField] int currentUpgrade;
    [SerializeField] int maxUpgrade = 5;
    bool[,] upgradeMatrix = new bool[5, 5] {
        { false, false, true, false, false },
        { false, true, false, true, false },
        { true, false, true, false, true },
        { true, true, false, true, true },
        { true, true, true, true, true }
    };

    public static Action OnGunFire;

    void Start()
    {
        GameManager.OnPlayingGame += OnPlayingGame;
        CalculateNextFireTime();
        SetGun();
    }
    void OnDestroy()
    {
        GameManager.OnPlayingGame -= OnPlayingGame;
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
    void UpgradeGun()
    {
        if (++currentUpgrade > maxUpgrade)
        {
            currentUpgrade = 0;
        }
        SetGun();
    }

    void SetGun()
    {
        for (int i = 0; i < gun.Length; i++)
        {
            gun[i].gameObject.SetActive(upgradeMatrix[currentUpgrade, i]);
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (GameManager.State != GameManager.GameState.Playing)
        {
            return;
        }

        //if (context.started)
        //{
        //    Attack();
            
        //    isAttacking = true;
        //}

        if (context.performed)
        {
            Debug.Log("performed");
            isAttacking = true;
        }
        else if (context.canceled)
        {
            Debug.Log("canceled");
            isAttacking = false;
        }
    }
}
