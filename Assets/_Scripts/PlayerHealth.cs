using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int armorStrength = 5;
    [SerializeField] int armorMaxStrength = 5;
    [SerializeField] SpriteRenderer[] armorDot;
    [SerializeField] FlashEffect flashEffect;

    [SerializeField] int explosionCount = 8;
    [SerializeField] float explosionDelay = 0.3f;
    [SerializeField] float explosionGap = 0.3f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TankShell"))
        {

            int damage = other.GetComponent<TankShell>().Damage;
            //delete projectile
            other.gameObject.SetActive(false);
            Debug.Log(damage);

            //spawn hit explosion
            var hitEffect = HitEffectPoolManager.Get();
            hitEffect.transform.position = other.transform.position;

            flashEffect.ProcessFlashing();

            DrawArmorDot(damage);
        }
    }

    void DrawArmorDot(int damage)
    {
        armorStrength -= damage;

        if (armorStrength > 0)
        {
            foreach (var armor in armorDot)
            {
                armor.enabled = false;
            }
            for (int i = 0; i < armorStrength; i++)
            {
                armorDot[i].enabled = true;
            }
        }
        else
        {
                ShipDestroied();
        }
    }

    void ShipDestroied()
    {
        Debug.Log("Ship Destroied!!!");

        GameManager.ChangeGameState(GameManager.GameState.Exploding);
        DestoryingEffect();
    }

    void Restart()
    {
        ResetArmor();
        GameManager.ChangeGameState(GameManager.GameState.Playing);
    }


    void DestoryingEffect()
    {
        var sequence = DOTween.Sequence();

        for (int i = 0; i <= explosionCount; i++)
        {
            sequence.AppendCallback(() =>
            {
                ExplosionPoolManager.Get()
                .transform.position = transform.position + Random.onUnitSphere * explosionGap;
            });

            sequence.AppendInterval(explosionDelay);
        }
        sequence.AppendCallback(() => Restart());
    }

    void ResetArmor()
    {
        armorStrength = 5;
        DrawArmorDot(0);
    }

    void OnDestroy()
    {
        DOTween.Kill(flashEffect);
    }
}
