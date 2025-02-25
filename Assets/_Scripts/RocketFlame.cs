using UnityEngine;
using DG.Tweening;

public class RocketFlame : MonoBehaviour
{
    [SerializeField] Transform tr;
    [SerializeField] float time = 0.2f;
    [SerializeField] float maxFlame = 1.2f;
    [SerializeField] float minFlame = 0.2f;

    void Start()
    {
        Effect();
    }

    void Effect()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(tr.DOScale(Vector2.one * maxFlame, time)
                    .SetEase(Ease.OutBounce))
                .Append(tr.DOScale(Vector2.one * minFlame, time * 0.5f)
                    .SetEase(Ease.OutBounce));
        sequence.SetLoops(-1);
    }
}
