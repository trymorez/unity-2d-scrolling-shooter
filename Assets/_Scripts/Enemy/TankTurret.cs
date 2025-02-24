using UnityEngine;
using DG.Tweening;

public class TankTurret : MonoBehaviour
{
    [SerializeField] Transform turret;
    Vector3 turretScale;
    [SerializeField] SpriteRenderer turretSprite;
    [SerializeField] float morphX = 1.1f;
    [SerializeField] float morphY = 0.8f;
    [SerializeField] float morphDuration = 0.1f;
    [SerializeField] float recoverDuration = 0.1f;

    void Start()
    {
        turretScale = new Vector3(1, 1, 1);
    }

    public void OnTurretMorph()
    {
        var sizeWhileShrink = new Vector2(morphX, morphY);
        turret?.DOScale(sizeWhileShrink, morphDuration).OnComplete(SetToOriginalSize);
    }

    void SetToOriginalSize()
    {
        turret?.DOScale(turretScale, recoverDuration);
    }

    void OnDestroy()
    {
        turret?.DOKill();
    }
}
