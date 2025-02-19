using UnityEngine;
using DG.Tweening;

public class Turret : MonoBehaviour
{
    [SerializeField] Transform turret;
    Vector3 turretScale;
    [SerializeField] SpriteRenderer turretSprite;
    [SerializeField] float shrinkX = 1.1f;
    [SerializeField] float shrinkY = 0.8f;
    [SerializeField] float shrinkDuration = 0.10f;
    [SerializeField] float recoverDuration = 0.10f;

    void Start()
    {
        turretScale = transform.localScale;
    }

    public void OnFireEffect()
    {
        var sizeWhileShrink = new Vector2(shrinkX, shrinkY);
        turret.DOScale(sizeWhileShrink, shrinkDuration).OnComplete(SetToOriginalSize);
    }

    void SetToOriginalSize()
    {
        turret?.DOScale(turretScale, recoverDuration);
    }
}
