using DG.Tweening;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float life = 0.5f;
    [SerializeField] float maxSize = 0.15f;
    [SerializeField] float minSize = 0.05f;

    void OnEnable()
    {
        World.OnScrollMap += OnScrollMap;

        var minVectorSize = new Vector2(minSize, minSize);
        var maxVectorSize = new Vector2(maxSize, maxSize);

        var sr = GetComponentInChildren<SpriteRenderer>();
        sr.DOFade(0.5f, life).OnComplete(DestroyObject);
        sr.transform.localScale = minVectorSize;

        var sequence = DOTween.Sequence();
        sequence.Append(sr.transform.DOScale(maxVectorSize, life * 0.5f))
                .Append(sr.transform.DOScale(minVectorSize, life * 0.5f));
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

    void DestroyObject()
    {
        ExplosionPoolManager.Release(this);
    }
}
