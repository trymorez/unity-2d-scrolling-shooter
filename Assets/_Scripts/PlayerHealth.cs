using DG.Tweening;
using UnityEngine;
using static GameManager.GameState;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int life = 3;
    [SerializeField] int armorCount = 5;
    [SerializeField] int armorMaxCount = 5;
    [SerializeField] SpriteRenderer[] armorDot;
    [SerializeField] SpriteRenderer plane;
    [SerializeField] GameObject graphics;
    [SerializeField] FlashEffect flashEffect;

    [SerializeField] int explosionCount = 8;
    [SerializeField] float explosionDelay = 0.3f;
    [SerializeField] float explosionGap = 0.3f;
    bool isGameOver;
    PlayerGun playerGun;

    void Start()
    {
        GUIManager.ChangeLifeIcon(ref life, 0);
        playerGun = GetComponent<PlayerGun>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("TankShell"))
        {
            return;
        }

        var tankShell = other.GetComponent<TankShell>();
        ApplyDamage(tankShell.Damage);

        //spawn hit explosion
        var hitEffect = HitEffectPoolManager.Get();
        hitEffect.transform.position = other.transform.position;

        flashEffect.ProcessFlashing();

        //delete projectile
        other.gameObject.SetActive(false);
    }

    public void ApplyDamage(int damage)
    {
        //armorCount -= damage;
        //if (armorCount > armorMaxCount)
        //{
        //    armorCount = armorMaxCount;
        //}
        armorCount = Mathf.Clamp(armorCount - damage, 0, armorMaxCount);

        foreach (var armor in armorDot)
        {
            armor.enabled = false;
        }

        if (armorCount > 0)
        {
            for (int i = 0; i < armorCount; i++)
            {
                armorDot[i].enabled = true;
            }
        }
        else
        {
            ShipCrash();
        }
    }

    void ShipCrash()
    {
        GameManager.ChangeGameState(Exploding);
        ShipCrashEffect();
    }

    void Restart()
    {
        //restore alpha
        SetPlayerColor(Color.white);

        GUIManager.ChangeLifeIcon(ref life, -1);

        playerGun.CurrentUpgrade = 0;
        playerGun.SetGun();

        if (life == 0)
        {
            GameManager.GameOver();
            graphics.SetActive(false);
        }
        else
        {
            ResetArmor();
            GameManager.ChangeGameState(Restarting);
        }
    }

    void SetPlayerColor(Color color)
    {
        plane.color = color;
    }

    void ShipCrashEffect()
    {
        var sequence = DOTween.Sequence();
        var crashFade = 0.3f;

        plane.DOFade(crashFade, explosionCount * explosionDelay);

        for (int i = 0; i <= explosionCount; i++)
        {
            sequence.AppendCallback(() =>
            {
                CrashingPoolManager.Get()
                .transform.position = transform.position + Random.onUnitSphere * explosionGap;
            });

            sequence.AppendInterval(explosionDelay);
        }
        sequence.AppendCallback(() => Restart());
    }

    void ResetArmor()
    {
        armorCount = armorMaxCount;
        ApplyDamage(0);
    }

    void OnDestroy()
    {
        DOTween.Kill(flashEffect);
    }
}
