using DG.Tweening;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int Health = 1;
    [SerializeField] int shieldStrength = 5;
    [SerializeField] int shieldMaxStrength = 5;
    [SerializeField] SpriteRenderer[] healthDot;

    [SerializeField] FlashEffect flashEffect;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TankShell"))
        {
            var damage = other.GetComponent<TankShell>().Damage;
            //spawn hit explosion
            var hitEffect = HitEffectPoolManager.Get();
            hitEffect.transform.position = other.transform.position;

            //delete bullet
            other.gameObject.SetActive(false);

            flashEffect.ProcessFlashing();

            if (shieldStrength >= damage)
            {
                shieldStrength -= damage;
            }
            else
            {
                Health -= damage;
                if (Health < 0)
                {
                    ShipDestroied();
                }
            }
        }
    }



    void ShipDestroied()
    {
        Debug.Log("Ship Destroied!!!");
        Reset();
    }

    void Reset()
    {
        Health = 1;
        shieldStrength = 5;
    }

    void OnDestroy()
    {
        DOTween.Kill(flashEffect);
    }
}
