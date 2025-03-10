using DG.Tweening;
using UnityEngine;

public class Crashing : MonoBehaviour
{
    [SerializeField] float life = 0.8f;
    [SerializeField] float maxSize = 0.2f;
    [SerializeField] float minSize = 0.05f;

    void OnEnable()
    {
        var minVectorSize = new Vector2(minSize, minSize);
        var maxVectorSize = new Vector2(maxSize, maxSize);

        var sr = GetComponentInChildren<SpriteRenderer>();
        sr.DOFade(0.9f, life).OnComplete(DestroyObject);
        sr.transform.localScale = minVectorSize;

        var sequence = DOTween.Sequence();
        sequence.Append(sr.transform.DOScale(maxVectorSize, life * 0.5f))
                .Append(sr.transform.DOScale(minVectorSize, life * 0.5f));
    }

    void DestroyObject()
    {
        CrashingPoolManager.Release(this);
    }
}
