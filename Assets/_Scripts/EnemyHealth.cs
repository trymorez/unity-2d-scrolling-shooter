using UnityEngine;
using DG.Tweening;
using System;

public class EnemyHealth : MonoBehaviour
{
    public int Health;
    [SerializeField] SpriteRenderer[] bodyPart;
    [SerializeField] float flashingTime = 0.3f;
    Color[] originalColor;
    bool isFlashing;

    void Start()
    {
        originalColor = new Color[bodyPart.Length];
        for (int i = 0; i < bodyPart.Length; i++)
        {
            originalColor[i] = bodyPart[i].color;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            var hitEffect = HitEffectPoolManager.Get();
            hitEffect.transform.position = other.transform.position;
            other.gameObject.SetActive(false);

            CheckIfDead();
            if (!isFlashing)
            {
                StartFlashing();
            }
        }
    }

    void CheckIfDead()
    {
        if (--Health < 0)
        {
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        DOTween.Kill(this);
    }

    void StartFlashing()
    {
        isFlashing = true;

        for (int i = 0; i < bodyPart.Length; i++)
        {
            int index = i;
            SpriteRenderer currentPart = bodyPart[index];
            Color bodyColor = currentPart.color;
            bodyColor.a = 0;

            Sequence flashSequence = DOTween.Sequence(this);
            flashSequence
                .AppendCallback(() => {
                currentPart.enabled = true;
            });

            flashSequence
                .Append(currentPart.DOColor(originalColor[index], flashingTime))
                .Append(currentPart.DOColor(bodyColor, flashingTime))
                .AppendCallback(() => {
                    currentPart.enabled = false;
                });

            flashSequence.OnComplete(() => {
                if (index == bodyPart.Length - 1)
                {
                    isFlashing = false;
                }
            });

            Sequence distortSequence = DOTween.Sequence(this);
            Vector3 originalSize = currentPart.transform.localScale;
            float newSize = 1.1f;

            distortSequence
                .Append(currentPart.transform.DOScale(originalSize * newSize, flashingTime))
                .Append(currentPart.transform.DOScale(originalSize, flashingTime));
        }
    }
}
