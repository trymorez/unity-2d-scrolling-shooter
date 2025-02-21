using DG.Tweening;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    [SerializeField] float life = 0.5f;
    [SerializeField] float size = 0.15f;

    void Start()
    {
        var minSize = new Vector2 (0, 0);
        var maxSize = new Vector2 (size, size);

        var sr = GetComponentInChildren<SpriteRenderer>();
        sr.DOFade(0.5f, life).OnComplete(DestroyObject);
        sr.transform.localScale = minSize;
        var sequence = DOTween.Sequence();
        sequence.Append(sr.transform.DOScale(maxSize, life * 0.5f))
                .Append(sr.transform.DOScale(minSize, life * 0.5f));
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
