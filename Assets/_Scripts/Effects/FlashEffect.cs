using DG.Tweening;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] bodyPart;
    Color[] originalColor;
    [SerializeField] float flashingTime = 0.3f;
    [SerializeField] float distortSize = 1f;
    [HideInInspector]
    public bool IsFlashing;

    void Start()
    {
        originalColor = new Color[bodyPart.Length];
        for (int i = 0; i < bodyPart.Length; i++)
        {
            originalColor[i] = bodyPart[i].color;
        }
    }

    public void ProcessFlashing()
    {
        if (IsFlashing)
        {
            return;
        }

        IsFlashing = true;

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
                    IsFlashing = false;
                }
            });

            if (distortSize != 0)
            {
                Sequence distortSequence = DOTween.Sequence(this);
                Vector3 originalSize = currentPart.transform.localScale;

                distortSequence
                    .Append(currentPart.transform.DOScale(originalSize * distortSize, flashingTime))
                    .Append(currentPart.transform.DOScale(originalSize, flashingTime));
            }
        }
    }
}
