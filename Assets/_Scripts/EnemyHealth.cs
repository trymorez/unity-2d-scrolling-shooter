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
    Sequence[] flashSequence;
    Sequence[] distortSequence;

    void Start()
    {
        flashSequence = new Sequence[bodyPart.Length];
        distortSequence = new Sequence[bodyPart.Length];
        originalColor = new Color[bodyPart.Length];
        for (int i = 0; i < bodyPart.Length; i++)
        {
            //flashSequence[i] = DOTween.Sequence();
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

            if (--Health < 0)
            {
                Destroy(this.gameObject);
                return;
            }
            if (!isFlashing)
            {
                StartFlashing();
            }
        }
    }

    void OnDestroy()
    {
                Debug.Log("destroied");
        //DOTween.Kill(this.gameObject);
            DOTween.Kill(this);
    }

    void StartFlashing()
    {
        isFlashing = true;

        for (int i = 0; i < bodyPart.Length; i++)
        {
            var index = i;
            var currentPart = bodyPart[index];
            var bodyColor = currentPart.color;
            bodyColor.a = 0;

            flashSequence[index] = DOTween.Sequence(this);
            flashSequence[index]
                .AppendCallback(() => {
                currentPart.enabled = true;
            });

            flashSequence[index]
                .Append(currentPart.DOColor(originalColor[index], flashingTime))
                .Append(currentPart.DOColor(bodyColor, flashingTime))
                .AppendCallback(() => {
                    currentPart.enabled = false;
                });

            flashSequence[index].OnComplete(() => {
                if (index == bodyPart.Length - 1)
                {
                    isFlashing = false;
                }
            });

            distortSequence[index] = DOTween.Sequence(this);
            var originalSize = currentPart.transform.localScale;
            var newSize = 1.1f;

            distortSequence[index]
                .Append(currentPart.transform.DOScale(originalSize * newSize, flashingTime))
                .Append(currentPart.transform.DOScale(originalSize, flashingTime));
        }
    }
}
