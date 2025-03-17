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
        //ShellPoolManager.Release(tankShell);
        other.gameObject.SetActive(false);
    }

    void ApplyDamage(int damage)
    {
        armorCount -= damage;

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
        SetPlayerColor(Color.white);

        if (life == 1)
        {
            GameManager.GameOver();
            graphics.SetActive(false);
        }
        else
        {
            GUIManager.ChangeLifeIcon(ref life, -1);
            ResetArmor();
            //restore alpha
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
