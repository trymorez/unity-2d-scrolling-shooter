using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGun : MonoBehaviour
{
    [SerializeField] Gun[] gun;
    [SerializeField] float fireRate = 0.3f;
    float nextFireTime;
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
        nextFireTime = Time.time + fireRate;
        SetGun();
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

        if (context.performed && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            OnGunFire?.Invoke();
        }
    }
}
