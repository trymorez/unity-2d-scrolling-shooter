using UnityEngine;
using DG.Tweening;

public class RocketFlame : MonoBehaviour
{
    [SerializeField] Transform tr;
    [SerializeField] float time = 0.5f;
    [SerializeField] float maxFlame = 0.5f;
    [SerializeField] float minFlame = 1.5f;

    void Start()
    {
        Effect();
    }

    void Effect()
    {
        //tr.DOScale(Vector2.one * maxFlame, time)
        //    .SetEase(Ease.OutBounce)
        //    .OnComplete(() =>
        //    {
        //        tr.DOScale(Vector2.one * minFlame, time * 0.5f)
        //        .SetEase(Ease.OutBounce)
        //        .OnComplete(() => Effect());
        //    });
        var sequence = DOTween.Sequence();
        sequence.Append(tr.DOScale(Vector2.one * maxFlame, time)
            .SetEase(Ease.OutBounce));
        sequence.Append(tr.DOScale(Vector2.one * minFlame, time * 0.5f)
            .SetEase(Ease.OutBounce));
        sequence.SetLoops(-1);
    }

}
