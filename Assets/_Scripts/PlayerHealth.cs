using DG.Tweening;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int Health;
    [SerializeField] FlashEffect flashEffect;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TankShell"))
        {
            var hitEffect = HitEffectPoolManager.Get();
            hitEffect.transform.position = other.transform.position;

            other.gameObject.SetActive(false);
            flashEffect.ProcessFlashing();
        }
    }

    void OnDestroy()
    {
        DOTween.Kill(flashEffect);
    }
}
