using UnityEngine;
using DG.Tweening;

public class Shield : MonoBehaviour
{
    [SerializeField] SpriteRenderer shield;
    [SerializeField] float maxAlpha = 1f;
    [SerializeField] float minAlpha = 0.1f;
    [SerializeField] float waitAfterMax = 0.3f;
    [SerializeField] float duration = 0.3f;

    void Start()
    {
        //ModulateShield();
    }

    void ModulateShield()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(shield.DOFade(maxAlpha, duration).SetEase(Ease.InOutSine));
        sequence.AppendInterval(waitAfterMax);
        sequence.Append(shield.DOFade(minAlpha, duration * 0.5f).SetEase(Ease.OutCubic));
        sequence.SetLoops(-1);
    }
}
