using DG.Tweening;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    [SerializeField] float life = 0.5f;

    void Start()
    {
        var Sprite = GetComponentInChildren<SpriteRenderer>();
        Sprite.DOFade(0f, life).OnComplete(DestroyObject);
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
