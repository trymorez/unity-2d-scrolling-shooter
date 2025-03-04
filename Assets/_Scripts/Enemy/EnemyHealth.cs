using UnityEngine;
using DG.Tweening;
using System;

public class EnemyHealth : MonoBehaviour
{
    public int Health;
    [SerializeField] FlashEffect flashEffect;
    [SerializeField] GameObject cratorPrefab;
    bool isDead;

    void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            //spawn explosion effect
            var hitEffect = HitEffectPoolManager.Get();
            hitEffect.transform.position = other.transform.position;

            CheckIfDead(other.GetComponent<Bullet>().Damage);
            other.gameObject.SetActive(false);
            flashEffect.ProcessFlashing();
        }
    }

    void CheckIfDead(int damage)
    {
        Health -= damage;
        if (Health < 0 && !isDead)
        {
            isDead = true;
            //spawn explosion effect
            var explosion = ExplosionPoolManager.Get();
            explosion.transform.position = transform.position;

            //spawn crator
            var crator = Instantiate(cratorPrefab, World.worldTransform);
            crator.transform.position = transform.position;
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        DOTween.Kill(flashEffect);
    }
}
