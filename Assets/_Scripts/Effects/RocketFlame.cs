using UnityEngine;
using DG.Tweening;

public class RocketFlame : MonoBehaviour
{
    [SerializeField] Transform tr;
    [SerializeField] float time = 0.2f;
    [SerializeField] float maxFlame = 1.2f;
    [SerializeField] float minFlame = 0.2f;
    Sequence sequence;

    void Start()
    {
        Effect();
    }

    void Effect()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(tr.DOScale(Vector2.one * maxFlame, time)
                    .SetEase(Ease.InOutSine))
                .Append(tr.DOScale(Vector2.one * minFlame, time)
                    .SetEase(Ease.InOutSine));
        sequence.SetLoops(-1);
    }

    void OnDisable()
    {
        sequence.Kill();
    }
}
