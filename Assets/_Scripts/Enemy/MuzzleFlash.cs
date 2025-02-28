using DG.Tweening;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    [SerializeField] float life = 0.5f;
    [SerializeField] float _maxSize = 0.15f;
    [SerializeField] float _minSize = 0.05f;

    void OnEnable()
    {
        World.OnScrollMap += OnScrollMap;

        var minSize = new Vector2(_minSize, _minSize);
        var maxSize = new Vector2(_maxSize, _maxSize);

        var sr = GetComponentInChildren<SpriteRenderer>();
        sr.DOFade(0.5f, life).OnComplete(Release);
        sr.transform.localScale = minSize;

        var sequence = DOTween.Sequence();
        sequence.Append(sr.transform.DOScale(maxSize, life * 0.5f))
                .Append(sr.transform.DOScale(minSize, life * 0.5f));
    }

    void OnDisable()
    {
        World.OnScrollMap -= OnScrollMap;
    }

    void OnScrollMap(float amount)
    {
        var pos = transform.position;
        pos.y -= amount;
        transform.position = pos;
    }

    void Release()
    {
        MuzzleFlashPoolManager.Pool.Release(this);
    }
}
